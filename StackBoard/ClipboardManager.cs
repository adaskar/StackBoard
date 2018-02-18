using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StackBoard
{
    partial class ClipboardManager : IDisposable
    {
        ClipboardListener _listener;
        Stack<object> _history;

        public ClipboardManager()
        {
            _listener = new ClipboardListener();
            _listener.ClipboardChanged += _listener_ClipboardChanged;

            _history = new Stack<object>();
        }

        private void _listener_ClipboardChanged(object sender, EventArgs e)
        {
            if (Clipboard.ContainsText())
            {
                _history.Push(Clipboard.GetText());
            }
            if (Clipboard.ContainsFileDropList())
            {
                // todo Clipboard.GetFileDropList();
            }
            if (Clipboard.ContainsImage())
            {
                // todo Clipboard.GetImage();
            }
            if (Clipboard.ContainsAudio())
            {
                // todo Clipboard.GetAudioStream();
            }
        }

        public object GetNext()
        {
            if (_history.Count == 0)
                return null;
            if (_history.Count == 1)
                return _history.Peek();
            return _history.Pop();
        }

        public void StartListen()
        {
            _listener.StartListen();
        }

        public void StopListen()
        {
            _listener.StopListen();
        }

        ~ClipboardManager()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _listener?.Dispose();
                _listener = null;
            }
        }
    }
}
