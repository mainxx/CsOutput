
using System;

namespace Mainxx.CsOutput
{
    /// <summary>
    /// Attribute used to label classes containing code examples. The examples browser
    ///             will use this information to organise the code examples
    /// 
    /// </summary>
    public class ExampleAttribute : Attribute
    {
        private string category;
        private string description;
        private string prefix;


        public string Category
        {
            get
            {
                return this.category;
            }
        }


        public string Description
        {
            get
            {
                return this.description;
            }
        }


        public string Prefix
        {
            get
            {
                return this.prefix;
            }
            set
            {
                this.prefix = value;
            }
        }


        public ExampleAttribute(string category, string description)
        {
            this.category = category;
            this.description = description;
        }
    }
}
