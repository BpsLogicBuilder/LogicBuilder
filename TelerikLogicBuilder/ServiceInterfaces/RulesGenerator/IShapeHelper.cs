using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.RulesGenerator;
using Microsoft.Office.Interop.Visio;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator
{
    internal interface IShapeHelper
    {
		/// <summary>
		/// Returns the index number of a choice connector with duplicate text
		/// </summary>
		/// <param name="fromShape"></param>
		/// <returns></returns>
		short CheckForDuplicateMultipleChoices(Shape fromShape);

		/// <summary>
		/// Returns a count of root level function elements configured as Dialog Functions in the Action or Dialog Shape
		/// </summary>
		/// <param name="shape"></param>
		/// <returns></returns>
		int CountDialogFunctions(Shape shape);

		/// <summary>
		/// Returns the total number of root level function elements in the Action or Dialog Shape
		/// </summary>
		/// <param name="shape"></param>
		/// <returns></returns>
		int CountFunctions(Shape shape);

		/// <summary>
		/// Given a shape, returns a count of connectors pointing towards the shape.
		/// </summary>
		/// <param name="toShape"></param>
		/// <returns></returns>
		short CountIncomingConnectors(Shape toShape);

		/// <summary>
		/// Returns the number of invalid connectors (options pointing away) for a Dialog Shape.
		/// </summary>
		/// <param name="fromShape"></param>
		/// <returns></returns>
		short CountInvalidMultipleChoiceConnectors(Shape fromShape);

		/// <summary>
		/// Returns the number of blank connectors pointing away from a shape.
		/// </summary>
		/// <param name="fromShape"></param>
		/// <returns></returns>
		short CountOutgoingBlankConnectors(Shape fromShape);

		/// <summary>
		/// Returns the number of connectors pointing away from a shape.
		/// </summary>
		/// <param name="fromShape"></param>
		/// <returns></returns>
		short CountOutgoingConnectors(Shape fromShape);

		/// <summary>
		/// Returns the number of non application (NOT application specific) connectors pointing away from a shape.
		/// </summary>
		/// <param name="fromShape"></param>
		/// <returns></returns>
		short CountOutgoingNonApplicationConnectors(Shape fromShape);

		/// <summary>
		/// Returns list of relevant applications given the connector.
		/// </summary>
		/// <param name="connector"></param>
		/// <param name="fromShapeBag"></param>
		/// <returns></returns>
		IList<string> GetApplicationList(Shape connector, ShapeBag fromShapeBag);

		/// <summary>
		/// Returns the coreesponing application name in the format $"App{appNumber.ToString("00", CultureInfo.CurrentCulture)}"
		/// where appNumber is an integer from 1 to 10.
		/// </summary>
		/// <param name="connector"></param>
		/// <returns></returns>
		string GetApplicationName(Shape connector);

		/// <summary>
		/// Returns the coreesponing application name in the format $"App{appNumber.ToString("00", CultureInfo.CurrentCulture)}"
		/// where appNumber is an integer from 1 to 10.
		/// </summary>
		/// <param name="connectorMasterNameU"></param>
		/// <returns></returns>
		string GetApplicationName(string connectorMasterNameU);

		/// <summary>
		/// Returns the shape connected to the starting end of the connector.
		/// </summary>
		/// <param name="connector"></param>
		/// <returns></returns>
		Shape GetFromShape(Shape connector);

		/// <summary>
		/// Returns the list of ConnectorData for the dialog shape.
		/// </summary>
		/// <param name="fromShape"></param>
		/// <returns></returns>
		IList<ConnectorData> GetMultipleChoiceConnectorData(Shape fromShape);

        /// <summary>
        /// Returns the next unused connector index for a Dialog, Condition or Decision Object.
        /// </summary>
        /// <param name="fromShape"></param>
        /// <param name="connectorCategory"></param>
        /// <returns>Returns the next available index if unused connectors exist. Returns null if all connectors are used.</returns>
        short? GetNextUnusedIndex(Shape fromShape, ConnectorCategory connectorCategory);

        /// <summary>
        /// Returns a list of applicable applications for the "other applications" connector when
        /// the arrow or the starting is connected to a Merge Object.
        /// The applicable applications are all application specific connectors NOT attached to the Merge Object.
        /// </summary>
        /// <param name="connector"></param>
        /// <returns></returns>
        IList<string> GetOtherApplications(Shape connector);

		/// <summary>
		/// Returns list of relevant applications to be associated with the OtherConnectObject.
		/// If the from Shape is NOT a merge shape then it collects the other applications from the fromShapeBag.
		/// Otherwise it collects the other applications from the merge object itself.
		/// </summary>
		/// <param name="connector"></param>
		/// <param name="fromShapeBag"></param>
		/// <returns></returns>
		IList<string> GetOtherApplicationsList(Shape connector, ShapeBag fromShapeBag);

		/// <summary>
		/// Returns the blank connector leaving the shape.
		/// </summary>
		/// <param name="fromShape"></param>
		/// <returns></returns>
		Shape GetOutgoingBlankConnector(Shape fromShape);

		/// <summary>
		/// Returns all the blank connectors leaving the shape - applicable to ShapeCollections.AllowedApplicationFlowShapes.
		/// </summary>
		/// <param name="fromShape"></param>
		/// <returns></returns>
		IList<Shape> GetOutgoingBlankConnectors(Shape fromShape);

		/// <summary>
		/// Returns the "NO" connector pointing away from a decision or condition shape.
		/// </summary>
		/// <param name="fromShape"></param>
		/// <returns></returns>
		Shape? GetOutgoingNoConnector(Shape fromShape);

		/// <summary>
		/// Returns the "YES" connector pointing away from a decision or condition shape.
		/// </summary>
		/// <param name="fromShape"></param>
		/// <returns></returns>
		Shape? GetOutgoingYesConnector(Shape fromShape);

		/// <summary>
		/// Returns the shape connected to the arrow.
		/// </summary>
		/// <param name="connector"></param>
		/// <returns></returns>
		Shape GetToShape(Shape connector);

		/// <summary>
		/// Gets a list of configured applications unaccounted for by a merge object (missing application connectors).
		/// </summary>
		/// <param name="shape"></param>
		/// <param name="isSplitting"></param>
		/// <returns></returns>
		IList<string> GetUnusedApplications(Shape shape, bool isSplitting);

		/// <summary>
		/// Returns true if application specific connectors are attached - otherwise false
		/// </summary>
		/// <param name="shape"></param>
		/// <returns></returns>
		bool HasAllApplicationConnectors(Shape shape);

		/// <summary>
		/// Returns true if the connector's starting end is connected to a shape otherwise false.
		/// </summary>
		/// <param name="connector"></param>
		/// <returns></returns>
		bool HasFromShape(Shape connector);

        /// <summary>
        /// Returns false if application specific connectors are attached or if there are no connectors attached - otherwise true
        /// </summary>
        /// <param name="shape"></param>
        /// <returns></returns>
        bool HasAllNonApplicationConnectors(Shape shape);
	}
}
