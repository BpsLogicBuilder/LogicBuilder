using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration
{
    internal class Application : IEquatable<Application>, IComparable<Application>
    {
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        public Application(string name, string nickname, string activityAssembly, string activityAssemblyPath, RuntimeType runtime, List<string> loadAssemblyPaths, string activityClass, string applicationExcecutable, string applicationExcecutablePath, List<string> startupArguments, string resourceFile, string resourceFileDeploymentPath, string rulesFile, string rulesDeploymentPath, List<string> modules, WebApiDeployment webApiDeployment, IContextProvider contextProvider)
        {
            this.Name = name;
            this.Nickname = nickname;
            this.ActivityAssembly = activityAssembly;
            this.ActivityAssemblyPath = activityAssemblyPath;
            this.LoadAssemblyPaths = loadAssemblyPaths;
            this.ActivityClass = activityClass;
            this.ApplicationExcecutable = applicationExcecutable;
            this.ApplicationExcecutablePath = applicationExcecutablePath;
            this.Runtime = runtime;
            this.StartupArguments = startupArguments;
            this.ResourceFile = resourceFile;
            this.ResourceFileDeploymentPath = resourceFileDeploymentPath;
            this.RulesFile = rulesFile;
            this.RulesDeploymentPath = rulesDeploymentPath;
            this.ExcludedModules = modules;
            this.WebApiDeployment = webApiDeployment;
            this._xmlDocumentHelpers = contextProvider.XmlDocumentHelpers;
        }

        #region Properties
        /// <summary>
        /// Unique name for this application
        /// </summary>
        internal string Name { get; }

        /// <summary>
        /// Unique name for this application
        /// </summary>
        internal string Nickname { get; }

        /// <summary>
        /// Activity's assembly
        /// </summary>
        internal string ActivityAssembly { get; }

        /// <summary>
        /// Path to the activity's assembly
        /// </summary>
        internal string ActivityAssemblyPath { get; }

        /// <summary>
        /// Runtime (.Net Framework, .Net Core, or Xamarin)
        /// </summary>
        internal RuntimeType Runtime { get; }

        /// <summary>
        /// Load Assembly Paths
        /// </summary>
        internal List<string> LoadAssemblyPaths { get; }

        /// <summary>
        /// Fully qualified name of the activity the rules run against
        /// </summary>
        internal string ActivityClass { get; }

        /// <summary>
        /// Executable using the generated rules
        /// </summary>
        internal string ApplicationExcecutable { get; }

        /// <summary>
        /// Path to executable using the generated rules
        /// </summary>
        internal string ApplicationExcecutablePath { get; }

        /// <summary>
        /// Startup Arguments
        /// </summary>
        internal List<string> StartupArguments { get; }

        /// <summary>
        /// Resource File
        /// </summary>
        internal string ResourceFile { get; }

        /// <summary>
        /// Path to which Logic Builder may deploy the resource file for this application
        /// </summary>
        internal string ResourceFileDeploymentPath { get; }

        /// <summary>
        /// Rules File Name
        /// </summary>
        internal string RulesFile { get; }

        /// <summary>
        /// Path to which Logic Builder may deploy the rules for this application
        /// </summary>
        internal string RulesDeploymentPath { get; }

        /// <summary>
        /// Modules in this application
        /// </summary>
        internal List<string> ExcludedModules { get; }

        /// <summary>
        ///  Web API Configuration for remote deployment
        /// </summary>
        internal WebApiDeployment WebApiDeployment { get; }

        internal string ToXml => BuildXml();
        #endregion Properties

        #region IEquatable<Application> Members
        public bool Equals(Application other)
        {
            return Name.Equals(other.Name);
        }
        #endregion

        #region IComparable<Application> Members
        public int CompareTo(Application other)
        {
            return Name.CompareTo(other.Name);
        }
        #endregion

        #region Methods
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj is not Application)
                return false;

            return this.Equals((Application)obj);
        }

        public override int GetHashCode()
        {
            return this.Name.GetHashCode();
        }

        private string BuildXml()
        {
            StringBuilder stringBuilder = new();
            using (XmlWriter xmlTextWriter = this._xmlDocumentHelpers.CreateUnformattedXmlWriter(stringBuilder))
            {
                xmlTextWriter.WriteStartElement(XmlDataConstants.APPLICATIONELEMENT);
                    xmlTextWriter.WriteAttributeString(XmlDataConstants.NAMEATTRIBUTE, this.Name);
                    xmlTextWriter.WriteAttributeString(XmlDataConstants.NICKNAMEATTRIBUTE, this.Nickname);

                    xmlTextWriter.WriteElementString(XmlDataConstants.ACTIVITYASSEMBLYELEMENT, this.ActivityAssembly);
                    xmlTextWriter.WriteElementString(XmlDataConstants.ACTIVITYASSEMBLYPATHELEMENT, this.ActivityAssemblyPath);
                    xmlTextWriter.WriteElementString(XmlDataConstants.RUNTIMEELEMENT, Enum.GetName(typeof(RuntimeType), this.Runtime));
                    xmlTextWriter.WriteStartElement(XmlDataConstants.LOADASSEMBLYPATHSELEMENT);
                        foreach (string argument in this.LoadAssemblyPaths)
                        {
                            xmlTextWriter.WriteElementString(XmlDataConstants.PATHELEMENT, argument);
                        }
                    xmlTextWriter.WriteEndElement();
                    xmlTextWriter.WriteElementString(XmlDataConstants.ACTIVITYCLASSELEMENT, this.ActivityClass);
                    xmlTextWriter.WriteElementString(XmlDataConstants.APPLICATIONEXECUTABLEELEMENT, this.ApplicationExcecutable);
                    xmlTextWriter.WriteElementString(XmlDataConstants.APPLICATIONEXECUTABLEPATHELEMENT, this.ApplicationExcecutablePath);
                    xmlTextWriter.WriteStartElement(XmlDataConstants.STARTUPARGUMENTSELEMENT);
                        foreach (string argument in this.StartupArguments)
                        {
                            xmlTextWriter.WriteElementString(XmlDataConstants.ARGUMENTELEMENT, argument);
                        }
                    xmlTextWriter.WriteEndElement();
                    xmlTextWriter.WriteElementString(XmlDataConstants.RESOURCEFILEELEMENT, this.ResourceFile);
                    xmlTextWriter.WriteElementString(XmlDataConstants.RESOURCEFILEDEPLOYMENTPATHELEMENT, this.ResourceFileDeploymentPath);
                    xmlTextWriter.WriteElementString(XmlDataConstants.RULESFILEELEMENT, this.RulesFile);
                    xmlTextWriter.WriteElementString(XmlDataConstants.RULESDEPLOYMENTPATHELEMENT, this.RulesDeploymentPath);
                    xmlTextWriter.WriteStartElement(XmlDataConstants.EXCLUDEDMODULESELEMENT);
                        foreach (string module in this.ExcludedModules)
                        {
                            xmlTextWriter.WriteElementString(XmlDataConstants.MODULEELEMENT, module);
                        }
                    xmlTextWriter.WriteEndElement();

                    xmlTextWriter.WriteStartElement(XmlDataConstants.REMOTEDEPLOYMENTELEMENT);
                        xmlTextWriter.WriteAttributeString(XmlDataConstants.ENDPOINTADDRESSATTRIBUTE, string.Empty);
                        xmlTextWriter.WriteAttributeString(XmlDataConstants.SECURITYMODEATTRIBUTE, "Message");
                        xmlTextWriter.WriteAttributeString(XmlDataConstants.DNSVALUEATTRIBUTE, string.Empty);
                        xmlTextWriter.WriteElementString(XmlDataConstants.USERNAMEELEMENT, string.Empty);
                        xmlTextWriter.WriteElementString(XmlDataConstants.PASSWORDELEMENT, string.Empty);
                        xmlTextWriter.WriteStartElement(XmlDataConstants.ADDRESSHEADERSELEMENT);
                        xmlTextWriter.WriteEndElement();
                    xmlTextWriter.WriteEndElement();

                    xmlTextWriter.WriteRaw(this.WebApiDeployment.ToXml);

                xmlTextWriter.WriteEndElement();
                xmlTextWriter.Flush();
                xmlTextWriter.Close();
            }
            return stringBuilder.ToString();
        }
        #endregion Methods
    }
}
