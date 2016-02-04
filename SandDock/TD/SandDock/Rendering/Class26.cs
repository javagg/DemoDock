using System;
using System.Collections;
using System.ComponentModel;

namespace TD.SandDock.Rendering
{
	internal class Class26 : Class25
	{
		public Class26()
		{
		}

		public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			return new TypeConverter.StandardValuesCollection(new ArrayList
			{
				"Everett",
				"Office 2003",
				"Whidbey",
				"Milborne",
				"Office 2007"
			});
		}
	}
}
