﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18408
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.18408.
// 
#pragma warning disable 1591

namespace Outsourcing_System.AutoTableDetection {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.18408")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="AutoTableDetectionSoap", Namespace="http://tempuri.org/")]
    public partial class AutoTableDetection : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback AutoDetectTablesOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public AutoTableDetection() {
            this.Url = "http://localhost:48750/AutoTableDetection.asmx";
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event AutoDetectTablesCompletedEventHandler AutoDetectTablesCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/AutoDetectTables", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public bool AutoDetectTables(string bookId, string filesSavingPath, bool cbxAlgo1, bool cbxAlgo2, bool cbxAlgo3) {
            object[] results = this.Invoke("AutoDetectTables", new object[] {
                        bookId,
                        filesSavingPath,
                        cbxAlgo1,
                        cbxAlgo2,
                        cbxAlgo3});
            return ((bool)(results[0]));
        }
        
        /// <remarks/>
        public void AutoDetectTablesAsync(string bookId, string filesSavingPath, bool cbxAlgo1, bool cbxAlgo2, bool cbxAlgo3) {
            this.AutoDetectTablesAsync(bookId, filesSavingPath, cbxAlgo1, cbxAlgo2, cbxAlgo3, null);
        }
        
        /// <remarks/>
        public void AutoDetectTablesAsync(string bookId, string filesSavingPath, bool cbxAlgo1, bool cbxAlgo2, bool cbxAlgo3, object userState) {
            if ((this.AutoDetectTablesOperationCompleted == null)) {
                this.AutoDetectTablesOperationCompleted = new System.Threading.SendOrPostCallback(this.OnAutoDetectTablesOperationCompleted);
            }
            this.InvokeAsync("AutoDetectTables", new object[] {
                        bookId,
                        filesSavingPath,
                        cbxAlgo1,
                        cbxAlgo2,
                        cbxAlgo3}, this.AutoDetectTablesOperationCompleted, userState);
        }
        
        private void OnAutoDetectTablesOperationCompleted(object arg) {
            if ((this.AutoDetectTablesCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.AutoDetectTablesCompleted(this, new AutoDetectTablesCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.18408")]
    public delegate void AutoDetectTablesCompletedEventHandler(object sender, AutoDetectTablesCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.18408")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class AutoDetectTablesCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal AutoDetectTablesCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public bool Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((bool)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591