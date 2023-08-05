using System;
using System.Reflection;
using System.Runtime.InteropServices;

//[assembly: SecurityPermission(SecurityAction.RequestMinimum, Execution = true)]
[assembly: CLSCompliant(true)]
//[assembly: FileIOPermission(SecurityAction.RequestOptional, Unrestricted = true)]
// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("BPS Logic Builder")]
[assembly: AssemblyDescription("BPS Logic Builder™ provides the context for dynamically generating forms and queries using flow diagrams.")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Business Policy Solutions")]
[assembly: AssemblyProduct("BPS Logic Builder")]
[assembly: AssemblyCopyright("Copyright © Business Policy Solutions 2018")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("b73c70aa-cb26-4d12-b99e-a7f4dc42e213")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
#if (TRIAL)
[assembly: AssemblyVersion("2.0.22.0")]
[assembly: AssemblyFileVersion("2.0.22.0")]
#else
[assembly: AssemblyVersion("3.0.15.0")]
[assembly: AssemblyFileVersion("3.0.15.0")]
#endif
