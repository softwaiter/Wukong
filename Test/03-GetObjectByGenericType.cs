using CodeM.Common.Ioc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using TestExample;

namespace Test
{
    [TestClass]
    public class _03_GetObjectByGenericType
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
        /// 通过GetObject泛型方法指定生成Person类型的实例，应不为null
        /// </summary>
        [TestMethod]
        public void GetPersonInstance()
        {
            Person person = Wukong.GetObject<Person>("TestExample.Person");
            Assert.IsNotNull(person);
        }

        /// <summary>
        /// 使用GetObject泛型方法指定相应参数生成Person类型的实例，应不为null，且实例属性为指定参数值
        /// </summary>
        [TestMethod]
        public void GetPersonInstanceWithArgs()
        {
            Person person = Wukong.GetObject<Person>("TestExample.Person", "张三", 18, true);
            Assert.IsNotNull(person);

            Assert.AreEqual("张三", person.Name);
            Assert.AreEqual(18, person.Age);
            Assert.AreEqual(true, person.IsChinese);
        }

        /// <summary>
        /// 使用GetObject泛型方法通过Chinese类型生成Person实例，应不为null，且实例类型为Chinese
        /// </summary>
        [TestMethod]
        public void CreatePersonByChineseType()
        {
            Person person = Wukong.GetObject<Person>("TestExample.Chinese");
            Assert.IsNotNull(person);
            Assert.AreEqual(typeof(Chinese), person.GetType());
        }

        [TestCleanup]
        public void Clean()
        {
            Wukong.RemoveSearchPath(mDI.FullName);
        }
    }
}
