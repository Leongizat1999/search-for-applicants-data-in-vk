
using Aspose.Cells;
using Aspose.Cells.Utility;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using Поиск_данных_абитуриентов.vk;//строчка за внутренние библиотеки

namespace Поиск_данных_абитуриентов
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        public String ResponseContent
        {
            get { return (String)GetValue(ResponseContentProperty); }
            set { SetValue(ResponseContentProperty, value); }
        }

        public static readonly DependencyProperty ResponseContentProperty = 
            DependencyProperty.Register("ResponseContent", typeof(String), typeof(MainWindow));


        public ObservableCollection<VKGroupMember> Members { get; set; } 
            = new ObservableCollection<VKGroupMember>();


        public MainWindow()
        {
            InitializeComponent();
        }

        async private void Button_Click(object sender, RoutedEventArgs e)
        {
            ResponseContent = "Гружу..";
            var result = await Utility.FetchMembersInfo(txtGroupId.Text);
            ResponseContent = Utility.PrettyJson(result.rawContent);

            Members.Clear();// когда добавим перезагрузки для постоянных обращений убрать
            
            foreach (var m in result.response.response.items) 
            {
                Members.Add(m);
            }

            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            SaveFileDialog dlg = new SaveFileDialog// проверка на сохранение
            {
                Filter = "*.xlsx|*.xlsx",
                RestoreDirectory = true
            };
            if (dlg.ShowDialog() == true) //трансформация json в exel
            {
                Workbook workbook = new Workbook();
                Worksheet worksheet = workbook.Worksheets[0];

                string jsonInput = txtResponse.Text;

                JsonLayoutOptions options = new JsonLayoutOptions();
                options.ArrayAsTable = true;

                JsonUtility.ImportData(jsonInput, worksheet.Cells, 0, 0, options);

                // File.WriteAllText(dlg.FileName, txtResponse.Text);
                workbook.Save(dlg.FileName);
            }
        }
    }
}
