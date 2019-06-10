using System;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Force.Crc32;
using WaaS.Business.Entities;
using WaaS.Business.Interfaces.Services;

namespace WaaS.Infrastructure.Scraper
{
  public class SimpleHtmlRegexScraper: IScraper
  {

    private readonly HttpClient _client;

    public SimpleHtmlRegexScraper(HttpClient client)
    {
      _client = client;
    }

    private class HttpResult
    {
      public HttpStatusCode StatusCode { get; set; }
      public long ResponseTimeInMs { get; set; }
      public string HtmlString { get; set; }
      public byte[] HtmlBytes { get; set; }
    }

    public async Task<ScrapeJobEvent> ExecuteAsync(Uri url, string searchPattern)
    {
      if (url == null)
      {
        return null;
      }

      var response = await GetResponseAsync(url);
      var eventType = ScrapeJobEventType.Error;
      string fingerprint = "";

      if (!string.IsNullOrWhiteSpace(response.HtmlString))
      {
        var matches = MatchesPattern(response.HtmlString, searchPattern);
        eventType = matches ? ScrapeJobEventType.Match : ScrapeJobEventType.NoMatch;
        fingerprint = Crc32CAlgorithm.Compute(response.HtmlBytes).ToString(CultureInfo.InvariantCulture);
      }

      return new ScrapeJobEvent
      {
        HttpResponseCode = response.StatusCode,
        HttpResponseTimeInMs = response.ResponseTimeInMs,
        TimeStamp = DateTime.UtcNow,
        Type = eventType,
        Url = url.AbsoluteUri,
        Fingerprint = fingerprint
      };
    }

    private async Task<HttpResult> GetResponseAsync(Uri url)
    {
      string htmlString = "";
      byte[] htmlBytes = { };

      Stopwatch stopwatch = Stopwatch.StartNew();
      HttpResponseMessage response = await _client.GetAsync(url);
      stopwatch.Stop();
      if (response.IsSuccessStatusCode)
      {
        htmlString = await response.Content.ReadAsStringAsync();
        htmlBytes = await response.Content.ReadAsByteArrayAsync();
      }

      return new HttpResult
      {
        StatusCode = response.StatusCode,
        ResponseTimeInMs = stopwatch.ElapsedMilliseconds,
        HtmlString = htmlString,
        HtmlBytes = htmlBytes
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
