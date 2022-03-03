using ABIS.LogicBuilder.FlowBuilder.Constants;
using LogicBuilder.Attributes;
using System;
using System.Collections.Generic;


namespace TelerikLogicBuilder.Tests.AttributeSamples
{
    public class InstructorModel
    {
        public InstructorModel(int iD, string lastName, string firstName, string fullName, DateTime hireDate, ICollection<CourseAssignmentModel> courses, OfficeAssignmentModel officeAssignment)
        {
            ID = iD;
            LastName = lastName;
            FirstName = firstName;
            FullName = fullName;
            HireDate = hireDate;
            Courses = courses;
            OfficeAssignment = officeAssignment;
        }

        [VariableEditorControl(VariableControlType.SingleLineTextBox)]
		[AlsoKnownAs("Instructor_ID")]
		public int ID { get; set; }

		[VariableEditorControl(VariableControlType.DomainAutoComplete)]
		[AlsoKnownAs("Instructor_LastName")]
		[Comments("Last Name")]
		public string LastName { get; set; }

		[VariableEditorControl(VariableControlType.SingleLineTextBox)]
		[AlsoKnownAs("Instructor_FirstName")]
		[NameValue(AttributeNames.DEFAULTVALUE, "John")]
		[NameValue(AttributeNames.PROPERTYSOURCE, nameof(FullName))]
		[Domain("A;B;C")]
		public string FirstName { get; set; }

        public string FullName { get; set; }

		[VariableEditorControl(VariableControlType.SingleLineTextBox)]
		[AlsoKnownAs("Instructor_HireDate")]
		public System.DateTime HireDate { get; set; }

        [ListEditorControl(ListControlType.ListForm)]
        [AlsoKnownAs("Instructor_Courses")]
		public ICollection<CourseAssignmentModel> Courses { get; set; }

        [AlsoKnownAs("Instructor_OfficeAssignment")]
		public OfficeAssignmentModel OfficeAssignment { get; set; }

		[Summary("DoSomething")]
		[AlsoKnownAs("Do Something")]
		[FunctionGroup(FunctionGroup.DialogForm)]
		public static string DoSomething()
        {
			return string.Empty;
        }

		public static string DoSomethingElse()
		{
			return string.Empty;
		}
	}
}