using System;
using System.Collections.Generic;

namespace NetCore60.Models;

public partial class RecordLogTable
{
    public int Id { get; set; }

    public string DataText { get; set; } = null!;
}
