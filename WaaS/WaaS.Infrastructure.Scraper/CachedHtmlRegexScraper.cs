using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Force.Crc32;
using WaaS.Business.Entities;
using WaaS.Business.Interfaces.Services;

namespace WaaS.Infrastructure.Scraper
{
  public class CachedHtmlRegexScraper: IScraper
  {

    private readonly HttpClient _client;

    private readonly IDictionary<Uri, HttpResult> _resultsCache;

    public CachedHtmlRegexScraper(HttpClient client)
    {
      _client = client;
      _resultsCache = new Dictionary<Uri, HttpResult>();
    }

    private class HttpResult
    {
      public HttpStatusCode StatusCode { get; set; }
      public long ResponseTimeInMs { get; set; }
      public string HtmlString { get; set; }
      public string HtmlFingerprint { get; set; }
    }

    public async Task<ScrapeJobEvent> ExecuteAsync(Uri url, string searchPattern)
    {
      if (url == null)
      {
        return null;
      }

      HttpResult response;

      if (_resultsCache.ContainsKey(url))
      {
        _resultsCache.TryGetValue(url, out response);
      }
      else
      {
        response = await GetResponseAsync(url);
        _resultsCache.Add(url, response);
      }


      var eventType = ScrapeJobEventType.Error;
      string fingerprint = "";

      if (response != null && !string.IsNullOrWhiteSpace(response.HtmlString))
      {
        var matches = MatchesPattern(response.HtmlString, searchPattern);
        eventType = matches ? ScrapeJobEventType.Match : ScrapeJobEventType.NoMatch;
        fingerprint = response.HtmlFingerprint;
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
        HtmlFingerprint = Crc32CAlgorithm.Compute(htmlBytes).ToString(CultureInfo.InvariantCulture)
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
