using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Drawing;
using System.Globalization;
using System.Reflection;

namespace TD.SandDock
{
	internal class Class23 : TypeConverter
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
			if (destinationType == typeof(InstanceDescriptor))
			{
				if (value.GetType().Name == "ControlLayoutSystem" || value.GetType().Name == "DocumentLayoutSystem")
				{
					Type type = value.GetType();
					type.Assembly.GetType("TD.SandDock.LayoutSystemBase");
					Type type2 = type.Assembly.GetType("TD.SandDock.DockControl");
					ConstructorInfo constructor = type.GetConstructor(new Type[]
					{
						typeof(SizeF),
						this.MakeArrayType(type2),
						type2
					});
					PropertyInfo property = type.GetProperty("Controls", BindingFlags.Instance | BindingFlags.Public);
					ICollection collection = (ICollection)property.GetValue(value, null);
					object[] array = (object[])Activator.CreateInstance(this.MakeArrayType(type2), new object[]
					{
						collection.Count
					});
					collection.CopyTo(array, 0);
					PropertyInfo property2 = type.GetProperty("WorkingSize", BindingFlags.Instance | BindingFlags.Public);
					SizeF sizeF = (SizeF)property2.GetValue(value, null);
					PropertyInfo property3 = type.GetProperty("SelectedControl", BindingFlags.Instance | BindingFlags.Public);
					object value2 = property3.GetValue(value, null);
					return new InstanceDescriptor(constructor, new[]
					{
						sizeF,
						array,
						value2
					});
				}
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}

		private Type MakeArrayType(Type firstType)
		{
			return firstType.Assembly.GetType(firstType.FullName + "[]");
		}
	}
}
