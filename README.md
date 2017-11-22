# Mainxx.CsOutput
这是一个执行C#源码工具，无需构建你的页面。

# Nuget Install
- pm> Install-Package Mainxx.CsOutput
- [Nuget链接](https://www.nuget.org/packages/Mainxx.CsOutput)

```
public class RunMe
{
    [STAThread]
    public static void Main()
    {
        Mainxx.CsOutput.CsOutput output = new Mainxx.CsOutput.CsOutput(typeof(RunMe),new string[] { "Demo","Category"});
        output.Run();
    }
}

[Mainxx.CsOutput.Example("Demo", "this demo1", Prefix = "Demo")]
public class Demo1
{
    public void Run()
    {
        string demo = "Mainxx.CsOutput";
        string output = "Demo1";
        Console.WriteLine($"{demo}---{output}");
    }
}
```

![one](https://raw.githubusercontent.com/mainxx/CsOutput/master/img/2.png)

![two](https://raw.githubusercontent.com/mainxx/CsOutput/master/img/1.png)
