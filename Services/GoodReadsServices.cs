using System.Linq;
using System.Collections.Generic;
using System.Net.Http;
using BooksReaderFunction.Models;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;

namespace BooksReaderFunction.Services
{
    public class GoodReadsServices
    {
        public static IList<Livro> GetBooksFromGoodReader(ILogger log, string userId, string shelf)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(shelf))
            {
                log.LogError("Invalid Parameters");
            }

            string url_GoodReads = $"https://www.goodreads.com/review/list/{userId}?ref=nav_mybooks";
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.OptionFixNestedTags = true;
            IList<Livro> meusLivros = new List<Livro>();
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = client.GetAsync(url_GoodReads).Result)
                {
                    using (HttpContent content = response.Content)
                    {
                        string result = content.ReadAsStringAsync().Result;
                        htmlDoc.LoadHtml(result);

                        if (htmlDoc.ParseErrors != null && htmlDoc.ParseErrors.Count() > 0)
                        {
                            log.LogError("Error on the Page parse");
                        }
                        else
                        {
                            if (htmlDoc.DocumentNode != null)
                            {
                                for (int i = 0; i < htmlDoc.DocumentNode.SelectNodes("//tr[@class='bookalike review']").Count; i++)
                                {
                                    meusLivros.Add(new Livro
                                    {
                                        ImagePath = htmlDoc.DocumentNode.SelectNodes("//td[@class='field cover']")[i].ChildNodes[1].ChildNodes[1].ChildNodes[1].ChildNodes[0].Attributes["src"].Value,
                                        Titulo = htmlDoc.DocumentNode.SelectNodes("//td[@class='field title']")[i].ChildNodes[1].ChildNodes[1].ChildNodes[1].Attributes["title"].Value,
                                        Autor = htmlDoc.DocumentNode.SelectNodes("//td[@class='field author']")[i].ChildNodes[1].ChildNodes[1].ChildNodes[1].InnerText
                                    });
                                }
                            }
                        }
                    }
                }
            }
            return meusLivros;
        }
    }
}