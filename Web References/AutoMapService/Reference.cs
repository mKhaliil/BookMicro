﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.42000.
// 
#pragma warning disable 1591

namespace Outsourcing_System.AutoMapService {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2556.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="AutoMappServiceSoap", Namespace="http://tempuri.org/")]
    public partial class AutoMappService : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback MergMethodOperationCompleted;
        
        private System.Threading.SendOrPostCallback CreateTaggingUntaggingOperationCompleted;
        
        private System.Threading.SendOrPostCallback AutoProcessAllOperationCompleted;
        
        private System.Threading.SendOrPostCallback ResetTaskOperationCompleted;
        
        private System.Threading.SendOrPostCallback InsertVolumeBreaksOperationCompleted;
        
        private System.Threading.SendOrPostCallback FinalizeBookOperationCompleted;
        
        private System.Threading.SendOrPostCallback ManualCorrectionsOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public AutoMappService() {
            this.Url = "http://localhost:11533/AutoMappService/AutoMappService.asmx";
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
        public event MergMethodCompletedEventHandler MergMethodCompleted;
        
        /// <remarks/>
        public event CreateTaggingUntaggingCompletedEventHandler CreateTaggingUntaggingCompleted;
        
        /// <remarks/>
        public event AutoProcessAllCompletedEventHandler AutoProcessAllCompleted;
        
        /// <remarks/>
        public event ResetTaskCompletedEventHandler ResetTaskCompleted;
        
        /// <remarks/>
        public event InsertVolumeBreaksCompletedEventHandler InsertVolumeBreaksCompleted;
        
        /// <remarks/>
        public event FinalizeBookCompletedEventHandler FinalizeBookCompleted;
        
        /// <remarks/>
        public event ManualCorrectionsCompletedEventHandler ManualCorrectionsCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/MergMethod", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string MergMethod(string pdfFilePath) {
            object[] results = this.Invoke("MergMethod", new object[] {
                        pdfFilePath});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void MergMethodAsync(string pdfFilePath) {
            this.MergMethodAsync(pdfFilePath, null);
        }
        
        /// <remarks/>
        public void MergMethodAsync(string pdfFilePath, object userState) {
            if ((this.MergMethodOperationCompleted == null)) {
                this.MergMethodOperationCompleted = new System.Threading.SendOrPostCallback(this.OnMergMethodOperationCompleted);
            }
            this.InvokeAsync("MergMethod", new object[] {
                        pdfFilePath}, this.MergMethodOperationCompleted, userState);
        }
        
        private void OnMergMethodOperationCompleted(object arg) {
            if ((this.MergMethodCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.MergMethodCompleted(this, new MergMethodCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/CreateTaggingUntagging", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string CreateTaggingUntagging(string adminUser, string bookTitle, string pdfPath) {
            object[] results = this.Invoke("CreateTaggingUntagging", new object[] {
                        adminUser,
                        bookTitle,
                        pdfPath});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void CreateTaggingUntaggingAsync(string adminUser, string bookTitle, string pdfPath) {
            this.CreateTaggingUntaggingAsync(adminUser, bookTitle, pdfPath, null);
        }
        
        /// <remarks/>
        public void CreateTaggingUntaggingAsync(string adminUser, string bookTitle, string pdfPath, object userState) {
            if ((this.CreateTaggingUntaggingOperationCompleted == null)) {
                this.CreateTaggingUntaggingOperationCompleted = new System.Threading.SendOrPostCallback(this.OnCreateTaggingUntaggingOperationCompleted);
            }
            this.InvokeAsync("CreateTaggingUntagging", new object[] {
                        adminUser,
                        bookTitle,
                        pdfPath}, this.CreateTaggingUntaggingOperationCompleted, userState);
        }
        
        private void OnCreateTaggingUntaggingOperationCompleted(object arg) {
            if ((this.CreateTaggingUntaggingCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.CreateTaggingUntaggingCompleted(this, new CreateTaggingUntaggingCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/AutoProcessAll", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public void AutoProcessAll(
                    string adminUser, 
                    string bookTitle, 
                    string pdfPath, 
                    bool isIndex, 
                    int indexStart, 
                    int indexEnd, 
                    bool isImage, 
                    bool isTable, 
                    bool Algo1, 
                    bool Algo2, 
                    bool Algo3, 
                    bool isNPara, 
                    bool nParaAlgo1, 
                    bool nParaAlgo2, 
                    bool isSPara, 
                    bool isFootNotes) {
            this.Invoke("AutoProcessAll", new object[] {
                        adminUser,
                        bookTitle,
                        pdfPath,
                        isIndex,
                        indexStart,
                        indexEnd,
                        isImage,
                        isTable,
                        Algo1,
                        Algo2,
                        Algo3,
                        isNPara,
                        nParaAlgo1,
                        nParaAlgo2,
                        isSPara,
                        isFootNotes});
        }
        
        /// <remarks/>
        public void AutoProcessAllAsync(
                    string adminUser, 
                    string bookTitle, 
                    string pdfPath, 
                    bool isIndex, 
                    int indexStart, 
                    int indexEnd, 
                    bool isImage, 
                    bool isTable, 
                    bool Algo1, 
                    bool Algo2, 
                    bool Algo3, 
                    bool isNPara, 
                    bool nParaAlgo1, 
                    bool nParaAlgo2, 
                    bool isSPara, 
                    bool isFootNotes) {
            this.AutoProcessAllAsync(adminUser, bookTitle, pdfPath, isIndex, indexStart, indexEnd, isImage, isTable, Algo1, Algo2, Algo3, isNPara, nParaAlgo1, nParaAlgo2, isSPara, isFootNotes, null);
        }
        
        /// <remarks/>
        public void AutoProcessAllAsync(
                    string adminUser, 
                    string bookTitle, 
                    string pdfPath, 
                    bool isIndex, 
                    int indexStart, 
                    int indexEnd, 
                    bool isImage, 
                    bool isTable, 
                    bool Algo1, 
                    bool Algo2, 
                    bool Algo3, 
                    bool isNPara, 
                    bool nParaAlgo1, 
                    bool nParaAlgo2, 
                    bool isSPara, 
                    bool isFootNotes, 
                    object userState) {
            if ((this.AutoProcessAllOperationCompleted == null)) {
                this.AutoProcessAllOperationCompleted = new System.Threading.SendOrPostCallback(this.OnAutoProcessAllOperationCompleted);
            }
            this.InvokeAsync("AutoProcessAll", new object[] {
                        adminUser,
                        bookTitle,
                        pdfPath,
                        isIndex,
                        indexStart,
                        indexEnd,
                        isImage,
                        isTable,
                        Algo1,
                        Algo2,
                        Algo3,
                        isNPara,
                        nParaAlgo1,
                        nParaAlgo2,
                        isSPara,
                        isFootNotes}, this.AutoProcessAllOperationCompleted, userState);
        }
        
        private void OnAutoProcessAllOperationCompleted(object arg) {
            if ((this.AutoProcessAllCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.AutoProcessAllCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/ResetTask", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public void ResetTask(string pdfPath, bool isSplitMerge, bool isIndex, int indexStart, int indexEnd, bool isImage, bool isTable, bool Algo1, bool Algo2, bool Algo3, bool isNPara, bool nParaAlgo1, bool nParaAlgo2, bool isSPara, bool isFootNotes) {
            this.Invoke("ResetTask", new object[] {
                        pdfPath,
                        isSplitMerge,
                        isIndex,
                        indexStart,
                        indexEnd,
                        isImage,
                        isTable,
                        Algo1,
                        Algo2,
                        Algo3,
                        isNPara,
                        nParaAlgo1,
                        nParaAlgo2,
                        isSPara,
                        isFootNotes});
        }
        
        /// <remarks/>
        public void ResetTaskAsync(string pdfPath, bool isSplitMerge, bool isIndex, int indexStart, int indexEnd, bool isImage, bool isTable, bool Algo1, bool Algo2, bool Algo3, bool isNPara, bool nParaAlgo1, bool nParaAlgo2, bool isSPara, bool isFootNotes) {
            this.ResetTaskAsync(pdfPath, isSplitMerge, isIndex, indexStart, indexEnd, isImage, isTable, Algo1, Algo2, Algo3, isNPara, nParaAlgo1, nParaAlgo2, isSPara, isFootNotes, null);
        }
        
        /// <remarks/>
        public void ResetTaskAsync(
                    string pdfPath, 
                    bool isSplitMerge, 
                    bool isIndex, 
                    int indexStart, 
                    int indexEnd, 
                    bool isImage, 
                    bool isTable, 
                    bool Algo1, 
                    bool Algo2, 
                    bool Algo3, 
                    bool isNPara, 
                    bool nParaAlgo1, 
                    bool nParaAlgo2, 
                    bool isSPara, 
                    bool isFootNotes, 
                    object userState) {
            if ((this.ResetTaskOperationCompleted == null)) {
                this.ResetTaskOperationCompleted = new System.Threading.SendOrPostCallback(this.OnResetTaskOperationCompleted);
            }
            this.InvokeAsync("ResetTask", new object[] {
                        pdfPath,
                        isSplitMerge,
                        isIndex,
                        indexStart,
                        indexEnd,
                        isImage,
                        isTable,
                        Algo1,
                        Algo2,
                        Algo3,
                        isNPara,
                        nParaAlgo1,
                        nParaAlgo2,
                        isSPara,
                        isFootNotes}, this.ResetTaskOperationCompleted, userState);
        }
        
        private void OnResetTaskOperationCompleted(object arg) {
            if ((this.ResetTaskCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.ResetTaskCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/InsertVolumeBreaks", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string InsertVolumeBreaks(string rhywPath) {
            object[] results = this.Invoke("InsertVolumeBreaks", new object[] {
                        rhywPath});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void InsertVolumeBreaksAsync(string rhywPath) {
            this.InsertVolumeBreaksAsync(rhywPath, null);
        }
        
        /// <remarks/>
        public void InsertVolumeBreaksAsync(string rhywPath, object userState) {
            if ((this.InsertVolumeBreaksOperationCompleted == null)) {
                this.InsertVolumeBreaksOperationCompleted = new System.Threading.SendOrPostCallback(this.OnInsertVolumeBreaksOperationCompleted);
            }
            this.InvokeAsync("InsertVolumeBreaks", new object[] {
                        rhywPath}, this.InsertVolumeBreaksOperationCompleted, userState);
        }
        
        private void OnInsertVolumeBreaksOperationCompleted(object arg) {
            if ((this.InsertVolumeBreaksCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.InsertVolumeBreaksCompleted(this, new InsertVolumeBreaksCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/FinalizeBook", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string FinalizeBook(string rhywFile) {
            object[] results = this.Invoke("FinalizeBook", new object[] {
                        rhywFile});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void FinalizeBookAsync(string rhywFile) {
            this.FinalizeBookAsync(rhywFile, null);
        }
        
        /// <remarks/>
        public void FinalizeBookAsync(string rhywFile, object userState) {
            if ((this.FinalizeBookOperationCompleted == null)) {
                this.FinalizeBookOperationCompleted = new System.Threading.SendOrPostCallback(this.OnFinalizeBookOperationCompleted);
            }
            this.InvokeAsync("FinalizeBook", new object[] {
                        rhywFile}, this.FinalizeBookOperationCompleted, userState);
        }
        
        private void OnFinalizeBookOperationCompleted(object arg) {
            if ((this.FinalizeBookCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.FinalizeBookCompleted(this, new FinalizeBookCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/ManualCorrections", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public void ManualCorrections(string rhywPath) {
            this.Invoke("ManualCorrections", new object[] {
                        rhywPath});
        }
        
        /// <remarks/>
        public void ManualCorrectionsAsync(string rhywPath) {
            this.ManualCorrectionsAsync(rhywPath, null);
        }
        
        /// <remarks/>
        public void ManualCorrectionsAsync(string rhywPath, object userState) {
            if ((this.ManualCorrectionsOperationCompleted == null)) {
                this.ManualCorrectionsOperationCompleted = new System.Threading.SendOrPostCallback(this.OnManualCorrectionsOperationCompleted);
            }
            this.InvokeAsync("ManualCorrections", new object[] {
                        rhywPath}, this.ManualCorrectionsOperationCompleted, userState);
        }
        
        private void OnManualCorrectionsOperationCompleted(object arg) {
            if ((this.ManualCorrectionsCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.ManualCorrectionsCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2556.0")]
    public delegate void MergMethodCompletedEventHandler(object sender, MergMethodCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2556.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class MergMethodCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal MergMethodCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2556.0")]
    public delegate void CreateTaggingUntaggingCompletedEventHandler(object sender, CreateTaggingUntaggingCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2556.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class CreateTaggingUntaggingCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal CreateTaggingUntaggingCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2556.0")]
    public delegate void AutoProcessAllCompletedEventHandler(object sender, System.ComponentModel.AsyncCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2556.0")]
    public delegate void ResetTaskCompletedEventHandler(object sender, System.ComponentModel.AsyncCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2556.0")]
    public delegate void InsertVolumeBreaksCompletedEventHandler(object sender, InsertVolumeBreaksCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2556.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class InsertVolumeBreaksCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal InsertVolumeBreaksCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2556.0")]
    public delegate void FinalizeBookCompletedEventHandler(object sender, FinalizeBookCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2556.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class FinalizeBookCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal FinalizeBookCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2556.0")]
    public delegate void ManualCorrectionsCompletedEventHandler(object sender, System.ComponentModel.AsyncCompletedEventArgs e);
}

#pragma warning restore 1591