using System;
using System.Collections.Generic;

namespace StationeryAPI.Models;

public partial class Department
{
    public int Id { get; set; }

    public string? DepName { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
