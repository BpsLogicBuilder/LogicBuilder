using ABIS.LogicBuilder.FlowBuilder.Constants;
using LogicBuilder.Attributes;
using System.Collections.Generic;

namespace TelerikLogicBuilder.Tests.AttributeSamples
{
    public class FormControlSettingsParameters
    {
        public FormControlSettingsParameters
        (
            [Comments("Update modelType first. Source property name from the target object.")]
            [ParameterEditorControl(ParameterControlType.ParameterSourcedPropertyInput)]
            [NameValue(AttributeNames.PROPERTYSOURCEPARAMETER, "modelType")]
            [NameValue(AttributeNames.DEFAULTVALUE, "field")]
            string field,

            [Comments("ID attribute for the DOM element - usually (field)_id - also used on the label's for attribute.")]
            [NameValue(AttributeNames.DEFAULTVALUE, "(field)_id")]
            string domElementId,

            [Comments("Title")]
            [NameValue(AttributeNames.DEFAULTVALUE, "Title")]
            string title,

            [Comments("Place holder text.")]
            [NameValue(AttributeNames.DEFAULTVALUE, "(Title) required")]
            string placeHolder,

            [Comments("text/numeric/boolean/date/email")]
            [Domain("text,numeric,boolean,date,email")]
            [ParameterEditorControl(ParameterControlType.SingleLineTextBox)]
            string type,

            [ParameterEditorControl(ParameterControlType.Form)]
            object someObject,

            [ListEditorControl(ListControlType.ListForm)]
            List<string> someList,

            [ParameterEditorControl(ParameterControlType.ParameterSourceOnly)]
            [NameValue(AttributeNames.DEFAULTVALUE, "Contoso.Domain.Entities")]
            [Comments("Fully qualified class name for the model type.")]
            string modelType = null
        )
        {
            Field = field;
            DomElementId = domElementId;
            Title = title;
            Placeholder = placeHolder;
            Type = type;
            SomeObject = someObject;  
            SomeList = someList;
        }

        public string Field { get; set; }
        public string DomElementId { get; set; }
        public string Title { get; set; }
        public string Placeholder { get; set; }
        public string Type { get; set; }
        public object SomeObject { get; set; }
        public List<string> SomeList { get; set; }
    }
}
