using CodeM.Common.Ioc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using TestExample;

namespace Test
{
    [TestClass]
    public class _01_GetObject
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
        /// 生成Person类对象实例，以Object类型返回，应不为null
        /// </summary>
        [TestMethod]
        public void CreatePerson()
        {
            object person = Wukong.GetObject("TestExample.Person");
            Assert.IsNotNull(person);
        }

        /// <summary>
        /// 生成Chinese类对象实例，以Object类型返回，应不为null，且属于Person兼容类型
        /// </summary>
        [TestMethod]
        public void CreateChinese()
        {
            object chinese = Wukong.GetObject("TestExample.Chinese");
            Assert.IsNotNull(chinese);
            Assert.IsTrue(chinese is Person);
        }

        /// <summary>
        /// 生成Person类对象实例，并通过传入不同参数使用不同构造函数，以Object类型返回，应不为null，且属性为传入的参数值
        /// </summary>
        [TestMethod]
        public void CreatePersonByArgs()
        {
            object obj = Wukong.GetObject("TestExample.Person", "Tom");
            Assert.IsNotNull(obj);
            Assert.IsTrue(obj is Person);
            Person person = (Person)obj;
            Assert.AreEqual("Tom", person.Name);
            Assert.AreEqual(false, person.IsChinese);

            object obj2 = Wukong.GetObject("TestExample.Person", "张三", true);
            Assert.IsNotNull(obj2);
            Person person2 = (Person)obj2;
            Assert.AreEqual("张三", person2.Name);
            Assert.AreEqual(true, person2.IsChinese);

            object obj3 = Wukong.GetObject("TestExample.Person", "李四", 18, true);
            Assert.IsNotNull(obj3);
            Person person3 = (Person)obj3;
            Assert.AreEqual("李四", person3.Name);
            Assert.AreEqual(18, person3.Age);
            Assert.AreEqual(true, person3.IsChinese);
        }

        /// <summary>
        /// 通过GetObject方法使用无参构造方法获取两次"TestExample.Person"实例，判断两次获取的实例是否为同一个，应为False
        /// </summary>
        [TestMethod]
        public void NotEqualTowPersonObject()
        {
            Object person = Wukong.GetObject("TestExample.Person");
            Object person2 = Wukong.GetObject("TestExample.Person");
            Assert.AreNotEqual(person, person2);
        }

        [TestCleanup]
        public void Clean()
        {
            Wukong.RemoveSearchPath(mDI.FullName);
        }
    }
}
