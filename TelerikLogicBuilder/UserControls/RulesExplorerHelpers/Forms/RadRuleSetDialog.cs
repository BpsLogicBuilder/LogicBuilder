﻿using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using LogicBuilder.Workflow.Activities.Rules;
using LogicBuilder.Workflow.ComponentModel.Compiler;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.Primitives;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls.RulesExplorerHelpers.Forms
{
    internal partial class RadRuleSetDialog : RadForm
    {
        private readonly IEnumHelper _enumHelper;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IFormInitializer _formInitializer;
        private readonly IRadDropDownListHelper _radDropDownListHelper;

        public RadRuleSetDialog(
            IEnumHelper enumHelper,
            IExceptionHelper exceptionHelper,
            IFormInitializer formInitializer,
            IRadDropDownListHelper radDropDownListHelper)
        {
            _enumHelper = enumHelper;
            _exceptionHelper = exceptionHelper;
            _formInitializer = formInitializer;
            _radDropDownListHelper = radDropDownListHelper;
            InitializeComponent();
            Initialize();
        }

        private readonly RadToolTip toolTip = new();
        private readonly Dictionary<string, string> validationErrors = new();
        private RuleValidation? ruleValidation;

        public void Setup(Type activityType, RuleSet ruleSet, List<Assembly> references)
        {
            ruleValidation = new RuleValidation(activityType, references);
            CollectValidationErrors(ruleSet);
            InitializeFields(ruleSet);
        }

        private void CollectValidationErrors(RuleSet ruleSet)
        {
            if (ruleValidation == null)
                throw _exceptionHelper.CriticalException("{AF773FC1-C139-44C7-BA30-C7F894BE48F9}");

            try
            {

                if (ruleSet.Validate(ruleValidation))
                    return;

                HashSet<string> ruleNames = ruleSet.Rules.Select(r => r.Name).ToHashSet();

                foreach (ValidationError validationError in ruleValidation.Errors)
                {
                    if (!Regex.IsMatch(validationError.ErrorText, Strings.ruleErrorMatch))
                        continue;

                    string[] parts = validationError.ErrorText.Split(new char[] { '"', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (!ruleNames.Contains(parts[1]))
                        continue;

                    string ruleName = parts[1];

                    if (validationErrors.TryGetValue(ruleName, out string? error))
                        validationErrors[ruleName] = $"{error}{Environment.NewLine}{validationError.ErrorText}";
                    else
                        validationErrors.Add(ruleName, validationError.ErrorText);
                }
            }
            catch (Exception ex)
            {
                throw new LogicBuilderException(ex.Message, ex);
            }
        }

        private void Initialize()
        {
            _formInitializer.SetFormDefaults(this, 789);

            radGroupBoxRuleSet.Anchor = AnchorConstants.AnchorsLeftTopRightBottom;
            radGroupBoxRule.Anchor = AnchorConstants.AnchorsLeftRightBottom;
            tableLayoutPanel1.Anchor = AnchorConstants.AnchorsLeftTop;
            radTextBoxConditions.Anchor = AnchorConstants.AnchorsLeftTopRight;
            radTextBoxActions.Anchor = AnchorConstants.AnchorsLeftTopRight;
            radButtonClose.Anchor = AnchorConstants.AnchorsRightBottom;
            ((BorderPrimitive)radPanelBottom.PanelElement.Children[1]).Visibility = ElementVisibility.Collapsed;
            ((BorderPrimitive)radPanelBottomFill.PanelElement.Children[1]).Visibility = ElementVisibility.Collapsed;

            dialogFormMessageControl1.Padding = new Padding(6, 0, 3, 0);
            radPanelBottomFill.Padding = new Padding(6, 0, 0, 0);
            radPanelBottom.Padding = new Padding(0, 0, 10, 10);

            radTextBoxConditions.ScrollBars = ScrollBars.Both;
            radTextBoxActions.ScrollBars = ScrollBars.Both;

            radDropDownListChaining.DropDownStyle = RadDropDownStyle.DropDownList;
            radDropDownListChaining.ReadOnly = true;
            _radDropDownListHelper.LoadComboItems<RuleChainingBehavior>(radDropDownListChaining);

            radGroupBoxRule.ShowItemToolTips = true;

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

            radLabelChaining.Font = new Font(radLabelChaining.Font, FontStyle.Bold);
            radLabelName.Font = new Font(radLabelName.Font, FontStyle.Bold);
            radLabelPriority.Font = new Font(radLabelPriority.Font, FontStyle.Bold);
            radButtonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
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
            e.VisualItem.ForeColor = validationErrors.ContainsKey(rule.Name)
                ? ForeColorUtility.GetErrorForeColor(ThemeResolutionService.ApplicationThemeName)
                : ForeColorUtility.GetOkForeColor(ThemeResolutionService.ApplicationThemeName);
        }

        private void RadListViewRuleSet_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (radListViewRuleSet.SelectedItem == null)
                return;

            Rule rule = (Rule)radListViewRuleSet.SelectedItem.Tag;
            BorderPrimitive border = (BorderPrimitive)radGroupBoxRule.GroupBoxElement.Content.Children[1];
            //if (ruleValidation.ErrorsPerRule.TryGetValue(rule.Name, out IList<string> errors))
            //{
            //    border.ForeColor = ForeColorUtility.GetGroupBoxBorderErrorColor();
            //    radGroupBoxRule.ForeColor = ForeColorUtility.GetErrorForeColor(ThemeResolutionService.ApplicationThemeName);
            //    toolTip.SetToolTip(radGroupBoxRule, validationErrors[rule.Name]);
            //    dialogFormMessageControl1.SetErrorMessage(validationErrors[rule.Name]);
            //}
            //else
            //{
            //    border.ForeColor = ForeColorUtility.GetGroupBoxBorderColor(ThemeResolutionService.ApplicationThemeName);
            //    radGroupBoxRule.ForeColor = ForeColorUtility.GetOkForeColor(ThemeResolutionService.ApplicationThemeName);
            //    toolTip.SetToolTip(radGroupBoxRule, String.Empty);
            //    dialogFormMessageControl1.ClearMessage();
            //}

            if (validationErrors.ContainsKey(rule.Name))
            {
                border.ForeColor = ForeColorUtility.GetGroupBoxBorderErrorColor();
                radGroupBoxRule.ForeColor = ForeColorUtility.GetErrorForeColor(ThemeResolutionService.ApplicationThemeName);
                toolTip.SetToolTip(radGroupBoxRule, validationErrors[rule.Name]);
                dialogFormMessageControl1.SetErrorMessage(validationErrors[rule.Name]);
            }
            else
            {
                border.ForeColor = ForeColorUtility.GetGroupBoxBorderColor(ThemeResolutionService.ApplicationThemeName);
                radGroupBoxRule.ForeColor = ForeColorUtility.GetOkForeColor(ThemeResolutionService.ApplicationThemeName);
                toolTip.SetToolTip(radGroupBoxRule, String.Empty);
                dialogFormMessageControl1.ClearMessage();
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
