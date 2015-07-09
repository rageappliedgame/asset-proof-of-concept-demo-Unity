// <copyright file="Logger.cs" company="RAGE"> Copyright (c) 2015 RAGE. All rights reserved.
// </copyright>
// <author>Veg</author>
// <date>13-4-2015</date>
// <summary>Implements the logger class</summary>
namespace asset_proof_of_concept_demo_CSharp
{
    using System;

    /// <summary>
    /// A logger.
    /// </summary>
    public class Logger : BaseAsset
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the asset_proof_of_concept_demo_CSharp.Logger class.
        /// </summary>
        public Logger()
            : base()
        {
            // Nothing yet
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Logs.
        /// </summary>
        ///
        /// <param name="msg"> The message. </param>
        public void log(String msg)
        {
            //! See what bridge code to call, Asset, Asset Manager or just expose Default behavior (if any).

            if (Bridge != null && Bridge is ILogger)
            {
                (Bridge as ILogger).doLog(msg);
            }
            else if (AssetManager.Instance.Bridge != null && AssetManager.Instance.Bridge is ILogger)
            {
                (AssetManager.Instance.Bridge as ILogger).doLog(msg);
            }
            else
            {
                Console.WriteLine(msg);
            }
        }

        #endregion Methods
    }
}