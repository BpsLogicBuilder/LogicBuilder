using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditXml.Factories
{
    internal class EditXmlFormFactory : IEditXmlFormFactory
    {
        private IDisposable? _scopedService;
        private readonly Func<string, Type, IEditConstructorFormXml> _getEditConstructorFormXml;

        public EditXmlFormFactory(
            Func<string, Type, IEditConstructorFormXml> getEditConstructorFormXml)
        {
            _getEditConstructorFormXml = getEditConstructorFormXml;
        }

        public IEditConstructorFormXml GetEditConstructorFormXml(string xml, Type assignedTo)
        {
            _scopedService = _getEditConstructorFormXml(xml, assignedTo);
            return (IEditConstructorFormXml)_scopedService;
        }

        public void Dispose()
        {
            _scopedService?.Dispose();
        }
    }
}
