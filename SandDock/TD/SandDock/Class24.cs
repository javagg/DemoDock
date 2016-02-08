using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using System.Windows.Forms;

namespace TD.SandDock
{
	internal class Class24 : TypeConverter
	{
	    public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType == typeof(InstanceDescriptor) || base.CanConvertTo(context, destinationType);
		}

		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException();
			}
			if (destinationType == typeof(InstanceDescriptor) && value.GetType().Name == "SplitLayoutSystem")
			{
				Type type = value.GetType();
				Type baseType = type.BaseType;
				MemberInfo constructor = type.GetConstructor(new Type[]
				{
					typeof(SizeF),
					typeof(Orientation),
					this.MakeArrayType(baseType)
				});
				PropertyInfo property = type.GetProperty("LayoutSystems", BindingFlags.Instance | BindingFlags.Public);
				ICollection collection = (ICollection)property.GetValue(value, null);
				object[] array = (object[])Activator.CreateInstance(this.MakeArrayType(baseType), new object[]
				{
					collection.Count
				});
				collection.CopyTo(array, 0);
				PropertyInfo property2 = type.GetProperty("WorkingSize", BindingFlags.Instance | BindingFlags.Public);
				SizeF sizeF = (SizeF)property2.GetValue(value, null);
				PropertyInfo property3 = type.GetProperty("SplitMode", BindingFlags.Instance | BindingFlags.Public);
				Orientation orientation = (Orientation)property3.GetValue(value, null);
				return new InstanceDescriptor(constructor, new object[]
				{
					sizeF,
					orientation,
					array
				});
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}

		private Type MakeArrayType(Type firstType)
		{
			return firstType.Assembly.GetType(firstType.FullName + "[]");
		}
	}
}
