using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditXml.Factories
{
    internal class EditXmlFormFactory : IEditXmlFormFactory
    {
        private IDisposable? _scopedService;
        private readonly Func<string, Type, IEditConstructorFormXml> _getEditConstructorFormXml;
        private readonly Func<string, Type, IEditLiteralListFormXml> _getEditLiteralListFormXml;

        public EditXmlFormFactory(
            Func<string, Type, IEditConstructorFormXml> getEditConstructorFormXml,
            Func<string, Type, IEditLiteralListFormXml> getEditLiteralListFormXml)
        {
            _getEditConstructorFormXml = getEditConstructorFormXml;
            _getEditLiteralListFormXml = getEditLiteralListFormXml;
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

        public void Dispose()
        {
            _scopedService?.Dispose();
        }
    }
}
