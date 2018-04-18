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

namespace Outsourcing_System.ImageValidator {
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
    [System.Web.Services.WebServiceBindingAttribute(Name="ImageValidationServiceSoap", Namespace="http://tempuri.org/")]
    public partial class ImageValidationService : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback ValidateImagesOperationCompleted;
        
        private System.Threading.SendOrPostCallback ValidateImagesAgainstListOperationCompleted;
        
        private System.Threading.SendOrPostCallback GenearatePDFPreviewOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public ImageValidationService() {
            this.Url = "http://localhost:15691/ImageValidator/ImageValidationService.asmx";
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
        public event ValidateImagesCompletedEventHandler ValidateImagesCompleted;
        
        /// <remarks/>
        public event ValidateImagesAgainstListCompletedEventHandler ValidateImagesAgainstListCompleted;
        
        /// <remarks/>
        public event GenearatePDFPreviewCompletedEventHandler GenearatePDFPreviewCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/ValidateImages", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string ValidateImages(string zipFile) {
            object[] results = this.Invoke("ValidateImages", new object[] {
                        zipFile});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void ValidateImagesAsync(string zipFile) {
            this.ValidateImagesAsync(zipFile, null);
        }
        
        /// <remarks/>
        public void ValidateImagesAsync(string zipFile, object userState) {
            if ((this.ValidateImagesOperationCompleted == null)) {
                this.ValidateImagesOperationCompleted = new System.Threading.SendOrPostCallback(this.OnValidateImagesOperationCompleted);
            }
            this.InvokeAsync("ValidateImages", new object[] {
                        zipFile}, this.ValidateImagesOperationCompleted, userState);
        }
        
        private void OnValidateImagesOperationCompleted(object arg) {
            if ((this.ValidateImagesCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.ValidateImagesCompleted(this, new ValidateImagesCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/ValidateImagesAgainstList", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string ValidateImagesAgainstList(string zipFilePath, long AID) {
            object[] results = this.Invoke("ValidateImagesAgainstList", new object[] {
                        zipFilePath,
                        AID});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void ValidateImagesAgainstListAsync(string zipFilePath, long AID) {
            this.ValidateImagesAgainstListAsync(zipFilePath, AID, null);
        }
        
        /// <remarks/>
        public void ValidateImagesAgainstListAsync(string zipFilePath, long AID, object userState) {
            if ((this.ValidateImagesAgainstListOperationCompleted == null)) {
                this.ValidateImagesAgainstListOperationCompleted = new System.Threading.SendOrPostCallback(this.OnValidateImagesAgainstListOperationCompleted);
            }
            this.InvokeAsync("ValidateImagesAgainstList", new object[] {
                        zipFilePath,
                        AID}, this.ValidateImagesAgainstListOperationCompleted, userState);
        }
        
        private void OnValidateImagesAgainstListOperationCompleted(object arg) {
            if ((this.ValidateImagesAgainstListCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.ValidateImagesAgainstListCompleted(this, new ValidateImagesAgainstListCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/GenearatePDFPreview", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string GenearatePDFPreview(string srcXMLFile, string targetPDFPath) {
            object[] results = this.Invoke("GenearatePDFPreview", new object[] {
                        srcXMLFile,
                        targetPDFPath});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void GenearatePDFPreviewAsync(string srcXMLFile, string targetPDFPath) {
            this.GenearatePDFPreviewAsync(srcXMLFile, targetPDFPath, null);
        }
        
        /// <remarks/>
        public void GenearatePDFPreviewAsync(string srcXMLFile, string targetPDFPath, object userState) {
            if ((this.GenearatePDFPreviewOperationCompleted == null)) {
                this.GenearatePDFPreviewOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGenearatePDFPreviewOperationCompleted);
            }
            this.InvokeAsync("GenearatePDFPreview", new object[] {
                        srcXMLFile,
                        targetPDFPath}, this.GenearatePDFPreviewOperationCompleted, userState);
        }
        
        private void OnGenearatePDFPreviewOperationCompleted(object arg) {
            if ((this.GenearatePDFPreviewCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GenearatePDFPreviewCompleted(this, new GenearatePDFPreviewCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
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
    public delegate void ValidateImagesCompletedEventHandler(object sender, ValidateImagesCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.18408")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ValidateImagesCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal ValidateImagesCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.18408")]
    public delegate void ValidateImagesAgainstListCompletedEventHandler(object sender, ValidateImagesAgainstListCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.18408")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ValidateImagesAgainstListCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal ValidateImagesAgainstListCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.18408")]
    public delegate void GenearatePDFPreviewCompletedEventHandler(object sender, GenearatePDFPreviewCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.18408")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GenearatePDFPreviewCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GenearatePDFPreviewCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591