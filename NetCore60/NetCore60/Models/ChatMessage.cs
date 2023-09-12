using System;
using System.Collections.Generic;

namespace NetCore60.Models;

public partial class ChatMessage
{
    public int MessageId { get; set; }

    public string? Content { get; set; }

    public int SenderId { get; set; }

    public int ReceiverId { get; set; }

    public DateTime? TimeStamp { get; set; }
}
