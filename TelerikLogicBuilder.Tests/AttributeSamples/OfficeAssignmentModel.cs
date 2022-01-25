using LogicBuilder.Attributes;


namespace TelerikLogicBuilder.Tests.AttributeSamples
{
    public class OfficeAssignmentModel
    {
		[VariableEditorControl(VariableControlType.SingleLineTextBox)]
		[AlsoKnownAs("OfficeAssignment_InstructorID")]
		public int InstructorID { get; set; }

		[VariableEditorControl(VariableControlType.SingleLineTextBox)]
		[AlsoKnownAs("OfficeAssignment_Location")]
		public string Location { get; set; }

    }
}