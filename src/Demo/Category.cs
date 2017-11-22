using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Demo
{
    [Mainxx.CsOutput.Example("Category", "this Category")]
    public class Category
    {
        public void Run()
        {
            string demo = "Mainxx.CsOutput";
            string output = "Category";
            Console.WriteLine($"{demo}---{output}");
        }
    }
}
