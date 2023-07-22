using System;

namespace ABIS.LogicBuilder.FlowBuilder.Forms
{
    internal interface ILogicBuilderAboutBox : IDisposable
    {
        string AssemblyCompany { get; }
        string AssemblyCopyright { get; }
        string AssemblyDescription { get; }
        string AssemblyProduct { get; }
        string AssemblyTitle { get; }
        string AssemblyVersion { get; }
    }
}
