
using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;

namespace Mainxx.CsOutput
{
    internal class ModelView
    {


        internal ModelView()
        {
        }


        public static Form ShowInForm(Control c, string title, bool maximise)
        {
            Form form = new Form();
            form.Text = title;
            //form.Icon = ModelView.Icon;
            form.Size = new System.Drawing.Size(1000, 800);
            c.Dock = DockStyle.Fill;
            form.Controls.Add(c);
            form.Show();
            form.BringToFront();
            if (maximise)
                form.WindowState = FormWindowState.Maximized;
            return form;
        }

        public static void RunInForm(Control c, string title, bool maximise)
        {
            Application.EnableVisualStyles();
            Application.Run(ModelView.ShowInForm(c, title, maximise));
        }


    }
}
