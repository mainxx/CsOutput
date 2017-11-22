using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Demo
{
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
}
