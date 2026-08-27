using LogicBuilder.Attributes;
using LogicBuilder.Domain;
using System.Collections.Generic;


namespace Contoso.Domain.Entities
{
    public class CourseModel : BaseModel
    {
		[VariableEditorControl(VariableControlType.SingleLineTextBox)]
		[AlsoKnownAs("Course_CourseID")]
		public int CourseID { get; set; }

		public string CourseIDString { get; set; } = "";

		[VariableEditorControl(VariableControlType.SingleLineTextBox)]
		[AlsoKnownAs("Course_Title")]
		public string Title { get; set; } = "";

		[VariableEditorControl(VariableControlType.SingleLineTextBox)]
		[AlsoKnownAs("Course_Credits")]
		public int Credits { get; set; }

		[VariableEditorControl(VariableControlType.SingleLineTextBox)]
		[AlsoKnownAs("Course_DepartmentID")]
		public int? DepartmentID { get; set; }

		[AlsoKnownAs("Course_DepartmentName")]
		public string? DepartmentName { get; set; }

		[ListEditorControl(ListControlType.HashSetForm)]
		[AlsoKnownAs("Course_Assignments")]
		public ICollection<CourseAssignmentModel> Assignments { get; set; } = [];
    }
}