using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Demo
{
    [Mainxx.CsOutput.Example("Test", "this Test1")]
    public class Test1
    {
        public void Run()
        {
            string demo = "Mainxx.CsOutput";
            string output = "Test1";
            Console.WriteLine($"{demo}---{output}");
        }
    }
}
