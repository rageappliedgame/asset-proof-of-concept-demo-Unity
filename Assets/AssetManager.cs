// <copyright file="AssetManager.cs" company="RAGE">
// Copyright (c) 2015 RAGE. All rights reserved.
// </copyright>
// <author>Veg</author>
// <date>10-4-2015</date>
// <summary>Implements the asset manager class</summary>
namespace asset_proof_of_concept_demo_CSharp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

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
        public object Bridge
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
        internal String registerAssetInstance(IAsset asset, String claz)
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