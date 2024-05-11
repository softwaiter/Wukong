<div align="center">
<article style="display: flex; flex-direction: column; align-items: center; justify-content: center;">
    <p align="center"><img width="256" src="http://res.dayuan.tech/images/wukong.png" /></p>
    <p>
        一款轻量级的IoC容器工具类库
    </p>
</article>
</div>


##  :beginner: 简介

本开源项目是为了满足fastapi.net开源框架的动态插件机制需要，结合.NetCore的反射机制和dynamic动态对象类型的特点，实现而成的一套IoC容器工具类库；本着简单易用、轻量级、最小满足、无第三方依赖等基本原则，Wukong具备如下功能特色：

##### :gem:通过以字符串形式指定类全名称获取对象实例。

##### :sparkling_heart:无需引用程序集，通过添加搜索路径，自动对路径内程序集进行搜索匹配。

##### :octopus:通过传入对象配置文件中的配置Id获取对象实例。

##### :art:通过配置文件，配置对象构造函数、参数，支持简单类型、枚举、数组和列表等常用类型。

##### :grapes:通过配置文件，可以为对象实例的属性赋值，进行实例属性无代码初始化。

##### :rocket:提供丰富的方法，支持泛型、单例等多种获取对象实例的方式。

##### :zap:通过以字符串形式指定方法名执行对象实例中的方法。

<br/>

## :rocket: 版本
**.NetCore3.1**：3.0.0之前的版本全部基于netcore3.1开发。
<br/>
**.Net8.0**：从3.0.0开始，项目全部基于net8.0开发。

<br/>

## :package: 依赖安装
#### Package Manager
```shell
Install-Package Wukong -Version 3.0.0
```

#### .NET CLI
```shell
dotnet add package Wukong --version 3.0.0
```

#### PackageReference
```xml
<PackageReference Include="Wukong" Version="3.0.0" />
```

#### Paket CLI
```shell
paket add Wukong --version 3.0.0
```
<br/>

## :hammer:使用说明

Wukong为了方便调用，将所有功能统一封装集成到静态类Wukong中；在使用时，通过调用Wukong类下的对应静态方法即可，简单方便。

###### 想成功的获取对象实例必须满足下面２个条件中的一个：

- 对象所在的程序集被当前项目正常引用。
- 对象所在的程序集必须在Wukong搜索路径范围内（可通过`AddSearchPath`方法添加搜索路径）。

###### 同样，Wukong为获取对象实例也提供了２个途径：

- 通过指定类全名称和构造参数生成对象实例（无需配置文件）

- 通过指定配置文件中定义的对象Id生成对象实例（需在配置文件中定义，并通过`LoadConfig`方法加载）。



##### 注意：
Wukong对加载的配置文件和单实例对象的缓存以调用程序集为特征进行隔离，不同程序集之间不进行共享。

<br/>

## :gear: API

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

### 三、加载对象配置文件（默认格式，对象路径遵循/objects/object格式）
#### 定义： 
public static void LoadConfig(string configFile, bool append = true)
#### 参数：
configFile: 配置文件，绝对路径。
<br>
append: 是否使用添加模式，默认为true。

#### 返回：
无<br/>

### 四、加载自定义对象配置文件（对象路径可自定义）

#### 定义：

public static void LoadConfig(string configFile, string objectPath, bool append = true)

#### 参数：

configFile: 配置文件，绝对路径。
<br>
objectPath: 自定义对象的XML路径。
<br>
append: 是否使用添加模式，默认为true。

#### 返回：

无<br/>

#### 例子：

```xml
<!--例：c:\\obj.xml对象配置文件采用如下格式-->
<processors>
	<processor id="test" class="TestExample.Person"></processor>
</processors>
```

```c#
// 使用如下方法加载c:\\obj.xml对象配置文件
Wukong.LoadConfig("c:\\obj.xml", "/processors/processor")
```

### 五、判断当前搜索范围内是否有指定类对象

#### 定义：

public static bool ExistsClass(string classFullName)

#### 参数：

classFullName: 类全名称。

#### 返回：

存在返回True；否则，返回False。

### 六、判断已加载对象配置文件中，是否存在指定Id的配置对象

#### 定义：

public static bool ExistsConfig(string configId)

#### 参数：

configId: 配置对象Id。

#### 返回：

存在返回True；否则，返回False。

### 七、根据类全名称获取对象实例

#### 定义：
public static object GetObject(string classFullName, params object[] args)
#### 参数：
classFullName: 类全名称。
<br>
args: 对象构造参数数组。

#### 返回：
生成的对象实例。<br/>

### 八、根据类全名称获取指定类型的对象实例
#### 定义：
public static T GetObject&lt;T&gt;(string classFullName, params object[] args)
#### 参数：
classFullName: 类全名称。
<br>
args: 对象构造参数数组。

#### 返回：
指定类型的对象实例。<br/>

### 九、以单例模式根据类全名称获取对象实例
#### 定义：
public static object GetSingleObject(string classFullName, params object[] args)
#### 参数：
classFullName: 类全名称。
<br>
args: 对象构造参数数组。

#### 返回：
生成的对象实例，相同构造参数多次调用返回同一实例。<br/>

### 十、以单例模式根据类全名称获取指定类型的对象实例
#### 定义：
public static T GetSingleObject&lt;T&gt;(string classFullName, params object[] args)
#### 参数：
classFullName: 类全名称。
<br>
args: 对象构造参数数组。

#### 返回：
指定类型的对象实例，相同构造参数多次调用返回同一实例。<br/>

### 十一、根据配置文件Id获取对象实例
#### 定义：
public static object GetObjectById(string objectId)
#### 参数：
objectId: 配置文件中对象Id
#### 返回：
生成的对象实例。<br/>

### 十二、根据配置文件Id获取指定类型对象实例
#### 定义：
public static T GetObjectById&lt;T&gt;(string objectId)
#### 参数：
objectId: 配置文件中对象Id
#### 返回：
指定类型的对象实例。<br/>

### 十三、以单例模式根据配置文件Id获取对象实例
#### 定义：
public static object GetSingleObjectById(string objectId)
#### 参数：
objectId: 配置文件中对象Id
#### 返回：
对象实例，多次调用返回同一实例。<br/>

### 十四、以单例模式根据配置文件Id获取指定类型的对象实例
#### 定义：
public static T GetSingleObjectById&lt;T&gt;(string objectId)
#### 参数：
objectId: 配置文件中对象Id
#### 返回：
指定类型的对象实例，多次调用返回同一实例。<br/>

### 十五、执行指定对象实例的指定方法，并获取返回值

#### 定义：

public static object Invoke(object inst, string method, params object[] args)

#### 参数：

inst: 要执行的方法所在的对象实例，必填。

method: 要执行方法的名称，忽略大小写，必填。

args: 要执行方法所需要的参数，可选。

#### 返回：

执行方法后得到的返回值。

### 十六、执行指定对象实例的指定方法，并获取指定类型的返回值

#### 定义：

public static T Invoke<T>(object inst, string method, params object[] args)

#### 参数：

T: 泛型参数，指定期望得到的返回值类型，必填。

inst: 要执行的方法所在的对象实例，必填。

method: 要执行方法的名称，忽略大小写，必填。

args: 要执行方法所需要的参数，可选。

#### 返回：

执行方法后得到的返回值。

### 十七、清除缓存

#### 定义：

public static void ClearCache()

#### 参数：

无。

#### 返回：

无。

<br/>

## :memo: 配置文件格式

通过配置文件可以预先把项目会用到的对象在XML文件中进行定义和初始化，配合项目框架使用，可以提供更大的灵活性和易用性。

配置文件默认以`objects`为根节点，每个对象都使用`object`标签定义。

### 一、对象定义

###### 父标签

objects

###### 标签名

object

###### 属性

Id：对象Id，在配置文件内必须唯一；必填。

class：对象类全名称；必填。

```xml
<objects>
    <object id="person" class="TestExample.Person"></object>
</objects>
```

### 二、构造参数定义

通过定义指定类型的参数，生成对象实例时，会自动调用对应的构造参数生成实例。

###### 父标签

object

###### 标签名

constructor-arg

###### 属性

type：参数类型；支持String、Byte、Int16、Int32、Double等基本类型，以及枚举类型（使用类型全名称）。

ref：引用对象的Id值，当参数为对象类型时需要此值。

###### 值

参数值，当未指定type属性值，且值为String时，请使用双引号或单引号进行注明。

### 三、属性定义

通过定义属性，可以在生成实例后对实例进行期望的初始化操作。

###### 父标签

object

###### 标签名

property

###### 属性

name：要初始化的属性名称，可忽略大小写。

ref：引用对象的Id值，当属性类型为对象类型时需要此值。

###### 值

参数值，当未指定type属性值，且值为String时，请使用双引号或单引号进行注明。

### 四、列表内容定义

当构造参数或属性类型为列表时，需要使用该标签。

###### 父标签

constructor-arg、property

###### 标签名

list

###### 属性

type：列表项目类型；支持String、Int16、Int32、Double等基本类型，以及枚举类型（使用类型全名称）。

### 五、数组内容定义

当构造参数或属性类型为列表时，需要使用该标签。

###### 父标签

constructor-arg、property

###### 标签名

array

###### 属性

type：列表项目类型；支持String、Int16、Int32、Double等基本类型，以及枚举类型（使用类型全名称）。

### 六、列表项/数组项

当定义列表项或数组项内容时，需要使用该标签。

###### 父标签

list、array

###### 标签名

value

###### 属性

ref：引用对象的Id值，当列表项或数组项类型为对象类型时需要此值。

###### 值

列表项/数组项的值。

<br/>

## :monkey: 测试代码

### 一、配置文件示例

```xml
<!--
配置文件 c:\ioc.xml
-->
<objects>
    <object id="defaultPerson" class="TestExample.Person">
    </object>
    
    <object id="initNameAndAgeAndIsChinese" class="TestExample.Person">
        <constructor-arg>"张三"</constructor-arg>
        <constructor-arg type="Int32">18</constructor-arg>
        <constructor-arg type="TestExample.Sex">Male</constructor-arg>
    </object>
    
	<object id="fater" class="TestExample.Person">
		<constructor-arg>"张三"</constructor-arg>
		<constructor-arg type="TestExample.Sex">Male</constructor-arg>
	</object>

    <object id="mother" class="TestExample.Person">
		<constructor-arg>"李四"</constructor-arg>
		<constructor-arg type="TestExample.Sex">Female</constructor-arg>
	</object>
    
	<object id="intParents" class="TestExample.Person">
		<constructor-arg ref="father"></constructor-arg>
		<constructor-arg ref="mother"></constructor-arg>
	</object>
    
	<object id="usedNames" class="TestExample.Person">
		<constructor-arg>"张三"</constructor-arg>
		<constructor-arg>
			<array type="string">
				<value>张飞</value>
				<value>张良</value>
			</array>
		</constructor-arg>
	</object>

	<object id="initHobbies" class="TestExample.Person">
		<constructor-arg>"张三"</constructor-arg>
		<constructor-arg>
			<list type="string">
				<value>"打篮球"</value>
				<value>看电影</value>
				<value>游泳</value>
			</list>
		</constructor-arg>
	</object>
</objects>
```

### 二、加载配置文件

```c#
// 加载配置文件
Wukong.LoadConfig("c:\\ioc.xml");
```

### 三、获取对象实例

###### 1. 通过全类名获取对象实例

```c#
// 通过无参构造函数生成实例
dynamic person = Wukong.GetObject("TestExample.Person");
// 通过特定参数的构造函数生成实例
dynamic person2 = Wukong.GetObject("TestExample.Person", "张三");
```

###### 2. 通过配置文件的Id获取对象实例

```c#
// 获取配置文件中Id为person的对象实例
dynamic person = Wukong.GetObjectById("defaultPerson");
// 获取配置文件中Id为person2的对象实例
dynamic person2 = Wukong.GetObjectById("initNameAndAgeAndIsChinese");
```

### 四、执行对象方法

```c#
Person pernson = new Person("张三");
object name = Wukong.Invoke(pernson, "GetName");
string name2 = Wukong.Invoke<string>(pernson, "GetName");
```

<br/>

