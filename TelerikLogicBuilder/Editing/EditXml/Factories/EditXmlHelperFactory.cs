using ABIS.LogicBuilder.FlowBuilder.Editing.EditXml.Helpers;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditXml.Factories
{
    internal class EditXmlHelperFactory : IEditXmlHelperFactory
    {
        public IValidateXmlTextHelper GetValidateXmlTextHelper(IEditFormXml editXmlForm)
            => new ValidateXmlTextHelper
            (
                editXmlForm
            );
    }
}
