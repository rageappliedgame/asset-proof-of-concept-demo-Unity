// <copyright file="Logger.cs" company="RAGE"> Copyright (c) 2015 RAGE. All rights reserved.
// </copyright>
// <author>Veg</author>
// <date>13-4-2015</date>
// <summary>Implements the logger class</summary>
namespace AssetPackage
{
    using System;
    using System.Diagnostics;

    /// <summary>
    /// A logger.
    /// </summary>
    public class Logger : BaseAsset
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the AssetPackage.Logger class.
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
            // 
            ILogger logger = getInterface<ILogger>();
            if (logger != null)
            {
                // Use a supplied bridge.
                logger.doLog(msg);
            }
            else
            {
                // Default behavior.
                Debug.WriteLine(msg);
            }
        }

        #endregion Methods
    }
}