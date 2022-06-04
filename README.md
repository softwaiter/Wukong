# .netcore 轻量级IOC容器
本开源项目是基于fastapi.net框架插件机制需要，结合.netcore的反射机制和dynamic动态类型的特点，实现而成的一套IOC容器类库，具备如下特色：

##### :sparkling_heart:无需引用程序集，通过添加搜索路径，自动对路径内程序集进行搜索匹配。

##### :art:通过配置文件，配置对象构造函数、参数，支持简单类型、枚举、数据和列表等常用类型。

##### :grapes:通过配置文件，可以为对象实例的属性赋值，进行实例初始化。

##### :rocket:提供丰富的方法，支持泛型、单例等多种获取对象实例的方式。

<br/>

## 依赖安装
#### Package Manager
```shell
Install-Package CodeM.Common.Ioc -Version 1.1.1
```

#### .NET CLI
```shell
dotnet add package CodeM.Common.Ioc --version 1.1.1
```

#### PackageReference
```xml
<PackageReference Include="CodeM.Common.Ioc" Version="1.1.1" />
```

#### Paket CLI
```shell
paket add CodeM.Common.Ioc --version 1.1.1
```
<br/>

## API

### 一、添加程序集搜索路径（默认只在应用执行目录中搜索）
#### 定义：
public static void AddSearchPath(string path)
#### 参数：
path: 搜索路径，绝对路径。
#### 返回：
无<br/>

### 二、移除程序集搜索路径
#### 定义：
public static void RemoveSearchPath(string path)
#### 参数：
path：搜索路径
#### 返回：
无<br/>

### 三、加载对象配置文件
#### 定义： 
public static void LoadConfig(string configFile, bool append = true)
#### 参数：
configFile: 配置文件，绝对路径。
<br>
append: 是否使用添加模式，默认为true
#### 返回：
无<br/>

### 四、根据类全名称获取对象实例
#### 定义：
public static object GetObject(string classFullName, params object[] args)
#### 参数：
classFullName: 类全名称。
<br>
args: 对象构造参数数组。
#### 返回：
对象实例。<br/>

### 五、根据类全名称获取指定类型的对象实例
#### 定义：
public static T GetObject&lt;T&gt;(string classFullName, params object[] args)
#### 参数：
classFullName: 类全名称。
<br>
args: 对象构造参数数组。
#### 返回：
指定类型的对象实例。<br/>

### 六、以单例模式根据类全名称获取对象实例
#### 定义：
public static object GetSingleObject(string classFullName, params object[] args)
#### 参数：
classFullName: 类全名称。
<br>
args: 对象构造参数数组。
#### 返回：
对象实例，多次调用返回同一实例。<br/>

### 七、以单例模式根据类全名称获取指定类型的对象实例
#### 定义：
public static T GetSingleObject&lt;T&gt;(string classFullName, params object[] args)
#### 参数：
classFullName: 类全名称。
<br>
args: 对象构造参数数组。
#### 返回：
指定类型的对象实例，多次调用返回同一实例。<br/>

### 八、根据配置文件Id获取对象实例
#### 定义：
public static object GetObjectById(string objectId)
#### 参数：
objectId: 配置文件中对象Id
#### 返回：
对象实例。<br/>

### 九、根据配置文件Id获取指定类型对象实例
#### 定义：
public static T GetObjectById&lt;T&gt;(string objectId)
#### 参数：
objectId: 配置文件中对象Id
#### 返回：
指定类型的对象实例。<br/>

### 十、以单例模式根据配置文件Id获取对象实例
#### 定义：
public static object GetSingleObjectById(string objectId)
#### 参数：
objectId: 配置文件中对象Id
#### 返回：
对象实例，多次调用返回同一实例。<br/>

### 十一、以单例模式根据配置文件Id获取指定类型的对象实例
#### 定义：
public static T GetSingleObjectById&lt;T&gt;(string objectId)
#### 参数：
objectId: 配置文件中对象Id
#### 返回：
指定类型的对象实例，多次调用返回同一实例。<br/>

## 代码示例
```c#
dynamic person = IocUtils.GetObject("TestLibrary.Person");
Console.WriteLine(person);

dynamic person2 = IocUtils.GetObject("TestLibrary.Person", "张三");
Console.WriteLine(person2.Name);
```

<br/>

## 配置文件示例

```xml
<!--
配置文件 c:\ioc.xml
-->
<objects>
    <object id="abc">
        <constructor-arg>wangxm</constructor-arg>
        <constructor-arg type="Int16">18</constructor-arg>
        <constructor-arg type="TestLibrary.Sex">Male</constructor-arg>
        
        <property name="name">王小明</property>
        <property name="age">21</property>
    </object>
</objects>
```

```c#
// 加载配置文件
IocUtils.LoadConfig("c:\\ioc.xml");
// 获取配置的对象abc
dynamic abc = IocUtils.GetObjectById("abc");
Console.WriteLine(abc.Age);
```

