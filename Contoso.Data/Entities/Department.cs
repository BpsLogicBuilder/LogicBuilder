using LogicBuilder.Data;
using System;
using System.Collections.Generic;

namespace Contoso.Data.Entities
{
    public class Department : BaseData
    {
        public Department()
        {
            Courses = new HashSet<Course>();
        }

        public int DepartmentID { get; set; }

        public string Name { get; set; } = "";

        public decimal Budget { get; set; }

        public DateTime StartDate { get; set; }

        public int? InstructorID { get; set; }

        public byte[] RowVersion { get; set; } = [];

        public Instructor? Administrator { get; set; }
        public ICollection<Course> Courses { get; set; }
    }
}
