using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;

namespace CSDcmTest
{
    class SecurityAccessTypeConverter : StringConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            var names =
                SecurityAccessAlgorithManager.Instance().GetSecurityAccessAlgorithNames();
            List<string> ret = new List<string>();
            foreach (var name in names)
            {
                ret.Add(name);
            }
            return new StandardValuesCollection(ret.ToArray());
        }
    }
}