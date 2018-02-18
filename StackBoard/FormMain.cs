using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StackBoard
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
            Button b = new Button();
            b.Click += (s, e) => this.Text = $"{cm.GetNext()}";
            this.Controls.Add(b);
        }

        ClipboardManager cm;
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            cm = new ClipboardManager();
            cm.StartListen();
        }
    }
}
