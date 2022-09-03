using CodeM.Common.Ioc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using TestExample;

namespace Test
{
    [TestClass]
    public class _05_GetObjectByConfig
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
        /// 获取defaultPerson配置实例，应不为null
        /// </summary>
        [TestMethod]
        public void GetDefaultPerson()
        {
            object person = Wukong.GetObjectById("defaultPerson");
            Assert.IsNotNull(person);
        }

        /// <summary>
        /// 获取initNamePerson配置实例，应不为null，且Name属性值为配置内容
        /// </summary>
        [TestMethod]
        public void GetInitNamePerson()
        {
            Person person = Wukong.GetObjectById<Person>("initNamePerson");
            Assert.IsNotNull(person);
            Assert.AreEqual("张三", person.Name);
        }

        /// <summary>
        /// 获取initNameAndIsChinese配置实例，应不为null，且Name、IsChinese属性值为配置内容
        /// </summary>
        [TestMethod]
        public void GetInitNameAndIsChinesePerson()
        {
            Person person = Wukong.GetObjectById<Person>("initNameAndIsChinese");
            Assert.IsNotNull(person);
            Assert.AreEqual("张三", person.Name);
            Assert.AreEqual(true, person.IsChinese);
        }

        /// <summary>
        /// 获取initNameAndAgeAndIsChinese配置实例，配置属性值并指定值类型，应生成成功，且Name、Age、IsChinese属性值为配置内容
        /// </summary>
        [TestMethod]
        public void GetInitNameAndAgeAndIsChinesePerson()
        {
            Person person = Wukong.GetObjectById<Person>("initNameAndAgeAndIsChinese");
            Assert.IsNotNull(person);
            Assert.AreEqual("张三", person.Name);
            Assert.AreEqual(18, person.Age);
            Assert.AreEqual(true, person.IsChinese);
        }

        /// <summary>
        /// 获取initNameAndSex配置实例，通过传入Sex枚举参数构建Person对象，应成功，且实例Sex属性值为Male。
        /// </summary>
        [TestMethod]
        public void GetInitNameAndSex()
        {
            Person person = Wukong.GetObjectById<Person>("initNameAndSex");
            Assert.IsNotNull(person);
            Assert.AreEqual("张三", person.Name);
            Assert.AreEqual(Sex.Male, person.Sex);
        }

        /// <summary>
        /// 获取intParents配置实例,通过传入father、mother对象引用生成实例，应成功，且实例Father、Mother属性为引用对象的实例。
        /// </summary>
        [TestMethod]
        public void CreateParents()
        {
            Person person = Wukong.GetObjectById<Person>("intParents");
            Assert.IsNotNull(person);
            Assert.IsNotNull(person.Father);
            Assert.IsNotNull(person.Mother);
            Assert.AreEqual("张三", person.Father.Name);
            Assert.AreEqual("李四", person.Mother.Name);
        }

        /// <summary>
        /// 获取usedNames配置实例,通过传入字符串数组构造参数生成实例，应成功，且曾用名为通过数组参数传入的值。
        /// </summary>
        [TestMethod]
        public void CreateUsedNames()
        {
            Person person = Wukong.GetObjectById<Person>("usedNames");
            Assert.IsNotNull(person);
            Assert.IsNotNull(person.UsedNames);
            Assert.AreEqual(2, person.UsedNames.Length);
            Assert.AreEqual("张飞", person.UsedNames[0]);
            Assert.AreEqual("张良", person.UsedNames[1]);
        }

        /// <summary>
        /// 获取initHobbies配置实例,通过传入字符串列表构造参数生成实例，应成功，且爱好为通过列表参数传入的值。
        /// </summary>
        [TestMethod]
        public void CreateInitHobbies()
        {
            Person person = Wukong.GetObjectById<Person>("initHobbies");
            Assert.IsNotNull(person);
            Assert.IsNotNull(person.Hobbies);
            Assert.AreEqual(3, person.Hobbies.Count);
            Assert.AreEqual("\"打篮球\"", person.Hobbies[0]);
            Assert.AreEqual("看电影", person.Hobbies[1]);
            Assert.AreEqual("游泳", person.Hobbies[2]);
        }

        /// <summary>
        /// 获取initChildren配置实例,通过传入对象列表构造参数生成实例，应成功，且子女信息为传入的对象列表内容。
        /// </summary>
        [TestMethod]
        public void CreateInitChildren()
        {
            Person person = Wukong.GetObjectById<Person>("initChildren");
            Assert.IsNotNull(person);
            Assert.IsNotNull(person.Children);
            Assert.AreEqual(2, person.Children.Count);
            Assert.AreEqual("张大成", person.Children[0].Name);
            Assert.AreEqual("张晓晓", person.Children[1].Name);
        }

        /// <summary>
        /// 获取initChildrenByArray配置实例,通过传入对象数组构造参数生成实例，应成功，且子女信息为传入的对象数组内容。
        /// </summary>
        [TestMethod]
        public void CreateInitChildrenByArray()
        {
            Person person = Wukong.GetObjectById<Person>("initChildrenByArray");
            Assert.IsNotNull(person);
            Assert.IsNotNull(person.Children);
            Assert.AreEqual(2, person.Children.Count);
            Assert.AreEqual("张大成", person.Children[0].Name);
            Assert.AreEqual("张晓晓", person.Children[1].Name);
        }

        /// <summary>
        /// 获取setValueProperty配置实例，通过无参构造方法生成实例，并根据配置对实例属性进行赋值，应成功，且实例属性值为配置内容
        /// </summary>
        [TestMethod]
        public void CreateAndSetValueProperty()
        {
            Person person = Wukong.GetObjectById<Person>("setValueProperty");
            Assert.IsNotNull(person);
            Assert.AreEqual("张三", person.Name);
            Assert.AreEqual(18, person.Age);
            Assert.AreEqual(true, person.IsChinese);
            Assert.AreEqual(Sex.Male, person.Sex);
        }

        /// <summary>
        /// 获取setObjectProperty配置实例，通过无参构造方法生成实例，并根据配置对实例属性进行赋值，应成功，且实例属性值为配置内容
        /// </summary>
        [TestMethod]
        public void CreateAndSetObjectProperty()
        {
            Person person = Wukong.GetObjectById<Person>("setObjectProperty");
            Assert.IsNotNull(person);
            Assert.AreEqual("张晓晓", person.Name);
            Assert.IsNotNull(person.Father);
            Assert.IsNotNull(person.Mother);
        }

        /// <summary>
        /// 获取setValueListProperty配置实例，通过无参构造方法生成实例，并根据配置对实例列表属性进行赋值，应成功，且实例属性值为列表配置内容
        /// </summary>
        [TestMethod]
        public void CreateAndSetValueListProperty()
        {
            Person person = Wukong.GetObjectById<Person>("setValueListProperty");
            Assert.IsNotNull(person);
            Assert.IsNotNull(person.Hobbies);
            Assert.AreEqual(3, person.Hobbies.Count);
            Assert.AreEqual("\"打篮球\"", person.Hobbies[0]);
            Assert.AreEqual("看电影", person.Hobbies[1]);
            Assert.AreEqual("游泳", person.Hobbies[2]);
        }

        /// <summary>
        /// 获取setValueArrayProperty配置实例，通过无参构造方法生成实例，并根据配置对实例数组属性进行赋值，应成功，且实例属性值为数组配置内容
        /// </summary>
        [TestMethod]
        public void CreateAndSetValueArrayProperty()
        {
            Person person = Wukong.GetObjectById<Person>("setValueArrayProperty");
            Assert.IsNotNull(person);
            Assert.IsNotNull(person.UsedNames);
            Assert.AreEqual(2, person.UsedNames.Length);
            Assert.AreEqual("张飞", person.UsedNames[0]);
            Assert.AreEqual("张良", person.UsedNames[1]);
        }

        /// <summary>
        /// 获取setValueRefListProperty配置实例，通过无参构造方法生成实例，并根据配置对实例列表属性进行赋值，应成功，且实例属性值为列表配置引用对象内容
        /// </summary>
        [TestMethod]
        public void CreateAndSetValueRefListProperty()
        {
            Person person = Wukong.GetObjectById<Person>("setValueRefListProperty");
            Assert.IsNotNull(person);
            Assert.IsNotNull(person.Children);
            Assert.AreEqual(2, person.Children.Count);
            Assert.AreEqual("张大成", person.Children[0].Name);
            Assert.AreEqual("张晓晓", person.Children[1].Name);
        }

        /// <summary>
        /// 获取setValueRefArrayProperty配置实例，通过无参构造方法生成实例，并根据配置对实例数组属性进行赋值，应成功，且实例属性值为数组配置引用对象内容
        /// </summary>
        [TestMethod]
        public void CreateAndSetValueRefArrayProperty()
        {
            Person person = Wukong.GetObjectById<Person>("setValueRefArrayProperty");
            Assert.IsNotNull(person);
            Assert.IsNotNull(person.PetAnimals);
            Assert.AreEqual(2, person.PetAnimals.Length);
            Assert.AreEqual("Tom", person.PetAnimals[0].Name);
            Assert.AreEqual("Huie", person.PetAnimals[1].Name);
        }

        [TestCleanup]
        public void Clean()
        {
            Wukong.RemoveSearchPath(mDI.FullName);
        }
    }
}
