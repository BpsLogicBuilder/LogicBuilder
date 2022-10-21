using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using Microsoft.Office.Interop.Visio;
using System;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.StructuresFactories
{
    internal class StructuresFactory : IStructuresFactory
    {
        private readonly Func<string, Page, Shape, List<ResultMessage>, IDiagramResultMessageHelper> _getResultMessageHelper;

        public StructuresFactory(Func<string, Page, Shape, List<ResultMessage>, IDiagramResultMessageHelper> getResultMessageHelper)
        {
            _getResultMessageHelper = getResultMessageHelper;
        }

        public IDiagramResultMessageHelper GetDiagramResultMessageHelper(string sourceFile, Page page, Shape shape, List<ResultMessage> resultMessages)
            => _getResultMessageHelper(sourceFile, page, shape, resultMessages);
    }
}
