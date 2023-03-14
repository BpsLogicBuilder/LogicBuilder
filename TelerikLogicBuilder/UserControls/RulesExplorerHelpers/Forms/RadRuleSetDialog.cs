using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using LogicBuilder.Workflow.Activities.Rules;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.Primitives;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls.RulesExplorerHelpers.Forms
{
    internal partial class RadRuleSetDialog : RadForm, IRadRuleSetDialog
    {
        private readonly IEnumHelper _enumHelper;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IFormInitializer _formInitializer;
        private readonly IRadDropDownListHelper _radDropDownListHelper;
        private readonly IDialogFormMessageControl _dialogFormMessageControl;

        public RadRuleSetDialog(
            IEnumHelper enumHelper,
            IExceptionHelper exceptionHelper,
            IFormInitializer formInitializer,
            IRadDropDownListHelper radDropDownListHelper,
            IDialogFormMessageControl dialogFormMessageControl)
        {
            _enumHelper = enumHelper;
            _exceptionHelper = exceptionHelper;
            _formInitializer = formInitializer;
            _radDropDownListHelper = radDropDownListHelper;
            _dialogFormMessageControl = dialogFormMessageControl;
            InitializeComponent();
            Initialize();
        }

        private readonly RadToolTip toolTip = new();
        private Dictionary<string, string>? validationErrors;
        private RuleValidation? ruleValidation;

        public void Setup(Type activityType, RuleSet ruleSet, List<Assembly> references)
        {
            ruleValidation = new RuleValidation(activityType, references);
            CollectValidationErrors(ruleSet);
            InitializeFields(ruleSet);
        }

        private static void CollapsePanelBorder(RadPanel radPanel)
            => ((BorderPrimitive)radPanel.PanelElement.Children[1]).Visibility = ElementVisibility.Collapsed;

        private void CollectValidationErrors(RuleSet ruleSet)
        {
            if (ruleValidation == null)
                throw _exceptionHelper.CriticalException("{8E3A26B9-83B4-41AE-AA83-0F74408DCA43}");

            try
            {

                if (ruleSet.Validate(ruleValidation))
                {
                    validationErrors = new Dictionary<string, string>();
                    return;
                }

                validationErrors = ruleValidation.ErrorsByRuleName
                    .ToDictionary
                    (
                        kvp => kvp.Key,
                        kvp => string.Join(Environment.NewLine, kvp.Value.Select(v => v.ErrorText))
                    );
            }
            catch (Exception ex)
            {
                throw new LogicBuilderException(ex.Message, ex);
            }
        }

        private void Initialize()
        {
            InitializeDialogFormMessageControl();

            _formInitializer.SetFormDefaults(this, 789);

            ((Control)_dialogFormMessageControl).Padding = new Padding(6, 0, 3, 0);
            radPanelBottomFill.Padding = new Padding(6, 0, 0, 0);
            radPanelBottom.Padding = new Padding(0, 0, 10, 10);

            radTextBoxConditions.ScrollBars = ScrollBars.Both;
            radTextBoxActions.ScrollBars = ScrollBars.Both;

            radDropDownListChaining.DropDownStyle = RadDropDownStyle.DropDownList;
            radDropDownListChaining.ReadOnly = true;
            _radDropDownListHelper.LoadComboItems<RuleChainingBehavior>(radDropDownListChaining);

            radGroupBoxRule.ShowItemToolTips = true;
            radGroupBoxActions.Padding = PerFontSizeConstants.GroupBoxPadding;
            radGroupBoxConditions.Padding = PerFontSizeConstants.GroupBoxPadding;
            radGroupBoxRule.Padding = PerFontSizeConstants.GroupBoxPadding;
            radGroupBoxRuleSet.Padding = PerFontSizeConstants.GroupBoxPadding;

            radListViewRuleSet.Columns.Add(RuleSetListViewColumnNames.Name, Strings.ruleSetListViewColumnHeaderTextName);
            radListViewRuleSet.Columns.Add(RuleSetListViewColumnNames.Priority, Strings.ruleSetListViewColumnHeaderTextPriority);
            radListViewRuleSet.Columns.Add(Strings.ruleSetListViewColumnHeaderTextReevaluation, RuleSetListViewColumnNames.Reevaluation);
            radListViewRuleSet.Columns.Add(Strings.ruleSetListViewColumnHeaderTextActive, RuleSetListViewColumnNames.Active);
            radListViewRuleSet.Columns.Add(Strings.ruleSetListViewColumnHeaderTextRulePreview, RuleSetListViewColumnNames.RulePreview);
            radListViewRuleSet.Columns[0].Width = 200;
            radListViewRuleSet.Columns[1].Width = 50;
            radListViewRuleSet.Columns[2].Width = 91;
            radListViewRuleSet.Columns[3].Width = 59;
            radListViewRuleSet.Columns[4].Width = 400;

            radListViewRuleSet.AllowEdit = false;
            radListViewRuleSet.MultiSelect = false;
            radListViewRuleSet.ViewType = ListViewType.DetailsView;
            radListViewRuleSet.EnableSorting = true;
            radListViewRuleSet.EnableColumnSort = true;
            radListViewRuleSet.VisualItemFormatting += RadListViewRuleSet_VisualItemFormatting;

            radListViewRuleSet.SelectedIndexChanged += RadListViewRuleSet_SelectedIndexChanged;

            radTextBoxName.ReadOnly = true;
            radTextBoxPriority.ReadOnly = true;
            radCheckBoxReevaluation.ReadOnly = true;
            radCheckBoxActive.ReadOnly = true;

            radLabelChaining.Font = new Font
            (
                ForeColorUtility.GetDefaultFont(ThemeResolutionService.ApplicationThemeName),
                FontStyle.Bold
            );
            radLabelName.Font = new Font
            (
                ForeColorUtility.GetDefaultFont(ThemeResolutionService.ApplicationThemeName),
                FontStyle.Bold
            );
            radLabelPriority.Font = new Font
            (
                ForeColorUtility.GetDefaultFont(ThemeResolutionService.ApplicationThemeName),
                FontStyle.Bold
            );
            radButtonClose.DialogResult = DialogResult.Cancel;

            CollapsePanelBorder(radPanelBottom);
            CollapsePanelBorder(radPanelBottomFill);
            CollapsePanelBorder(radPanelCloseButton);
            CollapsePanelBorder(radPanelFill);
            CollapsePanelBorder(radPanelRuleFillLeft);
            CollapsePanelBorder(radPanelRuleFillRight);
            CollapsePanelBorder(radPanelRuleSetFill);
            CollapsePanelBorder(radPanelRuleSetTop);
            CollapsePanelBorder(radPanelRuleSetTopLeft);
            CollapsePanelBorder(radPanelRuleSetTopFill);
        }

        private void InitializeDialogFormMessageControl()
        {
            ((ISupportInitialize)(this.radPanelBottomFill)).BeginInit();
            this.radPanelBottomFill.SuspendLayout();

            _dialogFormMessageControl.Dock = DockStyle.Fill;
            _dialogFormMessageControl.Location = new Point(0, 0);
            this.radPanelBottomFill.Controls.Add((Control)_dialogFormMessageControl);

            ((ISupportInitialize)(this.radPanelBottomFill)).EndInit();
            this.radPanelBottomFill.ResumeLayout(true);
        }

        private void InitializeFields(RuleSet ruleSet)
        {
            radDropDownListChaining.SelectedIndex = (int)ruleSet.ChainingBehavior;

            foreach (Rule rule in ruleSet.Rules)
            {
                ListViewDataItem item = new()
                {
                    Tag = rule
                };

                item[0] = rule.Name;
                item[1] = rule.Priority;
                item[2] = _enumHelper.GetVisibleEnumText(rule.ReevaluationBehavior);
                item[3] = rule.Active;
                item[4] = GetRulePreview(rule);

                radListViewRuleSet.Items.Add(item);
            }

            if (ruleSet.Rules.Count > 0)
                radListViewRuleSet.SelectedIndex = 0;

            static string GetRulePreview(Rule rule)
                => $"IF {rule.Condition} THEN {string.Join(" ", rule.ThenActions.Select(t => t.ToString()))}";
        }

        #region Event Handlers
        private void RadListViewRuleSet_VisualItemFormatting(object sender, ListViewVisualItemEventArgs e)
        {
            if (e.VisualItem.Data.Selected)
                return;

            Rule rule = (Rule)e.VisualItem.Data.Tag;
            e.VisualItem.ForeColor = validationErrors?.ContainsKey(rule.Name) == true
                ? ForeColorUtility.GetErrorForeColor(ThemeResolutionService.ApplicationThemeName)
                : ForeColorUtility.GetOkForeColor(ThemeResolutionService.ApplicationThemeName);
        }

        private void RadListViewRuleSet_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (radListViewRuleSet.SelectedItem == null)
                return;

            Rule rule = (Rule)radListViewRuleSet.SelectedItem.Tag;
            BorderPrimitive border = (BorderPrimitive)radGroupBoxRule.GroupBoxElement.Content.Children[1];

            if (validationErrors?.ContainsKey(rule.Name) == true)
            {
                border.ForeColor = ForeColorUtility.GetGroupBoxBorderErrorColor();
                radGroupBoxRule.ForeColor = ForeColorUtility.GetErrorForeColor(ThemeResolutionService.ApplicationThemeName);
                toolTip.SetToolTip(radGroupBoxRule, validationErrors[rule.Name]);
                _dialogFormMessageControl.SetErrorMessage(validationErrors[rule.Name]);
            }
            else
            {
                border.ForeColor = ForeColorUtility.GetGroupBoxBorderColor(ThemeResolutionService.ApplicationThemeName);
                radGroupBoxRule.ForeColor = ForeColorUtility.GetOkForeColor(ThemeResolutionService.ApplicationThemeName);
                toolTip.SetToolTip(radGroupBoxRule, String.Empty);
                _dialogFormMessageControl.ClearMessage();
            }

            radTextBoxName.Text = rule.Name;
            radTextBoxPriority.Text = rule.Priority.ToString(CultureInfo.CurrentCulture);

            radCheckBoxReevaluation.Checked = rule.ReevaluationBehavior == RuleReevaluationBehavior.Always;
            radCheckBoxActive.Checked = rule.Active;

            radTextBoxConditions.Text = rule.Condition?
                                            .ToString()?
                                            .Replace(Environment.NewLine, "\n")
                                            .Replace("\n", Environment.NewLine) ?? string.Empty;
            radTextBoxActions.Text = string.Join(Environment.NewLine, rule.ThenActions.Select(t => t.ToString()));
        }
        #endregion Event Handlers
    }
}
