﻿using Microsoft.AspNetCore.Mvc;
using NetCore60.Models;
using NetCore60.Services;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json.Nodes;

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
        public async Task<IActionResult> exDividendNoticeFormAsync(int limitDays=5,bool isCashDividend=false)
        {
            try
            {
                var response = await _getStocksService.getExDividendNoticeForm(limitDays, isCashDividend);

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



        /// <summary> 
        ///     取得三大法人買賣超日報
        /// </summary>
        /// 
        [HttpGet("getThreeMajorInstitutionalInvestors")]
        public async Task<IActionResult> getThreeMajorInstitutionalInvestors()
        {
            try
            {

                var responseJsonString = await _getStocksService.getThreeMajorInstitutionalInvestors();

                if (responseJsonString != null)
                {
                    // Serialize the data object to JSON
                    //string jsonString = JsonSerializer.Serialize(response);
                    // Create a ContentResult and set the Content-Type header
                    // Create a ContentResult and set the Content-Type header
                    // Create a ContentResult and set the Content-Type header
                    var contentResult = new ContentResult
                    {
                        Content = responseJsonString,
                        ContentType = "application/json",
                        StatusCode = 200 // HTTP status code for OK
                    };

                    return contentResult;
                }
                else
                {
                    return Content("未能获取响应数据。");
                }

            }
            catch (Exception ex)
            {
                //return Content($"发生异常：{ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

        }


        /// <summary> 
        ///     取得最後一次開盤日期
        /// </summary>
        /// 
        [HttpGet("getTheLatestOpeningDate")]
        public async Task<IActionResult> GetTheLatestOpeningDate()
        {
            try
            {
                var response = await _getStocksService.getTheLatestOpeningDate();

                if (response != null)
                {
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
        ///     取得市場開休市日期
        /// </summary>
        /// 
        [HttpGet("getStockMarketOpeningAndClosingDates")]
        public async Task<IActionResult> GetStockMarketOpeningAndClosingDates(bool requestAllData=false)
        {
            try
            {
                var response = await _getStocksService.getStockMarketOpeningAndClosingDates(requestAllData);

                if (response != null)
                {
                    // 将 List<string> 转换为 JSON 字符串
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
