using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.Storage.Table;
using System.Reflection;
using System.Data.Services.Common;
using System.ComponentModel;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Crondale.AzureWrapper.Storage
{
    public abstract class EntityModel : TableEntity
    {
        public const int MAGIC = 34521;
        [IgnoreProperty]
        protected bool IsLoaded { get; set; }

        public EntityModel()
            : base()
        {

        }

        public EntityModel(string partitionKey, string rowKey)
            : base(partitionKey, rowKey)
        {

        }

        public override IDictionary<string, EntityProperty> WriteEntity(Microsoft.WindowsAzure.Storage.OperationContext operationContext)
        {
            IDictionary<string, EntityProperty> result = base.WriteEntity(operationContext);

            foreach (PropertyInfo pi in this.GetType().GetProperties())
            {
                if (pi.PropertyType == typeof(List<Guid>))
                {
                    result[pi.Name] = new EntityProperty(String.Join(",", (List<Guid>)pi.GetValue(this, null)));
                }
                else if (pi.PropertyType == typeof(List<String>))
                {
                    result[pi.Name] = new EntityProperty(String.Join(";", (List<String>)pi.GetValue(this, null)));
                }
                else if (pi.PropertyType.IsEnum)
                {
                    result[pi.Name] = new EntityProperty((int)pi.GetValue(this, null));
                }
            }

            return result;
        }

        public override void ReadEntity(IDictionary<string, EntityProperty> properties, Microsoft.WindowsAzure.Storage.OperationContext operationContext)
        {
            IsLoaded = true;

            foreach (PropertyInfo pi in this.GetType().GetProperties())
            {
                if (properties.ContainsKey(pi.Name))
                {
                    if (pi.PropertyType == typeof(List<Guid>))
                    {
                        pi.SetValue(this, (from id in properties[pi.Name].StringValue.Split(',') select Guid.Parse(id)).ToList(), null);
                    }
                    else if (pi.PropertyType == typeof(List<String>))
                    {
                        pi.SetValue(this, (from id in properties[pi.Name].StringValue.Split(';') select id).ToList(), null);
                    }
                    else if (pi.PropertyType.IsEnum)
                    {
                        if (properties[pi.Name].PropertyType == EdmType.Int32)
                            pi.SetValue(this, properties[pi.Name].Int32Value, null);
                        else
                        {
                            try
                            {
                                pi.SetValue(this, Enum.Parse(pi.PropertyType, properties[pi.Name].StringValue), null);
                            }
                            catch
                            {

                            }
                        }
                    }
                }
                else
                {
                    object[] attributes = pi.GetCustomAttributes(typeof(DefaultValueAttribute), false);
                    if (attributes.Length > 0)
                    {
                        DefaultValueAttribute def = attributes[0] as DefaultValueAttribute;
                        pi.SetValue(this, def.Value, null);
                    }
                }


            }

            base.ReadEntity(properties, operationContext);
        }

        public String GetTableName()
        {
            EntityTableAttribute attr = (Attribute.GetCustomAttributes(this.GetType(), typeof(EntityTableAttribute)).FirstOrDefault() as EntityTableAttribute);
            
            return attr == null ? this.GetType().Name.ToLower() : attr.TableName;

        }



        public byte[] Serialize()
        {
            EntityModel em = this;

            MemoryStream memoryStream = new MemoryStream();


            using (BinaryWriter bw = new BinaryWriter(memoryStream))
            {

                bw.Write(MAGIC);
                bw.Write(em.GetType().Assembly.FullName);
                bw.Write(em.GetType().FullName);

                bw.Write(em.PartitionKey);
                bw.Write(em.RowKey);
                bw.Write(em.ETag);

                IDictionary<string, EntityProperty> dict = em.WriteEntity(new Microsoft.WindowsAzure.Storage.OperationContext());

                foreach (var kvp in dict)
                {
                    bw.Write(kvp.Key);
                    bw.Write((int)kvp.Value.PropertyType);

                    switch (kvp.Value.PropertyType)
                    {
                        case EdmType.String:
                            if (kvp.Value.StringValue != null)
                            {
                                bw.Write((Boolean)(false));
                                bw.Write((kvp.Value.StringValue));
                            }
                            else
                            {
                                bw.Write((Boolean)(true));
                                bw.Write((""));
                            }

                            break;
                        case EdmType.Boolean:
                            bw.Write((Boolean)kvp.Value.BooleanValue);
                            break;
                        case EdmType.Int32:
                            bw.Write((Int32)kvp.Value.Int32Value);
                            break;
                        case EdmType.Double:
                            bw.Write((Double)kvp.Value.DoubleValue);
                            break;
                        case EdmType.DateTime:
                            if (kvp.Value.DateTime.HasValue)
                                bw.Write(kvp.Value.DateTime.Value.Ticks);
                            else
                                bw.Write((long)0);
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                }
            }

            return memoryStream.ToArray();

        }

        public static object Deserialize(byte[] buffer)
        {


            if (buffer == null)
            {
                return null;
            }

            using (MemoryStream memoryStream = new MemoryStream(buffer))
            {
                using (BinaryReader br = new BinaryReader(memoryStream))
                {
                    int magic = br.ReadInt32();

                    if (magic == MAGIC)
                    {
                        string assName = br.ReadString();
                        string typeName = br.ReadString();

                        EntityModel em = (EntityModel)Activator.CreateInstance(assName, typeName).Unwrap();

                        Dictionary<string, EntityProperty> dict = new Dictionary<string, EntityProperty>();

                        String partitionKey = br.ReadString();
                        String rowKey = br.ReadString();
                        String eTag = br.ReadString();

                        while (br.PeekChar() != -1)
                        {
                            EntityProperty ep = null;
                            string key = br.ReadString();
                            EdmType type = (EdmType)br.ReadInt32();

                            switch (type)
                            {
                                case EdmType.String:
                                    if (br.ReadBoolean()) // it is null
                                    {
                                        br.ReadString();
                                        ep = null;
                                    }
                                    else // it is not null
                                    {
                                        ep = new EntityProperty(br.ReadString());
                                    }
                                    
                                    break;
                                case EdmType.Boolean:
                                    ep = new EntityProperty(br.ReadBoolean());
                                    break;
                                case EdmType.Int32:
                                    ep = new EntityProperty(br.ReadInt32());
                                    break;
                                case EdmType.Double:
                                    ep = new EntityProperty(br.ReadDouble());
                                    break;
                                case EdmType.DateTime:
                                    long ticks = br.ReadInt64();
                                    DateTime? dt = null;
                                    if (ticks != 0)
                                        dt = new DateTime(ticks, DateTimeKind.Utc);

                                    ep = new EntityProperty(dt);
                                    break;
                                default:
                                    throw new NotImplementedException();
                            }

                            dict[key] = ep;
                        }

                        em.ReadEntity(dict, new Microsoft.WindowsAzure.Storage.OperationContext());

                        em.PartitionKey = partitionKey;
                        em.RowKey = rowKey;
                        em.ETag = eTag;

                        return em;
                    }
                }


            }

            BinaryFormatter binaryFormatter = new BinaryFormatter();
            using (MemoryStream memoryStream = new MemoryStream(buffer))
            {
                object result = binaryFormatter.Deserialize(memoryStream);
                return result;
            }
        }

    }
}
