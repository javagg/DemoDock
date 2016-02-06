using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Xml;

namespace TD.SandDock
{
	internal class Class22
	{
	    internal static void smethod_0(SandDockManager sandDockManager_0, XmlNode xmlNode_0)
		{
			TypeConverter converter = TypeDescriptor.GetConverter(typeof(long));
			TypeConverter converter2 = TypeDescriptor.GetConverter(typeof(int));
			TypeConverter converter3 = TypeDescriptor.GetConverter(typeof(Size));
			TypeConverter converter4 = TypeDescriptor.GetConverter(typeof(Point));
			DockControl dockControl = sandDockManager_0.FindControl(new Guid(xmlNode_0.Attributes["Guid"].Value));
			if (dockControl != null)
			{
				if (xmlNode_0.Attributes["LastFocused"] != null)
				{
					dockControl.MetaData.method_0(DateTime.FromFileTime((long)converter.ConvertFromString(null, CultureInfo.InvariantCulture, xmlNode_0.Attributes["LastFocused"].Value)));
				}
				if (xmlNode_0.Attributes["DockedSize"] != null)
				{
					dockControl.MetaData.method_2((int)converter2.ConvertFromString(xmlNode_0.Attributes["DockedSize"].Value));
				}
				if (xmlNode_0.Attributes["PopupSize"] != null)
				{
					dockControl.PopupSize = (int)converter2.ConvertFromString(xmlNode_0.Attributes["PopupSize"].Value);
				}
				dockControl.FloatingLocation = (Point)converter4.ConvertFromString(null, CultureInfo.InvariantCulture, xmlNode_0.Attributes["FloatingLocation"].Value);
				dockControl.FloatingSize = (Size)converter3.ConvertFromString(null, CultureInfo.InvariantCulture, xmlNode_0.Attributes["FloatingSize"].Value);
				if (xmlNode_0.Attributes["LastOpenDockSituation"] != null)
				{
					dockControl.MetaData.method_3((DockSituation)Enum.Parse(typeof(DockSituation), xmlNode_0.Attributes["LastOpenDockSituation"].Value));
				}
				if (xmlNode_0.Attributes["LastFixedDockSituation"] != null)
				{
					dockControl.MetaData.method_4((DockSituation)Enum.Parse(typeof(DockSituation), xmlNode_0.Attributes["LastFixedDockSituation"].Value));
				}
				if (xmlNode_0.Attributes["LastFixedDockLocation"] != null)
				{
					ContainerDockLocation containerDockLocation = (ContainerDockLocation)Enum.Parse(typeof(ContainerDockLocation), xmlNode_0.Attributes["LastFixedDockLocation"].Value);
					if (!Enum.IsDefined(typeof(ContainerDockLocation), containerDockLocation))
					{
						containerDockLocation = ContainerDockLocation.Right;
					}
					dockControl.MetaData.method_1(containerDockLocation);
				}
				if (xmlNode_0.Attributes["LastFloatingWindowGuid"] != null)
				{
					dockControl.MetaData.method_5(new Guid(xmlNode_0.Attributes["LastFloatingWindowGuid"].Value));
				}
				if (xmlNode_0.Attributes["LastDockContainerCount"] != null)
				{
					dockControl.MetaData.Class19_0.Int32_3 = (int)converter2.ConvertFromString(xmlNode_0.Attributes["LastDockContainerCount"].Value);
				}
				if (xmlNode_0.Attributes["LastDockContainerIndex"] != null)
				{
					dockControl.MetaData.Class19_0.Int32_2 = (int)converter2.ConvertFromString(xmlNode_0.Attributes["LastDockContainerIndex"].Value);
				}
				Class22.smethod_1(dockControl, xmlNode_0, dockControl.MetaData.Class19_0, "Docked");
				Class22.smethod_1(dockControl, xmlNode_0, dockControl.MetaData.Class18_0, "Document");
				Class22.smethod_1(dockControl, xmlNode_0, dockControl.MetaData.Class18_1, "Floating");
				return;
			}
		}

		private static void smethod_1(DockControl dockControl_0, XmlNode xmlNode_0, Class18 class18_0, string string_0)
		{
			TypeConverter converter = TypeDescriptor.GetConverter(typeof(int));
			if (xmlNode_0.Attributes[string_0 + "WorkingSize"] != null)
			{
				class18_0.SizeF_0 = SandDockManager.ConvertStringToSizeF(xmlNode_0.Attributes[string_0 + "WorkingSize"].Value);
			}
			if (xmlNode_0.Attributes[string_0 + "WindowGroupGuid"] != null)
			{
				class18_0.Guid_0 = new Guid(xmlNode_0.Attributes[string_0 + "WindowGroupGuid"].Value);
			}
			if (xmlNode_0.Attributes[string_0 + "IndexInWindowGroup"] != null)
			{
				class18_0.Int32_1 = (int)converter.ConvertFromString(null, CultureInfo.InvariantCulture, xmlNode_0.Attributes[string_0 + "IndexInWindowGroup"].Value);
			}
			if (xmlNode_0.Attributes[string_0 + "SplitPath"] != null)
			{
				class18_0.Int32_0 = Class22.smethod_2(xmlNode_0.Attributes[string_0 + "SplitPath"].Value);
			}
		}

		private static int[] smethod_2(string string_0)
		{
			TypeConverter converter = TypeDescriptor.GetConverter(typeof(int));
			string[] array = string_0.Split(new char[]
			{
				'|'
			});
			int[] array2 = new int[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array2[i] = (int)converter.ConvertFromString(null, CultureInfo.InvariantCulture, array[i]);
			}
			return array2;
		}

		internal static void smethod_3(DockControl dockControl_0, XmlTextWriter xmlTextWriter_0)
		{
			TypeConverter converter = TypeDescriptor.GetConverter(typeof(long));
			TypeConverter converter2 = TypeDescriptor.GetConverter(typeof(int));
			TypeConverter converter3 = TypeDescriptor.GetConverter(typeof(Size));
			TypeConverter converter4 = TypeDescriptor.GetConverter(typeof(Point));
			xmlTextWriter_0.WriteStartElement("Window");
			xmlTextWriter_0.WriteAttributeString("Guid", dockControl_0.Guid.ToString());
			xmlTextWriter_0.WriteAttributeString("LastFocused", converter.ConvertToString(null, CultureInfo.InvariantCulture, dockControl_0.MetaData.LastFocused.ToFileTime()));
			xmlTextWriter_0.WriteAttributeString("DockedSize", converter2.ConvertToString(null, CultureInfo.InvariantCulture, dockControl_0.MetaData.DockedContentSize));
			xmlTextWriter_0.WriteAttributeString("PopupSize", converter2.ConvertToString(null, CultureInfo.InvariantCulture, dockControl_0.PopupSize));
			xmlTextWriter_0.WriteAttributeString("FloatingLocation", converter4.ConvertToString(null, CultureInfo.InvariantCulture, dockControl_0.FloatingLocation));
			xmlTextWriter_0.WriteAttributeString("FloatingSize", converter3.ConvertToString(null, CultureInfo.InvariantCulture, dockControl_0.FloatingSize));
			xmlTextWriter_0.WriteAttributeString("LastOpenDockSituation", dockControl_0.MetaData.LastOpenDockSituation.ToString());
			xmlTextWriter_0.WriteAttributeString("LastFixedDockSituation", dockControl_0.MetaData.LastFixedDockSituation.ToString());
			xmlTextWriter_0.WriteAttributeString("LastFixedDockLocation", dockControl_0.MetaData.LastFixedDockSide.ToString());
			xmlTextWriter_0.WriteAttributeString("LastFloatingWindowGuid", dockControl_0.MetaData.LastFloatingWindowGuid.ToString());
			xmlTextWriter_0.WriteAttributeString("LastDockContainerCount", converter2.ConvertToString(null, CultureInfo.InvariantCulture, dockControl_0.MetaData.Class19_0.Int32_3));
			xmlTextWriter_0.WriteAttributeString("LastDockContainerIndex", converter2.ConvertToString(null, CultureInfo.InvariantCulture, dockControl_0.MetaData.Class19_0.Int32_2));
			Class22.smethod_4(dockControl_0, xmlTextWriter_0, dockControl_0.MetaData.Class19_0, "Docked");
			Class22.smethod_4(dockControl_0, xmlTextWriter_0, dockControl_0.MetaData.Class18_0, "Document");
			Class22.smethod_4(dockControl_0, xmlTextWriter_0, dockControl_0.MetaData.Class18_1, "Floating");
			xmlTextWriter_0.WriteEndElement();
		}

		private static void smethod_4(DockControl dockControl_0, XmlTextWriter xmlTextWriter_0, Class18 class18_0, string string_0)
		{
			TypeConverter converter = TypeDescriptor.GetConverter(typeof(int));
			xmlTextWriter_0.WriteAttributeString(string_0 + "WorkingSize", SandDockManager.ConvertSizeFToString(class18_0.SizeF_0));
			xmlTextWriter_0.WriteAttributeString(string_0 + "WindowGroupGuid", class18_0.Guid_0.ToString());
			xmlTextWriter_0.WriteAttributeString(string_0 + "IndexInWindowGroup", converter.ConvertToString(null, CultureInfo.InvariantCulture, class18_0.Int32_1));
			xmlTextWriter_0.WriteAttributeString(string_0 + "SplitPath", Class22.smethod_5(class18_0.Int32_0));
		}

		private static string smethod_5(int[] int_0)
		{
			var converter = TypeDescriptor.GetConverter(typeof(int));
			var array = new string[int_0.Length];
			for (var i = 0; i < int_0.Length; i++)
			{
				array[i] = converter.ConvertToString(null, CultureInfo.InvariantCulture, int_0[i]);
			}
			return string.Join("|", array);
		}
	}
}
