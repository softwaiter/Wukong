using CodeM.Common.Tools.Xml;
using System;
using System.Collections.Concurrent;
using System.IO;

namespace CodeM.Common.Ioc
{
    internal static class AliasUtils
    {
        private static ConcurrentDictionary<string, string> sAlias = new ConcurrentDictionary<string, string>();

        public static void LoadFile(string path, bool append = true)
        {
            if (!append)
            {
                sAlias.Clear();
            }

            if (File.Exists(path))
            {
                XmlUtils.Iterate(path, (XmlNodeInfo nodeInfo) =>
                {
                    if (nodeInfo.IsNode && !nodeInfo.IsEndNode)
                    {
                        if (nodeInfo.Path == "/alias/item")
                        {
                            string attrName = nodeInfo.GetAttribute("name");
                            if (attrName == null)
                            {
                                throw new Exception("name属性是必须的" + ": " + path + " line " + nodeInfo.Line);
                            }
                            else if (string.IsNullOrWhiteSpace(attrName))
                            {
                                throw new Exception("name属性不能为空" + ": " + path + " line " + nodeInfo.Line);
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

                            sAlias.AddOrUpdate(attrName.ToLower(), attrClass, (oldKey, oldValue) => { return attrClass; });
                        }
                    }
                    return true;
                });
            }
        }

        public static string GetClassByAlias(string alias)
        {
            string result = null;
            sAlias.TryGetValue(alias.ToLower(), out result);
            return result;
        }

    }
}
