using HtmlAgilityPack;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Drawing.Printing;
using System.Xml.Linq;

public class GetStocksService
{
    private readonly HttpClient _httpClient;

    public GetStocksService()
    {
        _httpClient = new HttpClient();
    }

#pragma warning disable CS8603 // 可能有 Null 參考傳回
    public async Task<string> GetNordVPNDataAsync(string ipAddress)
    {
        try
        {
            string apiUrl = $"https://nordvpn.com/wp-admin/admin-ajax.php?action=get_user_info_data&ip={ipAddress}";
            HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                return responseBody;
            }
            else
            {
                Console.WriteLine("HTTP请求失败，状态码：" + response.StatusCode);
                //return null;
                return "HTTP请求失败，状态码：";
            }
        }
        catch (Exception ex)
        {
            //Console.WriteLine("发生异常：" + ex.Message);
            return "发生异常：" + ex.Message;
        }
    }

    public async Task<string> getExDividendNoticeForm()
    {
        try
        {
            // 创建 Chrome WebDriver 实例
            //var driverService = ChromeDriverService.CreateDefaultService();
            //using var driver = new ChromeDriver(driverService);

            //try
            //{
            //    // 访问初始页面以设置 cookies
            //    driver.Navigate().GoToUrl("https://www.wantgoo.com/stock/calendar/dividend-right");
            //    //Thread.Sleep(10000); // 等待页面加载和 JavaScript 执行
            //    var allCookies = driver.Manage().Cookies.AllCookies;
            //    //foreach (var cookie in allCookies)
            //    //{
            //    //    Console.WriteLine(cookie.Name + ": " + cookie.Value);
            //    //}
            //    // 接着访问目标 API
            //    driver.Navigate().GoToUrl("https://www.wantgoo.com/stock/calendar/dividend-right-data");
            //    //Thread.Sleep(2000); // 等待数据加载

            //    // 获取页面源代码或进行其他操作
            //    string pageSource = driver.PageSource;
            //    return pageSource;
            //    //Console.WriteLine(pageSource);
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine("An error occurred: " + ex.Message);
            //}
            //finally
            //{
            //    // 关闭浏览器
            //    driver.Quit();
            //}
            //return "enter";
            var apiUrl = "https://kgiweb.moneydj.com/b2brwdCommon/jsondata/63/56/6c/twstockdata.xdjjson?x=afterhours-bulletin0001&revision=2018_07_31_1\r\n";
            var request = new HttpRequestMessage(HttpMethod.Get, apiUrl);

            // Add standard headers
            //request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
            //request.Headers.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
            //request.Headers.AcceptEncoding.Add(new StringWithQualityHeaderValue("deflate"));
            //request.Headers.AcceptEncoding.Add(new StringWithQualityHeaderValue("br"));
            //request.Headers.AcceptLanguage.Add(new StringWithQualityHeaderValue("en-US", 0.9));
            //request.Headers.AcceptLanguage.Add(new StringWithQualityHeaderValue("zh-TW", 0.8));
            //request.Headers.AcceptLanguage.Add(new StringWithQualityHeaderValue("zh", 0.7));
            ////request.Headers.CacheControl = new CacheControlHeaderValue { NoCache = true };
            //request.Headers.Add("Cookie", "BID=CA4AAE15-0D0B-4C0D-BCB1-25DC11E09166; twk_idm_key=0BqUILyNhLa4ONJ4SIVah; client_fingerprint=ee08beec5cce0a966f9667469f9c7f9b5c64f67910e22198439121665f6b7422; cf_clearance=MrS1qSsUnDBygYFC.1VQdLs622.NccDUNydgm2gEKcc-1701328000-0-1-cf5244fa.961f637.98531b1a-0.2.1701328000; TawkConnectionTime=0; twk_uuid_630dbec937898912e9661d8a=%7B%22uuid%22%3A%221.70gsc8OZvmWaovjYZw82SRAW6RMMvXJp6DaiLMMYetvd1iARFoIKfxsIav5s3XgEcXFuS6rcbgEqMlzaVU4Ur84xWMLbruMz2SUJ6Ooz6Yz2yZZdzKVC%22%2C%22version%22%3A3%2C%22domain%22%3A%22wantgoo.com%22%2C%22ts%22%3A1701331318119%7D");
            ////request.Headers.Add("Pragma", "no-cache");
            //request.Headers.Referrer = new Uri("https://www.wantgoo.com/stock/calendar/dividend-right");
            //request.Headers.Add("Sec-Ch-Ua", "\"Google Chrome\";v=\"119\", \"Chromium\";v=\"119\", \"Not?A_Brand\";v=\"24\"");
            //request.Headers.Add("Sec-Ch-Ua-Mobile", "?0");
            //request.Headers.Add("Sec-Ch-Ua-Platform", "\"Windows\"");
            //request.Headers.Add("Sec-Fetch-Dest", "empty");
            //request.Headers.Add("Sec-Fetch-Mode", "cors");
            //request.Headers.Add("Sec-Fetch-Site", "same-origin");
            //request.Headers.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/119.0.0.0 Safari/537.36");
            //request.Headers.Add("X-Client-Signature", "975c97355589017eff3a53331570215d0d1a682011ac8119a351e5e607504cb0");
            //request.Headers.Add("X-Requested-With", "XMLHttpRequest");
            // Send the request
            HttpResponseMessage response = await _httpClient.SendAsync(request);
            //// 发送HTTP GET请求
            //HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                return responseBody;
            }
            else
            {
                //Console.WriteLine("HTTP请求失败，状态码：" + response.StatusCode);
                return "HTTP请求失败，状态码：" + response.StatusCode;
            }
        }
        catch (Exception ex)
        {
            //Console.WriteLine("发生异常：" + ex.Message);
            return "发生异常：" + ex.Message;
        }
    }

    public async Task<string> getFiveLevelsOfStockInformation(string stockCode)
    {
        try
        {
            //String interpolation using $
            var apiUrl = $"https://mis.twse.com.tw/stock/api/getStockInfo.jsp?ex_ch=tse_" +
                $"{stockCode}.tw&json=1&delay=0&_=1701445552510";


            var request = new HttpRequestMessage(HttpMethod.Get, apiUrl);
            HttpResponseMessage response = await _httpClient.SendAsync(request);
            //// 发送HTTP GET请求
            //HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();


                return responseBody;
            }
            else
            {
                //Console.WriteLine("HTTP请求失败，状态码：" + response.StatusCode);
                return "HTTP请求失败，状态码：" + response.StatusCode;
            }
        }
        catch (Exception ex)
        {
            //Console.WriteLine("发生异常：" + ex.Message);
            return "发生异常：" + ex.Message;
        }
    }





    public async Task<string> GetQuoteTimeSalesStore()
    {
        var httpClient = new HttpClient();
        var url = "https://tw.stock.yahoo.com/quote/3231.TW/time-sales"; // 替换成你的 URL

        try
        {
            var response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var htmlContent = await response.Content.ReadAsStringAsync();

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(htmlContent);

            // 以下是如何提取特定元素的例子
            // 例如，提取所有的 h1 标签
            var h1Tags = htmlDoc.DocumentNode.SelectNodes("//h1");
            if (h1Tags != null)
            {
                foreach (var tag in h1Tags)
                {
                    Console.WriteLine(tag.InnerText);
                }
            }

            // 你可以根据需要调整 XPath 查询
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine($"请求错误: {e.Message}");
        }
        return "HTTP请求失败，状态码：";
    }


    public async Task<string> FetchAndParseJson(string? url= "https://tw.stock.yahoo.com/quote/3231.TW/time-sales")
    {
        var httpClient = new HttpClient();
        var response = await httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();
        var htmlContent = await response.Content.ReadAsStringAsync();

        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(htmlContent);

        // Assuming the JSON is stored in a script tag; adjust the XPath as needed
        var scriptTags = htmlDoc.DocumentNode.SelectNodes("//script");
        //foreach (var scriptTag in scriptTags)
        //{
        //    if (scriptTag.InnerText.Contains("QuoteTimeSalesStore"))
        //    {
        //        var json = ExtractJson(scriptTag.InnerText);
        //        //var jObject = JObject.Parse(json);
        //        //var selectedToken = jObject.SelectToken(jsonPath);
        //        //return selectedToken?.ToString(Formatting.Indented);
        //        return "JSON data not found";
        //    }
        //}

        return "JSON data not found";
    }


}