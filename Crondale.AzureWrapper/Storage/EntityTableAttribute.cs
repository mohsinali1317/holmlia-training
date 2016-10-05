using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crondale.AzureWrapper.Storage
{
    [System.AttributeUsage(System.AttributeTargets.Class)]
    public class EntityTableAttribute : Attribute
    {
        public string TableName { get; set; }

        public EntityTableAttribute(string tableName)
        {
            this.TableName = tableName;
        }
    }
}
