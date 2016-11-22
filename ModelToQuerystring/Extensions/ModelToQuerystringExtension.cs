using System.Collections;
using System.Linq;
using System.Reflection;
using System.Text;
using ModelToQuerystring.Attributes;

namespace ModelToQuerystring.Extensions
{
    /// <summary>
    /// Generate Querystring from model
    /// </summary>
    public static class ModelToQuerystringExtension
    {
        /// <summary>
        ///  convert model to QueryString
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToQueryString<T>(this T obj) where T : class
        {
            var sb = new StringBuilder();
            var data = obj as IEnumerable ?? new[] { obj };

            foreach (var datum in data)
            {
                var t = datum.GetType();
                var properties = t.GetProperties();
                foreach (var prop in properties)
                {
                    var qParam = prop.GetQueryParameterValue();
                    if (prop.CanRead && qParam.IsQuerystring)
                    {
                        var indexes = prop.GetIndexParameters();
                        if (indexes.Any())
                        {
                            var pp = prop.GetValue(datum, new object[] { 1 });
                            sb.Append(ToQueryString(pp));
                        }
                        else if (typeof(IEnumerable).IsAssignableFrom(prop.PropertyType) &&
                                 prop.PropertyType != typeof(string))
                        {
                            sb.Append(ToQueryString(prop.GetValue(datum)));
                        }
                        else if (prop.GetValue(datum, null) != null && prop.PropertyType.FullName != prop.GetValue(datum, null).ToString())
                        {
                            if (!string.IsNullOrWhiteSpace(sb.ToString()) && !sb.ToString().EndsWith("&"))
                                sb.Append("&");

                            var propName = qParam.PropertyName;

                            propName = t.BaseType != null && t.BaseType.Name == "Object" && !qParam.HasPropertyName
                                    ? $"{t.Name}.{propName}"
                                    : propName;

                            sb.Append($"{propName}={prop.GetValue(datum, null)}&");
                        }
                        else if (prop.GetValue(datum, null) != null && prop.PropertyType.FullName == prop.GetValue(datum).ToString())
                        {
                            sb.Append(ToQueryString(prop.GetValue(datum)) + "&");
                        }
                        else
                        {
                            sb.Append(ToQueryString(prop.GetValue(datum, null) + "&") ?? (ToQueryString(prop.GetValue(datum)) + "&"));
                        }
                    }
                }
            }

            return sb.ToString().TrimEnd('&');
        }

        /// <summary>
        /// Get Custom Attribute Property Name
        /// </summary>
        /// <param name="prop"></param>
        /// <returns></returns>
        private static QueryParameterAttribute GetQueryParameterValue(this PropertyInfo prop)
        {
            var customAttribute = prop.GetCustomAttributes(typeof(QueryParameterAttribute), false);
            var parameters = new QueryParameterAttribute() { PropertyName = prop.Name, IsQuerystring = true };

            if (customAttribute.Length > 0)
            {
                var attr = customAttribute.FirstOrDefault() as QueryParameterAttribute;

                if (attr != null)
                {
                    parameters.HasPropertyName = !string.IsNullOrWhiteSpace(attr.PropertyName);
                    parameters.PropertyName = !string.IsNullOrWhiteSpace(attr.PropertyName) ? attr.PropertyName : prop.Name;
                    parameters.IsQuerystring = attr.IsQuerystring;
                }
            }

            return parameters;
        }
    }
}
