using System.Collections.Concurrent;

namespace CodeM.Common.Ioc
{
    public class IocUtils
    {
        private static bool sInited = false;

        private static ConcurrentDictionary<string, object> sSingleInstances = new ConcurrentDictionary<string, object>();

        /// <summary>
        /// 增加文件搜索位置
        /// </summary>
        public static void AddSearchPath(string path)
        {
            AssemblyUtils.AddSearchPath(path);
        }

        /// <summary>
        /// 移除文件搜索位置
        /// </summary>
        public static void RemoveSearchPath(string path)
        {
            AssemblyUtils.RemoveSearchPath(path);
        }

        /// <summary>
        /// 加载别名设置文件
        /// </summary>
        /// <param name="settingFile"></param>
        public static void LoadAliasSetting(string settingFile, bool append = true)
        {
            AliasUtils.LoadFile(settingFile, append);
        }

        /// <summary>
        /// 获取单例实例
        /// </summary>
        public static object GetSingleInstance(string classFullName, bool ignoreCase = false)
        {
            object result = null;
            if (!sSingleInstances.TryGetValue(classFullName.ToLower(), out result))
            {
                result = AssemblyUtils.CreateInstance(classFullName, ignoreCase);
                if (result != null)
                {
                    sSingleInstances.AddOrUpdate(classFullName.ToLower(), result, (oldKey, oldValue) => { return result; });
                }
            }
            return result;
        }

        public static T GetSingleInstance<T>(string classFullName, bool ignoreCase = false)
        {
            object result = GetSingleInstance(classFullName, ignoreCase);
            if (result != null)
            {
                return (T)result;
            }
            return default(T);
        }

        public static object GetSingleInstanceByAlias(string alias)
        {
            string classFullName = AliasUtils.GetClassByAlias(alias);
            return GetSingleInstance(classFullName);
        }

        public static T GetSingleInstanceByAlias<T>(string alias)
        {
            string classFullName = AliasUtils.GetClassByAlias(alias);
            return GetSingleInstance<T>(classFullName);
        }

        /// <summary>
        /// 获取实例
        /// </summary>
        public static object GetInstance(string classFullName, bool ignoreCase = false)
        {
            return AssemblyUtils.CreateInstance(classFullName, ignoreCase);
        }

        public static T GetInstance<T>(string classFullName, bool ignoreCase = false)
        {
            object result = GetInstance(classFullName, ignoreCase);
            if (result != null)
            {
                return (T)result;
            }
            return default(T);
        }

        public static object GetInstanceByAlias(string alias)
        {
            string classFullName = AliasUtils.GetClassByAlias(alias);
            return GetInstance(classFullName);
        }

        public static T GetInstanceByAlias<T>(string alias)
        {
            string classFullName = AliasUtils.GetClassByAlias(alias);
            return GetInstance<T>(classFullName);
        }
    }
}