namespace AssetPackage
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Reflection;

    /// <summary>
    /// A base settings.
    /// </summary>
    public class BaseSettings : ISettings
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the Swiss.BaseSettings class.
        /// </summary>
        public BaseSettings()
        {
            //! Initialize Settings to their specified default values.
            UpdateDefaultValues();
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Set the value of (Public Instance) properties to the <see cref="DefaultValueAttribute"/>'s
        /// Value of that property.
        /// </summary>
        private void UpdateDefaultValues()
        {
            foreach (PropertyInfo pi in GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                if (pi.CanWrite)
                {
                    foreach (Object att in pi.GetCustomAttributes(typeof(DefaultValueAttribute), false))
                    {
                        if (att is DefaultValueAttribute)
                        {
                            Debug.Print("Updating {0}.{1} to {2}", GetType().Name, pi.Name, ((DefaultValueAttribute)att).Value);

                            pi.SetValue(this, ((DefaultValueAttribute)att).Value, new object[] { });

                            return;
                        }
                    }

                    Debug.Print("Error Updating Default Value of {0}", pi.Name);
                }
            }
        }

        #endregion Methods
    }
}