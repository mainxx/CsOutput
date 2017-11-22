using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Demo
{
    [Mainxx.CsOutput.Example("Demo", "this demo2", Prefix = "Demo")]
    public class Demo2
    {
        public void Run()
        {
            string demo = "Mainxx.CsOutput";
            string output = "Demo2";
            Console.WriteLine($"{demo}---{output}");
        }
    }
}
