using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

namespace Поиск_данных_абитуриентов.vk
{
    public class Response<T>
    {
        public String rawContent { get; set; }
        public T response { get; set; }
    }
    
    class Utility
    {
        private static String ACCES_TOKEN = "cb4839b2cb4839b2cb4839b279cb0dd711ccb48cb4839b29294c351dbc18728681647e7";//сюда вставить токен приложения
        private static String Version = "5.131";

        private static HttpClient client = new HttpClient();
        public JavaScriptEncoder Encoder { get; private set; }

        public static Task<HttpResponseMessage> VKGet(String method, Dictionary<string,string> param) //обращение через access token 
        {
            var builder = new UriBuilder($"https://api.vk.com/method/{method}");
            var query = HttpUtility.ParseQueryString(builder.Query);
            query["access_token"] = ACCES_TOKEN;
            query["v"] = Version;
            foreach (var key in param.Keys)
            {
                query[key] = param[key];
            }
            builder.Query = query.ToString();
            string url = builder.ToString();
            return client.GetAsync(url);
        }

        public static string PrettyJson(string unPrettyJson)//просто выводит ответ в красивом виде 
        {
            var options = new JsonSerializerOptions()
            {
                WriteIndented = true,
                Encoder =System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping

            };

            var jsonElement = JsonSerializer.Deserialize<JsonElement>(unPrettyJson);
            return JsonSerializer.Serialize(jsonElement, options);
        }

        public static async Task<Response<VKDictResponse<VKItemsResponse<VKGroupMember>>>> FetchMembersInfo(String groupId)//функция запроса
        {
            HttpResponseMessage response = await VKGet("groups.getMembers", new Dictionary<string, string>
            {
                ["group_id"] = groupId,
                ["fields"] = "photo_100,bdate", //https://dev.vk.com/method/groups.getMembers#Параметры 
                ["count"] = "100",              // количество записей
                ["lang"] = "ru",
                


            }
                );
            var content = await response.Content.ReadAsStringAsync();//функция получения ответа

            var itemsResponse = JsonSerializer.Deserialize<VKDictResponse<VKItemsResponse<VKGroupMember>>>(content);

            return new Response<VKDictResponse<VKItemsResponse<VKGroupMember>>>
            {
                response=itemsResponse,
                rawContent=content
            };
        }
    }

}
