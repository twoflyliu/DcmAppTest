using System;
using System.ComponentModel;
using System.Globalization;

namespace CSDcmTest
{
    class CanIdTypeConverter : TypeConverter
    {
        public const int MinCanId = 0x000;
        public const int MaxCanId = 0x7FF;

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return (sourceType == typeof(string)) || base.CanConvertFrom(context, sourceType);
        }

        //From的源是输入
        public override object ConvertFrom(ITypeDescriptorContext context,
            CultureInfo culture, object value)
        {
            if (!(value is string))
            {
                return base.ConvertFrom(context, culture, value);
            }

            int canId = 0;
            try
            {
                canId = Convert.ToInt32((string)value, 16);
            }
            catch (FormatException)
            {
                throw new FormatException("The value should be between 0x000 and 0x7FF");
            }

            if (canId < 0x000 || canId > 0x7FF)
            {
                throw new FormatException("The value should be between 0x000 and 0x7FF");
            }
            return canId;
        }

        // 从内存到界面
        public override object ConvertTo(ITypeDescriptorContext context,
            CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == null)
            {
                throw new ArgumentNullException("destinationType");
            }
            if (destinationType == typeof(string))
            {
                int canId = (int)value;
                return string.Format("{0:X3}", canId);
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}