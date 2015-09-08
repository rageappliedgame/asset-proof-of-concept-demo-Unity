// <copyright file="Bridge.cs" company="RAGE"> Copyright (c) 2015 RAGE. All rights reserved.
// </copyright>
// <author>Veg</author>
// <date>27-7-2015</date>
// <summary>Implements a Bridge with 3 interfaces</summary>
using UnityEngine;

namespace asset_proof_of_concept_demo_CSharp
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml.Linq;
    using AssetPackage;

    /// <summary>
    /// A bridge.
    /// </summary>
    class Bridge : IBridge, ILogger, IDataStorage, IDataArchive, IDefaultSettings
    {
        /// <summary>
        /// The storage dir for IDataStorage use. The folder will be located in the Unity Assets Folder.
        /// </summary>
        private static String StorageDir;

        /// <summary>
        /// The archive dir for IDataArchive use. The folder will be located in the Unity Assets Folder.
        /// </summary>
        private static String ArchiveDir;

        /// <summary>
        /// The resource dir for IDefaulSettings use.
        /// </summary>
        ///
        /// <remarks>       This directory could be used to create and save for instance &lt;class&gt;
        ///                 AppSettings.xml Setting files at edit time but NOT at run-time.</remarks>
        /// <remarks>       Reading of files saved in this directory can be done with Unity's
        ///                 Resources.Load() methods, where the name passed is the filename relative to
        ///                 ResourceDir without file extension.</remarks>
        private static String ResourceDir;

        /// <summary>
        /// Initializes static members of the asset_proof_of_concept_demo_CSharp.Bridge class.
        /// </summary>
        static Bridge()
        {
            Debug.Log("Static Bridge Constructor");

            StorageDir = Application.dataPath + "/DataStorage";

            ArchiveDir = Application.dataPath + "/Archive";

            ResourceDir = Application.dataPath + "/Resources";
        }

        /// <summary>
        /// Initializes a new instance of the asset_proof_of_concept_demo_CSharp.Bridge class.
        /// </summary>
        public Bridge()
        {
            Debug.Log("Bridge Constructor");

            this.Prefix = "";

            if (!Directory.Exists(StorageDir))
            {
                Directory.CreateDirectory(StorageDir);
            }

            if (!Directory.Exists(ArchiveDir))
            {
                Directory.CreateDirectory(ArchiveDir);
            }

            if (!Directory.Exists(ResourceDir))
            {
                Directory.CreateDirectory(ResourceDir);
            }
        }

        /// <summary>
        /// Initializes a new instance of the asset_proof_of_concept_demo_CSharp.Bridge class.
        /// </summary>
        ///
        /// <param name="prefix"> The prefix. </param>
        public Bridge(String prefix)
            : base()
        {
            this.Prefix = prefix;
        }

        #region ILogger Members

        /// <summary>
        /// Executes the log operation.
        /// 
        /// Implement this in Game Engine Code.
        /// </summary>
        ///
        /// <param name="msg"> The message. </param>
        public void doLog(string msg)
        {
            //! Unity3D Specific code
            //!
            //! See http://answers.unity3d.com/questions/817189/how-to-redirect-systemconsolewrite-from-dll-to-deb.html
            //!
            Debug.Log(Prefix + msg);
        }

        #endregion

        #region ILogger Properties

        /// <summary>
        /// The prefix.
        /// </summary>
        public String Prefix
        {
            get;
            set;
        }

        #endregion

        #region IDataStorage Members

        /// <summary>
        /// Exists the given file.
        /// </summary>
        ///
        /// <param name="fileId"> The file identifier to delete. </param>
        ///
        /// <returns>
        /// true if it succeeds, false if it fails.
        /// </returns>
        public bool Exists(string fileId)
        {
            return File.Exists(Path.Combine(StorageDir, fileId));
        }

        /// <summary>
        /// Gets the files.
        /// </summary>
        ///
        /// <returns>
        /// A List&lt;String&gt;
        /// </returns>
        public List<String> Files()
        {
            return Directory.GetFiles(StorageDir).ToList().ConvertAll(
                new Converter<String, String>(p => p
                    .Replace(StorageDir, "")
                    .TrimStart(Path.DirectorySeparatorChar))).ToList();
        }

        /// <summary>
        /// Saves the given file.
        /// </summary>
        ///
        /// <param name="fileId">   The file identifier to delete. </param>
        /// <param name="fileData"> Information describing the file. </param>
        public void Save(string fileId, string fileData)
        {
            File.WriteAllText(Path.Combine(StorageDir, fileId), fileData);
        }

        /// <summary>
        /// Loads the given file.
        /// </summary>
        ///
        /// <param name="fileId"> The file identifier to delete. </param>
        ///
        /// <returns>
        /// A String.
        /// </returns>
        public string Load(string fileId)
        {
            return File.ReadAllText(Path.Combine(StorageDir, fileId));
        }

        /// <summary>
        /// Deletes the given fileId.
        /// </summary>
        ///
        /// <param name="fileId"> The file identifier to delete. </param>
        ///
        /// <returns>
        /// true if it succeeds, false if it fails.
        /// </returns>
        public bool Delete(string fileId)
        {
            if (Exists(fileId))
            {
                File.Delete(Path.Combine(StorageDir, fileId));

                return true;
            }

            return false;
        }

        #endregion

        #region IDataArchive Members

        /// <summary>
        /// Archives the given file.
        /// </summary>
        ///
        /// <param name="fileId"> The file identifier to delete. </param>
        ///
        /// <returns>
        /// true if it succeeds, false if it fails.
        /// </returns>
        public bool Archive(string fileId)
        {
            if (File.Exists(Path.Combine(StorageDir, fileId)))
            {
                if (File.Exists(Path.Combine(ArchiveDir, fileId)))
                {
                    File.Delete(Path.Combine(ArchiveDir, fileId));
                }

                String stampName = String.Format("{0}-{1}{2}",
                                                 Path.GetFileNameWithoutExtension(fileId),
                                                 DateTime.Now.ToString("yyyy-MM-dd [HH mm ss fff]"),
                                                 Path.GetExtension(fileId));

                File.Move(Path.Combine(StorageDir, fileId), Path.Combine(ArchiveDir, stampName));

                return true;
            }

            return false;
        }

        #endregion

        #region IVersionAndDependencies

        /*
		public String VersionAndDependenciesXml(Type Class) {
			#region Unity3D

			UnityEngine.Object xml = Resources.Load(String.Format("{0}.VersionAndDependencies", Class.Name));
			if (xml != null && xml is TextAsset) {
				return (xml as TextAsset).text;
			}

			#endregion

			#region .NET

			//return GetEmbeddedResource(GetType().Namespace, String.Format(String.Format("{0}.VersionAndDependencies.xml", GetType().Name))));

			#endregion

			return String.Empty;
		}
		*/
        #endregion

        #region IDefaultSettings

        private String DeriveAssetName(String Class, String Id)
        {
            return String.Format("{0}AppSettings", Class);
        }

        /// <summary>
        /// Query if a 'Class' with Id has default settings.
        /// </summary>
        ///
        /// <param name="Class"> The class. </param>
        /// <param name="Id">    The identifier. </param>
        ///
        /// <returns>
        /// true if default settings, false if not.
        /// </returns>
        public Boolean HasDefaultSettings(String Class, String Id)
        {
            String fn = DeriveAssetName(Class, Id);
            TextAsset ta = Resources.Load(fn, typeof(TextAsset)) as TextAsset;
            return ta != null;
        }

        /// <summary>
        /// Loads default settings for a 'Class' with Id.
        /// </summary>
        ///
        /// <remarks>
        /// Note that in Unity the file has to be located in the Resource Directory of the Assets Folder.
        /// </remarks>
        ///
        /// <param name="Class"> The class. </param>
        /// <param name="Id">    The identifier. </param>
        ///
        /// <returns>
        /// The default settings.
        /// </returns>
        public String LoadDefaultSettings(String Class, String Id)
        {
            TextAsset ta = Resources.Load(DeriveAssetName(Class, Id)) as TextAsset;

            if (ta != null)
            {
                return ta.text;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Saves a default settings.
        /// </summary>
        ///
        /// <param name="Class"> The class. </param>
        /// <param name="Id">    The identifier. </param>
        /// <param name="fileData">   The File Data. </param>
        public void SaveDefaultSettings(String Class, String Id, String fileData)
        {
            if (Application.isEditor)
            {
                File.WriteAllText(Path.Combine(ResourceDir, DeriveAssetName(Class, Id) + ".xml"), Xml);
            }
            else
            {
                Debug.Log("Warning: Cannot save resources at runtime!");
            }
        }

        #endregion IDefaultSettings
    }
}
