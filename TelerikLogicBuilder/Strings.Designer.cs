﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ABIS.LogicBuilder.FlowBuilder {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Strings {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Strings() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("ABIS.LogicBuilder.FlowBuilder.Strings", typeof(Strings).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to BPS Logic Builder.
        /// </summary>
        internal static string applicationNameLogicBuilder {
            get {
                return ResourceManager.GetString("applicationNameLogicBuilder", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An error has occurred..
        /// </summary>
        internal static string defaultErrorMessage {
            get {
                return ResourceManager.GetString("defaultErrorMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid Argument {0}..
        /// </summary>
        internal static string invalidArgumentTextFormat {
            get {
                return ResourceManager.GetString("invalidArgumentTextFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Method not implemented in {0}..
        /// </summary>
        internal static string notImplementedMessageFormat {
            get {
                return ResourceManager.GetString("notImplementedMessageFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to  .
        /// </summary>
        internal static string spaceString {
            get {
                return ResourceManager.GetString("spaceString", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to _.
        /// </summary>
        internal static string underscore {
            get {
                return ResourceManager.GetString("underscore", resourceCulture);
            }
        }
    }
}
