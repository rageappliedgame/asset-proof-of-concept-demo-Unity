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
            BaseSettings.UpdateDefaultValues(this);
        }

        /// <summary>
        /// Set the value of (Public Instance) properties to the
        /// <see cref="DefaultValueAttribute"/>'s Value of that property.
        /// </summary>
        ///
        /// <param name="obj"> The object. </param>
        public static void UpdateDefaultValues(Object obj)
        {
            // GetProperties not PCL
            // BindingFlags not PCL
            // foreach (PropertyInfo pi in obj.GetType().GetRuntimeProperties(/*BindingFlags.Instance | BindingFlags.Public*/))
            foreach (PropertyInfo pi in obj.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                Boolean foundandset = false;

                if (pi.CanWrite)
                {
                    foreach (Object att in pi.GetCustomAttributes(typeof(DefaultValueAttribute), false))
                    {
                        if (att is DefaultValueAttribute)
                        {
                            // Pretty Print Assigned Values.
                            // 
                            Object val = ((DefaultValueAttribute)att).Value;

                            if (val == null)
                            {
                                val = "null";
                            }
                            else if (val is String && String.IsNullOrEmpty(val.ToString()))
                            {
                                val = "\"\"";
                            }
                            else
                            {
                                val = String.Format("{0}", val);
                            }

                            Debug.WriteLine(String.Format("Updating {0}.{1} to {2}", obj.GetType().Name, pi.Name, val));

                            pi.SetValue(obj, ((DefaultValueAttribute)att).Value, new object[] { });

                            foundandset = true;

                            break;
                        }
                    }

                    if (foundandset)
                    {
                        continue;
                    }

                    Debug.WriteLine(String.Format("Error Updating Default Value of {0}.{1}", obj.GetType().Name, pi.Name));
                }
            }
        }

        #endregion Methods
    }
}