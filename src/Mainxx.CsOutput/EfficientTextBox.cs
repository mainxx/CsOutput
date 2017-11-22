
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Mainxx.CsOutput
{
    internal class EfficientTextBox
    {
        private StringBuilder buffer = new StringBuilder();
        private RichTextBox rtb;
        private Color foreColor;
        private Color backColor;
        private Font font;
        private Color foreColorOfBuffer;
        private Color backColorOfBuffer;
        private Font fontOfBuffer;

        internal Color SelectionColor
        {
            get
            {
                return this.foreColor;
            }
            set
            {
                this.foreColor = value;
            }
        }

        internal Color SelectionBackColor
        {
            get
            {
                return this.backColor;
            }
            set
            {
                this.backColor = value;
            }
        }

        internal Font SelectionFont
        {
            get
            {
                return this.font;
            }
            set
            {
                this.font = value;
            }
        }

        internal EfficientTextBox(RichTextBox rtb)
        {
            this.rtb = rtb;
            this.foreColor = rtb.SelectionColor;
            this.backColor = rtb.SelectionBackColor;
            this.font = rtb.SelectionFont;
            this.foreColorOfBuffer = this.foreColor;
            this.backColorOfBuffer = this.backColor;
            this.fontOfBuffer = this.font;
        }

        internal void Flush()
        {
            this.rtb.AppendText(this.buffer.ToString());
            this.buffer.Length = 0;
            if (this.backColor != this.backColorOfBuffer)
            {
                this.rtb.SelectionBackColor = this.backColor;
                this.backColorOfBuffer = this.backColor;
            }
            if (this.foreColor != this.foreColorOfBuffer)
            {
                this.rtb.SelectionColor = this.foreColor;
                this.foreColorOfBuffer = this.foreColor;
            }
            if (this.font == this.fontOfBuffer)
                return;
            this.rtb.SelectionFont = this.font;
            this.fontOfBuffer = this.font;
        }

        internal void AppendText(string s)
        {
            if (this.backColor != this.backColorOfBuffer || this.foreColor != this.foreColorOfBuffer || this.font != this.fontOfBuffer)
                this.Flush();
            this.buffer.Append(s);
        }
    }
}
