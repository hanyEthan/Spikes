﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ADS.Tamam.Resources.Culture {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Personnel {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Personnel() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("ADS.Tamam.Resources.Culture.Personnel", typeof(Personnel).Assembly);
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
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Active.
        /// </summary>
        public static string Active {
            get {
                return ResourceManager.GetString("Active", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid Person.
        /// </summary>
        public static string InvalidPerson {
            get {
                return ResourceManager.GetString("InvalidPerson", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to You cannot change your status.
        /// </summary>
        public static string InvalidPersonToActivateOrDeactivate {
            get {
                return ResourceManager.GetString("InvalidPersonToActivateOrDeactivate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Not Active.
        /// </summary>
        public static string NotActive {
            get {
                return ResourceManager.GetString("NotActive", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Person Cannot report to multiple persons.
        /// </summary>
        public static string PersonMultiReportTo {
            get {
                return ResourceManager.GetString("PersonMultiReportTo", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This Reporting To Person is not valid.
        /// </summary>
        public static string PersonReportingToCyclicdependency {
            get {
                return ResourceManager.GetString("PersonReportingToCyclicdependency", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Person Cannot report to himself.
        /// </summary>
        public static string PersonSelfReportingTo {
            get {
                return ResourceManager.GetString("PersonSelfReportingTo", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Search Persons Failed.
        /// </summary>
        public static string SearchPersonsFailed {
            get {
                return ResourceManager.GetString("SearchPersonsFailed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This user is not found.
        /// </summary>
        public static string UserAssociateFail {
            get {
                return ResourceManager.GetString("UserAssociateFail", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid user Name.
        /// </summary>
        public static string UserCreateFail {
            get {
                return ResourceManager.GetString("UserCreateFail", resourceCulture);
            }
        }
    }
}
