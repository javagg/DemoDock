using System;
using System.Configuration;

namespace TD.SandDock
{
	internal class LayoutSettings : ApplicationSettingsBase
	{
		public LayoutSettings(string key) : base(key)
		{
		}

		public override void Save()
		{
			IsDefault = false;
			base.Save();
		}

		[DefaultSettingValue("true"), UserScopedSetting]
		public bool IsDefault
		{
			get
			{
				return (bool)this["IsDefault"];
			}
			set
			{
				this["IsDefault"] = value;
			}
		}

		[UserScopedSetting]
		public string LayoutXml
		{
			get
			{
				return (string)this["LayoutXml"];
			}
			set
			{
				this["LayoutXml"] = value;
			}
		}
	}
}
