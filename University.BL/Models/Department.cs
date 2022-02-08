using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace University.BL.Models
{
    [Table("Department", Schema = "dbo")]
    public class Department
    {
        public int DepartmentID { get; set; }
        public string Name { get; set; }
        public double Budget { get; set; }
        public DateTime StartDate { get; set; }

        [ForeignKey("Instructor")]
        public int InstructorID { get; set; }
        public Instructor Instructor { get; set; }
    }
}