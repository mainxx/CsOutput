using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Mainxx.CsOutput
{
    public class CsOutput : UserControl, IComparer<TreeNode>
    {
        private static string[] RESERVED_WORDS = new string[77]
        {
          "abstract",
          "as",
          "base",
          "bool",
          "break",
          "byte",
          "case",
          "catch",
          "char",
          "checked",
          "class",
          "const",
          "continue",
          "decimal",
          "default",
          "delegate",
          "do",
          "double",
          "else",
          "enum",
          "event",
          "explicit",
          "extern",
          "false",
          "finally",
          "fixed",
          "float",
          "for",
          "foreach",
          "goto",
          "if",
          "implicit",
          "in",
          "int",
          "interface",
          "internal",
          "is",
          "lock",
          "long",
          "namespace",
          "new",
          "null",
          "object",
          "operator",
          "out",
          "override",
          "params",
          "private",
          "protected",
          "public",
          "readonly",
          "ref",
          "return",
          "sbyte",
          "sealed",
          "short",
          "sizeof",
          "stackalloc",
          "static",
          "string",
          "struct",
          "switch",
          "this",
          "throw",
          "true",
          "try",
          "typeof",
          "uint",
          "ulong",
          "unchecked",
          "unsafe",
          "ushort",
          "using",
          "virtual",
          "volatile",
          "void",
          "while"
        };
        private string RunMethodName { get; set; }
        private string[] Categories { get; set; } = new string[0];
        private bool TimingsShow { get; set; } = true;
        /// <summary>
        /// 创建一个CsOutput对象
        /// </summary>
        /// <param name="exampleType">对象类型</param>
        /// <param name="runMethodName">执行的函数名称</param>
        public CsOutput(Type exampleType, string runMethodName = "Run")
        {
            this.RunMethodName = runMethodName;
            this.InitializeComponent();
            this.ExampleType = exampleType;
            this.tutorialsTree.AfterSelect += new TreeViewEventHandler(this.tutorialsTree_AfterSelect);

        }
        /// <summary>
        /// 创建一个CsOutput对象
        /// </summary>
        /// <param name="exampleType">对象类型</param>
        /// <param name="categories">分类列表</param>
        /// <param name="runMethodName">执行的函数名称</param>
        public CsOutput(Type exampleType, string[] categories, string runMethodName = "Run")
        {
            this.Categories = categories;
            this.RunMethodName = runMethodName;
            this.InitializeComponent();
            this.ExampleType = exampleType;
            this.tutorialsTree.AfterSelect += new TreeViewEventHandler(this.tutorialsTree_AfterSelect);
        }


        protected Type exampleType;

        /// <summary>
        /// 选择的例子
        /// 
        /// </summary>
        public Type SelectedExample
        {
            get
            {
                TreeNode selectedNode = this.tutorialsTree.SelectedNode;
                if (selectedNode == null || !(selectedNode.Tag is Type))
                    return (Type)null;
                return (Type)selectedNode.Tag;
            }
        }

        /// <summary>
        /// 用于查找执行例子的类型，会搜索这里面的程序集
        /// The type to use to find examples - the assembly containing this type will be searched.
        /// </summary>
        public Type ExampleType
        {
            get
            {
                return this.exampleType;
            }
            set
            {
                this.exampleType = value;
                this.OnExampleTypeChanged();
            }
        }

        private Font normalFont = new Font("Courier New", 9f, FontStyle.Regular, GraphicsUnit.Point);
        private Font boldFont = new Font("Courier New", 9f, FontStyle.Bold, GraphicsUnit.Point);
        private Regex reg = new Regex("[\\w]+");
        private Dictionary<string, bool> reservedSet = new Dictionary<string, bool>();
        private const int WM_SCROLL = 276;
        private const int WM_VSCROLL = 277;
        private const int SB_LINEUP = 0;
        private const int SB_LINELEFT = 0;
        private const int SB_LINEDOWN = 1;
        private const int SB_LINERIGHT = 1;
        private const int SB_PAGEUP = 2;
        private const int SB_PAGELEFT = 2;
        private const int SB_PAGEDOWN = 3;
        private const int SB_PAGERIGTH = 3;
        private const int SB_PAGETOP = 6;
        private const int SB_LEFT = 6;
        private const int SB_PAGEBOTTOM = 7;
        private const int SB_RIGHT = 7;
        private const int SB_ENDSCROLL = 8;
        private SplitContainer splitContainer1;
        private TreeView tutorialsTree;
        private SplitContainer splitContainer2;
        private RichTextBox sourceTextBox;
        private Panel panel2;
        private Button button1;
        private BackgroundWorker worker;
        private CheckBox browserCheckBox;
        private RichTextBox outputTextBox;
        private RichTextBox exampleSummaryBox;

        private void InitializeComponent()
        {
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tutorialsTree = new System.Windows.Forms.TreeView();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.sourceTextBox = new System.Windows.Forms.RichTextBox();
            this.exampleSummaryBox = new System.Windows.Forms.RichTextBox();
            this.outputTextBox = new System.Windows.Forms.RichTextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.browserCheckBox = new System.Windows.Forms.CheckBox();
            this.worker = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tutorialsTree);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(975, 493);
            this.splitContainer1.SplitterDistance = 267;
            this.splitContainer1.TabIndex = 0;
            // 
            // tutorialsTree
            // 
            this.tutorialsTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tutorialsTree.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tutorialsTree.HideSelection = false;
            this.tutorialsTree.Location = new System.Drawing.Point(0, 0);
            this.tutorialsTree.Name = "tutorialsTree";
            this.tutorialsTree.Size = new System.Drawing.Size(267, 493);
            this.tutorialsTree.TabIndex = 0;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.sourceTextBox);
            this.splitContainer2.Panel1.Controls.Add(this.exampleSummaryBox);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.outputTextBox);
            this.splitContainer2.Panel2.Controls.Add(this.panel2);
            this.splitContainer2.Size = new System.Drawing.Size(704, 493);
            this.splitContainer2.SplitterDistance = 320;
            this.splitContainer2.TabIndex = 0;
            // 
            // sourceTextBox
            // 
            this.sourceTextBox.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.sourceTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sourceTextBox.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sourceTextBox.Location = new System.Drawing.Point(0, 55);
            this.sourceTextBox.Name = "sourceTextBox";
            this.sourceTextBox.ReadOnly = true;
            this.sourceTextBox.ShowSelectionMargin = true;
            this.sourceTextBox.Size = new System.Drawing.Size(704, 265);
            this.sourceTextBox.TabIndex = 3;
            this.sourceTextBox.Text = "";
            this.sourceTextBox.WordWrap = false;
            // 
            // exampleSummaryBox
            // 
            this.exampleSummaryBox.BackColor = System.Drawing.SystemColors.Control;
            this.exampleSummaryBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.exampleSummaryBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.exampleSummaryBox.Location = new System.Drawing.Point(0, 0);
            this.exampleSummaryBox.Margin = new System.Windows.Forms.Padding(8, 3, 8, 3);
            this.exampleSummaryBox.Name = "exampleSummaryBox";
            this.exampleSummaryBox.ReadOnly = true;
            this.exampleSummaryBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.exampleSummaryBox.ShowSelectionMargin = true;
            this.exampleSummaryBox.Size = new System.Drawing.Size(704, 55);
            this.exampleSummaryBox.TabIndex = 2;
            this.exampleSummaryBox.Text = "";
            // 
            // outputTextBox
            // 
            this.outputTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.outputTextBox.Font = new System.Drawing.Font("Courier New", 9F);
            this.outputTextBox.Location = new System.Drawing.Point(0, 32);
            this.outputTextBox.Name = "outputTextBox";
            this.outputTextBox.ReadOnly = true;
            this.outputTextBox.Size = new System.Drawing.Size(704, 137);
            this.outputTextBox.TabIndex = 4;
            this.outputTextBox.Text = "";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.button1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(704, 32);
            this.panel2.TabIndex = 3;
            // 
            // button1
            // 
            this.button1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(0, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(704, 32);
            this.button1.TabIndex = 2;
            this.button1.Text = "运行(Run this example)";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // browserCheckBox
            // 
            this.browserCheckBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.browserCheckBox.AutoSize = true;
            this.browserCheckBox.Location = new System.Drawing.Point(280, 7);
            this.browserCheckBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 1);
            this.browserCheckBox.Name = "browserCheckBox";
            this.browserCheckBox.Size = new System.Drawing.Size(113, 17);
            this.browserCheckBox.TabIndex = 0;
            this.browserCheckBox.Text = "Transform browser";
            this.browserCheckBox.UseVisualStyleBackColor = true;
            // 
            // worker
            // 
            this.worker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.worker_DoWork);
            this.worker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.worker_RunWorkerCompleted);
            // 
            // CsOutput
            // 
            this.Controls.Add(this.splitContainer1);
            this.DoubleBuffered = true;
            this.Name = "CsOutput";
            this.Size = new System.Drawing.Size(975, 493);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        /// <summary>
        /// 比较两个树节点
        /// </summary>
        /// <param name="x">First node</param><param name="y">Second node</param>
        /// <returns/>
        public int Compare(TreeNode x, TreeNode y)
        {
            return string.Compare(x.Text, y.Text, StringComparison.InvariantCulture);
        }

        /// <summary>
        /// 运行浏览器
        /// 
        /// </summary>
        public void Run()
        {
            ModelView.RunInForm((Control)this, "Mainxx.CsOutput example browser", false);
        }

        /// <summary>
        /// 当示例类型更改时调用
        /// 
        /// </summary>
        protected void OnExampleTypeChanged()
        {
            Type[] types = this.exampleType.Assembly.GetTypes();
            this.tutorialsTree.Nodes.Clear();
            this.tutorialsTree.ShowNodeToolTips = true;

            foreach (string text in Categories)
            {
                TreeNode treeNode = this.tutorialsTree.Nodes.Add(text);
                treeNode.Tag = (object)text;
                treeNode.Name = text;
            }
            foreach (Type exampleClass in types)
            {
                if (!(exampleClass.GetMethod(RunMethodName) == (MethodInfo)null))
                {
                    string text1 = "Examples";
                    ExampleAttribute exampleAttribute = this.GetExampleAttribute(exampleClass);
                    if (exampleAttribute != null)
                        text1 = exampleAttribute.Category;
                    TreeNode treeNode1 = (TreeNode)null;
                    foreach (TreeNode treeNode2 in this.tutorialsTree.Nodes)
                    {
                        if (text1.Equals(treeNode2.Tag))
                            treeNode1 = treeNode2;
                    }
                    if (treeNode1 == null)
                    {
                        treeNode1 = this.tutorialsTree.Nodes.Add(text1);
                        treeNode1.Tag = (object)text1;
                        treeNode1.Name = text1;
                    }
                    string text2 = exampleClass.Name;
                    if (exampleAttribute != null && exampleAttribute.Prefix != null)
                        text2 = exampleAttribute.Prefix + " " + text2;
                    TreeNode treeNode3 = treeNode1.Nodes.Add(text2);
                    if (exampleAttribute != null)
                        treeNode3.ToolTipText = exampleAttribute.Description;
                    treeNode3.Tag = (object)exampleClass;
                }
            }
            this.tutorialsTree.ExpandAll();
            foreach (TreeNode nd in this.tutorialsTree.Nodes)
                this.SortChildNodesByName(nd);
            if (this.tutorialsTree.Nodes.Count <= 0 || this.tutorialsTree.Nodes[0].Nodes.Count <= 0)
                return;
            this.tutorialsTree.SelectedNode = this.tutorialsTree.Nodes[0].Nodes[0];
        }

        private ExampleAttribute GetExampleAttribute(Type exampleClass)
        {
            object[] customAttributes = exampleClass.GetCustomAttributes(typeof(ExampleAttribute), true);
            if (customAttributes != null && customAttributes.Length > 0)
                return (ExampleAttribute)customAttributes[0];
            return (ExampleAttribute)null;
        }
        private void SortChildNodesByName(TreeNode nd)
        {
            List<TreeNode> list = new List<TreeNode>();
            foreach (TreeNode treeNode in nd.Nodes)
                list.Add(treeNode);
            list.Sort((IComparer<TreeNode>)this);
            nd.Nodes.Clear();
            nd.Nodes.AddRange(list.ToArray());
        }
        private void tutorialsTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            this.OnSelectionChanged();
        }

        protected void OnSelectionChanged()
        {
            Type selectedExample = this.SelectedExample;
            if (selectedExample == (Type)null)
                return;
            this.SuspendLayout();
            this.exampleSummaryBox.Clear();
            this.exampleSummaryBox.SelectionColor = Color.DarkBlue;
            this.exampleSummaryBox.SelectionFont = new Font(FontFamily.GenericSansSerif, 11f, FontStyle.Bold);
            this.exampleSummaryBox.AppendText(selectedExample.Name + Environment.NewLine);
            ExampleAttribute exampleAttribute = this.GetExampleAttribute(selectedExample);
            this.exampleSummaryBox.SelectionFont = new Font(FontFamily.GenericSansSerif, 10f, FontStyle.Regular);
            this.exampleSummaryBox.SelectionColor = Color.FromArgb(0, 0, 100);
            string text = "";
            if (exampleAttribute != null)
                text = exampleAttribute.Description;
            this.exampleSummaryBox.AppendText(text);
            this.exampleSummaryBox.Size = this.exampleSummaryBox.GetPreferredSize(this.exampleSummaryBox.Size);
            this.exampleSummaryBox.Height += 30;
            this.exampleSummaryBox.Refresh();
            string sourceCodeFilename = this.GetSourceCodeFilename(selectedExample);
            this.DoubleBuffered = true;
            try
            {
                if (sourceCodeFilename == null)
                {
                    this.sourceTextBox.Text = "";
                    this.sourceTextBox.AppendText("源码没有找到(Example source code was not found).  " + Environment.NewLine
                        + "在源文件-属性-复制到输出目录 修改为 始终复制 " + Environment.NewLine +
                        "(Go to the properties of the source file in Visual Studio and set 'Copy To Output Directory' to 'Copy if newer').");
                }
                else
                {
                    RichTextBox rtb = new RichTextBox();
                    rtb.Font = this.sourceTextBox.Font;
                    rtb.SelectionTabs = this.sourceTextBox.SelectionTabs;
                    EfficientTextBox targetTextBox = new EfficientTextBox(rtb);
                    StreamReader streamReader = new StreamReader(sourceCodeFilename);
                    while (true)
                    {
                        string s = streamReader.ReadLine();
                        if (s != null)
                            this.PrintWithSyntaxHighlighting(targetTextBox, s);
                        else
                            break;
                    }
                    targetTextBox.Flush();
                    streamReader.Close();
                    this.sourceTextBox.Rtf = rtb.Rtf;
                }
            }
            finally
            {
                this.ResumeLayout(true);
            }
        }
        private string GetSourceCodeFilename(Type exampleClass)
        {
            string fileName = Path.GetFileName(exampleClass.Name + ".cs");
            if (File.Exists(fileName))
                return fileName;
            DirectoryInfo parent1 = Directory.GetParent(Directory.GetCurrentDirectory());
            if (parent1 != null)
            {
                DirectoryInfo parent2 = parent1.Parent;
                if (parent2 != null)
                {
                    string path = Path.Combine(parent2.ToString(), fileName);
                    if (File.Exists(path))
                        return path;
                }
                string path1 = Path.Combine(parent1.ToString(), "Samples", "C#", "ExamplesBrowser", fileName);
                if (File.Exists(path1))
                    return path1;
            }
            return (string)null;
        }
        /// <summary>
        /// 高亮显示
        /// </summary>
        /// <param name="targetTextBox"/><param name="s"/>
        internal void PrintWithSyntaxHighlighting(EfficientTextBox targetTextBox, string s)
        {
            if (s.Trim().StartsWith("[Example("))
                return;
            if (s.Trim().StartsWith("//"))
            {
                targetTextBox.SelectionColor = Color.Green;
                targetTextBox.AppendText(s + Environment.NewLine);
                targetTextBox.SelectionColor = Color.Black;
            }
            else
            {
                targetTextBox.SelectionBackColor = Color.White;
                if (s.Contains("//highlight"))
                {
                    s = s.Replace("//highlight", "");
                    targetTextBox.SelectionBackColor = Color.Yellow;
                }
                MatchCollection matchCollection = this.reg.Matches(s);
                int startIndex = 0;
                foreach (Match match in matchCollection)
                {
                    targetTextBox.AppendText(s.Substring(startIndex, match.Index - startIndex));
                    startIndex = match.Index + match.Length;
                    string str = s.Substring(match.Index, match.Length);
                    if (this.IsReservedWord(str))
                        targetTextBox.SelectionColor = Color.Blue;

                    targetTextBox.AppendText(str);
                    targetTextBox.SelectionColor = Color.Black;
                }
                if (startIndex < s.Length)
                    targetTextBox.AppendText(s.Substring(startIndex));
                targetTextBox.AppendText(Environment.NewLine);
            }
        }


        private bool IsReservedWord(string word)
        {
            if (this.reservedSet.Count == 0)
            {
                foreach (string index in CsOutput.RESERVED_WORDS)
                    this.reservedSet[index] = true;
            }
            return this.reservedSet.ContainsKey(word);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.RunSelectedExample();
        }
        private void RunSelectedExample()
        {
            Type selectedExample = this.SelectedExample;
            if (selectedExample == (Type)null)
                return;
            this.button1.Enabled = false;
            this.button1.Text = selectedExample.Name + " running...";
            this.button1.Refresh();
            this.outputTextBox.Clear();
            this.outputTextBox.Refresh();
            this.worker.RunWorkerAsync((object)selectedExample);
        }
        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            Type type = (Type)e.Argument;
            TextWriter @out = Console.Out;
            Console.SetOut((TextWriter)new GUITextWriter(this));
            if (type != (Type)null)
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                try
                {
                    Console.WriteLine("====== 输出(Output from) " + type.Name + " ======");
                    Console.WriteLine();
                    object instance = Activator.CreateInstance(type);
                    MethodInfo method = type.GetMethod(RunMethodName);
                    if (method != (MethodInfo)null)
                        method.Invoke(instance, new object[0]);
                }
                catch (Exception ex)
                {
                    Exception exception = ex;
                    while (exception is TargetInvocationException)
                        exception = exception.InnerException;
                    Console.WriteLine("抛出的异常(Example failed with exception): " + (object)exception);
                }
                if (this.TimingsShow)
                    Console.WriteLine("运行耗时(Time to run example) " + (object)stopwatch.ElapsedMilliseconds + "ms.");
            }
            Console.SetOut(@out);
        }

        /// <summary>
        /// 输出文本
        /// </summary>
        /// <param name="s"/>
        public void AppendOutputText(string s)
        {
            this.outputTextBox.AppendText(s);
            CsOutput.SendMessage(this.Handle, 277, (IntPtr)7, IntPtr.Zero);
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.button1.Enabled = true;
            this.button1.Text = "运行(Run this example)";
        }

    }
}
