using CodeM.Common.Tools.Xml;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

namespace CodeM.Common.Ioc
{
    internal static class IocConfig
    {
        //对象引用类型参数对象
        internal class RefObjectSetting
        {
            public RefObjectSetting(string refId, int index)
            {
                RefId = refId;
                Index = index;
            }

            public string RefId
            {
                get;
                set;
            }

            public int Index
            {
                get;
                set;
            }
        }

        internal class RefListObjectSetting
        {
            private List<string> mItemList = new List<string>();

            public RefListObjectSetting(int index, IList list)
            {
                Index = index;
                List = list;
                IsArray = false;
            }

            public RefListObjectSetting(int index, IList list, bool isArray)
            {
                Index = index;
                List = list;
                IsArray = isArray;
            }

            public int Index
            {
                get;
                set;
            }

            public IList List
            {
                get;
                set;
            }

            public bool IsArray
            {
                get;
                set;
            }

            public List<string> ItemList
            {
                get
                {
                    return mItemList;
                }
            }
        }

        //对象配置信息解析存储
        internal class ObjectConfig
        {
            private List<object> mConstructorParams = new List<object>();
            private List<object> mRefConstructorParams = new List<object>();

            public string Id
            {
                get;
                set;
            }

            public string Class
            {
                get;
                set;
            }

            public List<object> ConstructorParams
            {
                get
                {
                    return mConstructorParams;
                }
            }

            public List<object> RefConstructorParams
            {
                get
                {
                    return mRefConstructorParams;
                }
            }
        }

        private static ConcurrentDictionary<string, ObjectConfig> sConfigs = new ConcurrentDictionary<string, ObjectConfig>();

        private static Regex sReInt = new Regex("^[0-9]*$", RegexOptions.None);
        private static Regex sReDouble = new Regex("^[0-9\\.]*$", RegexOptions.None);

        private static void handleListValue(IList list, string value, Type valueType, string xmlPath, int xmlLine)
        {
            if (valueType != null)
            {
                if (valueType.IsEnum)
                {
                    list.Add(Enum.Parse(valueType, value, true));
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(value))
                    {
                        list.Add(null);
                    }
                    else
                    {
                        list.Add(Convert.ChangeType(value, valueType));
                    }
                }
            }
            else if (string.IsNullOrWhiteSpace(value))
            {
                list.Add(null);
            }
            else
            {
                if ((value.StartsWith("\"") && value.EndsWith("\"")) ||
                    (value.StartsWith("'") && value.EndsWith("'")))
                {
                    list.Add(value.Substring(1, value.Length - 2));
                }
                else if ("true".Equals(value, StringComparison.OrdinalIgnoreCase))
                {
                    list.Add(true);
                }
                else if ("false".Equals(value, StringComparison.OrdinalIgnoreCase))
                {
                    list.Add(false);
                }
                else if (sReInt.IsMatch(value))
                {
                    list.Add(int.Parse(value));
                }
                else if (sReDouble.IsMatch(value))
                {
                    list.Add(Double.Parse(value));
                }
                else
                {
                    throw new Exception("不能识别的构造参数" + ": " + xmlPath + " line " + xmlLine);
                }
            };
        }

        public static void LoadFile(string path, bool append = true)
        {
            if (!append)
            {
                sConfigs.Clear();
            }

            if (File.Exists(path))
            {
                ObjectConfig currentConfig = null;
                string currentParamRefId = string.Empty;
                Type currentParamType = null;
                IList currentParamList = null;
                RefListObjectSetting currentParamRefList = null;

                Regex reInt = new Regex("^[0-9]*$", RegexOptions.None);
                Regex reDouble = new Regex("^[0-9\\.]*$", RegexOptions.None);

                XmlUtils.Iterate(path, (XmlNodeInfo nodeInfo) =>
                {
                    if (nodeInfo.Path == "/objects/object")
                    {
                        if (!nodeInfo.IsEndNode)
                        {
                            string attrId = nodeInfo.GetAttribute("id");
                            if (attrId == null)
                            {
                                throw new Exception("id属性是必须的" + ": " + path + " line " + nodeInfo.Line);
                            }
                            else if (string.IsNullOrWhiteSpace(attrId))
                            {
                                throw new Exception("id属性不能为空" + ": " + path + " line " + nodeInfo.Line);
                            }

                            string attrClass = nodeInfo.GetAttribute("class");
                            if (attrClass == null)
                            {
                                throw new Exception("class属性是必须的" + ": " + path + " line " + nodeInfo.Line);
                            }
                            else if (string.IsNullOrWhiteSpace(attrClass))
                            {
                                throw new Exception("class属性不能为空" + ": " + path + " line " + nodeInfo.Line);
                            }

                            currentConfig = new ObjectConfig();
                            currentConfig.Id = attrId;
                            currentConfig.Class = attrClass;
                            sConfigs.AddOrUpdate(attrId.ToLower(), currentConfig, (oldKey, oldValue) => { return currentConfig; });
                        }
                        else
                        {
                            currentConfig = null;
                        }
                    }
                    else if (nodeInfo.Path == "/objects/object/constructor-arg")
                    {
                        if (!nodeInfo.IsEndNode && nodeInfo.HasAttributes())
                        {
                            currentParamRefId = nodeInfo.GetAttribute("ref");
                            if (string.IsNullOrWhiteSpace(currentParamRefId))
                            {
                                string typeName = nodeInfo.GetAttribute("type");
                                if (typeName != null)
                                {
                                    if (!string.IsNullOrWhiteSpace(typeName))
                                    {
                                        if (!typeName.Contains("."))
                                        {
                                            //System下的简单数据类型
                                            currentParamType = Type.GetType("System." + typeName, true, true);
                                        }
                                        else
                                        {
                                            //用户自定义类型
                                            currentParamType = AssemblyUtils.GetType(typeName);
                                        }
                                    }
                                    else
                                    {
                                        throw new Exception("type属性不能为空" + ": " + path + " line " + nodeInfo.Line);
                                    }
                                }
                                else
                                {
                                    currentParamType = null;
                                }
                            }
                        }
                        else
                        {
                            currentParamRefId = string.Empty;
                            currentParamType = null;
                        }
                    }
                    else if (nodeInfo.Path == "/objects/object/constructor-arg/@text")
                    {
                        if (string.IsNullOrWhiteSpace(currentParamRefId))
                        {
                            string paramValue = nodeInfo.Text.Trim();
                            handleListValue(currentConfig.ConstructorParams, paramValue, currentParamType, path, nodeInfo.Line);
                        }
                        else
                        {
                            int index = currentConfig.ConstructorParams.Count +
                                currentConfig.RefConstructorParams.Count;
                            currentConfig.RefConstructorParams.Add(new RefObjectSetting(currentParamRefId, index));
                        }

                        //Hashtable a = new Hashtable();
                        //Dictionary<string, string> b = new Dictionary<string, string>();
                        //HashSet<int> c = new HashSet<int>();

                        //HashSet
                        //Hashtable
                    }
                    else if (nodeInfo.Path == "/objects/object/constructor-arg/list" ||
                            nodeInfo.Path == "/objects/object/constructor-arg/array")
                    {
                        if (!nodeInfo.IsEndNode)
                        {
                            bool isUserType = false;

                            string typeName = nodeInfo.GetAttribute("type");
                            if (typeName != null)
                            {
                                if (!string.IsNullOrWhiteSpace(typeName))
                                {
                                    if (!typeName.Contains("."))
                                    {
                                        //System下的简单数据类型
                                        currentParamType = Type.GetType("System." + typeName, true, true);
                                    }
                                    else
                                    {
                                        //用户自定义类型
                                        currentParamType = AssemblyUtils.GetType(typeName);
                                        isUserType = !currentParamType.IsEnum;
                                    }
                                }
                                else
                                {
                                    throw new Exception("type属性不能为空" + ": " + path + " line " + nodeInfo.Line);
                                }

                                Type geneType = typeof(List<>);
                                Type implType = geneType.MakeGenericType(currentParamType);
                                ConstructorInfo ci = implType.GetConstructor(new Type[] { });
                                currentParamList = ci.Invoke(new Object[] { }) as IList;
                            }
                            else
                            {
                                currentParamType = Type.GetType("System.Object", true, true);
                                currentParamList = new List<object>();
                            }

                            if (isUserType)
                            {
                                int index = currentConfig.ConstructorParams.Count +
                                    currentConfig.RefConstructorParams.Count;
                                bool isArray = nodeInfo.Path == "/objects/object/constructor-arg/array";
                                currentParamRefList = new RefListObjectSetting(index, currentParamList, isArray);
                            }
                        }
                        else
                        {
                            if (currentParamRefList == null)
                            {
                                if (nodeInfo.Path == "/objects/object/constructor-arg/list")
                                {
                                    currentConfig.ConstructorParams.Add(currentParamList);
                                }
                                else if (nodeInfo.Path == "/objects/object/constructor-arg/array")
                                {
                                    Array argArray = Array.CreateInstance(currentParamType, currentParamList.Count);
                                    currentParamList.CopyTo(argArray, 0);
                                    currentConfig.ConstructorParams.Add(argArray);
                                }
                            }
                            else
                            {
                                currentConfig.RefConstructorParams.Add(currentParamRefList);
                            }

                            currentParamType = null;
                            currentParamList = null;
                            currentParamRefList = null;
                        }
                    }
                    else if (nodeInfo.Path == "/objects/object/constructor-arg/list/value" ||
                            nodeInfo.Path == "/objects/object/constructor-arg/array/value")
                    {
                        if (!nodeInfo.IsEndNode)
                        {
                            if (currentParamRefList != null)
                            {
                                currentParamRefId = nodeInfo.GetAttribute("ref");
                                if (!string.IsNullOrWhiteSpace(currentParamRefId))
                                {
                                    currentParamRefList.ItemList.Add(currentParamRefId);
                                }
                                else
                                {
                                    throw new Exception("ref属性不能为空" + ": " + path + " line " + nodeInfo.Line);
                                }
                            }
                        }
                        else
                        {
                            currentParamRefId = string.Empty;
                        }
                    }
                    else if (nodeInfo.Path == "/objects/object/constructor-arg/list/value/@text" ||
                            nodeInfo.Path == "/objects/object/constructor-arg/array/value/@text")
                    {
                        if (currentParamRefList == null)
                        {
                            string paramValue = nodeInfo.Text.Trim();
                            handleListValue(currentParamList, paramValue, currentParamType, path, nodeInfo.Line);
                        }
                    }

                    return true;
                });
            }
        }

        public static ObjectConfig GetObjectConfig(string id)
        {
            ObjectConfig result = null;
            sConfigs.TryGetValue(id.ToLower(), out result);
            return result;
        }

    }
}
