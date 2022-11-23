using System;
using System.Collections.Generic;
using System.Text;

namespace Поиск_данных_абитуриентов.vk
{
    public class VKItemsResponse<T>
    {
        public int count { get; set; }
        public List<T> items { get; set; }
    }
}
