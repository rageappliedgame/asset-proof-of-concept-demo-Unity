// <copyright file="BaseAsset.cs" company="RAGE">
// Copyright (c) 2015 RAGE All rights reserved.
// </copyright>
// <author>Veg</author>
// <date>13-4-2015</date>
// <summary>Implements the base asset class</summary>
namespace asset_proof_of_concept_demo_CSharp
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using System.Xml.Linq;
    using System.Xml.XPath;

    /// <summary>
    /// A base asset.
    /// 
    /// For debugging the RageAssets, one needs to do several things:
    /// <list type="number">
    /// <item><description>The generated assembly should have full Debug Symbols enabled and the   
    ///    Debug Constant to be defined (i.e. a Debug Build).</description></item>
    /// <item><description>The csproj file needs to  be adjusted to include a pdb to mdb (Debug Symbol) conversion.</description></item>
    /// </list><para />
    /// Add the following lines (for Unity 5.x) to the PropertyGroup containing the Debug configuration:
    /// <br /><code>
    ///     &lt;!-- Look up Unity install folder, and set the ReferencePath for locating managed assembly references. --&gt;
    ///     &lt;UnityInstallFolder&gt;$(registry:HKEY_CURRENT_USER\Software\Unity Technologies\Installer\Unity@Location x64)&lt;/UnityInstallFolder&gt;
    ///     &lt;ReferencePath&gt;$(UnityInstallFolder)\Editor\Data\&lt;/ReferencePath&gt;
    ///     &lt;MonoFolder&gt;$(UnityInstallFolder)\Editor\Data\MonoBleedingEdge&lt;/MonoFolder&gt;
    ///     &lt;MonoMdbGenerator&gt;$(MonoFolder)\lib\mono\4.5\pdb2mdb.exe&lt;/MonoMdbGenerator&gt;
    ///     &lt;MonoCLI&gt;$(MonoFolder)\bin\cli.bat&lt;/MonoCLI&gt;
    /// </code>
    /// <para />
    /// Add the following lines (for Unity 5.x) after the last PropertyGroup:
    /// <br /><code>
    ///     &lt;Target Name="AfterBuild"&gt;
    ///        &lt;CallTarget Targets="GenerateMonoSymbols" Condition=" Exists('$(OutputPath)\$(AssemblyName).pdb') " /&gt;
    ///      &lt;/Target&gt;
    ///      &lt;Target Name="GenerateMonoSymbols"&gt;
    ///        &lt;Message Text="Unity install folder: $(UnityInstallFolder)" Importance="high" /&gt;
    ///        &lt;Message Text="$(ProjectName) -&gt; $(TargetPath).mdb" Importance="High" /&gt;
    ///        &lt;Exec Command="&quot;$(MonoCLI)&quot; &quot;$(MonoMdbGenerator)&quot; $(AssemblyName).dll" WorkingDirectory="$(MSBuildProjectDirectory)\$(OutputPath)" /&gt;
    ///      &lt;/Target&gt;
    /// </code>
    /// 
    /// <seealso cref="http://forum.unity3d.com/threads/how-to-build-and-debug-external-dlls.161685/"/>
    /// </summary>
    public class BaseAsset : IAsset
    {
        #region Fields

        /// <summary>
        /// The test subscription.
        /// </summary>
        private String testSubscription;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the asset_proof_of_concept_demo_CSharp.Asset class.
        /// </summary>
        public BaseAsset()
        {
            Debug.WriteLine("BaseAsset!");

            this.Id = AssetManager.Instance.registerAssetInstance(this, this.Class);

            //! NOTE Unlike the JavaScript and Typescript versions (using a setTimeout) registration will not get triggered during publish in the AssetManager constructor.
            //
            testSubscription = pubsubz.subscribe("EventSystem.Init", (topics, data) =>
                                                 {
                                                     //This code fails in TypeScript (coded there as 'this.Id') as this points to the method and not the Asset.
                                                     Console.WriteLine("[{0}].{1}: {2}", this.Id, topics, data);
                                                 });

            // http://www.mindthecube.com/blog/2009/11/reading-text-data-into-a-unity-game
            // http://forum.unity3d.com/threads/enabling-embedded-resources-with-webgl.326069/

            //TextAsset bindata= Resources.Load("Asset.VersionAndDependencies") as TextAsset;
            //Console.WriteLine(bindata.text);

            //TextAsset scriptdata= Resources.Load("script.txt") as TextAsset;
            //Console.WriteLine(scriptdata.text);
            Assembly me = GetType().Assembly;

            Console.WriteLine("RageAssets: {0}", GetType().Namespace);
            Console.WriteLine("RageAssets: {0}", me.FullName);
            Console.WriteLine("RageAssets: {0}", me.CodeBase);

            //RageAssets, Version=1.0.5667.28223, Culture=neutral, PublicKeyToken=null

            //asset_proof_of_concept_demo_CSharp.script.txt
            //asset_proof_of_concept_demo_CSharp.Resources.Asset.VersionAndDependencies.xml
            //asset_proof_of_concept_demo_CSharp.Resources.BaseAsset.VersionAndDependencies.xml
            //asset_proof_of_concept_demo_CSharp.Resources.DialogueAsset.VersionAndDependencies.xml
            //asset_proof_of_concept_demo_CSharp.Resources.Logger.VersionAndDependencies.xml

            foreach (string name in me.GetManifestResourceNames())
            {
                Console.WriteLine("Resources: {0}", name);
            }
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
        public object Bridge
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
        /// Gets the default settings.
        /// </summary>
        ///
        /// <value>
        /// The default settings.
        /// </value>
        public virtual String DefaultSettings
        {
            get
            {
                return XDocument.Parse
                ("<Settings></Settings>").ToString(SaveOptions.None);
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

        #region Other

        //public Dictionary<String, String> DefaultSettings
        //{
        //    get
        //    {
        //        Dictionary<String, String> defaults = new Dictionary<String, String>();
        //        foreach (String key in ConfigurationManager.AppSettings.Keys)
        //        {
        //            defaults.Add(key, ConfigurationManager.AppSettings[key]);
        //        }
        //        return defaults;
        //    }
        //}

        #endregion Other
    }
}