using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace NetCore60.DTO
{
    public class ScreenDTO
    {
        public int ScreenType { get; set; }
        public int ScreenID { get; set; }
        public string ScreenParameter { get; set; }
        public List<IFormFile> Images { get; set; }
        public string ScreenName { get; set; }
        public string DeleteFiles { get; set; }


    }
}