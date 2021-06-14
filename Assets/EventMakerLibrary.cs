using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace EventMakersLibrary
{
    public class EventMakerLibrary : MonoBehaviour
    {
        //private static string https = "https://mosdit.site/api/";
        public static async Task CreatePreferences(string id, List<string> ChosenCategory) //uuid пользователя и категории отсортированные по предпочтениям
        {
            string ChosenCategoryString = "";
            foreach (string category in ChosenCategory)
            {
                ChosenCategoryString += category + ",";
            }
            string url = "https://mosdit.site/api/create?id=" + id + "&categories=" + ChosenCategoryString.Remove(ChosenCategoryString.Length - 1);
            //https://mosdit.site/api/create?id=11111111111&categories=80299   Пример запроса
            using (var httpClient = new HttpClient())
            {
                print("Запрос на  сервер");
                using (var request = new HttpRequestMessage(new HttpMethod("GET"), url))
                {
                    var response = await httpClient.SendAsync(request);
                    HttpContent responseContent = response.Content;
                    print("получили ответ");
                    using (var reader = new StreamReader(await responseContent.ReadAsStreamAsync()))
                    {
                        string JSON = (await reader.ReadToEndAsync()).ToString();
                        print(JSON);
                        RecomendationList recomendationList = JsonUtility.FromJson<RecomendationList>(JSON);
                        print("Пропарсили JSON. Количество элементов = " + recomendationList.groups.Count);
                        print("Название группы = " + recomendationList.groups[0].title + ". Количество элементов = " + recomendationList.groups[0].items.Count);
                    }
                }
            }
        }
        public static async Task GetRecomendation(string id) //uuid пользователя и категории отсортированные по предпочтениям
        {
            string url = "https://mosdit.site/api/get?id=" + id;
            //https://mosdit.site/api/get?id=11111111111   Пример запроса
            using (var httpClient = new HttpClient())
            {
                print("Запрос на  сервер");
                using (var request = new HttpRequestMessage(new HttpMethod("GET"), url))
                {
                    var response = await httpClient.SendAsync(request);
                    HttpContent responseContent = response.Content;
                    print("получили ответ");
                    using (var reader = new StreamReader(await responseContent.ReadAsStreamAsync()))
                    {
                        string JSON = (await reader.ReadToEndAsync()).ToString();
                        print(JSON);
                        RecomendationList recomendationList = JsonUtility.FromJson<RecomendationList>(JSON);
                        print("Пропарсили JSON. Количество элементов = " + recomendationList.groups.Count);
                        print("Пропарсили JSON. Количество элементов = " + recomendationList.groups[0].items.Count);
                        MainMenu.instance.CreateRecomendationGroup(recomendationList);
                    }
                }
            }
        }
        public static EventMakerLibrary instance { get; private set; }

        private void Awake()
        {
            instance = this;
        }
        public static void GetImage(UnityEngine.UI.Image image, string url)
        {
            instance.StartCoroutine(instance.GetImageCoroutine(image, url));
        }
        IEnumerator GetImageCoroutine(UnityEngine.UI.Image image, string url) //получаем картинку с mos.ru, парсим в наше приложение
        {
            UnityWebRequest request = UnityWebRequestTexture.GetTexture("https://www.mos.ru" + url);
            //print(url);
            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
                Debug.Log(request.error);
            else
            {
                Texture2D tex = ((DownloadHandlerTexture)request.downloadHandler).texture;
                image.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(tex.width / 2, tex.height / 2));
                image.preserveAspect = true;
            }
        }
        public static async Task GETEventFilterArray(List<string> filtername,List<string> filterchosen)
        {
            string url = CreateJSON(filtername,filterchosen);
            using (var httpClient = new HttpClient())
            {
                print("Запрос на  сервер");
                print(url);
                using (var request = new HttpRequestMessage(new HttpMethod("GET"), url))
                {
                    request.Headers.TryAddWithoutValidation("authority", "www.mos.ru");
                    request.Headers.TryAddWithoutValidation("x-kl-ajax-request", "Ajax_Request");
                    request.Headers.TryAddWithoutValidation("sec-ch-ua-mobile", "?0");
                    request.Headers.TryAddWithoutValidation("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/90.0.4430.72 YaBrowser/21.5.1.330 Yowser/2.5 Safari/537.36");
                    request.Headers.TryAddWithoutValidation("sec-ch-ua", "^^");
                    request.Headers.TryAddWithoutValidation("x-caller-id", "mosru.main-page");
                    request.Headers.TryAddWithoutValidation("accept", "*/*");
                    request.Headers.TryAddWithoutValidation("sec-fetch-site", "same-origin");
                    request.Headers.TryAddWithoutValidation("sec-fetch-mode", "cors");
                    request.Headers.TryAddWithoutValidation("sec-fetch-dest", "empty");
                    request.Headers.TryAddWithoutValidation("referer", "https://www.mos.ru/");
                    request.Headers.TryAddWithoutValidation("accept-language", "ru,en;q=0.9");
                    var response = await httpClient.SendAsync(request);

                    // Get the response content.
                    HttpContent responseContent = response.Content;
                    print("получили ответ");
                    // Get the stream of the content.
                    using (var reader = new StreamReader(await responseContent.ReadAsStreamAsync()))
                    {

                        // Write the output.
                        string JSON = (await reader.ReadToEndAsync()).ToString();
                        //Text.text = JsonUtility.FromJson<EventList>(JSON).ToString();
                        JSON = fixjson(JSON);
                        print(JSON);
                        EventList jsonarray = JsonUtility.FromJson<EventList>(JSON);
                        print("Пропарсили JSON. Количество элементов = " + jsonarray.items.Count);
                        MainMenu.instance.CreateEventOnFilter(jsonarray);
                    }
                }
            }
        }
        public static string CreateJSON(List<string> filtername, List<string> filter)
        {
            //string filters = "{\"<=occurrences.date_from\":\"2021-06-10 23:59:59\",\">=occurrences.date_from\":\"2021-06-01 00:00:00\",\"spheres.id\":[\"15299\",\"305299\"],\"free\":1}";
            string filters = "{";
            for (int i = 0;i<filtername.Count;i++)
            {
                filters += "\"" + filtername[i] + "\":\"" + filter[i] + "\",";
            }
            filters = filters.Remove(filters.Length - 1);
            filters += "}";
            string JSON = "https://www.mos.ru/api/newsfeed/v4/frontend/json/ru/afisha?expand=url"; //expand
            JSON += "&fields=id,title,date,image,url"; //fields
            JSON += "&filter=" + HttpUtility.UrlEncode(filters);
            JSON += "&per-page=30";
            JSON += "&sort=occurrences.date_to,-occurrences.date_from";
            print(JSON);
            return JSON;
        }
        //короткая до 12 штук (без изображений)https://www.mos.ru/api/newsfeed/v4/frontend/json/ru/afisha?expand=url&fields=id,title,date,url&filter=%7B%22%3E%3Doccurrences.date_from%22%3A%222021-06-13%2000%3A00%3A00%22%2C%22spheres.id%22%3A%2278299%22%7D&per-page=12&sort=occurrences.date_to,-occurrences.date_from;
        //с изображениями до 30 штук https://www.mos.ru/api/newsfeed/v4/frontend/json/ru/afisha?expand=url&fields=id,title,date,image,url&filter=%7B%22%3E%3Doccurrences.date_from%22%3A%222021-06-13%2000%3A00%3A00%22%2C%22spheres.id%22%3A%2278299%22%7D&per-page=30&sort=occurrences.date_to,-occurrences.date_from
        static string fixjson(string s)
        {
            int b = s.IndexOf("],\"_links\"");
            //print(b);
            s = s.Remove(b + 1, s.Length - b - 1);
            s += "}";
            //print(s);
            return s;
        }
    }

    [Serializable]
    public class Events //мероприятия
    {
        public int id;
        public string title;
        public string date_from;
        public string date_to;
        public int free;
        public string image;
    }
    [Serializable]
    public class Group//сгрупированные мероприятия (рекомендации на главном меню)
    {
        public int id;
        public string title;
        public List<Events> items;
    }
    [Serializable]
    public class RecomendationList //Все рекомендации для главного меню
    {
        public List<Group> groups;
    }




    //мусорка/помойка для парса JSON от mos.ru
    [Serializable]
    public class Original
    {
        public long id;
        public object title;
        public string src;
        public int width;
        public int height;
        public string type;
    }
    [Serializable]
    public class Small
    {
        public long id;
        public object title;
        public string src;
        public int width;
        public int height;
        public string type;
    }
    [Serializable]
    public class Middle
    {
        public long id;
        public object title;
        public string src;
        public int width;
        public int height;
        public string type;
    }
    [Serializable]
    public class Big
    {
        public long id;
        public object title;
        public string src;
        public int width;
        public int height;
        public string type;
    }
    [Serializable]
    public class Download
    {
        public long id;
        public object title;
        public string src;
        public int width;
        public int height;
        public string type;
    }
    [Serializable]
    public class Thumb
    {
        public long id;
        public object title;
        public string src;
        public int width;
        public int height;
        public string type;
    }
    [Serializable]
    public class Show
    {
        public long id;
        public object title;
        public string src;
        public int width;
        public int height;
        public string type;
    }
    [Serializable]
    public class _1x1M
    {
        public long id;
        public object title;
        public string src;
        public int width;
        public int height;
        public string type;
    }
    [Serializable]
    public class _1x1S
    {
        public long id;
        public object title;
        public string src;
        public int width;
        public int height;
        public string type;
    }
    [Serializable]
    public class _2x1B
    {
        public long id;
        public object title;
        public string src;
        public int width;
        public int height;
        public string type;
    }
    [Serializable]
    public class _2x1M
    {
        public long id;
        public object title;
        public string src;
        public int width;
        public int height;
        public string type;
    }
    [Serializable]
    public class _2x1S
    {
        public long id;
        public object title;
        public string src;
        public int width;
        public int height;
        public string type;
    }
    [Serializable]
    public class _3x1B
    {
        public long id;
        public object title;
        public string src;
        public int width;
        public int height;
        public string type;
    }
    [Serializable]
    public class _3x1M
    {
        public long id;
        public object title;
        public string src;
        public int width;
        public int height;
        public string type;
    }
    [Serializable]
    public class _3x1S
    {
        public long id;
        public object title;
        public string src;
        public int width;
        public int height;
        public string type;
    }
    [Serializable]
    public class _4x1B
    {
        public long id;
        public object title;
        public string src;
        public int width;
        public int height;
        public string type;
    }
    [Serializable]
    public class _4x1M
    {
        public long id;
        public object title;
        public string src;
        public int width;
        public int height;
        public string type;
    }
    [Serializable]
    public class _4x1S
    {
        public long id;
        public object title;
        public string src;
        public int width;
        public int height;
        public string type;
    }
    [Serializable]
    public class Image
    {
        public long id;
        public object title;
        public object copyright;
        public Original original;
        public Small small;
        public Middle middle;
        public Big big;
        public Download download;
        public Thumb thumb;
        public Show show;
        public _1x1M _1x1_m;
        public _1x1S _1x1_s;
        public _2x1B _2x1_b;
        public _2x1M _2x1_m;
        public _2x1S _2x1_s;
        public _3x1B _3x1_b;
        public _3x1M _3x1_m;
        public _3x1S _3x1_s;
        public _4x1B _4x1_b;
        public _4x1M _4x1_m;
        public _4x1S _4x1_s;
    }
    [Serializable]
    public class Item
    {
        public int id;
        public string title;
        public string date;
        public Image image;
        public string url;
    }
    [Serializable]
    public class EventList
    {
        public List<Item> items;
        //public Links _links ;
        //public Meta _meta ;
    }
}
