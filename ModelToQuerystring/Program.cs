using System;
using ModelToQuerystring.Extensions;
using ModelToQuerystring.Model;

namespace ModelToQuerystring
{
    class Program
    {
        static void Main(string[] args)
        {
            var model = new EmployeeModel()
            {
                EmployeeId = 1,
                EmployeeName = "Some one",
                JoiningDate = DateTime.Now.Date.ToString("dd/MM/yyyy"),
                Address = new AddressInfo()
                {
                    Address1 = "506, some building",
                    Address2 = "some street, area",
                    City = "Dubai",
                    State = "Dubai",
                    Country = "United Arab Emirates"
                }
            };

            Console.WriteLine(model.ToQueryString());
            Console.ReadKey();
        }
    }
}