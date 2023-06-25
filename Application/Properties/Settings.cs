namespace Flextronics.Applications.ApplicationFactory.Properties
{
    using System;
    using System.CodeDom.Compiler;
    using System.Configuration;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    [CompilerGenerated, GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "8.0.0.0")]
    internal sealed class Settings : ApplicationSettingsBase
    {
        private static Settings defaultInstance = ((Settings) SettingsBase.Synchronized(new Settings()));

        [ApplicationScopedSetting, SpecialSetting(SpecialSetting.WebServiceUrl), DebuggerNonUserCode, DefaultSettingValue("http://www.maticsoft.com/upserver.asmx")]
        public string Codematic_UpServer_UpServer
        {
            get
            {
                return (string) this["Codematic_UpServer_UpServer"];
            }
        }

        public static Settings Default
        {
            get
            {
                return defaultInstance;
            }
        }
    }
}

