using Newtonsoft.Json;

namespace StationeryWEB.Models
{
    public class DemoCoreViewModel
    {
        [JsonProperty("departments")]
        public List<Department> Departments { get; set; }

        [JsonProperty("employees")]
        public List<Employee> Employees { get; set; }
    }

    public class Department
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("depName")]
        public string DepName { get; set; }

        [JsonProperty("Employees")]
        public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    }

    public class Employee
    {
        [JsonProperty("idEmp")]
        public int IdEmp { get; set; }

        [JsonProperty("empName")]
        public string EmpName { get; set; }

        [JsonProperty("depId")]
        public int DepId { get; set; }

        [JsonProperty("Dep")]
        public virtual Department? Dep { get; set; }

    }


}

