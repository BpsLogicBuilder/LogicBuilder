using ABIS.LogicBuilder.FlowBuilder.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.ConfigureConstructorsHelper;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.TreeNodes;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense
{
    internal class IntellisenseConstructorsFormManager : IIntellisenseConstructorsFormManager
    {
        private readonly IImageListService _imageListService;
        private readonly IIntellisenseHelper _intellisenseHelper;
        private readonly ITypeAutoCompleteManager _cmbClassTypeAutoCompleteManager;
        private readonly ITypeLoadHelper _typeLoadHelper;
        private readonly IConfigureConstructorsHelperForm configureConstructorsHelperForm;

        public IntellisenseConstructorsFormManager(
            IImageListService imageListService,
            IIntellisenseHelper intellisenseHelper,
            IServiceFactory serviceFactory,
            ITypeLoadHelper typeLoadHelper,
            IConfigureConstructorsHelperForm configureConstructorsHelperForm)
        {
            _imageListService = imageListService;
            _intellisenseHelper = intellisenseHelper;
            _typeLoadHelper = typeLoadHelper;
            this.configureConstructorsHelperForm = configureConstructorsHelperForm;
            _cmbClassTypeAutoCompleteManager = serviceFactory.GetTypeAutoCompleteManager
            (
                configureConstructorsHelperForm,
                CmbClass
            );
        }

        private ApplicationTypeInfo Application => configureConstructorsHelperForm.Application;
        private ApplicationDropDownList CmbApplication => configureConstructorsHelperForm.CmbApplication;
        private AutoCompleteRadDropDownList CmbClass => configureConstructorsHelperForm.CmbClass;
        private RadTreeView TreeView => configureConstructorsHelperForm.TreeView;

        public void ApplicationChanged() => HandleSourceChanged();

        public void BuildTreeView(string classFullName)
        {
            configureConstructorsHelperForm.ClearMessage();
            if (!_typeLoadHelper.TryGetSystemType(classFullName, Application, out Type? type))
            {
                configureConstructorsHelperForm.SetErrorMessage(string.Format(CultureInfo.CurrentCulture, Strings.cannotLoadClassFormat, classFullName));
                return;
            }

            ClearTreeView();
            BuildMembersTreeView(type);

            if (TreeView.Nodes.Count > 0)
                TreeView.SelectedNode = TreeView.Nodes[0];
        }

        public void ClearTreeView() => configureConstructorsHelperForm.ClearTreeView();

        public void CmbClassTextChanged() => HandleSourceChanged();

        public void Initialize()
        {
            TreeView.ImageList = _imageListService.ImageList;
            TreeView.TreeViewElement.ShowNodeToolTips = true;
            TreeView.ShowRootLines = true;
            TreeView.HideSelection = false;
            _cmbClassTypeAutoCompleteManager.Setup();
            configureConstructorsHelperForm.ValidateOk();
        }

        public void UpdateSelection(ConstructorHelperStatus? helperStatus)
        {
            if (helperStatus == null)
                return;

            CmbApplication.SelectedValue = helperStatus.Application.Name;
            CmbClass.Text = helperStatus.ClassName;

            RadTreeNode? radTreeNode = TreeView.Nodes.OfType<ConstructorTreeNode>().FirstOrDefault(item => item.Equals(helperStatus.Node));
            if (radTreeNode == null)
                return;

            TreeView.SelectedNode = radTreeNode;
        }

        private void BuildMembersTreeView(Type type)
        {
            if (type.Namespace == null)
                return;

            List<ConstructorTreeNode> infoList = new();

            TreeView.BeginUpdate();
            foreach (ConstructorInfo constructorInfo in type.GetConstructors(_intellisenseHelper.GetConstructorBindingFlags()))
                infoList.Add(new ConstructorTreeNode(constructorInfo));

            TreeView.Nodes.AddRange(infoList.OrderBy(n => n.Text));

            TreeView.EndUpdate();
        }

        private void HandleSourceChanged()
        {
            ClearTreeView();

            if (!Application.AssemblyAvailable)
            {
                configureConstructorsHelperForm.SetErrorMessage(Application.UnavailableMessage);
                return;
            }

            BuildTreeView(CmbClass.Text.Trim());
        }
    }
}
