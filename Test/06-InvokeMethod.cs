using CodeM.Common.Ioc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using TestExample;

namespace Test
{

    [TestClass]
    public class _06_InvokeMethod
    {
        DirectoryInfo mDI;

        [TestInitialize]
        public void Init()
        {
            string testlibraryPath = Path.Combine(Environment.CurrentDirectory, "..\\..\\..\\..\\TestExample\\bin\\Debug\\netcoreapp3.1");
            mDI = new DirectoryInfo(testlibraryPath);

            Wukong.AddSearchPath(mDI.FullName);

            string config = Path.Combine(Environment.CurrentDirectory, "..\\..\\..\\config\\ioc.xml");
            Wukong.LoadConfig(config);
        }

        /// <summary>
        /// 使用Invoke通过方法名GetName获取Person用户名，应返回正确。
        /// </summary>
        [TestMethod]
        public void GetNameByInvoke()
        {
            Person pernson = new Person("张三");
            object name = Wukong.Invoke(pernson, "GetName");
            Assert.IsNotNull(name);
            Assert.AreEqual("张三", name);
        }

        /// <summary>
        /// 使用Invoke通过全小写方法名getname获取Person用户名，应返回正确。
        /// </summary>
        [TestMethod]
        public void GetNameByInvokeAndUseLowercase()
        {
            Person pernson = new Person("张三");
            object name = Wukong.Invoke(pernson, "getname");
            Assert.IsNotNull(name);
            Assert.AreEqual("张三", name);
        }

        /// <summary>
        /// 使用泛型Invoke通过方法名GetName获取Person用户名，应返回正确。
        /// </summary>
        [TestMethod]
        public void GetNameByInvokeGeneric()
        {
            Person pernson = new Person("张三");
            string name = Wukong.Invoke<string>(pernson, "GetName");
            Assert.IsNotNull(name);
            Assert.AreEqual("张三", name);
        }

        /// <summary>
        /// 使用泛型Invoke通过全小写方法名getname获取Person用户名，应返回正确。
        /// </summary>
        [TestMethod]
        public void GetNameByInvokeGenericAndUseLowercase()
        {
            Person pernson = new Person("张三");
            string name = Wukong.Invoke<string>(pernson, "getname");
            Assert.IsNotNull(name);
            Assert.AreEqual("张三", name);
        }

        [TestCleanup]
        public void Clean()
        {
            Wukong.RemoveSearchPath(mDI.FullName);
        }
    }
}
