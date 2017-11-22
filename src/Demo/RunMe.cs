using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Demo
{
    public class RunMe
    {
        [STAThread]
        public static void Main()
        {
            Mainxx.CsOutput.CsOutput output = new Mainxx.CsOutput.CsOutput(typeof(RunMe),new string[] { "Demo","Category"});
            output.Run();
        }
    }
}
