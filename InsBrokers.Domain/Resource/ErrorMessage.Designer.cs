﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace InsBrokers.Domain.Resource {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class ErrorMessage {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ErrorMessage() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("InsBrokers.Domain.Resource.ErrorMessage", typeof(ErrorMessage).Assembly);
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
        ///   Looks up a localized string similar to شماره حساب باید حتما به صورت عدد وارد شود.
        /// </summary>
        public static string AccountNumberIsInt {
            get {
                return ResourceManager.GetString("AccountNumberIsInt", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to شماره کارت وارد شده صحیح نمی باشد.
        /// </summary>
        public static string CardNumberFormat {
            get {
                return ResourceManager.GetString("CardNumberFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to حتما عدد وارد نمایید..
        /// </summary>
        public static string IntRequired {
            get {
                return ResourceManager.GetString("IntRequired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to تاریخ شمسی به صورت صحیح وارد نشده است، به عنوان مثال 1400/02/03.
        /// </summary>
        public static string InvalidDate {
            get {
                return ResourceManager.GetString("InvalidDate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to آدرس ایمیل باید به صورت کامل وارد شود، به عنوان مثال xxxx@xxxx.com.
        /// </summary>
        public static string InvalidEmail {
            get {
                return ResourceManager.GetString("InvalidEmail", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to شماره موبایل صحیح نیست، به عنوان مثال 09301234567.
        /// </summary>
        public static string InvalidMobileNumber {
            get {
                return ResourceManager.GetString("InvalidMobileNumber", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to کد ملی باید 10 کاراکتر کامل باشد.
        /// </summary>
        public static string InvalidNationalCode {
            get {
                return ResourceManager.GetString("InvalidNationalCode", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to شبا باید با IR شروع شده و شامل 24 عدد باشد.
        /// </summary>
        public static string InvalidShaba {
            get {
                return ResourceManager.GetString("InvalidShaba", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to لطفا بیشتر از {1} کاراکتر وارد نکنید.
        /// </summary>
        public static string MaxLength {
            get {
                return ResourceManager.GetString("MaxLength", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to لطفا کمتر از {1} کاراکتر وارد نکنید.
        /// </summary>
        public static string MinLength {
            get {
                return ResourceManager.GetString("MinLength", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to این فیلد اجباری است.
        /// </summary>
        public static string Required {
            get {
                return ResourceManager.GetString("Required", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ایمیل وارد شده صحیح نمی باشد.
        /// </summary>
        public static string WrongEmailFormat {
            get {
                return ResourceManager.GetString("WrongEmailFormat", resourceCulture);
            }
        }
    }
}
