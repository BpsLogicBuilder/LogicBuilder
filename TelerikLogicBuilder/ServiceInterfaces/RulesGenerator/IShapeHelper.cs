using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.RulesGenerator;
using Microsoft.Office.Interop.Visio;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator
{
    internal interface IShapeHelper
    {
		short CheckForDuplicateMultipleChoices(Shape fromShape);
		int CountDialogFunctions(Shape fromShape);
		int CountFunctions(Shape fromShape);
		short CountIncomingConnectors(Shape fromShape);
		short CountInvalidMultipleChoiceConnectors(Shape fromShape);
		short CountOutgoingBlankConnectors(Shape fromShape);
		short CountOutgoingConnectors(Shape fromShape);
		short CountOutgoingNonApplicationConnectors(Shape fromShape);
		Shape GetFromShape(Shape connector);
		IList<string> GetApplicationList(Shape connector, ShapeBag fromShapeBag);
		IList<ConnectorData> GetMultipleChoiceConnectorData(Shape fromShape);
		Shape GetNextApplicationSpecificConnector(Shape lastShape, string connectorMasterNameU);
		IList<string> GetOtherApplicationsList(Shape connector, ShapeBag fromShapeBag);
		Shape GetOutgoingBlankConnector(Shape fromShape);
		Shape GetOutgoingNoConnector(Shape fromShape);
		Shape GetOutgoingYesConnector(Shape fromShape);
		Shape GetToShape(Shape connector);
		bool HasAllApplicationConnectors(Shape shape);
		bool HasAllNonApplicationConnectors(Shape shape);
	}
}
