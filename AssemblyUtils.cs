﻿using System;
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

        private static ConcurrentDictionary<string, bool> sLoadedAssemblies = new ConcurrentDictionary<string, bool>();

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
            return false;
        }

        private static Assembly FindAssemblyInSearchPath(string classFullName)
        {
            List<string> searchDirs = new List<string>();
            lock (sSearchPathLock)
            {
                searchDirs.AddRange(sSearchPath);
            }

            bool abc;
            foreach (string dir in searchDirs)
            {
                IEnumerable<string> dllFiles = Directory.EnumerateFiles(dir, "*.dll", SearchOption.AllDirectories);
                IEnumerator<string> fileIterator = dllFiles.GetEnumerator();
                while (fileIterator.MoveNext()) {
                    string filename = fileIterator.Current.ToLower();
                    if (!sLoadedAssemblies.TryGetValue(filename, out abc))
                    {
                        Assembly assembly = Assembly.LoadFrom(filename);
                        sLoadedAssemblies.TryAdd(filename, true);

                        if (HasType(assembly, classFullName))
                        {
                            return assembly;
                        }
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

        public static object CreateInstance(string classFullName, bool ignoreCase = false)
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
    }
}
