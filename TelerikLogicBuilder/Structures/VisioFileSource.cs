using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;

namespace ABIS.LogicBuilder.FlowBuilder.Structures
{
    internal class VisioFileSource
    {
        public VisioFileSource(IPathHelper pathHelper, string sourceFileFullname, int pageId, short pageIndex, string shapeMasterName, int shapeId, int shapeIndex)
        {
            SourceFileFullname = sourceFileFullname;
            FileName = pathHelper.GetFileName(sourceFileFullname);
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
