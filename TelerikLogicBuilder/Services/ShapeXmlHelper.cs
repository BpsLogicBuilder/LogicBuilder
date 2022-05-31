using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation;
using Microsoft.Office.Interop.Visio;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services
{
    internal class ShapeXmlHelper : IShapeXmlHelper
    {
        private readonly IEncryption _encryption;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IShapeDataCellManager _shapeDataCellManager;
        private readonly IXmlValidator _xmlValidator;

        public ShapeXmlHelper(IEncryption encryption, IExceptionHelper exceptionHelper, IShapeDataCellManager shapeDataCellManager, IXmlValidator xmlValidator)
        {
            _encryption = encryption;
            _exceptionHelper = exceptionHelper;
            _shapeDataCellManager = shapeDataCellManager;
            _xmlValidator = xmlValidator;
        }

        public string GetXmlString(Shape shape)
        {
            try
            {
                string xml = _encryption.Decrypt
                (
                    _shapeDataCellManager.GetRulesDataString(shape)
                );

                if (string.IsNullOrEmpty(xml))
                    return string.Empty;

                var validationResponse = _xmlValidator.Validate
                (
                    GetSchemaName(shape.Master.NameU),
                    xml
                );

                return validationResponse.Success ? xml : string.Empty;
            }
            catch (XmlException)
            {
                return string.Empty;
            }
        }

        public void SetXmlString(Shape shape, string xml, string visibleText)
        {
            try
            {
                var validationResponse = _xmlValidator.Validate
                (
                    GetSchemaName(shape.Master.NameU),
                    xml
                );

                if (validationResponse.Success)
                {
                    _shapeDataCellManager.UnlockUpdate(shape);
                    _shapeDataCellManager.SetRulesDataString(shape, _encryption.Encrypt(xml));
                    shape.Text = visibleText;
                    _shapeDataCellManager.LockUpdate(shape);
                }
            }
            catch (XmlException)
            {
            }
        }

        private SchemaName GetSchemaName(string universalMasterName)
        {
            return universalMasterName switch
            {
                UniversalMasterName.ACTION or UniversalMasterName.DIALOG => SchemaName.FunctionsDataSchema,
                UniversalMasterName.CONDITIONOBJECT or UniversalMasterName.WAITCONDITIONOBJECT => SchemaName.ConditionsDataSchema,
                UniversalMasterName.DECISIONOBJECT or UniversalMasterName.WAITDECISIONOBJECT => SchemaName.DecisionsDataSchema,
                UniversalMasterName.JUMPOBJECT or UniversalMasterName.MODULE => SchemaName.ShapeDataSchema,
                UniversalMasterName.CONNECTOBJECT => SchemaName.ConnectorDataSchema,
                _ => throw _exceptionHelper.CriticalException("{C8295A80-33B0-495C-B268-228FA7C0A221}"),
            };
        }
    }
}
