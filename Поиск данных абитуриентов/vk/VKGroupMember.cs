using System;
using System.Collections.Generic;
using System.Text;

namespace Поиск_данных_абитуриентов.vk
{
    public class VKGroupMember
    {
        public int id { get; set; }

        public String first_name { get; set; }

        public String last_name { get; set; }

        public String photo_100 { get; set; }

        public String bdate { get; set; }


        public override string ToString() // функция для вывода на форму
        {
            return $"{first_name} {last_name} {id}";
        }
    }
}
