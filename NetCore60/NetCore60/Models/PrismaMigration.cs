using System;
using System.Collections.Generic;

namespace NetCore60.Models;

public partial class PrismaMigration
{
    public string Id { get; set; } = null!;

    public string Checksum { get; set; } = null!;

    public DateTime? FinishedAt { get; set; }

    public string MigrationName { get; set; } = null!;

    public string? Logs { get; set; }

    public DateTime? RolledBackAt { get; set; }

    public DateTime StartedAt { get; set; }

    public uint AppliedStepsCount { get; set; }
}
