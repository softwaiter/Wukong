using CodeM.Common.Ioc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using TestExample;

namespace Test
{
    [TestClass]
    public class _04_GetSingleObjectByGenericType
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
        /// 使用GetSingleObject泛型方法使用无参构造方法获取两次"TestExample.Person"实例，判断两次获取的实例是否为同一个，应为True
        /// </summary>
        [TestMethod]
        public void CompareTowPersonObjectIsEqualed()
        {
            Person person = Wukong.GetSingleObject<Person>("TestExample.Person");
            Person person2 = Wukong.GetSingleObject<Person>("TestExample.Person");
            Assert.AreEqual(person, person2);
        }

        /// <summary>
        /// 使用GetSingleObject泛型方法使用相同构造参数获取两次"TestExample.Person"实例，修改一个实例的属性，另一个实例的属性值应同时修改
        /// </summary>
        [TestMethod]
        public void UpdateSingleObjectProperty()
        {
            Person person = Wukong.GetSingleObject<Person>("TestExample.Person", "张三");
            Person person2 = Wukong.GetSingleObject<Person>("TestExample.Person", "张三");
            Assert.AreEqual(person.Name, person2.Name);

            person.Name = "李四";
            Assert.AreEqual(person.Name, person2.Name);
        }

        /// <summary>
        /// 通过GetSingleObject泛型方法使用不同构造参数获取两次"TestExample.Person"实例，判断两次获取的实例是否为同一个，应为False
        /// </summary>
        [TestMethod]
        public void CompareTowDiffArgsPersonObjectIsNotEqualed()
        {
            Person person = Wukong.GetSingleObject<Person>("TestExample.Person", "张三");
            Person person2 = Wukong.GetSingleObject<Person>("TestExample.Person", "李四");
            Assert.AreNotEqual(person, person2);
        }

        [TestCleanup]
        public void Clean()
        {
            Wukong.RemoveSearchPath(mDI.FullName);
        }
    }
}
