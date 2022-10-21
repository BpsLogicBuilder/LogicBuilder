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
        private readonly Func<string, int, int, TableFileSource> _getTableFileSource;
        private readonly Func<string, int, short, string, int, int, VisioFileSource> _getVisioFileSource;

        public StructuresFactory(
            Func<string, Page, Shape, List<ResultMessage>, IDiagramResultMessageHelper> getResultMessageHelper,
            Func<string, int, int, TableFileSource> getTableFileSource,
            Func<string, int, short, string, int, int, VisioFileSource> getVisioFileSource)
        {
            _getResultMessageHelper = getResultMessageHelper;
            _getTableFileSource = getTableFileSource;
            _getVisioFileSource = getVisioFileSource;
        }

        public IDiagramResultMessageHelper GetDiagramResultMessageHelper(string sourceFile, Page page, Shape shape, List<ResultMessage> resultMessages)
            => _getResultMessageHelper(sourceFile, page, shape, resultMessages);

        public TableFileSource GetTableFileSource(string sourceFileFullname, int row, int column)
            => _getTableFileSource(sourceFileFullname, row, column);

        public VisioFileSource GetVisioFileSource(string sourceFileFullname, int pageId, short pageIndex, string shapeMasterName, int shapeId, int shapeIndex)
            => _getVisioFileSource(sourceFileFullname, pageId, pageIndex, shapeMasterName, shapeId, shapeIndex);
    }
}
