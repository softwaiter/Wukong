using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace CodeM.Common.Ioc
{
    internal static class AssemblyUtils
    {
        private static List<string> sSearchPath = new List<string>();
        private static object sSearchPathLock = new object();

        private static ConcurrentDictionary<string, Assembly> sLoadedAssemblies = new ConcurrentDictionary<string, Assembly>();

        private static ConcurrentDictionary<string, Assembly> sTypeAssemblies = new ConcurrentDictionary<string, Assembly>();

        public static void AddSearchPath(string path)
        {
            lock (sSearchPathLock)
            {
                sSearchPath.Add(path.ToLower());
            }
        }

        public static void RemoveSearchPath(string path)
        {
            lock (sSearchPathLock)
            {
                sSearchPath.Remove(path.ToLower());
            }
        }

        private static bool HasType(Assembly assembly, string classFullName)
        {
            if (!assembly.IsDynamic)
            {
                IEnumerable<Type> exportedTypes = assembly.ExportedTypes;
                IEnumerator<Type> typesIterator = exportedTypes.GetEnumerator();
                while (typesIterator.MoveNext())
                {
                    if (typesIterator.Current.FullName.Equals(classFullName,
                        StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private static Assembly FindAssemblyInSearchPath(string classFullName)
        {
            List<string> searchDirs = new List<string>();
            searchDirs.Add(AppDomain.CurrentDomain.BaseDirectory);
            lock (sSearchPathLock)
            {
                searchDirs.AddRange(sSearchPath);
            }

            Assembly assembly;
            foreach (string dir in searchDirs)
            {
                IEnumerable<string> dllFiles = Directory.EnumerateFiles(dir, "*.dll", SearchOption.AllDirectories);
                IEnumerator<string> fileIterator = dllFiles.GetEnumerator();
                while (fileIterator.MoveNext()) {
                    string filename = fileIterator.Current.ToLower();
                    if (!sLoadedAssemblies.TryGetValue(filename, out assembly))
                    {
                        try
                        {
                            assembly = Assembly.LoadFrom(filename);
                            sLoadedAssemblies.TryAdd(filename, assembly);
                        }
                        catch
                        {
                            assembly = null;
                            continue;
                        }
                    }

                    if (assembly != null && HasType(assembly, classFullName))
                    {
                        return assembly;
                    }
                }
            }
            return null;
        }

        private static Assembly GetAssemblyByClassFullName(string classFullName)
        {
            Assembly result = null;
            if (!sTypeAssemblies.TryGetValue(classFullName.ToLower(), out result))
            {
                Assembly[] loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();
                for (int i = 0; i < loadedAssemblies.Length; i++)
                {
                    Assembly currAssembly = loadedAssemblies[i];
                    if (HasType(currAssembly, classFullName))
                    {
                        result = currAssembly;
                        sTypeAssemblies.AddOrUpdate(classFullName.ToLower(), result, (oldKey, oldValue) =>
                        {
                            return result;
                        });
                        break;
                    }
                }
            }
            return result;
        }

        public static Type GetType(string typeFullName)
        {
            Assembly assembly = GetAssemblyByClassFullName(typeFullName);
            if (assembly == null)
            {
                assembly = FindAssemblyInSearchPath(typeFullName);
            }
            if (assembly != null)
            {
                return assembly.GetType(typeFullName, true, true);
            }
            return null;
        }

        public static object CreateInstance(string classFullName, bool ignoreCase = true)
        {
            object result = null;
            Assembly assembly = GetAssemblyByClassFullName(classFullName);
            if (assembly == null)
            {
                assembly = FindAssemblyInSearchPath(classFullName);
            }
            if (assembly != null)
            {
                result = assembly.CreateInstance(classFullName, ignoreCase);
            }
            return result;
        }

        public static object CreateInstance(string classFullName, object[] args = null)
        {
            object result = null;
            Assembly assembly = GetAssemblyByClassFullName(classFullName);
            if (assembly == null)
            {
                assembly = FindAssemblyInSearchPath(classFullName);
            }
            if (assembly != null)
            {
                if (args == null || args.Length == 0)
                {
                    result = assembly.CreateInstance(classFullName, true);
                }
                else
                {
                    result = assembly.CreateInstance(classFullName, true, BindingFlags.CreateInstance, null, args, null, null);
                }
            }
            return result;
        }
    }
}
