using System.Collections.Generic;
using System.Linq;

namespace SimpleODataQueryMaker
{
    public class ODataQueryMaker
    {
        /// <summary>
        /// Generating OData query string by list of property paths. It returns a query options that contain only from $select and $expand query options
        /// </summary>
        /// <param name="propertyPaths"></param>
        /// <returns></returns>
        public string GenerateODataQueryString(List<string> propertyPaths)
        {
            //property path without dot ('.') should be in $select query
            string select = "$select=" + string.Join(",", propertyPaths.Where(p => !p.Contains('.')));
            //property path with dot ('.') should be in $expand query
            string expand = "";
            var expandableProperties = propertyPaths.Where(p => p.Contains('.')).ToList();
            if (expandableProperties.Any())
            {
                Dictionary<string, string> expandQueryOptions = new Dictionary<string, string> { };
                for (int i = 0; i < expandableProperties.Count; i++)
                {
                    string propertyPath = expandableProperties[i];
                    string queryKey = propertyPath.Substring(0, propertyPath.IndexOf('.'));
                    var queryValue = expandQueryOptions.FirstOrDefault(t => t.Key.Equals(queryKey)).Value;
                    if (queryValue == null)
                    {
                        string query = ConvertToODataQueryOption(propertyPath);
                        if (query.StartsWith("$expand=")) query = query.Substring(8);
                        expandQueryOptions[queryKey] = query;
                    }
                    else
                    {
                        var query = ConvertToODataQueryOption(queryValue, propertyPath, queryKey);
                        expandQueryOptions[queryKey] = query;
                    }
                }
                expand = "$expand=" + string.Join(",", expandQueryOptions.Values);
            }

            return (string.IsNullOrEmpty(expand)) ? select : select + "&" + expand;
        }

        private string ConvertToODataQueryOption(string propertyPath)
        {
            int indexDot = propertyPath.IndexOf('.');
            if (indexDot < 0)
            {
                return "$select=" + propertyPath.Substring(indexDot + 1);
            }
            else
            {
                string queryOption = ConvertToODataQueryOption(propertyPath.Substring(indexDot + 1));
                return $"$expand={propertyPath.Substring(0, indexDot)}({queryOption})";
            }
        }

        private string ConvertToODataQueryOption(string destinationPath, string sourcePath, string parentProperty)
        {
            int indexDot = sourcePath.IndexOf('.');
            if (indexDot < 0)
            {
                if (destinationPath.Contains($"{parentProperty}($select="))
                    return destinationPath.Replace($"{parentProperty}($select=", $"{parentProperty}($select={sourcePath},");
                else
                    return destinationPath.Replace($"{parentProperty}(", $"{parentProperty}($select={sourcePath},");
            }

            string property = sourcePath.Substring(0, indexDot);
            if (destinationPath.Contains($"{property}("))
            {
                return ConvertToODataQueryOption(destinationPath, sourcePath.Substring(indexDot + 1), property);
            }
            else
            {
                string result = ConvertToODataQueryOption(sourcePath);
                return destinationPath.Insert(destinationPath.IndexOf(')'), $";{result}");
            }
        }
    }
}
