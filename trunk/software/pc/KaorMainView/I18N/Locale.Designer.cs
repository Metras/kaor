﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.1434
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace KaorMainView.I18N {
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
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("KaorMainView.I18N.Locale", typeof(Locale).Assembly);
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
        ///   Looks up a localized string similar to Do you really want to exit?.
        /// </summary>
        internal static string confirm_quit {
            get {
                return ResourceManager.GetString("confirm_quit", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Confirmation.
        /// </summary>
        internal static string confirmation {
            get {
                return ResourceManager.GetString("confirmation", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to KAOR Radio Monitoring System v{0} build {1}.
        /// </summary>
        internal static string kaor_name {
            get {
                return ResourceManager.GetString("kaor_name", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to System is not configured.
        ///Do you want to config it now?.
        /// </summary>
        internal static string not_configured {
            get {
                return ResourceManager.GetString("not_configured", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Question.
        /// </summary>
        internal static string question {
            get {
                return ResourceManager.GetString("question", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Configuration has changed. You need to restart system..
        /// </summary>
        internal static string restart {
            get {
                return ResourceManager.GetString("restart", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to System loaded!.
        /// </summary>
        internal static string status_system_loaded {
            get {
                return ResourceManager.GetString("status_system_loaded", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to System crash detected. Do you want to load last system state?.
        /// </summary>
        internal static string system_crashed {
            get {
                return ResourceManager.GetString("system_crashed", resourceCulture);
            }
        }
    }
}