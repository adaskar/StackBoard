using System;
using System.Windows.Forms;

namespace StackBoard
{
    partial class ClipboardManager
    {
        class ClipboardListener : Form
        {
            IntPtr _this;
            IntPtr _next;

            public delegate void ClipboardChangedEventHandler(object sender, EventArgs e);
            public event ClipboardChangedEventHandler ClipboardChanged;

            public ClipboardListener()
            {
                this.Visible = false;
                this.ShowInTaskbar = false;
                this.FormBorderStyle = FormBorderStyle.None;
                this.HandleCreated += (s, e) => _this = this.Handle;
                this.Load += (s, e) => this.Size = new System.Drawing.Size(0, 0);
                this.Disposed += (s, e) => StopListen();
                CreateHandle();
                CreateControl();
            }

            public void StartListen()
            {
                if (_this == IntPtr.Zero)
                    throw new InvalidOperationException();

                if (_next != IntPtr.Zero)
                    return;

                _next = Native.SetClipboardViewer(_this);
                if (_next == IntPtr.Zero &&
                    Native.GetLastError() != 0)
                    throw new Win32Exception(Native.GetLastError());
            }

            public void StopListen()
            {
                if (_this == IntPtr.Zero ||
                    _next == IntPtr.Zero)
                    return;

                if (!Native.ChangeClipboardChain(this.Handle, _next) &&
                    Native.GetLastError() != 0)
                    throw new Win32Exception(Native.GetLastError());
                _next = IntPtr.Zero;
            }

            protected override void WndProc(ref Message m)
            {
                if (m.Msg == Native.WM_DRAWCLIPBOARD)
                {
                    this.ClipboardChanged?.Invoke(this, EventArgs.Empty);
                }
                base.WndProc(ref m);
            }
        }
    }
}
