using LogicBuilder.Data;

namespace Contoso.Data.Entities
{
    public class CourseAssignment : BaseData
    {
        public int InstructorID { get; set; }
        public int CourseID { get; set; }
        public Instructor? Instructor { get; set; }
        public Course? Course { get; set; }
    }
}
