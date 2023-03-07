using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Factories;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.Helpers
{
    internal class LoadParameterControlsDictionary : ILoadParameterControlsDictionary
    {
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IFieldControlFactory _fieldControlFactory;
        private readonly IServiceFactory _serviceFactory;
        private readonly IEditingControl editingControl;
        private readonly IEditingForm editingForm;

        public LoadParameterControlsDictionary(
            IExceptionHelper exceptionHelper,
            IFieldControlFactory fieldControlFactory,
            IServiceFactory serviceFactory,
            IEditingControl editingControl,
            IEditingForm editingForm)
        {
            _exceptionHelper = exceptionHelper;
            _fieldControlFactory = fieldControlFactory;
            _serviceFactory = serviceFactory;
            
            this.editingControl = editingControl;
            this.editingForm = editingForm;
        }

        public void Load(IDictionary<string, ParameterControlSet> editControlsSet, IList<ParameterBase> parameters)
        {
            foreach (ParameterBase parameter in parameters)
                AddParameterControlSet(editControlsSet, parameter);
        }

        private void AddParameterControlSet(IDictionary<string, ParameterControlSet> editControlsSet, ParameterBase parameter)
        {
            RadToolTip toolTip = new();
            RadLabel label = new()
            {
                Dock = System.Windows.Forms.DockStyle.Top,
                Name = parameter.Name,
                Image = GetLabelImage(parameter.ParameterCategory)
            };
            if (parameter.Comments.Trim().Length > 0)
                toolTip.SetToolTip(label, parameter.Comments);

            RadCheckBox radCheckBox = new()
            {
                Dock = System.Windows.Forms.DockStyle.Top,
                TabStop = false,
                Name = parameter.Name,
                Text = parameter.Name,
                Checked = !parameter.IsOptional,
                Enabled = parameter.IsOptional
            };

            IValueControl? valueControl = null;
            if (parameter is LiteralParameter literalParameter)
            {
                switch (literalParameter.Control)
                {
                    case LiteralParameterInputStyle.DomainAutoComplete:
                        valueControl = _fieldControlFactory.GetLiteralParameterDomainAutoCompleteControl(editingControl, literalParameter);
                        break;
                    case LiteralParameterInputStyle.DropDown:
                        valueControl = _fieldControlFactory.GetLiteralParameterDropDownListControl(editingControl, literalParameter);
                        break;
                    case LiteralParameterInputStyle.MultipleLineTextBox:
                        valueControl = literalParameter.Domain.Any()
                                            ? _fieldControlFactory.GetLiteralParameterDomainMultilineControl(editingControl, literalParameter)
                                            : _fieldControlFactory.GetLiteralParameterMultilineControl(editingControl, literalParameter);
                        break;
                    case LiteralParameterInputStyle.ParameterSourcedPropertyInput:
                        valueControl = _fieldControlFactory.GetLiteralParameterSourcedPropertyRichInputBoxControl(editingControl, literalParameter, editControlsSet);
                        break;
                    case LiteralParameterInputStyle.ParameterSourceOnly:
                    case LiteralParameterInputStyle.TypeAutoComplete:
                        ILiteralParameterTypeAutoCompleteControl typeAutoCompleteControl = _fieldControlFactory.GetLiteralParameterTypeAutoCompleteControl(editingControl, literalParameter);
                        ITypeAutoCompleteManager typeAutoCompleteManager = _serviceFactory.GetTypeAutoCompleteManager(editingForm, typeAutoCompleteControl);
                        typeAutoCompleteManager.Setup();
                        valueControl = typeAutoCompleteControl;
                        break;
                    case LiteralParameterInputStyle.PropertyInput:
                        valueControl = _fieldControlFactory.GetLiteralParameterPropertyInputRichInputBoxControl(editingControl, literalParameter);
                        break;
                    case LiteralParameterInputStyle.SingleLineTextBox:
                        valueControl = literalParameter.Domain.Any()
                                            ? _fieldControlFactory.GetLiteralParameterDomainRichInputBoxControl(editingControl, literalParameter)
                                            : _fieldControlFactory.GetLiteralParameterRichInputBoxControl(editingControl, literalParameter);
                        break;
                    default:
                        break;
                }
            }
            else if (parameter is ObjectParameter objectParameter)
            {
                valueControl = _fieldControlFactory.GetObjectParameterRichTextBoxControl(editingControl, objectParameter);
            }
            else if (parameter is ListOfLiteralsParameter listOfLiteralsParameter)
            {
                valueControl = _fieldControlFactory.GetiteralListParameterRichTextBoxControl(editingControl, listOfLiteralsParameter, editControlsSet);
            }
            else if (parameter is ListOfObjectsParameter listOfObjectsParameter)
            {
                valueControl = _fieldControlFactory.GetObjectListParameterRichTextBoxControl(editingControl, listOfObjectsParameter);
            }

            if (valueControl != null)
            {
                if (parameter.Comments.Trim().Length > 0)
                    valueControl.SetToolTipHelp(parameter.Comments);

                valueControl.Location= new Point(0, 0);
                valueControl.Dock = System.Windows.Forms.DockStyle.Fill;
                editControlsSet.Add(parameter.Name, new ParameterControlSet(label, radCheckBox, valueControl));
            }
        }

        private Image GetLabelImage(ParameterCategory category)
        {
            return category switch
            {
                ParameterCategory.Literal => Properties.Resources.LiteralParameter,
                ParameterCategory.Object => Properties.Resources.ObjectParameter,
                ParameterCategory.LiteralList => Properties.Resources.LiteralListParameter,
                ParameterCategory.ObjectList => Properties.Resources.ObjectListParameter,
                _ => throw _exceptionHelper.CriticalException("{A63CD744-9368-463F-A214-E33DE978F11C}"),
            };
        }
    }
}
