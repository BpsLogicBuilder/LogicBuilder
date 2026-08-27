using LogicBuilder.Data;
using System.Collections.Generic;

namespace Contoso.Data.Entities
{
    public class Course : BaseData
    {
        public Course()
        {
            Enrollments = new HashSet<Enrollment>();
            Assignments = new HashSet<CourseAssignment>();
        }

        public int CourseID { get; set; }

        public string Title { get; set; } = "";

        public int Credits { get; set; }

        public int DepartmentID { get; set; }
        public Department? Department { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; }
        public ICollection<CourseAssignment> Assignments { get; set; }
    }
}
