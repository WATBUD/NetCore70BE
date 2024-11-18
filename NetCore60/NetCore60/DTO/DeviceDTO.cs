using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace NetCore60.DTO
{
    public class DeviceDTO
    {
        public string PairCode { get; set; }
        public string DeviceName { get; set; }
        public int ScreenID { get; set; }
        public int PlaceID { get; set; }
        public int Status { get; set; }
    }
}