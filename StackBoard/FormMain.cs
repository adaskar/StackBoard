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
        }

        ClipboardManager cm;
        KeyboardHandler kh;
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            kh = new KeyboardHandler();
            kh.KeyPressed += (sender, args) =>
            {
                if ((args.Keys & Keys.Control) == Keys.Control &&
                    (args.Keys & Keys.V) == Keys.V)
                {
                    cm.SetNext();
                }
            };
            kh.Init();



            cm = new ClipboardManager();
            cm.StartListen();
        }

    }
}
