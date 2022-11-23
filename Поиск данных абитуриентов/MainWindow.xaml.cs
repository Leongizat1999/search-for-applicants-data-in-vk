
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

            Members.Clear();
            
            foreach (var m in result.response.response.items) 
            {
                Members.Add(m);
            }

            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "*.json|*.json";
            dlg.RestoreDirectory = true;
            if (dlg.ShowDialog() == true)
            {
                File.WriteAllText(dlg.FileName, txtResponse.Text);
            }
        }
    }
}
