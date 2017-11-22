
using System;
using System.IO;
using System.Text;

namespace Mainxx.CsOutput
{

    internal class GUITextWriter : TextWriter
    {
        private CsOutput outputView;

        public override Encoding Encoding
        {
            get
            {
                return Encoding.UTF8;
            }
        }

        internal GUITextWriter(CsOutput view)
        {
            this.outputView = view;
        }

        public override void Write(char value)
        {
            if ((int)value == 13)
                return;
            this.outputView.Invoke((Delegate)new GUITextWriter.UpdateTextCallback(this.outputView.AppendOutputText), (object)value.ToString());
        }

        public delegate void UpdateTextCallback(string text);
    }
}
