# Simple OData Query Maker
This project to make simple OData query string containing from select and expand query options.
The solution contains from 2 projects:
* **SimpleODataQueryMaker** - class library project that has ODataQueryMaker class. It has one public GenerateODataQueryString method, that you should pass list of string (property paths) and it returns string of generated OData query based on those list of properties.
* **SimpleODataQueryMakerConsoleTest** - console app project to test SimpleODataQueryMaker project.
This console project uses this list of properties as an example:
```csharp
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
```
To use that class library project to generate OData query string you should create a new instance of ODataQueryMaker class and then call GenerateODataQueryString method giving the list of properties. See the next example of code: 
```csharp
  var queryMaker = new ODataQueryMaker();
  var result = queryMaker.GenerateODataQueryString(propertyPaths);
```
The result of the OData query after you write `Console.WriteLine(result);` will be 
**<em>$select=Id,Title&$expand=GLAccount($select=Number),Contact($select=FullName;$expand=MailingAddress($select=State,City;$expand=Country($select=Name,Code)))<em>**
