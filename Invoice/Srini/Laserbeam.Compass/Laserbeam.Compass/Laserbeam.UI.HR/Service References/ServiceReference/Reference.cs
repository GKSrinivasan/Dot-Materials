﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Laserbeam.UI.HR.ServiceReference {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ExceptionLogModel", Namespace="http://schemas.datacontract.org/2004/07/BusinessObject")]
    [System.SerializableAttribute()]
    public partial class ExceptionLogModel : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ApplicationNameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string AssemblyNameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.DateTime ExceptionDateTimeField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ExceptionMessageField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ExceptionTypeField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string InnerExceptionMessageField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string InnerExceptionTypeField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private bool IsServerSideField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string MethodNameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string SessionIDField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string URLField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string UserIDField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ApplicationName {
            get {
                return this.ApplicationNameField;
            }
            set {
                if ((object.ReferenceEquals(this.ApplicationNameField, value) != true)) {
                    this.ApplicationNameField = value;
                    this.RaisePropertyChanged("ApplicationName");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string AssemblyName {
            get {
                return this.AssemblyNameField;
            }
            set {
                if ((object.ReferenceEquals(this.AssemblyNameField, value) != true)) {
                    this.AssemblyNameField = value;
                    this.RaisePropertyChanged("AssemblyName");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime ExceptionDateTime {
            get {
                return this.ExceptionDateTimeField;
            }
            set {
                if ((this.ExceptionDateTimeField.Equals(value) != true)) {
                    this.ExceptionDateTimeField = value;
                    this.RaisePropertyChanged("ExceptionDateTime");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ExceptionMessage {
            get {
                return this.ExceptionMessageField;
            }
            set {
                if ((object.ReferenceEquals(this.ExceptionMessageField, value) != true)) {
                    this.ExceptionMessageField = value;
                    this.RaisePropertyChanged("ExceptionMessage");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ExceptionType {
            get {
                return this.ExceptionTypeField;
            }
            set {
                if ((object.ReferenceEquals(this.ExceptionTypeField, value) != true)) {
                    this.ExceptionTypeField = value;
                    this.RaisePropertyChanged("ExceptionType");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string InnerExceptionMessage {
            get {
                return this.InnerExceptionMessageField;
            }
            set {
                if ((object.ReferenceEquals(this.InnerExceptionMessageField, value) != true)) {
                    this.InnerExceptionMessageField = value;
                    this.RaisePropertyChanged("InnerExceptionMessage");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string InnerExceptionType {
            get {
                return this.InnerExceptionTypeField;
            }
            set {
                if ((object.ReferenceEquals(this.InnerExceptionTypeField, value) != true)) {
                    this.InnerExceptionTypeField = value;
                    this.RaisePropertyChanged("InnerExceptionType");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool IsServerSide {
            get {
                return this.IsServerSideField;
            }
            set {
                if ((this.IsServerSideField.Equals(value) != true)) {
                    this.IsServerSideField = value;
                    this.RaisePropertyChanged("IsServerSide");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string MethodName {
            get {
                return this.MethodNameField;
            }
            set {
                if ((object.ReferenceEquals(this.MethodNameField, value) != true)) {
                    this.MethodNameField = value;
                    this.RaisePropertyChanged("MethodName");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string SessionID {
            get {
                return this.SessionIDField;
            }
            set {
                if ((object.ReferenceEquals(this.SessionIDField, value) != true)) {
                    this.SessionIDField = value;
                    this.RaisePropertyChanged("SessionID");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string URL {
            get {
                return this.URLField;
            }
            set {
                if ((object.ReferenceEquals(this.URLField, value) != true)) {
                    this.URLField = value;
                    this.RaisePropertyChanged("URL");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string UserID {
            get {
                return this.UserIDField;
            }
            set {
                if ((object.ReferenceEquals(this.UserIDField, value) != true)) {
                    this.UserIDField = value;
                    this.RaisePropertyChanged("UserID");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ServiceReference.IErrorLog")]
    public interface IErrorLog {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IErrorLog/LogSingleException", ReplyAction="http://tempuri.org/IErrorLog/LogSingleExceptionResponse")]
        void LogSingleException(Laserbeam.UI.HR.ServiceReference.ExceptionLogModel exceptionLog);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IErrorLog/LogSingleException", ReplyAction="http://tempuri.org/IErrorLog/LogSingleExceptionResponse")]
        System.Threading.Tasks.Task LogSingleExceptionAsync(Laserbeam.UI.HR.ServiceReference.ExceptionLogModel exceptionLog);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IErrorLog/LogMultipleException", ReplyAction="http://tempuri.org/IErrorLog/LogMultipleExceptionResponse")]
        void LogMultipleException(System.Collections.Generic.List<Laserbeam.UI.HR.ServiceReference.ExceptionLogModel> exceptionLog);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IErrorLog/LogMultipleException", ReplyAction="http://tempuri.org/IErrorLog/LogMultipleExceptionResponse")]
        System.Threading.Tasks.Task LogMultipleExceptionAsync(System.Collections.Generic.List<Laserbeam.UI.HR.ServiceReference.ExceptionLogModel> exceptionLog);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IErrorLogChannel : Laserbeam.UI.HR.ServiceReference.IErrorLog, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ErrorLogClient : System.ServiceModel.ClientBase<Laserbeam.UI.HR.ServiceReference.IErrorLog>, Laserbeam.UI.HR.ServiceReference.IErrorLog {
        
        public ErrorLogClient() {
        }
        
        public ErrorLogClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ErrorLogClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ErrorLogClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ErrorLogClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public void LogSingleException(Laserbeam.UI.HR.ServiceReference.ExceptionLogModel exceptionLog) {
            base.Channel.LogSingleException(exceptionLog);
        }
        
        public System.Threading.Tasks.Task LogSingleExceptionAsync(Laserbeam.UI.HR.ServiceReference.ExceptionLogModel exceptionLog) {
            return base.Channel.LogSingleExceptionAsync(exceptionLog);
        }
        
        public void LogMultipleException(System.Collections.Generic.List<Laserbeam.UI.HR.ServiceReference.ExceptionLogModel> exceptionLog) {
            base.Channel.LogMultipleException(exceptionLog);
        }
        
        public System.Threading.Tasks.Task LogMultipleExceptionAsync(System.Collections.Generic.List<Laserbeam.UI.HR.ServiceReference.ExceptionLogModel> exceptionLog) {
            return base.Channel.LogMultipleExceptionAsync(exceptionLog);
        }
    }
}
