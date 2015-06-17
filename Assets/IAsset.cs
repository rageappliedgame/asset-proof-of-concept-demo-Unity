// <copyright file="iasset.cs" company="RAGE">
// Copyright (c) 2015 RAGE. All rights reserved.
// </copyright>
// <author>Veg</author>
// <date>10-4-2015</date>
// <summary>Implements the iasset class</summary>
namespace asset_proof_of_concept_demo_CSharp
{
    using System;

    /// <summary>
    /// Interface for asset.
    /// </summary>
    public interface IAsset
    {
        #region Properties

        /// <summary>
        /// Gets the class.
        /// </summary>
        ///
        /// <value>
        /// The class.
        /// </value>
        String Class
        {
            get;
        }

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        ///
        /// <value>
        /// The identifier.
        /// </value>
        String Id
        {
            get;
        }

        #endregion Properties
    }
}