using LogicBuilder.Data;

namespace Contoso.Data.Entities
{

    public class OfficeAssignment : BaseData
    {
        public int InstructorID { get; set; }

        public string Location { get; set; } = "";

        
        public virtual Instructor? Instructor { get; set; }
    }
}
