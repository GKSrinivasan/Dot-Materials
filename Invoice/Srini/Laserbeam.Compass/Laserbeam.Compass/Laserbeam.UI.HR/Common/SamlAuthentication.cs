

// Copyright (c) 2015 LaserBeam Software, Inc.  All rights reserved.
// Confidential and proprietary.

// Component Name: 	AssertionUtility
// Description: 	SAML 2.0 library
// Author:		    Boobalan		
// Creation Date: 	07-11-2014

      
using System;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using System.Security.Cryptography.Xml;
namespace Laserbeam.SAML2
{
    public class SamlAuthentication
    {
        #region Fields
        public readonly string TargetUrl;
        private XmlDocument m_samlResponse;
        private Configuration m_samlConfiguration;
        private XmlNamespaceManager m_samlNamespace;
        private RSACryptoServiceProvider m_requestPublicKey;
        private XmlDocument m_samlRequest;
        private readonly X509Certificate2 m_requestCertificate;
        #endregion

        #region Constructors
        
        // Author        :  Boobalan		
        // Creation Date :  07-11-2014
        /// <summary>
        /// Creates an instance of AssertionUtility with X.509 certificate and service provider URL to generate SAML request
        /// </summary>
        /// <param name="certificate">X.509 certificate having assymmetric key</param>
        /// <param name="serviceProviderURL">A valid services provider URL to which the SAML response needs to be sent</param>
        public SamlAuthentication(X509Certificate2 certificate,string serviceProviderURL)
        {
            m_requestCertificate = certificate;
            initializeRequest(serviceProviderURL);
            signRequestAssertion(m_requestCertificate);
            TargetUrl = m_samlConfiguration.AppSettings.Settings["TargetURL"].Value;
        }

        
        // Author        :  Boobalan		
        // Creation Date :  07-11-2014
        /// <summary>
        /// Creates an instance of AssertionUtility with SAML response
        /// </summary>
        /// <param name="samlResponse">Encrypted SAML Response as request</param>
        public SamlAuthentication(XmlDocument samlResponse)
        {
            initializeResponse(samlResponse);
            setPublicKeyFromAssertionRequest(samlResponse);
            TargetUrl = samlResponse.DocumentElement.GetAttribute("AssertionConsumerServiceURL");
        }
        #endregion

        #region Public Methods
        
        // Author        :  Boobalan		
        // Creation Date :  07-11-2014
        /// <summary>
        /// Adds new AttributeValue node to AttributeStatement node accepting string value
        /// </summary>
        /// <param name="name">Name for AttributeValue node</param>
        /// <param name="value">Value for AttributeValue node</param>
        public void AddResponseAttribute(string name, string value)
        {
            XmlNode nodeValue = m_samlResponse.CreateNode(XmlNodeType.Element, "saml", "AttributeValue", "urn:oasis:names:tc:SAML:2.0:assertion");
            XmlAttribute nameFormat = m_samlResponse.CreateAttribute("xsi", "type", "http://www.w3.org/2001/XMLSchema-instance");
            nameFormat.Value = "xs:string";
            nodeValue.Attributes.Append(nameFormat);
            nodeValue.InnerText = encryptData(value);
            createResponseAssertionAttribute(name, nodeValue);
        }

        // Author        :  Boobalan		
        // Creation Date :  07-11-2014
        /// <summary>
        /// Adds new AttributeValue node to AttributeStatement node accepting int value
        /// </summary>
        /// <param name="name">Name for AttributeValue node</param>
        /// <param name="value">Value for AttributeValue node</param>
        public void AddResponseAttribute(string name, int value)
        {
            XmlNode nodeValue = m_samlResponse.CreateNode(XmlNodeType.Element, "saml", "AttributeValue", "urn:oasis:names:tc:SAML:2.0:assertion");
            XmlAttribute nameFormat = m_samlResponse.CreateAttribute("xsi", "type", "http://www.w3.org/2001/XMLSchema-instance");
            nameFormat.Value = "xs:int";
            nodeValue.Attributes.Append(nameFormat);
            nodeValue.InnerText = encryptData(Convert.ToString(value));
            createResponseAssertionAttribute(name, nodeValue);
        }

        // Author        :  Boobalan		
        // Creation Date :  07-11-2014
        /// <summary>
        /// Adds new AttributeValue node to AttributeStatement node accepting double value
        /// </summary>
        /// <param name="name">Name for AttributeValue node</param>
        /// <param name="value">Value for AttributeValue node</param>
        public void AddResponseAttribute(string name, double value)
        {
            XmlNode nodeValue = m_samlResponse.CreateNode(XmlNodeType.Element, "saml", "AttributeValue", "urn:oasis:names:tc:SAML:2.0:assertion");
            XmlAttribute nameFormat = m_samlResponse.CreateAttribute("xsi", "type", "http://www.w3.org/2001/XMLSchema-instance");
            nameFormat.Value = "xs:decimal";
            nodeValue.Attributes.Append(nameFormat);
            nodeValue.InnerText = encryptData(Convert.ToString(value));
            createResponseAssertionAttribute(name, nodeValue);
        }

        // Author        :  Boobalan		
        // Creation Date :  07-11-2014
        /// <summary>
        /// Adds new AttributeValue node to AttributeStatement node accepting datetime value
        /// </summary>
        /// <param name="name">Name for AttributeValue node</param>
        /// <param name="value">Value for AttributeValue node</param>
        public void AddResponseAttribute(string name, DateTime value)
        {
            XmlNode nodeValue = m_samlResponse.CreateNode(XmlNodeType.Element, "saml", "AttributeValue", "urn:oasis:names:tc:SAML:2.0:assertion");
            XmlAttribute nameFormat = m_samlResponse.CreateAttribute("xsi", "type", "http://www.w3.org/2001/XMLSchema-instance");
            nameFormat.Value = "xs:dateTime";
            nodeValue.Attributes.Append(nameFormat);
            nodeValue.InnerText = encryptData(Convert.ToString(value.ToUniversalTime()));
            createResponseAssertionAttribute(name, nodeValue);
        }

        // Author        :  Boobalan		
        // Creation Date :  07-11-2014
        /// <summary>
        /// Encrypts SAML response assertion to Base64String
        /// </summary>
        /// <returns>Returns string</returns>
        public string EncryptResponseAssertion()
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(m_samlResponse.OuterXml));
        }

        // Author        :  Boobalan		
        // Creation Date :  07-11-2014
        /// <summary>
        /// Encrypts SAML request assertion to Base64String
        /// </summary>
        /// <returns>Returns string</returns>
        public string EncryptRequestAssertion()
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(m_samlRequest.OuterXml));
        }

        // Author        :  Boobalan		
        // Creation Date :  07-11-2014
        /// <summary>
        /// Exports SAML request assertion to the provided path
        /// </summary>
        /// <param name="exportPath">A valid file path where file needs to be exported</param>
        public void ExportRequestAssertion(string exportPath)
        {
            m_samlRequest.Save(exportPath);
        }

        // Author        :  Boobalan		
        // Creation Date :  07-11-2014
        /// <summary>
        /// Exports SAML response assertion to the provided path
        /// </summary>
        /// <param name="exportPath">A valid file path where file needs to be exported</param>
        public void ExportResponseAssertion(string exportPath)
        {
            m_samlResponse.Save(exportPath);
        }

        // Author        :  Boobalan		
        // Creation Date :  07-11-2014
        /// <summary>
        /// Decrypts encrypted SAML reponse with the provided privatekey
        /// </summary>
        /// <param name="privateKey">Private key of X.509 certificate with which SAML response was encrypted</param>
        /// <param name="encryptedValue">Encrypted SAML response string</param>
        /// <returns>Returns decrypted SAML response</returns>
        public static string Decrypt(RSACryptoServiceProvider privateKey, string encryptedValue)
        {
            byte[] decrypt = privateKey.Decrypt(Convert.FromBase64String(encryptedValue), true);
            return Encoding.UTF8.GetString(decrypt);
        }

        // Author        :  Boobalan		
        // Creation Date :  07-11-2014
        /// <summary>
        /// Decrypts Base64String SAML response to string
        /// </summary>
        /// <param name="encryptedAssertion">SAML response in Base64String format</param>
        /// <returns>Returns string</returns>
        public static string DecryptAssertion(string encryptedAssertion)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(encryptedAssertion));
        }

        // Author        :  Boobalan		
        // Creation Date :  07-11-2014
        /// <summary>
        /// Gets AttributeStatement node from SAML response
        /// </summary>
        /// <param name="response">SAML response as XMLDocument</param>
        /// <returns>Returns AttributeStatement as XmlNode</returns>
        public static XmlNode GetResponseAttributes(XmlDocument response)
        {
            XmlNamespaceManager xmlNameSpace = new XmlNamespaceManager(response.NameTable);
            xmlNameSpace.AddNamespace("samlp", "urn:oasis:names:tc:SAML:1.0:protocol");
            xmlNameSpace.AddNamespace("saml", "urn:oasis:names:tc:SAML:1.0:assertion");
            xmlNameSpace.AddNamespace("ds", "http://www.w3.org/2000/09/xmldsig#");
            return response.SelectSingleNode("/samlp:Response/saml:Assertion/saml:AttributeStatement", xmlNameSpace);
        }
        #endregion
        
        #region Private Methods
        // Author        :  Boobalan		
        // Creation Date :  07-11-2014
        /// <summary>
        /// Signs SAML request assertion with X.509 certificate
        /// </summary>
        /// <param name="certificate">X.509 certificate object with which SAML request needs to be signed</param>
        private void signRequestAssertion(X509Certificate2 certificate)
        {
            SignedXml signedRequest = new SignedXml(m_samlRequest);
            signedRequest.SigningKey = ((RSA)certificate.PrivateKey);
            Reference reference = new Reference();
            reference.Uri = "";
            XmlDsigEnvelopedSignatureTransform env = new XmlDsigEnvelopedSignatureTransform();
            reference.AddTransform(env);
            signedRequest.AddReference(reference);
            KeyInfo keyInfo = new KeyInfo();
            KeyInfoX509Data keyInfoData = new KeyInfoX509Data(certificate);
            keyInfo.AddClause(keyInfoData);
            signedRequest.KeyInfo = keyInfo;
            signedRequest.ComputeSignature();
            bool dfd = signedRequest.CheckSignature(certificate, true);
            XmlElement xmlDigitalSignature = signedRequest.GetXml();
            m_samlRequest.DocumentElement.AppendChild(m_samlRequest.ImportNode(xmlDigitalSignature, true));
        }

        // Author        :  Boobalan		
        // Creation Date :  07-11-2014
        /// <summary>
        /// Encrypts a string with X.509 public key
        /// </summary>
        /// <param name="value">A string value which needs to be encrypted</param>
        /// <returns>Returns encrypted string value</returns>
        private string encryptData(string value)
        {
           byte[] encryptedData = m_requestPublicKey.Encrypt(Encoding.UTF8.GetBytes(value??""), true);
           return Convert.ToBase64String(encryptedData);
        }

        // Author        :  Boobalan		
        // Creation Date :  07-11-2014
        /// <summary>
        /// Creates AttributeStatement node for SAML response1
        /// </summary>
        /// <param name="name">String value for name attribute of AttributeStatement node</param>
        /// <param name="nodeValue">String value for AttributeStatement node value</param>
        private void createResponseAssertionAttribute(string name,XmlNode nodeValue)
        {
            XmlNode attributeStatement = m_samlResponse.SelectSingleNode("//saml:AttributeStatement", m_samlNamespace);
            XmlNode node = m_samlResponse.CreateNode(XmlNodeType.Element, "saml", "Attribute", "urn:oasis:names:tc:SAML:2.0:assertion");
            XmlAttribute nameFormat = m_samlResponse.CreateAttribute("NameFormat");
            nameFormat.Value = "urn:oasis:names:tc:SAML:2.0:attrname-format:basic";
            node.Attributes.Append(nameFormat);
            XmlAttribute nameValue = m_samlResponse.CreateAttribute("Name");
            nameValue.Value = name;
            node.Attributes.Append(nameValue);
            node.AppendChild(nodeValue);
            node.Attributes.Remove(node.Attributes["xmlns:saml"]);
            attributeStatement.AppendChild(node);
        }

        // Author        :  Boobalan		
        // Creation Date :  07-11-2014
        /// <summary>
        /// Gets X.509 certificate public key from signed SAML request
        /// </summary>
        /// <param name="request">Signed SAML request as XmlDocument</param>
        private void setPublicKeyFromAssertionRequest(XmlDocument request)
        {
            SignedXml requestSignedXml = new SignedXml(request);
            XmlNodeList nodeList = request.GetElementsByTagName("Signature");
            requestSignedXml.LoadXml((XmlElement)nodeList[0]);
            var x509 = requestSignedXml.Signature.KeyInfo.OfType<KeyInfoX509Data>().First();
            X509Certificate2 requestCertificate = x509.Certificates[0] as X509Certificate2;
            m_requestPublicKey = (RSACryptoServiceProvider)requestCertificate.PublicKey.Key;
        }

        // Author        :  Boobalan		
        // Creation Date :  07-11-2014
        /// <summary>
        /// Initializes AssertionUtility to process SAML reponse 
        /// </summary>
        /// <param name="request">SAML request as XmlDocument</param>
        private void initializeResponse(XmlDocument request)
        {
            string responseId = Guid.NewGuid().ToString();
            ExeConfigurationFileMap configFile = new ExeConfigurationFileMap();
            configFile.ExeConfigFilename =  HttpContext.Current.Server.MapPath("~/SAML/SAMLResponse.config");
            m_samlConfiguration = ConfigurationManager.OpenMappedExeConfiguration(configFile, ConfigurationUserLevel.None);
            m_samlResponse = new XmlDocument();
            m_samlResponse.Load(HttpContext.Current.Server.MapPath(m_samlConfiguration.AppSettings.Settings["SAMLSkeletonFile"].Value));
            m_samlNamespace = new XmlNamespaceManager(m_samlResponse.NameTable);
            m_samlNamespace.AddNamespace("saml", "urn:oasis:names:tc:SAML:2.0:assertion");
            m_samlResponse.DocumentElement.SetAttribute("ResponseID", responseId);
            m_samlResponse.DocumentElement.SetAttribute("IssueInstant", DateTime.Now.ToUniversalTime().ToString());
            m_samlResponse.DocumentElement.SetAttribute("InResponseTo", request.DocumentElement.Attributes["ID"].Value);
            m_samlResponse.SelectSingleNode("//saml:Issuer", m_samlNamespace).InnerText = m_samlConfiguration.AppSettings.Settings["Issuer"].Value;
            XmlNode assertion = m_samlResponse.SelectSingleNode("//saml:Assertion", m_samlNamespace);
            assertion.Attributes["ID"].Value = request.DocumentElement.Attributes["ID"].Value;
            assertion.Attributes["IssueInstant"].Value = DateTime.Now.ToUniversalTime().ToString();
            assertion.SelectSingleNode("//saml:Assertion/saml:Issuer", m_samlNamespace).InnerText = m_samlConfiguration.AppSettings.Settings["Issuer"].Value;
            XmlNode condition = assertion.SelectSingleNode("//saml:Conditions", m_samlNamespace);
            condition.Attributes["NotBefore"].Value = DateTime.Now.ToUniversalTime().ToString();
            condition.Attributes["NotOnOrAfter"].Value = DateTime.Now.AddHours(1).ToUniversalTime().ToString();
            XmlNode authnStatement = assertion.SelectSingleNode("//saml:AuthnStatement", m_samlNamespace);
            authnStatement.Attributes["AuthnInstant"].Value = responseId;
        }

        // Author        :  Boobalan		
        // Creation Date :  07-11-2014
        /// <summary>
        /// Initializes AssertionUtility to process SAML request
        /// </summary>
        /// <param name="serviceProviderURL">A valid service provider URL</param>
        private void initializeRequest(string serviceProviderURL)
        {
            ExeConfigurationFileMap configFile = new ExeConfigurationFileMap();
            configFile.ExeConfigFilename = HttpContext.Current.Server.MapPath("~/SAML/SAMLRequest.config");
            m_samlConfiguration = ConfigurationManager.OpenMappedExeConfiguration(configFile, ConfigurationUserLevel.None);
            m_samlRequest = new XmlDocument();
            m_samlRequest.Load(HttpContext.Current.Server.MapPath(m_samlConfiguration.AppSettings.Settings["SAMLSkeletonFile"].Value));
            m_samlNamespace = new XmlNamespaceManager(m_samlRequest.NameTable);
            m_samlNamespace.AddNamespace("saml", "urn:oasis:names:tc:SAML:2.0:assertion");
            m_samlRequest.DocumentElement.SetAttribute("ID", Guid.NewGuid().ToString());
            m_samlRequest.DocumentElement.SetAttribute("IssueInstant", DateTime.Now.ToUniversalTime().ToString());
            m_samlRequest.DocumentElement.SetAttribute("AssertionConsumerServiceURL", serviceProviderURL);
            m_samlRequest.SelectSingleNode("//saml:Issuer", m_samlNamespace).InnerText = serviceProviderURL;
        }
        #endregion
    }
}