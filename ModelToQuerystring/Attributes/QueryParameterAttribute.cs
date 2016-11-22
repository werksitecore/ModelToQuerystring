using System;

namespace ModelToQuerystring.Attributes
{
    public class QueryParameterAttribute : Attribute
    {
        internal bool HasPropertyName { get; set; }
        public string PropertyName;
        public bool IsQuerystring;

        public QueryParameterAttribute()
        {
            IsQuerystring = true;
        }
    }
}
