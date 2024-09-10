using System;
using System.Collections.Generic;

namespace NetCore60.Models;

public partial class UserStock
{
    public int Index { get; set; }

    public string StockId { get; set; } = null!;

    public string UserId { get; set; } = null!;

    public string? Note { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool? IsBlocked { get; set; }
}
