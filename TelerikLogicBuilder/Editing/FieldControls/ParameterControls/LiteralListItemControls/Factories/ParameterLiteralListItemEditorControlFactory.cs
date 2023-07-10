using ABIS.LogicBuilder.FlowBuilder.Components;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Editing.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Editing.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.ParameterControls.LiteralListItemControls.Factories
{
    internal class ParameterLiteralListItemEditorControlFactory : IParameterLiteralListItemEditorControlFactory
    {
        public IListOfLiteralsParameterItemDomainAutoCompleteControl GetListOfLiteralsParameterItemDomainAutoCompleteControl(ListOfLiteralsParameter literalListParameter)
            => new ListOfLiteralsParameterItemDomainAutoCompleteControl
            (
                Program.ServiceProvider.GetRequiredService<IRadDropDownListHelper>(),
                Program.ServiceProvider.GetRequiredService<IXmlDataHelper>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                literalListParameter
            );

        public IListOfLiteralsParameterItemDomainMultilineControl GetListOfLiteralsParameterItemDomainMultilineControl(IDataGraphEditingControl editingControl, ListOfLiteralsParameter literalListParameter)
            => new ListOfLiteralsParameterItemDomainMultilineControl
            (
                Program.ServiceProvider.GetRequiredService<IEditingControlHelperFactory>(),
                Program.ServiceProvider.GetRequiredService<IEnumHelper>(),
                Program.ServiceProvider.GetRequiredService<IFieldControlCommandFactory>(),
                Program.ServiceProvider.GetRequiredService<IFieldControlHelperFactory>(),
                Program.ServiceProvider.GetRequiredService<IImageListService>(),
                Program.ServiceProvider.GetRequiredService<ILayoutFieldControlButtons>(),
                Program.ServiceProvider.GetRequiredService<IUpdateRichInputBoxXml>(),
                new RichInputBox(),
                Program.ServiceProvider.GetRequiredService<IXmlDataHelper>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                editingControl,
                literalListParameter
            );

        public IListOfLiteralsParameterItemDomainRichInputBoxControl GetListOfLiteralsParameterItemDomainRichInputBoxControl(IDataGraphEditingControl editingControl, ListOfLiteralsParameter literalListParameter)
            => new ListOfLiteralsParameterItemDomainRichInputBoxControl
            (
                Program.ServiceProvider.GetRequiredService<IEditingControlHelperFactory>(),
                Program.ServiceProvider.GetRequiredService<IEnumHelper>(),
                Program.ServiceProvider.GetRequiredService<IFieldControlCommandFactory>(),
                Program.ServiceProvider.GetRequiredService<IFieldControlHelperFactory>(),
                Program.ServiceProvider.GetRequiredService<IImageListService>(),
                Program.ServiceProvider.GetRequiredService<ILayoutFieldControlButtons>(),
                Program.ServiceProvider.GetRequiredService<IUpdateRichInputBoxXml>(),
                new RichInputBox(),
                Program.ServiceProvider.GetRequiredService<IXmlDataHelper>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                editingControl,
                literalListParameter
            );

        public IListOfLiteralsParameterItemDropDownListControl GetListOfLiteralsParameterItemDropDownListControl(ListOfLiteralsParameter literalListParameter)
            => new ListOfLiteralsParameterItemDropDownListControl
            (
                Program.ServiceProvider.GetRequiredService<IRadDropDownListHelper>(),
                Program.ServiceProvider.GetRequiredService<IXmlDataHelper>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                literalListParameter
            );

        public IListOfLiteralsParameterItemMultilineControl GetListOfLiteralsParameterItemMultilineControl(IDataGraphEditingControl editingControl, ListOfLiteralsParameter literalListParameter)
            => new ListOfLiteralsParameterItemMultilineControl
            (
                Program.ServiceProvider.GetRequiredService<IEditingControlHelperFactory>(),
                Program.ServiceProvider.GetRequiredService<IEnumHelper>(),
                Program.ServiceProvider.GetRequiredService<IFieldControlCommandFactory>(),
                Program.ServiceProvider.GetRequiredService<IFieldControlHelperFactory>(),
                Program.ServiceProvider.GetRequiredService<IImageListService>(),
                Program.ServiceProvider.GetRequiredService<ILayoutFieldControlButtons>(),
                Program.ServiceProvider.GetRequiredService<IUpdateRichInputBoxXml>(),
                new RichInputBox(),
                Program.ServiceProvider.GetRequiredService<IXmlDataHelper>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                editingControl,
                literalListParameter
            );

        public IListOfLiteralsParameterItemPropertyInputRichInputBoxControl GetListOfLiteralsParameterItemPropertyInputRichInputBoxControl(IDataGraphEditingControl editingControl, ListOfLiteralsParameter literalListParameter)
            => new ListOfLiteralsParameterItemPropertyInputRichInputBoxControl
            (
                Program.ServiceProvider.GetRequiredService<IEditingControlHelperFactory>(),
                Program.ServiceProvider.GetRequiredService<IEnumHelper>(),
                Program.ServiceProvider.GetRequiredService<IFieldControlCommandFactory>(),
                Program.ServiceProvider.GetRequiredService<IFieldControlHelperFactory>(),
                Program.ServiceProvider.GetRequiredService<IImageListService>(),
                Program.ServiceProvider.GetRequiredService<ILayoutFieldControlButtons>(),
                Program.ServiceProvider.GetRequiredService<IUpdateRichInputBoxXml>(),
                new RichInputBox(),
                Program.ServiceProvider.GetRequiredService<IXmlDataHelper>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                editingControl,
                literalListParameter
            );

        public IListOfLiteralsParameterItemRichInputBoxControl GetListOfLiteralsParameterItemRichInputBoxControl(IDataGraphEditingControl editingControl, LiteralListParameterElementInfo listInfo)
            => new ListOfLiteralsParameterItemRichInputBoxControl
            (
                Program.ServiceProvider.GetRequiredService<IEditingControlHelperFactory>(),
                Program.ServiceProvider.GetRequiredService<IEnumHelper>(),
                Program.ServiceProvider.GetRequiredService<IFieldControlCommandFactory>(),
                Program.ServiceProvider.GetRequiredService<IFieldControlHelperFactory>(),
                Program.ServiceProvider.GetRequiredService<IImageListService>(),
                Program.ServiceProvider.GetRequiredService<ILayoutFieldControlButtons>(),
                Program.ServiceProvider.GetRequiredService<IUpdateRichInputBoxXml>(),
                new RichInputBox(),
                Program.ServiceProvider.GetRequiredService<IXmlDataHelper>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                editingControl,
                listInfo
            );

        public IListOfLiteralsParameterItemParameterSourcedPropertyRichInputBoxControl GetListOfLiteralsParameterItemParameterSourcedPropertyRichInputBoxControl(IDataGraphEditingControl editingControl, LiteralListParameterElementInfo listInfo)
            => new ListOfLiteralsParameterItemParameterSourcedPropertyRichInputBoxControl
            (
                Program.ServiceProvider.GetRequiredService<IEditingControlHelperFactory>(),
                Program.ServiceProvider.GetRequiredService<IEnumHelper>(),
                Program.ServiceProvider.GetRequiredService<IFieldControlCommandFactory>(),
                Program.ServiceProvider.GetRequiredService<IFieldControlHelperFactory>(),
                Program.ServiceProvider.GetRequiredService<IImageListService>(),
                Program.ServiceProvider.GetRequiredService<ILayoutFieldControlButtons>(),
                Program.ServiceProvider.GetRequiredService<IUpdateRichInputBoxXml>(),
                new RichInputBox(),
                Program.ServiceProvider.GetRequiredService<IXmlDataHelper>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                editingControl,
                listInfo
            );

        public IListOfLiteralsParameterItemTypeAutoCompleteControl GetListOfLiteralsParameterItemTypeAutoCompleteControl()
            => new ListOfLiteralsParameterItemTypeAutoCompleteControl
            (
                Program.ServiceProvider.GetRequiredService<IImageListService>(),
                Program.ServiceProvider.GetRequiredService<ILayoutFieldControlButtons>(),
                Program.ServiceProvider.GetRequiredService<IRadDropDownListHelper>(),
                Program.ServiceProvider.GetRequiredService<IXmlDataHelper>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>()
            );
    }
}
