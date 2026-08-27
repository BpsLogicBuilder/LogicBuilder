using LogicBuilder.Data;
using System;
using System.Collections.Generic;

namespace Contoso.Data.Entities
{
    public class Instructor : BaseData
    {
        public Instructor()
        {
            Courses = new HashSet<CourseAssignment>();
        }

        public int ID { get; set; }

        public string LastName { get; set; } = "";

        public string FirstName { get; set; } = "";

        public DateTime HireDate { get; set; }

        public ICollection<CourseAssignment> Courses { get; set; }
        public OfficeAssignment? OfficeAssignment { get; set; }
    }
}
