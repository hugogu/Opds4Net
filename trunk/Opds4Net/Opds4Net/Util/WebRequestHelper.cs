using System;
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
            Assembly aNetAssembly = Assembly.GetAssembly(typeof(System.Net.Configuration.SettingsSection));
            if (aNetAssembly != null)
            {
                //Use the assembly in order to get the internal type for the internal class
                Type aSettingsType = aNetAssembly.GetType("System.Net.Configuration.SettingsSectionInternal");
                if (aSettingsType != null)
                {
                    //Use the internal static property to get an instance of the internal settings class.
                    //If the static instance isn't created allready the property will create it for us.
                    object anInstance = aSettingsType.InvokeMember("Section",
                      BindingFlags.Static | BindingFlags.GetProperty | BindingFlags.NonPublic, null, null, new object[] { });

                    if (anInstance != null)
                    {
                        //Locate the private bool field that tells the framework is unsafe header parsing should be allowed or not
                        FieldInfo aUseUnsafeHeaderParsing = aSettingsType.GetField("useUnsafeHeaderParsing", BindingFlags.NonPublic | BindingFlags.Instance);
                        if (aUseUnsafeHeaderParsing != null)
                        {
                            aUseUnsafeHeaderParsing.SetValue(anInstance, true);
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
