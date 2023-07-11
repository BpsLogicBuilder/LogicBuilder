using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditXml;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditXml.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows.Forms;
using System.Xml;

namespace TelerikLogicBuilder.FormsPreviewer.Commands.Xml
{
    internal class EditFunctionsFormXmlCommand : ClickCommandBase
    {
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly RadForm1 radForm;

        public EditFunctionsFormXmlCommand(IXmlDocumentHelpers xmlDocumentHelpers, RadForm1 radForm)
        {
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.radForm = radForm;
        }

        public override void Execute()
        {
            IEditXmlFormFactory disposableManager = ABIS.LogicBuilder.FlowBuilder.Program.ServiceProvider.GetRequiredService<IEditXmlFormFactory>();
            XmlDocument xmlDococument = new();
            xmlDococument.LoadXml(xml);
            using IEditFunctionsFormXml editXmlForm = disposableManager.GetEditFunctionsFormXml
            (
                _xmlDocumentHelpers.GetXmlString(xmlDococument)
            );
            editXmlForm.ShowDialog(radForm);
            if (editXmlForm.DialogResult != DialogResult.OK)
                return;
        }

        readonly string xml = @"<functions>
									<function name=""SetupNavigationMenu"" visibleText=""SetupNavigationMenu: NavigationBarParameters: navBar"">
										<genericArguments />
										<parameters>
											<objectParameter name=""navBar"">
												<constructor name=""NavigationBarParameters"" visibleText=""NavigationBarParameters: brandText=Contoso;currentModule=about;GenericListOfNavigationMenuItemParameters: MenuItems"">
													<genericArguments />
													<parameters>
														<literalParameter name=""brandText"">Contoso</literalParameter>
														<literalParameter name=""currentModule"">about</literalParameter>
														<objectListParameter name=""MenuItems"">
															<objectList objectType=""Contoso.Forms.Parameters.Navigation.NavigationMenuItemParameters"" listType=""GenericList"" visibleText=""MenuItems: Count(6)"">
																<object>
																	<constructor name=""NavigationMenuItemParameters"" visibleText=""NavigationMenuItemParameters: initialModule=home;text=Home;icon=Home"">
																		<genericArguments />
																		<parameters>
																			<literalParameter name=""initialModule"">home</literalParameter>
																			<literalParameter name=""text"">Home</literalParameter>
																			<literalParameter name=""icon"">Home</literalParameter>
																		</parameters>
																	</constructor>
																</object>
																<object>
																	<constructor name=""NavigationMenuItemParameters"" visibleText=""NavigationMenuItemParameters: initialModule=about;text=About;icon=University"">
																		<genericArguments />
																		<parameters>
																			<literalParameter name=""initialModule"">about</literalParameter>
																			<literalParameter name=""text"">About</literalParameter>
																			<literalParameter name=""icon"">University</literalParameter>
																		</parameters>
																	</constructor>
																</object>
																<object>
																	<constructor name=""NavigationMenuItemParameters"" visibleText=""NavigationMenuItemParameters: initialModule=students;text=Students;icon=Users"">
																		<genericArguments />
																		<parameters>
																			<literalParameter name=""initialModule"">students</literalParameter>
																			<literalParameter name=""text"">Students</literalParameter>
																			<literalParameter name=""icon"">Users</literalParameter>
																		</parameters>
																	</constructor>
																</object>
																<object>
																	<constructor name=""NavigationMenuItemParameters"" visibleText=""NavigationMenuItemParameters: initialModule=courses;text=Courses;icon=BookOpen"">
																		<genericArguments />
																		<parameters>
																			<literalParameter name=""initialModule"">courses</literalParameter>
																			<literalParameter name=""text"">Courses</literalParameter>
																			<literalParameter name=""icon"">BookOpen</literalParameter>
																		</parameters>
																	</constructor>
																</object>
																<object>
																	<constructor name=""NavigationMenuItemParameters"" visibleText=""NavigationMenuItemParameters: initialModule=departments;text=Departments;icon=Building"">
																		<genericArguments />
																		<parameters>
																			<literalParameter name=""initialModule"">departments</literalParameter>
																			<literalParameter name=""text"">Departments</literalParameter>
																			<literalParameter name=""icon"">Building</literalParameter>
																		</parameters>
																	</constructor>
																</object>
																<object>
																	<constructor name=""NavigationMenuItemParameters"" visibleText=""NavigationMenuItemParameters: initialModule=instructors;text=Instructors;icon=ChalkboardTeacher"">
																		<genericArguments />
																		<parameters>
																			<literalParameter name=""initialModule"">instructors</literalParameter>
																			<literalParameter name=""text"">Instructors</literalParameter>
																			<literalParameter name=""icon"">ChalkboardTeacher</literalParameter>
																		</parameters>
																	</constructor>
																</object>
															</objectList>
														</objectListParameter>
													</parameters>
												</constructor>
											</objectParameter>
										</parameters>
									</function>
								</functions>";
    }
}
