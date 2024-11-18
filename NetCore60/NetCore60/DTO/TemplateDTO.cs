using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace NetCore60.DTO
{
    public class TemplateDTO
    {
        public string TemplateID { get; set; }
        public string TemplateName { get; set; }
        public DateTime CreateTime { get; set; }
        public int RefreshTime { get; set; }
        public int Sort { get; set; }
    }
}