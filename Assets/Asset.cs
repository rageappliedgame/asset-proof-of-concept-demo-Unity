// <copyright file="Asset.cs" company="RAGE">
// Copyright (c) 2015 RAGE All rights reserved.
// </copyright>
// <author>Veg</author>
// <date>10-4-2015</date>
// <summary>Implements the asset class</summary>
namespace asset_proof_of_concept_demo_CSharp
{
	using System;
	using System.Linq;
	using System.Collections.Generic;
	
	/// <summary>
	/// An asset.
	/// </summary>
	public class Asset : BaseAsset
	{
		private Dictionary<String, String> FileStorage = new Dictionary<String, String>();
		
		#region Constructors
		
		/// <summary>
		/// Initializes a new instance of the asset_proof_of_concept_demo_CSharp.Asset class.
		/// </summary>
		public Asset()
			: base()
		{
			// Nothing yet
		}
		
		#endregion Constructors
		
		#region Methods
		
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
		
		String fId1 = "Hello1.txt";
		String fId2 = "Hello2.txt";
		
		String fData = "Hello Storage World";

		/// <summary>
		/// Executes the load operation.
		/// </summary>
		public String doLoad(String fn) {
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
		
		#endregion Methods
	}
}