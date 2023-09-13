using System;
using System.Collections.Generic;

namespace NetCore60.Models;

public partial class RequestLog
{
    public int Id { get; set; }

    public string Path { get; set; } = null!;

    public string Method { get; set; } = null!;

    public string ClientIp { get; set; } = null!;

    public DateTime CreatedAt { get; set; }
}
