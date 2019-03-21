# .netcore 轻量级IOC容器
---

### 一、添加程序集搜索路径（默认只在应用执行目录中搜索）
#### 定义：
public static void AddSearchPath(string path)
#### 参数：
path: 搜索路径，绝对路径。
#### 返回：
无

### 二、移除程序集搜索路径
#### 定义：
public static void RemoveSearchPath(string path)
#### 参数：
path：搜索路径
#### 返回：
无

### 三、加载对象配置文件
#### 定义： 
public static void LoadConfig(string configFile, bool append = true)
#### 参数：
configFile: 配置文件，绝对路径。
<br>
append: 是否使用添加模式，默认为true
#### 返回：
无

### 四、根据类全名称获取对象实例
#### 定义：
public static object GetObject(string classFullName, params object[] args)
#### 参数：
classFullName: 类全名称。
<br>
args: 对象构造参数数组。
#### 返回：
对象实例。

### 五、根据类全名称获取指定类型的对象实例
#### 定义：
public static T GetObject<T>(string classFullName, params object[] args)
#### 参数：
classFullName: 类全名称。
<br>
args: 对象构造参数数组。
#### 返回：
指定类型的对象实例。

### 六、以单例模式根据类全名称获取对象实例
#### 定义：
public static object GetSingleObject(string classFullName, params object[] args)
#### 参数：
classFullName: 类全名称。
<br>
args: 对象构造参数数组。
#### 返回：
对象实例，多次调用返回同一实例。

### 七、以单例模式根据类全名称获取指定类型的对象实例
#### 定义：
public static T GetSingleObject<T>(string classFullName, params object[] args)
#### 参数：
classFullName: 类全名称。
<br>
args: 对象构造参数数组。
#### 返回：
指定类型的对象实例，多次调用返回同一实例。

### 八、根据配置文件Id获取对象实例
#### 定义：
public static object GetObjectById(string objectId)
#### 参数：
objectId: 配置文件中对象Id
#### 返回：
对象实例。

### 九、根据配置文件Id获取指定类型对象实例
#### 定义：
public static T GetObjectById<T>(string objectId)
#### 参数：
objectId: 配置文件中对象Id
#### 返回：
指定类型的对象实例。

### 十、以单例模式根据配置文件Id获取对象实例
#### 定义：
public static object GetSingleObjectById(string objectId)
#### 参数：
objectId: 配置文件中对象Id
#### 返回：
对象实例，多次调用返回同一实例。

### 十一、以单例模式根据配置文件Id获取指定类型的对象实例
#### 定义：
public static T GetSingleObjectById<T>(string objectId)
#### 参数：
objectId: 配置文件中对象Id
#### 返回：
指定类型的对象实例，多次调用返回同一实例。

## 示例
```
object person = IocUtils.GetObject("TestLibrary.Person");
Console.WriteLine(person);

object person2 = IocUtils.GetObject("TestLibrary.Person", "张三");
Console.WriteLine(person2);
```
