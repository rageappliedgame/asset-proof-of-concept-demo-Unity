// <copyright file="IDataArchive.cs" company="RAGE"> Copyright (c) 2015 RAGE. All rights reserved.
// </copyright>
// <author>Veg</author>
// <date>13-4-2015</date>
// <summary>Defines the IDataStorage Interface</summary>
namespace asset_proof_of_concept_demo_CSharp
{
    using System;

    public interface IDataArchive
    {
        /// <summary>
        /// Archives the given file.
        /// </summary>
        ///
        /// <param name="fileId"> The file identifier to delete. </param>
        ///
        /// <returns>
        /// true if it succeeds, false if it fails.
        /// </returns>
        Boolean Archive(String fileId);
    }
}
