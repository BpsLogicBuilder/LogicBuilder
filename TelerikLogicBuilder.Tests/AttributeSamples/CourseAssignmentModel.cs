using LogicBuilder.Attributes;


namespace TelerikLogicBuilder.Tests.AttributeSamples
{
    public class CourseAssignmentModel
    {
        public CourseAssignmentModel(int instructorID, int courseID, string courseTitle, string courseNumberAndTitle, string department)
        {
            InstructorID = instructorID;
            CourseID = courseID;
            CourseTitle = courseTitle;
            CourseNumberAndTitle = courseNumberAndTitle;
            Department = department;
        }

        [VariableEditorControl(VariableControlType.SingleLineTextBox)]
		[AlsoKnownAs("CourseAssignment_InstructorID")]
		public int InstructorID { get; set; }

		[VariableEditorControl(VariableControlType.SingleLineTextBox)]
		[AlsoKnownAs("CourseAssignment_CourseID")]
		public int CourseID { get; set; }

        public string CourseTitle { get; set; }

        public string CourseNumberAndTitle { get; set; }

        public string Department { get; set; }
    }
}