namespace ABIS.LogicBuilder.FlowBuilder.Structures
{
    internal class ShapeIdInfo
    {
        public ShapeIdInfo(string shapeMasterNameU, string shapeMasterName, short pageIndex, int shapeIndex, int pageId, int shapeId)
        {
            ShapeMasterNameU = shapeMasterNameU;
            ShapeMasterName = shapeMasterName;
            PageIndex = pageIndex;
            ShapeIndex = shapeIndex;
            PageId = pageId;
            ShapeId = shapeId;
        }

        internal string ShapeMasterNameU { get; }
        internal string ShapeMasterName { get; }
        internal short PageIndex { get; }
        internal int ShapeIndex { get; }
        internal int PageId { get; }
        internal int ShapeId { get; }
    }
}
