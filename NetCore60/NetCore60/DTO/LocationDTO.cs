using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NetCore60.DTO
{
    public partial class LocationDTO
    {
        public int? ParentId { get; set; }
        [Required(ErrorMessage = "LayerName is required.")]
        public string LayerName { get; set; }
        public int? Type { get; set; }
    }
}
