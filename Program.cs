using System;
using System.Threading.Tasks;
using RestSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace ConsoleApiCall
{
  class Program
  {
    public class Article
    {
        public string Section { get; set; }
        public string Title { get; set; }
        public string Abstract { get; set; }
        public string Url { get; set; }
        public string Byline { get; set; }
        public List<ArticleMultimedia> Multimedia { get; set; }

    }
    public class ArticleMultimedia
    {
      public string Url { get; set; }
      public string Format { get; set; }
      public string Height { get; set; }
      public string Width { get; set; }
      public string Type { get; set; }
      public string Subtype { get; set; }
      public string Caption { get; set; }
      public string Copyright { get; set; }

    }

    static void Main()
    {
      var apiCallTask = ApiHelper.ApiCall("MY NYT API KEY");
      var result = apiCallTask.Result;

      JObject jsonResponse = JsonConvert.DeserializeObject<JObject>(result);
      List<Article> articleList = JsonConvert.DeserializeObject<List<Article>>(jsonResponse["results"].ToString());

      foreach (Article article in articleList)
      {
          Console.WriteLine($"Section: {article.Section}");
          Console.WriteLine($"Title: {article.Title}");
          Console.WriteLine($"Abstract: {article.Abstract}");
          Console.WriteLine($"Url: {article.Url}");
          Console.WriteLine($"Byline: {article.Byline}");
          if(article.Multimedia.Count > 0)
            Console.WriteLine($"Image Copyright: {article.Multimedia[0].Copyright}");
      }
    }
  }

  class ApiHelper
  {
    public static async Task<string> ApiCall(string apiKey)
    {
      RestClient client = new RestClient("https://api.nytimes.com/svc/topstories/v2");
      RestRequest request = new RestRequest($"home.json?api-key={apiKey}", Method.GET);
      var response = await client.ExecuteTaskAsync(request);
      return response.Content;
    }
  }
}
