using System;
using System.Collections.Generic;

namespace backend.Models;

public partial class Event
{
    public int Id { get; set; }

    public DateOnly? Date { get; set; }

    public string? Location { get; set; }

    public string? Description { get; set; }

    public decimal? Price { get; set; }
}
