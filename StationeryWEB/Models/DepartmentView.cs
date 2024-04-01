namespace StationeryWEB.Models
{
    public class DepartmentView
    {
        public int Id { get; set; }

        public string? DepName { get; set; }

        public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
