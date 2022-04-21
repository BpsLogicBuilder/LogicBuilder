using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;

namespace ABIS.LogicBuilder.FlowBuilder.RulesGenerator
{
    internal class VisioFileSource
    {
        public VisioFileSource(string sourceFileFullname, int pageId, short pageIndex, string shapeMasterName, int shapeId, int shapeIndex, IContextProvider contextProvider)
        {
            SourceFileFullname = sourceFileFullname;
            FileName = contextProvider.PathHelper.GetFileName(sourceFileFullname);
            PageId = pageId;
            PageIndex = pageIndex;
            ShapeMasterName = shapeMasterName;
            ShapeId = shapeId;
            ShapeIndex = shapeIndex;
        }

        internal string SourceFileFullname { get; }
        internal string FileName { get; }
        internal int PageId { get; }
        internal short PageIndex { get; }
        internal string ShapeMasterName { get; }
        internal int ShapeId { get; }
        internal int ShapeIndex { get; }
    }
}
