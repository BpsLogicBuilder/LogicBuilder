using System;

namespace ABIS.LogicBuilder.FlowBuilder.RulesGenerator.Factories
{
    internal class VisioFileSourceFactory : IVisioFileSourceFactory
    {
        private readonly Func<string, int, short, string, int, int, VisioFileSource> _getVisioFileSource;

        public VisioFileSourceFactory(Func<string, int, short, string, int, int, VisioFileSource> getVisioFileSource)
        {
            _getVisioFileSource = getVisioFileSource;
        }

        public VisioFileSource GetVisioFileSource(string sourceFileFullname, int pageId, short pageIndex, string shapeMasterName, int shapeId, int shapeIndex)
            => _getVisioFileSource(sourceFileFullname, pageId, pageIndex, shapeMasterName, shapeId, shapeIndex);
    }
}
