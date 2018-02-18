using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace StackBoard
{
    public static class Native
    {
        public const uint WM_DRAWCLIPBOARD = 0x0308;

        [DllImport("User32.dll", SetLastError = true)]
        public static extern IntPtr SetClipboardViewer(IntPtr hWndNewViewer);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool ChangeClipboardChain(IntPtr hWndRemove, IntPtr hWndNewNext);
        [DllImport("kernel32.dll")]
        public static extern uint GetLastError();
    }
    public class Win32Exception : Exception
    {
        public uint ErrorCode { get; private set; }
        public string ErrorMessage { get; private set; }

        public Win32Exception(uint errorCode, string errorMessage)
        {
            this.ErrorCode = errorCode;
            this.ErrorMessage = errorMessage;
        }

        public Win32Exception(uint errorCode) :
            this(errorCode, string.Empty)
        {
        }

        public override string ToString()
        {
            return $"Win32 error. {ErrorMessage}({ErrorCode})";
        }
    }
}
