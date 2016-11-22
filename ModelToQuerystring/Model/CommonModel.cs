using System;
using ModelToQuerystring.Attributes;

namespace ModelToQuerystring.Model
{
    public class BasePageModel
    {
        [QueryParameter(PropertyName = "pageId", IsQuerystring = true)]
        public int PageId { get; set; } = 1;

        [QueryParameter(PropertyName = "pageSize", IsQuerystring = true)]
        public int PageSize { get; set; } = 10;
    }

    public class EmployeeModel : BasePageModel
    {
        [QueryParameter(PropertyName = "eId", IsQuerystring = true)]
        public int EmployeeId { get; set; }

        [QueryParameter(PropertyName = "joiningDate", IsQuerystring = true)]
        public string JoiningDate { get; set; } = string.Empty;

        public string EmployeeName { get; set; }

        public AddressInfo Address { get; set; }
    }

    public class AddressInfo
    {
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
    }
}


