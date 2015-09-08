// <copyright file="AssetManager.cs" company="RAGE">
// Copyright (c) 2015 RAGE. All rights reserved.
// </copyright>
// <author>Veg</author>
// <date>10-4-2015</date>
// <summary>Implements the asset manager class</summary>
namespace AssetManagerPackage
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using AssetPackage;

    /// <summary>
    /// Manager for assets.
    /// </summary>
    public class AssetManager
    {
        #region Fields

        /// <summary>
        /// The instance.
        /// </summary>
        static readonly AssetManager _instance = new AssetManager();

        /// <summary>
        /// The assets.
        /// </summary>
        private Dictionary<String, IAsset> assets = new Dictionary<String, IAsset>();

        /// <summary>
        /// The identifier generator.
        /// </summary>
        private Int32 idGenerator = 0;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Explicit static constructor tells # compiler not to mark type as beforefieldinit.
        /// </summary>
        static AssetManager()
        {
            //
        }

        /// <summary>
        /// Prevents a default instance of the AssetManager class from being created.
        /// </summary>
        private AssetManager()
        {
            initEventSystem();
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Visible when reflecting.
        /// </summary>
        ///
        /// <value>
        /// The instance.
        /// </value>
        public static AssetManager Instance
        {
            get
            {
                return _instance;
            }
        }

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

        #endregion Properties

        #region Methods

        /// <summary>
        /// Searches for the first asset by class.
        /// </summary>
        ///
        /// <param name="claz"> The claz. </param>
        ///
        /// <returns>
        /// The found asset by class.
        /// </returns>
        public IAsset findAssetByClass(String claz)
        {
            Regex mask = new Regex(String.Format(@"{0}_(\d+)", claz));

            return assets.First(p => mask.IsMatch(p.Key)).Value;
        }

        /// <summary>
        /// Searches for the first asset by identifier.
        /// </summary>
        ///
        /// <param name="id"> The identifier. </param>
        ///
        /// <returns>
        /// The found asset by identifier.
        /// </returns>
        public IAsset findAssetById(String id)
        {
            return assets[id];
        }

        /// <summary>
        /// Searches for assets by class.
        /// </summary>
        ///
        /// <param name="claz"> The claz. </param>
        ///
        /// <returns>
        /// The found assets by class.
        /// </returns>
        public List<IAsset> findAssetsByClass(String claz)
        {
            Regex mask = new Regex(String.Format(@"{0}_(\d+)", claz));

            // Return the values of all matching keys using the regex.
            return assets.Where(p => mask.IsMatch(p.Key)).Select(p => p.Value).ToList();
        }

        /// <summary>
        /// Registers the asset instance.
        /// </summary>
        ///
        /// <param name="asset"> The asset. </param>
        /// <param name="claz">  The claz. </param>
        ///
        /// <returns>
        /// A String.
        /// </returns>
        public String registerAssetInstance(IAsset asset, String claz)
        {
            foreach (KeyValuePair<String, IAsset> kvp in assets)
            {
                if (asset.Equals(kvp.Value))
                {
                    return kvp.Key;
                }
            }

            String Id = String.Format("{0}_{1}", claz, idGenerator++);

            Console.WriteLine("Registering Asset {0}/{1} as {2}", asset.GetType().Name, claz, Id);

            assets.Add(Id, asset);

            Console.WriteLine("Registered " + assets.Count + " Asset(s)");

            return Id;
        }

        /// <summary>
        /// Reports version and dependencies.
        /// </summary>
        ///
        /// <value>
        /// The version and dependencies report.
        /// </value>
        public String VersionAndDependenciesReport
        {
            get
            {
                const Int32 col1w = 40;
                const Int32 col2w = 32;

                StringBuilder report = new StringBuilder();

                report.AppendFormat("{0}{1}", "Asset".PadRight(col1w), "Depends on").AppendLine();
                report.AppendFormat("{0}+{1}", "".PadRight(col1w - 1, '-'), "".PadRight(col2w, '-')).AppendLine();

                foreach (KeyValuePair<String, IAsset> asset in assets)
                {
                    report.Append(String.Format("{0} v{1}", asset.Value.Class, asset.Value.Version).PadRight(col1w - 1));

                    // Console.WriteLine("[{0}]\r\n{1}=v{2}\t;{3}", asset.Key, asset.Value.Class, asset.Value.Version, asset.Value.Maturity);
                    Int32 cnt = 0;
                    foreach (KeyValuePair<String, String> dependency in asset.Value.Dependencies)
                    {
                        //! Better version matches (see Microsoft).
                        // 
                        //! https://msdn.microsoft.com/en-us/library/system.version(v=vs.110).aspx
                        //
                        //! dependency.value has min-max format (inclusive) like:
                        // 
                        //? v1.2.3-*        (v1.2.3 or higher)
                        //? v0.0-*          (all versions)
                        //? v1.2.3-v2.2     (v1.2.3 or higher less than or equal to v2.1)
                        //
                        String[] vrange = dependency.Value.Split('-');

                        Version low = null;

                        Version hi = null;

                        switch (vrange.Length)
                        {
                            case 1:
                                low = new Version(vrange[0]);
                                hi = low;
                                break;
                            case 2:
                                low = new Version(vrange[0]);
                                if (vrange[1].Equals("*"))
                                {
                                    hi = new Version(99, 99);
                                }
                                else
                                {
                                    hi = new Version(vrange[1]);
                                }
                                break;

                            default:
                                break;
                        }

                        Boolean found = false;

                        if (low != null)
                        {
                            foreach (IAsset dep in findAssetsByClass(dependency.Key))
                            {
                                // Console.WriteLine("Dependency {0}={1}",dep.Class, dep.Version);
                                Version vdep = new Version(dep.Version);
                                if (low <= vdep && vdep <= hi)
                                {
                                    found = true;
                                    break;
                                }
                            }

                            report.AppendFormat("|{0} v{1} [{2}]", dependency.Key, dependency.Value, found ? "resolved" : "missing").AppendLine();
                        }
                        else
                        {
                            report.AppendLine("error");
                        }

                        if (cnt != 0)
                        {
                            report.Append("".PadRight(col1w - 1));
                        }

                        cnt++;
                    }

                    if (cnt == 0)
                    {
                        report.AppendFormat("|{0}", "No dependencies").AppendLine();
                    }
                }

                report.AppendFormat("{0}+{1}", "".PadRight(col1w - 1, '-'), "".PadRight(col2w, '-')).AppendLine();

                return report.ToString();
            }
        }

        /// <summary>
        /// Initialises the event system.
        /// </summary>
        private void initEventSystem()
        {
            pubsubz.define("EventSystem.Init");

            //! NOTE Unlike the JavaScript and Typescript versions (using a setTimeout) this call here will not trigger any event handling code.
            //
            pubsubz.publish("EventSystem.Init', 'hello event!");
        }

        #endregion Methods
    }
}