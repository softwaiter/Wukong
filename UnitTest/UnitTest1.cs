using CodeM.Common.Ioc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void BuildByClassName()
        {
            string testlibraryPath = Path.Combine(Environment.CurrentDirectory, "..\\..\\..\\..\\TestLibrary\\bin\\Debug\\netcoreapp2.1");
            DirectoryInfo di = new DirectoryInfo(testlibraryPath);

            IocUtils.AddSearchPath(di.FullName);

            object a = IocUtils.GetObject("TestLibrary.Person");
            Console.WriteLine("a: " + a);

            object b = IocUtils.GetObject("TestLibrary.Person", "张三");
            Console.WriteLine("b: " + b);

            object c = IocUtils.GetObject("TestLibrary.Person", "张三", true);
            Console.WriteLine("c: " + c);

            object d = IocUtils.GetObject("TestLibrary.Person", "张三", (Int16)18, true);
            Console.WriteLine("d: " + d);

            IocUtils.RemoveSearchPath(di.FullName);
        }

        [TestMethod]
        public void BuildByConfigId()
        {
            string testlibraryPath = Path.Combine(Environment.CurrentDirectory, "..\\..\\..\\..\\TestLibrary\\bin\\Debug\\netcoreapp2.1");
            DirectoryInfo di = new DirectoryInfo(testlibraryPath);

            IocUtils.AddSearchPath(di.FullName);

            string config = Path.Combine(Environment.CurrentDirectory, "..\\..\\..\\ioc.xml");
            IocUtils.LoadConfig(config);

            object aaa = IocUtils.GetObjectById("aaa");
            Console.WriteLine("aaa: " + aaa);

            object bbb = IocUtils.GetObjectById("bbb");
            Console.WriteLine("bbb: " + bbb);

            object ccc = IocUtils.GetObjectById("ccc");
            Console.WriteLine("ccc: " + ccc);

            object ddd = IocUtils.GetObjectById("ddd");
            Console.WriteLine("ddd: " + ddd);

            object eee = IocUtils.GetObjectById("eee");
            Console.WriteLine("eee: " + eee);

            object fff = IocUtils.GetObjectById("fff");
            Console.WriteLine("fff: " + fff);

            IocUtils.RemoveSearchPath(di.FullName);
        }

        [TestMethod]
        public void BuildByConfigIdWithObjectReference()
        {
            string testlibraryPath = Path.Combine(Environment.CurrentDirectory, "..\\..\\..\\..\\TestLibrary\\bin\\Debug\\netcoreapp2.1");
            DirectoryInfo di = new DirectoryInfo(testlibraryPath);

            IocUtils.AddSearchPath(di.FullName);

            string config = Path.Combine(Environment.CurrentDirectory, "..\\..\\..\\ioc.xml");
            IocUtils.LoadConfig(config);

            object ggg = IocUtils.GetObjectById("ggg");
            Console.WriteLine("ggg: " + ggg);

            object hhh = IocUtils.GetObjectById("hhh");
            Console.WriteLine("hhh: " + hhh);

            IocUtils.RemoveSearchPath(di.FullName);
        }
    }
}
