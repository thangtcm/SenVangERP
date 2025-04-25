using System;
using System.Reflection;
using Shared.Attributes;
using Shared.Enums;

namespace Application.Extentions
{
    public static class ResponseFormatExtension
    {

        public static int GetStatusCode(this ResponseCodes value)
        {
            FieldInfo? field = value.GetType().GetField(value.ToString());
            if (field != null)
            {
                object[] attribs = field.GetCustomAttributes(typeof(ServiceStatusAttribute), true);
                if (attribs.Length > 0)
                {
                    return ((ServiceStatusAttribute)attribs[0]).ResponseCode;
                }
            }
            return 9999;
        }

        public static string GetStatusMessage(this ResponseCodes value)
        {
            FieldInfo? field = value.GetType().GetField(value.ToString());
            if (field != null)
            {
                object[] attribs = field.GetCustomAttributes(typeof(ServiceStatusAttribute), true);
                if (attribs.Length > 0)
                {
                    return ((ServiceStatusAttribute)attribs[0]).Message ?? "Lỗi không xác định";
                }
            }
            return value.ToString();
        }
    }
}
