using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace CodeM.Common.Ioc
{
    public class IocUtils
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

        /// <summary>
        /// 获取单例实例
        /// </summary>
        public static object GetSingleObject(string classFullName, params object[] args)
        {
            object result = null;
            if (!sSingleInstances.TryGetValue(classFullName.ToLower(), out result))
            { 
                lock (sSingleLock)
                {
                    if (!sSingleInstances.TryGetValue(classFullName.ToLower(), out result))
                    {
                        result = AssemblyUtils.CreateInstance(classFullName, args);
                        if (result != null)
                        {
                            sSingleInstances.AddOrUpdate(classFullName.ToLower(), result, (oldKey, oldValue) => { return result; });
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

                        rlos.List.Clear();
                        foreach (string itemRefId in rlos.ItemList)
                        {
                            object refObj = GetObjectById(itemRefId);
                            rlos.List.Add(refObj);
                        }

                        if (!rlos.IsArray)
                        {
                            result.Insert(rlos.Index, rlos.List);
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
                            Array paramArray = Array.CreateInstance(genericType, rlos.List.Count);
                            rlos.List.CopyTo(paramArray, 0);

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

        public static object GetSingleObjectById(string objectId)
        {
            IocConfig.ObjectConfig item = IocConfig.GetObjectConfig(objectId);
            AssertObjectConfig(item, objectId);

            object[] cps = BuildConstructorArgs(item);
            return GetSingleObject(item.Class, cps);
        }

        public static T GetSingleObjectById<T>(string objectId)
        {
            IocConfig.ObjectConfig item = IocConfig.GetObjectConfig(objectId);
            AssertObjectConfig(item, objectId);

            object[] cps = BuildConstructorArgs(item);
            return GetSingleObject<T>(item.Class, cps);
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
            return GetObject(item.Class, cps);
        }

        public static T GetObjectById<T>(string objectId)
        {
            IocConfig.ObjectConfig item = IocConfig.GetObjectConfig(objectId);
            AssertObjectConfig(item, objectId);

            object[] cps = BuildConstructorArgs(item);
            return GetObject<T>(item.Class, cps);
        }
    }
}