using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditXml.Factories
{
    internal class EditXmlFormFactory : IEditXmlFormFactory
    {
        private IDisposable? _scopedService;
        private readonly Func<string, Type, IEditConstructorFormXml> _getEditConstructorFormXml;
        private readonly Func<string, Type, IEditLiteralListFormXml> _getEditLiteralListFormXml;
        private readonly Func<string, Type, IEditObjectListFormXml> _getEditObjectListFormXml;

        public EditXmlFormFactory(
            Func<string, Type, IEditConstructorFormXml> getEditConstructorFormXml,
            Func<string, Type, IEditLiteralListFormXml> getEditLiteralListFormXml,
            Func<string, Type, IEditObjectListFormXml> getEditObjectListFormXml)
        {
            _getEditConstructorFormXml = getEditConstructorFormXml;
            _getEditLiteralListFormXml = getEditLiteralListFormXml;
            _getEditObjectListFormXml = getEditObjectListFormXml;
        }

        public IEditConstructorFormXml GetEditConstructorFormXml(string xml, Type assignedTo)
        {
            _scopedService = _getEditConstructorFormXml(xml, assignedTo);
            return (IEditConstructorFormXml)_scopedService;
        }

        public IEditLiteralListFormXml GetEditLiteralListFormXml(string xml, Type assignedTo)
        {
            _scopedService = _getEditLiteralListFormXml(xml, assignedTo);
            return (IEditLiteralListFormXml)_scopedService;
        }

        public IEditObjectListFormXml GetEditObjectListFormXml(string xml, Type assignedTo)
        {
            _scopedService = _getEditObjectListFormXml(xml, assignedTo);
            return (IEditObjectListFormXml)_scopedService;
        }

        public void Dispose()
        {
            _scopedService?.Dispose();
        }
    }
}
