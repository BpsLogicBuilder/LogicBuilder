using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using Microsoft.Office.Interop.Visio;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.StructuresFactories
{
    internal interface IStructuresFactory
    {
        IDiagramResultMessageHelper GetDiagramResultMessageHelper(string sourceFile,
            Page page,
            Shape shape,
            List<ResultMessage> resultMessages);

        TableFileSource GetTableFileSource(string sourceFileFullname, int row, int column);

        VisioFileSource GetVisioFileSource(string sourceFileFullname, int pageId, short pageIndex, string shapeMasterName, int shapeId, int shapeIndex);
    }
}
