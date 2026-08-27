using LogicBuilder.Data;

namespace Contoso.Data.Entities
{
    public class Enrollment : BaseData
    {
        public int EnrollmentID { get; set; }
        public int CourseID { get; set; }
        public int StudentID { get; set; }
        public Grade? Grade { get; set; }
        public Course? Course { get; set; }
        public Student? Student { get; set; }
    }
}
