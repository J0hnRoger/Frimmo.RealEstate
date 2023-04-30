using System.Net.Http.Headers;
using CSharpFunctionalExtensions;

namespace Frimmo.Scrapper;

public class WebScrapper
{
    private HttpClient _httpClient;

    public WebScrapper()
    {
        _httpClient = new HttpClient();

        // _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/112.0.0.0 Safari/537.36");
        // _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Host", "www.leboncoin.fr:443");
        // _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7");
        // _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Language", "fr-FR,fr;q=0.9,en-US;q=0.8,en;q=0.7");
        // _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Encoding", "gzip, deflate, br");
        // _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Connection", "keep-alive");
        // _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Upgrade-Insecure-Requests", "1");
        // _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Sec-Fetch-Dest", "document");
        // _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Sec-Fetch-Mode", "navigate");
        // _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Sec-Fetch-Site", "none");
        // _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Sec-Fetch-User", "?1");
        // _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Cache-Control", "max-age=0"); */
    }
    
    public async Task<Result<string>> GetContent(string url)
    {
        HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, $"{url}");
        
        httpRequestMessage.Headers.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/112.0.0.0 Safari/537.36");  
        httpRequestMessage.Headers.Accept.ParseAdd("text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7");  
        httpRequestMessage.Headers.AcceptLanguage.ParseAdd("fr-FR,fr;q=0.9,en-US;q=0.8,en;q=0.7");  
        
        var httpResponseMessage = await _httpClient.SendAsync(httpRequestMessage);

        if (!httpResponseMessage.IsSuccessStatusCode)
            return Result.Failure<string>($"Echec de la requete: {httpResponseMessage.StatusCode}"); 
        string result = await httpResponseMessage.Content.ReadAsStringAsync();
        
        return Result.Success(result);
    }
}