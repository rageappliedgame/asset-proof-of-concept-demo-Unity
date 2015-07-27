// <copyright file="Asset.cs" company="RAGE">
// Copyright (c) 2015 RAGE All rights reserved.
// </copyright>
// <author>Veg</author>
// <date>10-4-2015</date>
// <summary>Implements the asset class</summary>
namespace asset_proof_of_concept_demo_CSharp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// An asset.
    /// </summary>
    public class Asset : BaseAsset
    {
        #region Fields

        String fData = "Hello Storage World";
        String fId1 = "Hello1.txt";
        String fId2 = "Hello2.txt";

        /// <summary>
        /// The file storage.
        /// </summary>
        private Dictionary<String, String> FileStorage = new Dictionary<String, String>();

        /// <summary>
        /// Options for controlling the operation.
        /// </summary>
        private AssetSettings settings = null;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the asset_proof_of_concept_demo_CSharp.Asset class.
        /// </summary>
        public Asset()
            : base()
        {
            //! Create Settings and let it's BaseSettings class assign Defaultvalues where it can.
            // 
            settings = new AssetSettings();

            settings.TestProperty += "test";

            //! Assign other properties values if necessary;
            // 
            //settings.TestList = new String[] { "Hello", "World" };
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets options for controlling the operation.
        /// </summary>
        ///
        /// <remarks>   Besides the toXml() and fromXml() methods, we never use this property but use
        ///                it's correctly typed backing field 'settings' instead. </remarks>
        /// <remarks> This property should go into each asset having Settings of its own. </remarks>
        /// <remarks>   The actual class used should be derived from BaseAsset (and not directly from
        ///             ISetting). </remarks>
        ///
        /// <value>
        /// The settings.
        /// </value>
        public override ISettings Settings
        {
            get
            {
                return settings;
            }
            set
            {
                settings = (value as AssetSettings);
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Executes the remove operation.
        /// </summary>
        public void doArchive()
        {
            IDataArchive ds = getInterface<IDataArchive>();

            if (ds != null)
            {
                ds.Archive(fId2);
            }
            else
            {
                FileStorage.Remove(fId2);
            }
        }

        /// <summary>
        /// Executes the list operation.
        /// </summary>
        ///
        /// <returns>
        /// A List&lt;String&gt;
        /// </returns>
        public List<String> doList()
        {
            IDataStorage ds = getInterface<IDataStorage>();

            if (ds != null)
            {
                return ds.Files();
            }
            else
            {
                return FileStorage.Keys.ToList();
            }
        }

        /// <summary>
        /// Executes the load operation.
        /// </summary>
        ///
        /// <param name="fn"> The filename. </param>
        ///
        /// <returns>
        /// A String.
        /// </returns>
        public String doLoad(String fn)
        {
            IDataStorage ds = getInterface<IDataStorage>();

            if (ds != null)
            {
                return ds.Load(fn);
            }
            else
            {
                return FileStorage[fn];
            }
        }

        /// <summary>
        /// Executes the remove operation.
        /// </summary>
        public void doRemove()
        {
            IDataStorage ds = getInterface<IDataStorage>();

            if (ds != null)
            {
                ds.Delete(fId1);
            }
            else
            {
                FileStorage.Remove(fId1);
            }
        }

        /// <summary>
        /// Executes the store operation.
        /// </summary>
        public void doStore()
        {
            IDataStorage ds = getInterface<IDataStorage>();

            if (ds != null)
            {
                ds.Save(fId1, fData);
                ds.Save(fId2, fData);
            }
            else
            {
                FileStorage[fId1] = fData;
                FileStorage[fId2] = fData;
            }
        }

        /// <summary>
        /// Test if asset1 can find the Logger (asset3) thru the AssetManager.
        /// 
        /// This method can be called by the Game Engine.
        /// </summary>
        ///
        /// <remarks>
        /// This method does not belong here in a base class. So for testing purposes only.
        /// </remarks>
        ///
        /// <param name="msg"> The message. </param>
        public void publicMethod(String msg)
        {
            //! TODO Nicer would be to return the correct type of Asset.
            //
            List<IAsset> loggers = AssetManager.Instance.findAssetsByClass("Logger");

            if (loggers.Count > 0)
            {
                foreach (IAsset l in loggers)
                {
                    (l as Logger).log(l.Id + " - " + msg);
                }
            }
        }

        #endregion Methods
    }
}