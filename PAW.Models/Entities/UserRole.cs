using System;
using System.Collections.Generic;

namespace PAW.Models;

public partial class UserRole
{
    public int Id { get; set; }

    public int? RoldId { get; set; }

    public int? UserId { get; set; }
}