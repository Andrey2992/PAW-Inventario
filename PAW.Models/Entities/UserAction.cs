using System;
using System.Collections.Generic;

namespace PAW.Models;

public partial class UserAction
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }
}
