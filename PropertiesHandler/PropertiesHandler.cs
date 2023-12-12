using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PropertiesHandler
{
    public static class PropertiesHandler<T1, T2> where T1 : class
                                                     where T2 : class
    {
        /*How about custom object*/
        public static bool Compare(T1? obj1, T2? obj2)
        {
            if (obj1 == null || obj2 == null)
            {
                return false;
            }

            var p1Props = obj1.GetType().GetProperties();
            var p2Props = obj2.GetType().GetProperties();

            foreach (PropertyInfo p1PropInfo in p1Props)
            {
                foreach (PropertyInfo p2PropInfo in p2Props)
                {
                    if (p1PropInfo.Name == p2PropInfo.Name && p1PropInfo.PropertyType == p2PropInfo.PropertyType)
                    {
                        var v1 = p1PropInfo.GetValue(obj1);
                        var v2 = p2PropInfo.GetValue(obj2);

                        if (v1?.Equals(v2) == false)
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Copy src into dest
        /// </summary>
        /// <param name="obj1"></param>
        /// <param name="obj2"></param>
        public static T2? Copy(T1? src, T2? dest)
        {
            if (src is null || dest is null)
            {
                return null;
            }

            var p1Props = src.GetType().GetProperties();
            var p2Props = dest.GetType().GetProperties();

            foreach (PropertyInfo p1PropInfo in p1Props)
            {
                foreach (PropertyInfo p2PropInfo in p2Props)
                {
                    var v1 = p1PropInfo.GetValue(src);

                    if (p1PropInfo.Name == p2PropInfo.Name)
                    {
                        if (p1PropInfo.PropertyType == p2PropInfo.PropertyType)
                        {
                            if (v1 is not null)
                            {
                                p2PropInfo.SetValue(dest, v1);
                            }
                        }
                        else
                        {                               
                            if (v1 is not null)
                            {
                                // Check is Nullable or not
                                if (IsNullable(p1PropInfo.PropertyType))
                                {
                                    Type typeFromNullableTypeOfP1PropInfo = p1PropInfo.PropertyType.GetGenericArguments()[0];
                                    
                                    // Convert Enum to string
                                    if (typeFromNullableTypeOfP1PropInfo == typeof(GenderOptions))
                                    {
                                        p2PropInfo.SetValue(dest, Enum.GetName(typeof(GenderOptions), v1));                                       
                                    }

                                    if (typeFromNullableTypeOfP1PropInfo == typeof(string))
                                    {                                         
                                        if (IsNullable(p2PropInfo.PropertyType))
                                        {
                                            if (p2PropInfo.PropertyType.GetGenericArguments()[0] == typeof(GenderOptions))
                                            {
                                                p2PropInfo.SetValue(dest, Enum.Parse(typeof(GenderOptions), v1.ToString()!, true));
                                            }
                                        } else
                                        {
                                            if (p2PropInfo.PropertyType == typeof(GenderOptions))
                                            {
                                                p2PropInfo.SetValue(dest, Enum.Parse(typeof(GenderOptions), v1.ToString()!, true));
                                            }
                                        }
                                    }
                                } else
                                {
                                    SetAndConvertEnumToAnotherType(p1PropInfo, v1, p2PropInfo, dest);
                                }
                            }
                        }
                    }
                }
            }

            return dest;
        }

        private static void SetAndConvertEnumToAnotherType<T>(PropertyInfo prop1, 
                                                        object valueofProp1, 
                                                        PropertyInfo prop2, 
                                                        T descOfProp2)
        {
            // convert Enum to string into p2PropInfo
            if (prop1.PropertyType == typeof(GenderOptions))
            {
                prop2.SetValue(descOfProp2, Enum.GetName(prop1.PropertyType, valueofProp1));
            }

            // convert string to Enum
            if (prop1.PropertyType == typeof(string))
            {
                if (IsNullable(prop2.PropertyType))
                {
                    if (prop2.PropertyType.GetGenericArguments()[0] == typeof(GenderOptions))
                    {
                        prop2.SetValue(descOfProp2, Enum.Parse(typeof(GenderOptions), valueofProp1.ToString()!, true));
                    }
                }
                else
                {
                    if (prop2.PropertyType == typeof(GenderOptions))
                    {
                        prop2.SetValue(descOfProp2, Enum.Parse(typeof(GenderOptions), valueofProp1.ToString()!, true));
                    }
                }
            }
        }

        private static bool IsNullable(Type type)
        {
            return Nullable.GetUnderlyingType(type) != null;    
        }
    }
}
