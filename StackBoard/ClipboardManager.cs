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
        private class ClipboardItem
        {
            public DateTime Time { get; set; } = DateTime.Now;
            public object Item { get; set; }

            public ClipboardItem(object item)
            {
                Item = item;
            }
        }

        ClipboardListener _listener;
        Stack<ClipboardItem> _history;
        ClipboardItem _lastItem;

        public ClipboardManager()
        {
            _listener = new ClipboardListener();
            _listener.ClipboardChanged += _listener_ClipboardChanged;

            _history = new Stack<ClipboardItem>();
        }

        private void _listener_ClipboardChanged(object sender, EventArgs e)
        {
            if (Clipboard.ContainsText())
            {
                ClipboardItem ci = new ClipboardItem(Clipboard.GetText());

                // if user pressed ctrl c in less then one second for same item, 
                // add the item in the history so user will be able to use it.
                if (_lastItem != null &&
                    $"{ci.Item}" == $"{_lastItem.Item}" &&
                    $"{ci.Item}" != $"{(_history.Count > 0 ? _history.Peek()?.Item : null)}" &&
                    ci.Time.Subtract(_lastItem.Time).TotalMilliseconds < 1000)
                    _history.Push(ci);

                _lastItem = ci;
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
            return _history.Pop().Item;
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
