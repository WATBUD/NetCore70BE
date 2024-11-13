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
    [ApiExplorerSettings(GroupName = "SwaggerGroupGuitarTutorial")]
    public class GuitarTutorialController : ControllerBase
    {
        private readonly RNDatingService _databaseService;

        public GuitarTutorialController(RNDatingService databaseService) // Constructor
        {
            _databaseService = databaseService;
        }

        /// <summary> 
        ///     基礎和弦
        /// </summary>
        /// 
        [HttpGet("chords")]
        public IActionResult GetChords()
        {
            var chords = new List<object>
    {
        new
        {
            Name = "C Major",
            Fingering = "032010",
            Diagram = "   |---|---|---|---|---|---|---|---|---|",
            Description = "C大调和弦：在第1弦到第5弦按照指示的位置按下。"
        },
        new
        {
            Name = "D Minor",
            Fingering = "xx0231",
            Diagram = "   |---|---|---|---|---|---|---|---|---|",
            Description = "D小调和弦：在第1弦到第4弦按照指示的位置按下。"
        },
        // 添加其他和弦信息...
    };

            return new JsonResult(new { Chords = chords });
        }

    }
}
