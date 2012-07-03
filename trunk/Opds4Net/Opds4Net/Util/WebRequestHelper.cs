using System;
using System.Net.Configuration;
using System.Reflection;
using System.Web;

namespace Opds4Net.Util
{
    /// <summary>
    /// 
    /// </summary>
    public static class WebRequestHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static bool SetAllowUnsafeHeaderParsing()
        {
            //Get the assembly that contains the internal class
            var assembly = Assembly.GetAssembly(typeof(SettingsSection));
            if (assembly != null)
            {
                //Use the assembly in order to get the internal type for the internal class
                var settingsType = assembly.GetType("System.Net.Configuration.SettingsSectionInternal");
                if (settingsType != null)
                {
                    //Use the internal static property to get an instance of the internal settings class.
                    //If the static instance isn't created allready the property will create it for us.
                    var instance = settingsType.InvokeMember("Section",
                      BindingFlags.Static | BindingFlags.GetProperty | BindingFlags.NonPublic, null, null, new object[] { });

                    if (instance != null)
                    {
                        //Locate the private bool field that tells the framework is unsafe header parsing should be allowed or not
                        var useUnsafeHeaderParsing = settingsType.GetField("useUnsafeHeaderParsing", BindingFlags.NonPublic | BindingFlags.Instance);
                        if (useUnsafeHeaderParsing != null)
                        {
                            useUnsafeHeaderParsing.SetValue(instance, true);
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="parameterName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string UpdateUrlParameter(string url, string parameterName, string value)
        {
            var uri = new Uri(url, UriKind.RelativeOrAbsolute);
            var parameters = HttpUtility.ParseQueryString(uri.Query);
            parameters[parameterName] = value;

            return uri.GetComponents(UriComponents.SchemeAndServer | UriComponents.Path, UriFormat.Unescaped) + "?" + parameters.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        public static string CurrentHostUri
        {
            get
            {
                if (HttpContext.Current != null)
                {
                    return HttpContext.Current.Request.Url.GetComponents(UriComponents.HostAndPort | UriComponents.SchemeAndServer, UriFormat.Unescaped) +
                           HttpContext.Current.Request.ApplicationPath.TrimEnd('/');
                }

                return String.Empty;
            }
        }
    }
}
