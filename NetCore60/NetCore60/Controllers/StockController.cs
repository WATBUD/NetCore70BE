using Microsoft.AspNetCore.Mvc;
using NetCore60.Models;
using NetCore60.Services;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "G_Stocks")]
    public class StockController : ControllerBase
    {
        private readonly RNDatingService _databaseService;

        private readonly GetStocksService _getStocksService = new GetStocksService();



        public StockController(RNDatingService databaseService) // Constructor
        {
            _databaseService = databaseService;
        }

        //[HttpGet("FetchAndParseJson")]
        //public async Task<IActionResult> FetchAndParseJson()
        //{
        //    try
        //    {

        //        var getStocksService = new GetStocksService();
        //        //string response;
        //        var mylocalip = await getStocksService.FetchAndParseJson();
        //        return Content($"User IP Address:");
        //        //if (response != null)
        //        //{
        //        //    //string combinedData = $"User IP Address: {ipAddress}\n响应数据：\n{response}";
        //        //    return Content(response);
        //        //}
        //        //else
        //        //{
        //        //    return Content("未能获取响应数据。");
        //        //}

        //    }
        //    catch (Exception ex)
        //    {
        //        return Content($"发生异常：{ex.Message}");
        //    }

        //}

        /// <summary> 
        ///     除權息預告表
        /// </summary>
        /// 
        [HttpGet("exDividendNotice")]
        public async Task<IActionResult> exDividendNoticeFormAsync(int limitDays)
        {
            try
            {
                var response = await _getStocksService.getExDividendNoticeForm(limitDays);

                if (response != null)
                {
                    //string combinedData = $"User IP Address: {ipAddress}\n响应数据：\n{response}";
                    return Content(response);
                }
                else
                {
                    return Content("未能获取响应数据。");
                }

            }
            catch (Exception ex)
            {
                return Content($"发生异常：{ex.Message}");
            }

        }

        /// <summary> 
        ///     取得股票五檔
        /// </summary>
        /// 
        [HttpGet("getFiveLevelsOfStockInformation")]
        public async Task<IActionResult> getFiveLevelsOfStockInformationAync(string stockCode)
        {
            try
            {

                var response = await _getStocksService.getFiveLevelsOfStockInformation(stockCode);

                if (response != null)
                {
                    //string combinedData = $"User IP Address: {ipAddress}\n响应数据：\n{response}";
                    return Content(response);
                }
                else
                {
                    return Content("未能获取响应数据。");
                }

            }
            catch (Exception ex)
            {
                return Content($"发生异常：{ex.Message}");
            }

        }




    }
}
