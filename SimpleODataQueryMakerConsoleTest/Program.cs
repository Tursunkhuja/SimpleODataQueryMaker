using SimpleODataQueryMaker;
using System;
using System.Collections.Generic;

namespace SimpleODataQueryMakerConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> propertyPaths = new List<string>
            {
                "GLAccount.Number",
                "Contact.FullName",
                "Contact.MailingAddress.Country.Code",
                "Contact.MailingAddress.City",
                "Contact.MailingAddress.State",
                "Contact.MailingAddress.Country.Name",
                "Id",
                "Title",
            };

            var queryMaker = new ODataQueryMaker();
            var result = queryMaker.GenerateODataQueryString(propertyPaths);
            Console.WriteLine(result);
            Console.ReadKey();
        }
    }
}
