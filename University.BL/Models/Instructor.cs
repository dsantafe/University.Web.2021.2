using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace University.BL.Models
{
    [Table("Instructor", Schema = "dbo")]
    public class Instructor
    {
        [Key]
        public int ID { get; set; }
        public string LastName { get; set; }
        public string FirstMidName { get; set; }
        public DateTime HireDate { get; set; }

        //dependencies
        public virtual OfficeAssignment OfficeAssignment { get; set; }
        public virtual ICollection<Department> Departments { get; set; }
        public virtual ICollection<CourseInstructor> CourseInstructors { get; set; }
    }
}