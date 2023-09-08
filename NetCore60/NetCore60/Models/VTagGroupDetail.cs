using System;
using System.Collections.Generic;

namespace NetCore60.Models;

public partial class VTagGroupDetail
{
    public int? TagId { get; set; }

    public string? TagName { get; set; }

    public int TagGroupId { get; set; }

    public string? TagGroupName { get; set; }
}
