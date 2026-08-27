using LogicBuilder.Attributes;
using LogicBuilder.Domain;
using System.Collections.Generic;


namespace Contoso.Domain.Entities
{
    public class StudentModel : BaseModel
    {
		[VariableEditorControl(VariableControlType.SingleLineTextBox)]
		[AlsoKnownAs("Student_ID")]
		public int ID { get; set; }

		[VariableEditorControl(VariableControlType.SingleLineTextBox)]
		[AlsoKnownAs("Student_LastName")]
		public string LastName { get; set; } = "";

        [VariableEditorControl(VariableControlType.SingleLineTextBox)]
		[AlsoKnownAs("Student_FirstName")]
		public string FirstName { get; set; } = "";

        [AlsoKnownAs("Student_FullName")]
		public string FullName { get; set; } = "";

        [VariableEditorControl(VariableControlType.SingleLineTextBox)]
		[AlsoKnownAs("Student_EnrollmentDate")]
		public System.DateTime EnrollmentDate { get; set; }

        public string EnrollmentDateString { get; set; } = "";

		[ListEditorControl(ListControlType.HashSetForm)]
		[AlsoKnownAs("Student_Enrollments")]
		public ICollection<EnrollmentModel> Enrollments { get; set; } = [];
    }
}