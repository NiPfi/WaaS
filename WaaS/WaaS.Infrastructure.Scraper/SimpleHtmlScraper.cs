using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WaaS.Business.Entities;
using WaaS.Business.Interfaces.Services;

namespace WaaS.Infrastructure.Scraper
{
  public class SimpleHtmlScraper: IScraper
  {

    private readonly HttpClient _client;

    public SimpleHtmlScraper(HttpClient client)
    {
      _client = client;
    }

    private class HttpResult
    {
      public HttpStatusCode StatusCode { get; set; }
      public long ResponseTimeInMs { get; set; }
      public string HtmlString { get; set; }
    }

    public async Task<ScrapeJobEvent> ExecuteAsync(Uri url, string searchPattern)
    {

      var response = await GetResponseAsync(url);
      var eventType = ScrapeJobEventType.Error;
      if (!string.IsNullOrWhiteSpace(response.HtmlString))
      {
        var matches = MatchesPattern(response.HtmlString, searchPattern);
        eventType = matches ? ScrapeJobEventType.Match : ScrapeJobEventType.NoMatch;
      }

      return new ScrapeJobEvent
      {
        HttpResponseCode = response.StatusCode,
        HttpResponseTimeInMs = response.ResponseTimeInMs,
        TimeStamp = DateTime.UtcNow,
        Type = eventType
      };
    }

    private async Task<HttpResult> GetResponseAsync(Uri url)
    {
      string htmlString = "";

      Stopwatch stopwatch = Stopwatch.StartNew();
      HttpResponseMessage response = await _client.GetAsync(url);
      stopwatch.Stop();
      if (response.IsSuccessStatusCode)
      {
        htmlString = await response.Content.ReadAsStringAsync();
      }

      return new HttpResult
      {
        StatusCode = response.StatusCode,
        ResponseTimeInMs = stopwatch.ElapsedMilliseconds,
        HtmlString = htmlString
      };
    }

    private static bool MatchesPattern(string html, string searchPattern)
    {
      Regex regex = new Regex(searchPattern);
      var result = regex.Match(html);
      return result.Success;
    }
  }
}
