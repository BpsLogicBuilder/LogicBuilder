﻿using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Reflection;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using System;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows.Forms;
using Telerik.WinControls;
using System.ComponentModel;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls
{
    internal partial class ApplicationDropDownList : UserControl, IApplicationDropDownList
    {
        private readonly IApplicationTypeInfoManager _applicationTypeInfoManager;
        private readonly IConfigurationService _configurationService;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IApplicationHostControl applicationHostControl;

        private ApplicationTypeInfo _application;
        public ApplicationTypeInfo Application => _application;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object SelectedValue
        {
            get => cmbApplication.SelectedValue; 
            set => cmbApplication.SelectedValue = value;
        }

        public ApplicationDropDownList(
            IApplicationTypeInfoManager applicationTypeInfoManager,
            IConfigurationService configurationService,
            IExceptionHelper exceptionHelper,
            IApplicationHostControl applicationHostControl)
        {
            InitializeComponent();
            _applicationTypeInfoManager = applicationTypeInfoManager;
            _configurationService = configurationService;
            _exceptionHelper = exceptionHelper;
            this.applicationHostControl = applicationHostControl;
            Initialize();
        }

        public event EventHandler<ApplicationChangedEventArgs>? ApplicationChanged;

        [MemberNotNull(nameof(_application))]
        private void Initialize()
        {
            cmbApplication.AutoSize = false;
            cmbApplication.SelectedIndexChanged += CmbApplication_SelectedIndexChanged;
            Disposed += ApplicationDropDownList_Disposed;
            LoadApplicationsComboBox();
        }

        [MemberNotNull(nameof(_application))]
        private void LoadApplicationsComboBox()
        {
            cmbApplication.DropDownStyle = RadDropDownStyle.DropDownList;
            cmbApplication.Items.AddRange
            (
                _configurationService.ProjectProperties.ApplicationList
                    .Select(kvp => kvp.Value.Name)
                    .OrderBy(n => n)
            );
            cmbApplication.SelectedIndex = 0;

            Configuration.Application? application = _configurationService.GetApplication(cmbApplication.SelectedItem.Text) ?? throw _exceptionHelper.CriticalException("{824AB5B6-7ED3-4265-905F-CD211033D7CE}");
            _application = _applicationTypeInfoManager.GetApplicationTypeInfo(application.Name);
            if (!_application.AssemblyAvailable)
            {
                applicationHostControl.SetErrorMessage(_application.UnavailableMessage);
            }
        }

        private void RemoveEventHandlers()
        {
            cmbApplication.SelectedIndexChanged -= CmbApplication_SelectedIndexChanged;
        }

        private void SelectedApplicationChanged()
        {
            Configuration.Application? application = _configurationService.GetApplication(cmbApplication.SelectedItem.Text) ?? throw _exceptionHelper.CriticalException("{E6CC4E1A-ECEE-4BA9-A2CE-5FFC515C8EE6}");
            applicationHostControl.ClearMessage();

            _application = _applicationTypeInfoManager.GetApplicationTypeInfo(application.Name);
            if (!_application.AssemblyAvailable)
            {
                applicationHostControl.SetErrorMessage(_application.UnavailableMessage);
            }
            
            ApplicationChanged?.Invoke(this, new ApplicationChangedEventArgs(_application));
        }

        #region Event Handlers
        private void ApplicationDropDownList_Disposed(object? sender, EventArgs e)
        {
            RemoveEventHandlers();
        }

        private void CmbApplication_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e) => SelectedApplicationChanged();
        #endregion Event Handlers
    }
}
