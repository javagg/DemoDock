using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;

namespace TD.SandDock.Rendering
{
	internal class Class25 : TypeConverter
	{
	    public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
		}

		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType == typeof(string) || destinationType == typeof(InstanceDescriptor) || base.CanConvertTo(context, destinationType);
		}

		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value is string)
			{
				string a;
				if ((a = (string)value) != null)
				{
					if (a == "Everett")
					{
						return new EverettRenderer();
					}
					if (a == "Office 2003")
					{
						return new Office2003Renderer();
					}
					if (a == "Whidbey")
					{
						return new WhidbeyRenderer();
					}
					if (a == "Milborne")
					{
						return new MilborneRenderer();
					}
					if (a == "Office 2007")
					{
						return new Office2007Renderer();
					}
				}
				return null;
			}
			return base.ConvertFrom(context, culture, value);
		}

		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == typeof(string))
			{
				if (!(value is string))
				{
					return value.ToString();
				}
				return value;
			}
			else
			{
				if (destinationType != typeof(InstanceDescriptor))
				{
					return base.ConvertTo(context, culture, value, destinationType);
				}
				ConstructorInfo constructor = value.GetType().GetConstructor(Type.EmptyTypes);
				return new InstanceDescriptor(constructor, new object[0], true);
			}
		}

		public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			ArrayList arrayList = new ArrayList();
			if (context?.Instance is DockContainer)
			{
				arrayList.Add("(default)");
			}
			arrayList.Add("Everett");
			arrayList.Add("Office 2003");
			arrayList.Add("Whidbey");
			arrayList.Add("Office 2007");
			return new StandardValuesCollection(arrayList);
		}

		public override bool GetStandardValuesExclusive(ITypeDescriptorContext context) => true;

	    public override bool GetStandardValuesSupported(ITypeDescriptorContext context) => true;
	}
}
