﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.17379
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Opds4Net.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "11.0.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute(@"<?xml version=""1.0"" encoding=""utf-16""?>
<ArrayOfString xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
  <string>.7z,application/x-7z-compressed</string>
  <string>.cebx,application/cebx</string>
  <string>.chm,application/x-chm</string>
  <string>.doc,application/msword</string>
  <string>.docx,application/vnd.openxmlformats-officedocument.wordprocessingml.document</string>
  <string>.epub,application/epub+zip</string>
  <string>.htm,text/html</string>
  <string>.html,text/html</string>
  <string>.mobi,application/x-mobipocket-ebook</string>
  <string>.pdf,application/pdf</string>
  <string>.rar,application/x-rar-compressed</string>
  <string>.rtf,application/rtf</string>
  <string>.snb,application/snb</string>
  <string>.txt,text/plain</string>
  <string>.zip,application/zip</string>
  <string>.xps,application/vnd.ms-xpsdocument</string>
</ArrayOfString>")]
        public global::System.Collections.Specialized.StringCollection MimeTypes {
            get {
                return ((global::System.Collections.Specialized.StringCollection)(this["MimeTypes"]));
            }
        }
    }
}
