﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.1434
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace kaor.I18N {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "2.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Locale {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Locale() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("kaor.I18N.Locale", typeof(Locale).Assembly);
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
        ///   Looks up a localized string similar to It seems that KAOR RCS is already running!
        ///Please close other instances of the software..
        /// </summary>
        internal static string already_running {
            get {
                return ResourceManager.GetString("already_running", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error.
        /// </summary>
        internal static string error {
            get {
                return ResourceManager.GetString("error", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Exception information: {0}.
        /// </summary>
        internal static string ex_information {
            get {
                return ResourceManager.GetString("ex_information", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Exception was thrown in {0}.
        /// </summary>
        internal static string ex_method {
            get {
                return ResourceManager.GetString("ex_method", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Object throwed exception: {0}.
        /// </summary>
        internal static string ex_source {
            get {
                return ResourceManager.GetString("ex_source", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Stack: {0}.
        /// </summary>
        internal static string ex_stack {
            get {
                return ResourceManager.GetString("ex_stack", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Exception type: {0}.
        /// </summary>
        internal static string ex_type {
            get {
                return ResourceManager.GetString("ex_type", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Loading user interface....
        /// </summary>
        internal static string status_loading_ui {
            get {
                return ResourceManager.GetString("status_loading_ui", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to You can try  to reload automatically saved system state in &quot;{0}&quot;..
        /// </summary>
        internal static string system_state {
            get {
                return ResourceManager.GetString("system_state", resourceCulture);
            }
        }
    }
}
