using ABIS.LogicBuilder.FlowBuilder.Editing.EditXml.Helpers;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditXml.Factories
{
    internal interface IEditXmlHelperFactory
    {
        IValidateXmlTextHelper GetValidateXmlTextHelper(IEditFormXml editXmlForm);
    }
}
