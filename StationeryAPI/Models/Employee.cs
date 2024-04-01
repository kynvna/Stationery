using System;
using System.Collections.Generic;

namespace StationeryAPI.Models;

public partial class Employee
{
    public int IdEmp { get; set; }

    public string? EmpName { get; set; }

    public int? DepId { get; set; }

    public virtual Department? Dep { get; set; }
}
