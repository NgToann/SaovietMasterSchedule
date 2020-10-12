using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace MasterSchedule.Helpers
{
    public class AppSettingsHelper
    {
        public static string ReadSetting(string key)
        {
            string result = "";
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                result = appSettings[key];
                if (result == null)
                    return "";
            }
            catch (ConfigurationErrorsException)
            {
                return "";
            }
            return result;
        }
    }
}
