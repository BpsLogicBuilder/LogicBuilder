using LogicBuilder.Attributes;
using LogicBuilder.Domain;


namespace Contoso.Domain.Entities
{
    public class CourseAssignmentModel : BaseModel
    {
		[VariableEditorControl(VariableControlType.SingleLineTextBox)]
		[AlsoKnownAs("CourseAssignment_InstructorID")]
		public int InstructorID { get; set; }

		[VariableEditorControl(VariableControlType.SingleLineTextBox)]
		[AlsoKnownAs("CourseAssignment_CourseID")]
		public int CourseID { get; set; }

		[AlsoKnownAs("CourseAssignment_CourseTitle")]
		public string CourseTitle { get; set; } = "";

        [AlsoKnownAs("CourseAssignment_CourseNumberAndTitle")]
		public string CourseNumberAndTitle { get; set; } = "";

        [AlsoKnownAs("CourseAssignment_Department")]
		public string? Department { get; set; }
    }
}