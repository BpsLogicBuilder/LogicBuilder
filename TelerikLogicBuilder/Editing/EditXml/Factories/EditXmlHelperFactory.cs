using ABIS.LogicBuilder.FlowBuilder.Editing.EditXml.Helpers;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditXml.Factories
{
    internal class EditXmlHelperFactory : IEditXmlHelperFactory
    {
        private readonly Func<IEditFormXml, IValidateXmlTextHelper> _getValidateXmlTextHelper;

        public EditXmlHelperFactory(
            Func<IEditFormXml, IValidateXmlTextHelper> getValidateXmlTextHelper)
        {
            _getValidateXmlTextHelper = getValidateXmlTextHelper;
        }

        public IValidateXmlTextHelper GetValidateXmlTextHelper(IEditFormXml editXmlForm)
            => _getValidateXmlTextHelper(editXmlForm);
    }
}
