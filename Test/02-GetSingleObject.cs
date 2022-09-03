using CodeM.Common.Ioc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace Test
{
    [TestClass]
    public class _02_GetSingleObject
    {
        DirectoryInfo mDI;

        [TestInitialize]
        public void Init()
        {
            string testlibraryPath = Path.Combine(Environment.CurrentDirectory, "..\\..\\..\\..\\TestExample\\bin\\Debug\\netcoreapp3.1");
            mDI = new DirectoryInfo(testlibraryPath);

            Wukong.AddSearchPath(mDI.FullName);
        }

        /// <summary>
        /// 通过GetSingleObject方法使用无参构造方法获取两次"TestExample.Person"实例，判断两次获取的实例是否为同一个，应为True
        /// </summary>
        [TestMethod]
        public void CompareTowPersonObjectIsEqualed()
        {
            Object person = Wukong.GetSingleObject("TestExample.Person");
            Object person2 = Wukong.GetSingleObject("TestExample.Person");
            Assert.AreEqual(person, person2);
        }

        /// <summary>
        /// 通过GetSingleObject方法使用相同构造参数获取两次"TestExample.Person"实例，判断两次获取的实例是否为同一个，应为True
        /// </summary>
        [TestMethod]
        public void CompareTowSameArgsPersonObjectIsEqualed()
        {
            Object person = Wukong.GetSingleObject("TestExample.Person", "张三", true);
            Object person2 = Wukong.GetSingleObject("TestExample.Person", "张三", true);
            Assert.AreEqual(person, person2);
        }

        /// <summary>
        /// 通过GetSingleObject方法使用不同构造参数获取两次"TestExample.Person"实例，判断两次获取的实例是否为同一个，应为False
        /// </summary>
        [TestMethod]
        public void CompareTowDiffArgsPersonObjectIsNotEqualed()
        {
            Object person = Wukong.GetSingleObject("TestExample.Person", "张三", true);
            Object person2 = Wukong.GetSingleObject("TestExample.Person", "李四", true);
            Assert.AreNotEqual(person, person2);
        }

        [TestCleanup]
        public void Clean()
        {
            Wukong.RemoveSearchPath(mDI.FullName);
        }
    }
}
