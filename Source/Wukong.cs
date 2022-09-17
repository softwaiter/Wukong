using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;

namespace CodeM.Common.Ioc
{
    public class Wukong
    {
        private static ConcurrentDictionary<string, IocFactory> mSingleInstances = new ConcurrentDictionary<string, IocFactory>();

        private static IocFactory GetFactory(Assembly assembly)
        {
            string key = assembly.FullName;
            return mSingleInstances.GetOrAdd<IocFactory>(key, (arg, value) =>
            {
                return new IocFactory();
            }, null);
        }

        /// <summary>
        /// 添加程序集搜索位置
        /// </summary>
        public static void AddSearchPath(string path)
        {
            GetFactory(Assembly.GetCallingAssembly()).AddSearchPath(path);
        }

        /// <summary>
        /// 移除程序集搜索位置
        /// </summary>
        public static void RemoveSearchPath(string path)
        {
            GetFactory(Assembly.GetCallingAssembly()).RemoveSearchPath(path);
        }

        /// <summary>
        /// 加载对象配置文件
        /// </summary>
        /// <param name="configFile"></param>
        /// <param name="append"></param>
        public static void LoadConfig(string configFile, bool append = true)
        {
            GetFactory(Assembly.GetCallingAssembly()).LoadConfig(configFile, append);
        }

        /// <summary>
        /// 加载对象配置文件，可自定义XML对象标签名称
        /// </summary>
        /// <param name="configFile"></param>
        /// <param name="objectPath">自定义对象标签名称</param>
        /// <param name="append"></param>
        public static void LoadConfig(string configFile, string objectPath, bool append = true)
        {
            GetFactory(Assembly.GetCallingAssembly()).LoadConfig(configFile, objectPath, append);
        }

        /// <summary>
        /// 获取单例实例
        /// </summary>
        public static object GetSingleObject(string classFullName, params object[] args)
        {
            return GetFactory(Assembly.GetCallingAssembly()).GetSingleObject(classFullName, args);
        }

        public static T GetSingleObject<T>(string classFullName, params object[] args)
        {
            return GetFactory(Assembly.GetCallingAssembly()).GetSingleObject<T>(classFullName, args);
        }

        public static object GetSingleObjectById(string objectId)
        {
            return GetFactory(Assembly.GetCallingAssembly()).GetSingleObjectById(objectId);
        }

        public static T GetSingleObjectById<T>(string objectId)
        {
            return GetFactory(Assembly.GetCallingAssembly()).GetSingleObjectById<T>(objectId);
        }

        /// <summary>
        /// 获取实例
        /// </summary>
        public static object GetObject(string classFullName, params object[] args)
        {
            return GetFactory(Assembly.GetCallingAssembly()).GetObject(classFullName, args);
        }

        public static T GetObject<T>(string classFullName, params object[] args)
        {
            return GetFactory(Assembly.GetCallingAssembly()).GetObject<T>(classFullName, args);
        }

        public static object GetObjectById(string objectId)
        {
            return GetFactory(Assembly.GetCallingAssembly()).GetObjectById(objectId);
        }

        public static T GetObjectById<T>(string objectId)
        {
            return GetFactory(Assembly.GetCallingAssembly()).GetObjectById<T>(objectId);
        }

        public static bool ExistsClass(string classFullName)
        {
            return GetFactory(Assembly.GetCallingAssembly()).ExistsClass(classFullName);
        }

        public static bool ExistsConfig(string configId)
        {
            return GetFactory(Assembly.GetCallingAssembly()).ExistsConfig(configId);
        }

        public static object Invoke(object inst, string method, params object[] args)
        {
            List<Type> _typs = new List<Type>();
            foreach (object obj in args)
            {
                _typs.Add(obj.GetType());
            }

            MethodInfo mi = inst.GetType().GetMethod(method,
                BindingFlags.Instance | BindingFlags.Public | 
                BindingFlags.NonPublic | BindingFlags.IgnoreCase,
                null, _typs.ToArray(), null);
            if (mi != null)
            {
                return mi.Invoke(inst, args);
            }

            throw new Exception(string.Concat("执行方法异常：", method));
        }

        public static T Invoke<T>(object inst, string method, params object[] args)
        {
            object result = Invoke(inst, method, args);
            if (result != null)
            {
                return (T)result;
            }
            return default(T);
        }
    }
}