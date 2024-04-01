using System;
using System.Collections.Generic;

namespace StationeryAPI.ShoppingModels;

public partial class TblAdEmpAccount
{
    public string UserId { get; set; } = null!;

    public string Username { get; set; } = null!;

    public string? Fullname { get; set; }

    public string Passw { get; set; } = null!;

    public int Role { get; set; }

    public bool? Active { get; set; }
}
