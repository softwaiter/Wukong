using CodeM.Common.Tools;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace CodeM.Common.Ioc
{
    public class Wukong
    {
        private static ConcurrentDictionary<string, object> sSingleInstances = new ConcurrentDictionary<string, object>();
        private static object sSingleLock = new object();

        /// <summary>
        /// 添加程序集搜索位置
        /// </summary>
        public static void AddSearchPath(string path)
        {
            AssemblyUtils.AddSearchPath(path);
        }

        /// <summary>
        /// 移除程序集搜索位置
        /// </summary>
        public static void RemoveSearchPath(string path)
        {
            AssemblyUtils.RemoveSearchPath(path);
        }

        /// <summary>
        /// 加载对象配置文件
        /// </summary>
        /// <param name="configFile"></param>
        /// <param name="append"></param>
        public static void LoadConfig(string configFile, bool append = true)
        {
            IocConfig.LoadFile(configFile, append);
        }

        private static string GetSingleObjectKey(string classFullName, params object[] args)
        {
            StringBuilder sbResult = new StringBuilder(classFullName);
            foreach (object arg in args)
            {
                if (arg != null)
                {
                    sbResult.Append(arg.ToString());
                }
            }
            return sbResult.ToString().ToLower();
        }

        /// <summary>
        /// 获取单例实例
        /// </summary>
        public static object GetSingleObject(string classFullName, params object[] args)
        {
            object result = null;
            string key = GetSingleObjectKey(classFullName, args);
            if (!sSingleInstances.TryGetValue(key, out result))
            { 
                lock (sSingleLock)
                {
                    if (!sSingleInstances.TryGetValue(key, out result))
                    {
                        result = AssemblyUtils.CreateInstance(classFullName, args);
                        if (result != null)
                        {
                            sSingleInstances.AddOrUpdate(key, result, (oldKey, oldValue) => { return result; });
                        }
                    }
                }
            }
            return result;
        }

        public static T GetSingleObject<T>(string classFullName, params object[] args)
        {
            object result = GetSingleObject(classFullName, args);

            if (result != null)
            {
                return (T)result;
            }
            return default(T);
        }

        /// <summary>
        /// 根据对象配置信息构建构造参数数组
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private static object[] BuildConstructorArgs(IocConfig.ObjectConfig item)
        {
            if (item.RefConstructorParams.Count > 0)
            {
                List<object> result = new List<object>();
                result.AddRange(item.ConstructorParams);

                foreach (object refItem in item.RefConstructorParams)
                {
                    if (refItem is IocConfig.RefObjectSetting)
                    {
                        IocConfig.RefObjectSetting ros = refItem as IocConfig.RefObjectSetting;
                        object refObj = GetObjectById(ros.RefId);
                        result.Insert(ros.Index, refObj);
                    }
                    else if (refItem is IocConfig.RefListObjectSetting)
                    {
                        IocConfig.RefListObjectSetting rlos = refItem as IocConfig.RefListObjectSetting;

                        IList refObjList = Xmtool.Type().CloneList(rlos.List, false);
                        foreach (string itemRefId in rlos.ItemList)
                        {
                            object refObj = GetObjectById(itemRefId);
                            refObjList.Add(refObj);
                        }

                        if (!rlos.IsArray)
                        {
                            result.Insert(rlos.Index, refObjList);
                        }
                        else
                        {
                            Type genericType = Type.GetType("System.Object", true, true);

                            Type listType = rlos.List.GetType();
                            Type[] argTypes = listType.GetGenericArguments();
                            if (argTypes != null && argTypes.Length > 0)
                            {
                                genericType = argTypes[0];
                            }
                            Array paramArray = Array.CreateInstance(genericType, refObjList.Count);
                            refObjList.CopyTo(paramArray, 0);

                            result.Insert(rlos.Index, paramArray);
                        }
                    }
                }

                return result.ToArray();
            }
            else
            {
                return item.ConstructorParams.ToArray();
            }
        }

        private static void SetProperties(IocConfig.ObjectConfig item, object inst)
        {
            List<IocConfig.PropertySetting> props = item.Properties;
            if (props.Count > 0)
            {
                Type instType = inst.GetType();
                foreach (IocConfig.PropertySetting prop in props)
                {
                    PropertyInfo propInfo = instType.GetProperty(prop.Name, BindingFlags.Static |
                        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic |
                        BindingFlags.IgnoreCase);
                    if (propInfo != null && propInfo.CanWrite)
                    {
                        object value = null;
                        if (prop.RefObject != null)
                        {
                            value = GetObjectById(prop.RefObject.RefId);
                        }
                        else if (prop.RefListObject != null)
                        {
                            IList refObjList = Xmtool.Type().CloneList(prop.RefListObject.List, false);
                            foreach (string itemRefId in prop.RefListObject.ItemList)
                            {
                                object refObj = GetObjectById(itemRefId);
                                refObjList.Add(refObj);
                            }

                            if (prop.RefListObject.IsArray)
                            {
                                Type genericType = Type.GetType("System.Object", true, true);

                                Type listType = prop.RefListObject.List.GetType();
                                Type[] argTypes = listType.GetGenericArguments();
                                if (argTypes != null && argTypes.Length > 0)
                                {
                                    genericType = argTypes[0];
                                }
                                Array propArray = Array.CreateInstance(genericType, refObjList.Count);
                                refObjList.CopyTo(propArray, 0);

                                value = propArray;
                            }
                            else
                            {
                                value = refObjList;
                            }
                        }
                        else
                        {
                            if (propInfo.PropertyType.IsEnum)
                            {
                                value = Enum.Parse(propInfo.PropertyType, prop.Value.ToString(), true);
                            }
                            else
                            {
                                value = Convert.ChangeType(prop.Value, propInfo.PropertyType);
                            }
                        }
                        propInfo.SetValue(inst, value);
                    }
                }
            }
        }

        public static object GetSingleObjectById(string objectId)
        {
            IocConfig.ObjectConfig item = IocConfig.GetObjectConfig(objectId);
            AssertObjectConfig(item, objectId);

            object[] cps = BuildConstructorArgs(item);
            object result = GetSingleObject(item.Class, cps);
            SetProperties(item, result);
            return result;
        }

        public static T GetSingleObjectById<T>(string objectId)
        {
            IocConfig.ObjectConfig item = IocConfig.GetObjectConfig(objectId);
            AssertObjectConfig(item, objectId);

            object[] cps = BuildConstructorArgs(item);
            T result = GetSingleObject<T>(item.Class, cps);
            SetProperties(item, result);
            return result;
        }

        /// <summary>
        /// 获取实例
        /// </summary>
        public static object GetObject(string classFullName, params object[] args)
        {
            return AssemblyUtils.CreateInstance(classFullName, args);
        }

        public static T GetObject<T>(string classFullName, params object[] args)
        {
            object result = GetObject(classFullName, args);
            if (result != null)
            {
                return (T)result;
            }
            return default(T);
        }

        private static void AssertObjectConfig(IocConfig.ObjectConfig config, string configId)
        {
            if (config == null)
            {
                throw new Exception(string.Concat("找不到id为“", configId, "”的配置项"));
            }
        }

        public static object GetObjectById(string objectId)
        {
            IocConfig.ObjectConfig item = IocConfig.GetObjectConfig(objectId);
            AssertObjectConfig(item, objectId);

            object[] cps = BuildConstructorArgs(item);
            object result = GetObject(item.Class, cps);
            SetProperties(item, result);
            return result;
        }

        public static T GetObjectById<T>(string objectId)
        {
            IocConfig.ObjectConfig item = IocConfig.GetObjectConfig(objectId);
            AssertObjectConfig(item, objectId);

            object[] cps = BuildConstructorArgs(item);
            T result = GetObject<T>(item.Class, cps);
            SetProperties(item, result);
            return result;
        }
    }
}