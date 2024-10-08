﻿using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System.Drawing;
using System.Globalization;
using System.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.Forms
{
    partial class LogicBuilderAboutBox : Telerik.WinControls.UI.RadForm, ILogicBuilderAboutBox
    {
        private readonly IFormInitializer _formInitializer;
        public LogicBuilderAboutBox(IFormInitializer formInitializer)
        {
            InitializeComponent();
            _formInitializer = formInitializer;

            //  Initialize the AboutBox to display the product information from the assembly information.
            //  Change assembly information settings for your application through either:
            //  - Project->Properties->Application->Assembly Information
            //  - AssemblyInfo.cs
            this.Text = string.Format(CultureInfo.CurrentCulture, Strings.aboutStringFormat, AssemblyTitle);
            this.radLabelProductName.Text = AssemblyProduct;
            this.radLabelVersion.Text = string.Format(CultureInfo.CurrentCulture, Strings.versionStringFormat, AssemblyVersion);
            this.radLabelCopyright.Text = AssemblyCopyright;
            this.radLabelCompanyName.Text = AssemblyCompany;
            this.radTextBoxDescription.Text = AssemblyDescription;
            Initialize();
        }


        #region Assembly Attribute Accessors

        public string AssemblyTitle
        {
            get
            {
                // Get all Title attributes on this assembly
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                // If there is at least one Title attribute
                if (attributes.Length > 0)
                {
                    // Select the first one
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    // If it is not an empty string, return it
                    if (titleAttribute.Title != "")
                        return titleAttribute.Title;
                }
                // If there was no Title attribute, or if the Title attribute was the empty string, return the .exe name
                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().Location) ?? string.Empty;
            }
        }

        public string AssemblyVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? string.Empty;
            }
        }

        public string AssemblyDescription
        {
            get
            {
                // Get all Description attributes on this assembly
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                // If there aren't any Description attributes, return an empty string
                if (attributes.Length == 0)
                    return "";
                // If there is a Description attribute, return its value
                return ((AssemblyDescriptionAttribute)attributes[0]).Description;
            }
        }

        public string AssemblyProduct
        {
            get
            {
                // Get all Product attributes on this assembly
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                // If there aren't any Product attributes, return an empty string
                if (attributes.Length == 0)
                    return "";
                // If there is a Product attribute, return its value
                return ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        public string AssemblyCopyright
        {
            get
            {
                // Get all Copyright attributes on this assembly
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                // If there aren't any Copyright attributes, return an empty string
                if (attributes.Length == 0)
                    return "";
                // If there is a Copyright attribute, return its value
                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        public string AssemblyCompany
        {
            get
            {
                // Get all Company attributes on this assembly
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                // If there aren't any Company attributes, return an empty string
                if (attributes.Length == 0)
                    return "";
                // If there is a Company attribute, return its value
                return ((AssemblyCompanyAttribute)attributes[0]).Company;
            }
        }
        #endregion

        #region Methods
        private void Initialize()
        {
            this.Icon = _formInitializer.GetLogicBuilderIcon();
            okRadButton.Size = new Size(okRadButton.Width, (int)PerFontSizeConstants.SingleLineHeight);
        }
        #endregion Methods
    }
}
