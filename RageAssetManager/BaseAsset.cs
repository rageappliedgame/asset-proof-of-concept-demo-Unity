﻿// <copyright file="BaseAsset.cs" company="RAGE">
// Copyright (c) 2015 RAGE All rights reserved.
// </copyright>
// <author>Veg</author>
// <date>13-4-2015</date>
// <summary>Implements the base asset class</summary>
namespace AssetPackage
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Xml.Linq;
    using System.Xml.Serialization;
    using System.Xml.XPath;

    using AssetManagerPackage;

    /// <summary>
    /// A base asset.
    /// </summary>
    public class BaseAsset : IAsset
    {
        #region Fields

        /// <summary>
        /// The test subscription.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        private String testSubscription;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the asset_proof_of_concept_demo_CSharp.Asset class.
        /// </summary>
        public BaseAsset()
        {
            this.Id = AssetManager.Instance.registerAssetInstance(this, this.Class);

            //! NOTE Unlike the JavaScript and Typescript versions (using a setTimeout) registration will not get triggered during publish in the AssetManager constructor.
            //
            //testSubscription = pubsubz.subscribe("EventSystem.Init", (topics, data) =>
            //{
            //This code fails in TypeScript (coded there as 'this.Id') as this points to the method and not the Asset.
            //Console.WriteLine("[{0}].{1}: {2}", this.Id, topics, data);
            //});

            //! List Embedded Resources.
            //foreach (String name in Assembly.GetCallingAssembly().GetManifestResourceNames())
            //{
            //    Console.WriteLine("{0}", name);
            //}
        }

        /// <summary>
        /// Initializes a new instance of the AssetPackage.BaseAsset class.
        /// </summary>
        ///
        /// <param name="bridge"> The bridge. </param>
        public BaseAsset(IBridge bridge)
            : this()
        {
            this.Bridge = bridge;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets the bridge.
        /// </summary>
        ///
        /// <value>
        /// The bridge.
        /// </value>
        public IBridge Bridge
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the class.
        /// </summary>
        ///
        /// <value>
        /// The class.
        /// </value>
        public String Class
        {
            get
            {
                return this.GetType().Name;
            }
        }

        /// <summary>
        /// Gets the dependencies.
        /// </summary>
        ///
        /// <value>
        /// The dependencies.
        /// </value>
        public Dictionary<String, String> Dependencies
        {
            get
            {
                Dictionary<String, String> dependencies = new Dictionary<String, String>();

                foreach (XElement dependency in VersionAndDependencies().XPathSelectElements("version/dependencies/depends"))
                {
                    String minv = dependency.Attribute("minVersion") != null ? dependency.Attribute("minVersion").Value : "0.0";
                    String maxv = dependency.Attribute("maxVersion") != null ? dependency.Attribute("maxVersion").Value : "*";

                    dependencies.Add(dependency.Value, String.Format("{0}-{1}", minv, maxv));
                }

                return dependencies;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this object has settings.
        /// </summary>
        ///
        /// <value>
        /// true if this object has settings, false if not.
        /// </value>
        public Boolean hasSettings
        {
            get
            {
                return Settings != null;
            }
        }

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        ///
        /// <value>
        /// The identifier.
        /// </value>
        public String Id
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the maturity.
        /// </summary>
        ///
        /// <value>
        /// The maturity.
        /// </value>
        public String Maturity
        {
            get
            {
                return XmlTagValue(VersionAndDependencies(), "version/maturity");
            }
        }

        /// <summary>
        /// Gets or sets options for controlling the operation.
        /// </summary>
        ///
        /// <value>
        /// The settings.
        /// </value>
        public virtual ISettings Settings
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the version.
        /// </summary>
        ///
        /// <value>
        /// The version.
        /// </value>
        public String Version
        {
            get
            {
                XDocument versionXml = VersionAndDependencies();

                return String.Format("{0}.{1}.{2}.{3}",
                    XmlTagValue(versionXml, "version/major"),
                    XmlTagValue(versionXml, "version/minor"),
                    XmlTagValue(versionXml, "version/build"),
                    XmlTagValue(versionXml, "version/revision")).TrimEnd('.');
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Loads Settings object from Default (Design-time) Settings.
        /// </summary>
        ///
        /// <remarks>
        /// In Unity Resources.Load() must be used and the files will be loaded a Assets\\Resources
        /// Folder.
        /// </remarks>
        ///
        /// <returns>
        /// true if it succeeds, false if it fails.
        /// </returns>
        public Boolean LoadDefaultSettings()
        {
            IDefaultSettings ds = getInterface<IDefaultSettings>();

            if (ds != null && hasSettings && ds.HasDefaultSettings(Class, Id))
            {
                String xml = ds.LoadDefaultSettings(Class, Id);

                Settings = SettingsFromXml(xml);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Loads Settings object as Run-time Settings.
        /// </summary>
        ///
        /// <remarks>
        /// The resulting file will be read using the IDataStorage interface.
        /// </remarks>
        ///
        /// <param name="filename"> Filename of the file. </param>
        ///
        /// <returns>
        /// true if it succeeds, false if it fails.
        /// </returns>
        public Boolean LoadSettings(String filename)
        {
            IDataStorage ds = getInterface<IDataStorage>();

            if (ds != null && hasSettings && ds.Exists(filename))
            {
                String xml = ds.Load(filename);

                Settings = SettingsFromXml(xml);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Saves Settings object as Default (Design-time) Settings.
        /// </summary>
        ///
        /// <remarks>
        /// In Unity the file will be saved in a Assets\\Resources Folder in the editor environment (As
        /// resources are read-only at run-time).
        /// </remarks>
        ///
        /// <returns>
        /// true if it succeeds, false if it fails.
        /// </returns>
        public Boolean SaveDefaultSettings(bool force)
        {
            IDefaultSettings ds = getInterface<IDefaultSettings>();

            if (ds != null && hasSettings && (force || !ds.HasDefaultSettings(Class, Id)))
            {
                ds.SaveDefaultSettings(Class, Id, SettingsToXml());

                return true;
            }

            return false;
        }

        /// <summary>
        /// Loads Settings object from Run-time Settings.
        /// </summary>
        ///
        /// <remarks>
        /// The resulting file will be written using the IDataStorage interface.
        /// </remarks>
        ///
        /// <param name="filename"> Filename of the file. </param>
        ///
        /// <returns>
        /// true if it succeeds, false if it fails.
        /// </returns>
        public Boolean SaveSettings(String filename)
        {
            IDataStorage ds = getInterface<IDataStorage>();

            if (ds != null && hasSettings)
            {
                ds.Save(filename, SettingsToXml());

                return true;
            }

            return false;
        }

        /// <summary>
        /// Settings from XML.
        /// </summary>
        ///
        /// <param name="xml"> The XML. </param>
        ///
        /// <returns>
        /// The ISettings.
        /// </returns>
        public ISettings SettingsFromXml(String xml)
        {
            XmlSerializer ser = new XmlSerializer(Settings.GetType());

            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(xml)))
            {
                //! Use DataContractSerializer or DataContractJsonSerializer?
                //
                return (ISettings)ser.Deserialize(ms);
            }
        }

        /// <summary>
        /// Settings to XML.
        /// </summary>
        ///
        /// <returns>
        /// A String.
        /// </returns>
        public String SettingsToXml()
        {
            XmlSerializer ser = new XmlSerializer(Settings.GetType());

            using (StringWriterUtf8 textWriter = new StringWriterUtf8())
            {
                //! Use DataContractSerializer or DataContractJsonSerializer?
                //
                ser.Serialize(textWriter, Settings);

                textWriter.Flush();

                return textWriter.ToString();
            }
        }

        /// <summary>
        /// Version and dependencies.
        /// </summary>
        ///
        /// <returns>
        /// An XDocument.
        /// </returns>
        /// <summary>
        /// Version and dependencies.
        /// </summary>
        ///
        /// <returns>
        /// An XDocument.
        /// </returns>
        internal XDocument VersionAndDependencies()
        {
            //! asset_proof_of_concept_demo_CSharp.Resources.Asset.VersionAndDependencies.xml
            //! <namespace>.Resources.<AssetType>.VersionAndDependencies.xml
            //
            String xml = GetEmbeddedResource(GetType().Namespace, String.Format("Resources.{0}.VersionAndDependencies.xml", GetType().Name));

            if (!String.IsNullOrEmpty(xml))
            {
                return XDocument.Parse(xml);
            }

            return new XDocument();
        }

        /// <summary>
        /// Gets embedded resource.
        /// </summary>
        ///
        /// <param name="ns">  The namespace. </param>
        /// <param name="res"> The resource name. </param>
        ///
        /// <returns>
        /// The embedded resource.
        /// </returns>
        protected String GetEmbeddedResource(String ns, String res)
        {
            String path = String.Format("{0}.{1}", ns, res);

            // Console.WriteLine("Loading Resources: {0}",path);

            using (StreamReader reader = new StreamReader(GetType().Assembly.GetManifestResourceStream(path)))
            {
                return reader.ReadToEnd();
            }
        }

        /// <summary>
        /// Gets the interface.
        /// </summary>
        ///
        /// <typeparam name="T"> Generic type parameter. </typeparam>
        ///
        /// <returns>
        /// The interface.
        /// </returns>
        protected T getInterface<T>()
        {
            if (Bridge != null && Bridge is T)
            {
                return (T)Bridge;
            }
            else if (AssetManager.Instance.Bridge != null && AssetManager.Instance.Bridge is T)
            {
                return (T)(AssetManager.Instance.Bridge);
            }

            return default(T);
        }

        /// <summary>
        /// XML tag value.
        /// </summary>
        ///
        /// <param name="doc">   The document. </param>
        /// <param name="xpath"> The xpath. </param>
        ///
        /// <returns>
        /// A String.
        /// </returns>
        private String XmlTagValue(XDocument doc, String xpath)
        {
            if (doc.XPathSelectElement(xpath) != null)
            {
                return doc.XPathSelectElement(xpath).Value;
            }
            return String.Empty;
        }

        #endregion Methods

        #region Nested Types

        /// <summary>
        /// A string writer utf-8.
        /// </summary>
        ///
        /// <remarks>
        /// Fix-up for XDocument Serialization defaulting to utf-16.
        /// </remarks>
        internal class StringWriterUtf8 : StringWriter
        {
            #region Properties

            //public StringWriterUtf8(StringBuilder sb)
            //    : base(sb)
            //{
            //}
            public override Encoding Encoding
            {
                get { return Encoding.UTF8; }
            }

            #endregion Properties
        }

        #endregion Nested Types
    }
}