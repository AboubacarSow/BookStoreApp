using Entities.Models;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
namespace Repositories.EFCore.Extensions
{
    public static class OrderQueryBuilder
    {
        public static String CreateOrderQuery<T>(string orderByQueryString)
        {
            var orderParams = orderByQueryString.Trim().Split(',');
            var propertyInfos = typeof(T)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var orderQueryBuilder = new StringBuilder();
            foreach (var param in orderParams)
            {
                if (param is null) continue;
                var propertyFromQueryString = param.Split(' ')[0];
                var objectProperty = propertyInfos
                    .FirstOrDefault(p => p.Name.Equals(propertyFromQueryString,
                    StringComparison.InvariantCultureIgnoreCase));
                if (objectProperty is null) continue;

                var direction = param.EndsWith("desc") ? "descending" : "ascending";

                orderQueryBuilder.Append($"{objectProperty.Name.ToString()} {direction},");
            }
            return  orderQueryBuilder.ToString().TrimEnd(',', ' ');
        }
    }
}
