using LogicBuilder.Attributes;
using LogicBuilder.Domain;


namespace Contoso.Domain.Entities
{
    public class EnrollmentModel : BaseModel
    {
		[VariableEditorControl(VariableControlType.SingleLineTextBox)]
		[AlsoKnownAs("Enrollment_EnrollmentID")]
		public int EnrollmentID { get; set; }

		[VariableEditorControl(VariableControlType.SingleLineTextBox)]
		[AlsoKnownAs("Enrollment_CourseID")]
		public int CourseID { get; set; }

		[VariableEditorControl(VariableControlType.SingleLineTextBox)]
		[AlsoKnownAs("Enrollment_StudentID")]
		public int StudentID { get; set; }

		[AlsoKnownAs("Enrollment_Grade")]
		public Grade? Grade { get; set; }

		[AlsoKnownAs("Enrollment_GradeLetter")]
		public string? GradeLetter { get; set; }

		[AlsoKnownAs("Enrollment_CourseTitle")]
		public string? CourseTitle { get; set; }

		[AlsoKnownAs("Enrollment_StudentName")]
		public string? StudentName { get; set; }
    }
}