using LogicBuilder.Attributes;
using LogicBuilder.Domain;


namespace Contoso.Domain.Entities
{
    public class OfficeAssignmentModel : BaseModel
    {
		[VariableEditorControl(VariableControlType.SingleLineTextBox)]
		[AlsoKnownAs("OfficeAssignment_InstructorID")]
		public int InstructorID { get; set; }

		[VariableEditorControl(VariableControlType.SingleLineTextBox)]
		[AlsoKnownAs("OfficeAssignment_Location")]
		public string Location { get; set; } = "";
    }
}