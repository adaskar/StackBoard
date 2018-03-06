using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StackBoard
{
    class KeyboardHandler
    {
        IntPtr _next;
        Native.KBDLLHOOKSTRUCT _last;

        public delegate void KeyPressedEventHandler(object sender, KeyboardHandlerKeyPressed args);
        public event KeyPressedEventHandler KeyPressed;

        public KeyboardHandler()
        {

        }

        private IntPtr HookHandler(int code, IntPtr w, IntPtr l)
        {
            if (code >= 0 && w == (IntPtr)Native.WM.KEYDOWN)
            {
                Keys keys;
                Native.KBDLLHOOKSTRUCT kbd = (Native.KBDLLHOOKSTRUCT)Marshal.PtrToStructure(l, typeof(Native.KBDLLHOOKSTRUCT));

                keys = (Keys)kbd.vkCode;

                if (_last?.time == kbd.time &&
                     (_last?.vkCode == (uint)Keys.RControlKey ||
                     _last?.vkCode == (uint)Keys.LControlKey))
                {
                    keys |= Keys.Control;
                }

                this.KeyPressed?.Invoke(this, new KeyboardHandlerKeyPressed(keys, kbd.time));

                _last = kbd;
            }
            return Native.CallNextHookEx(_next, code, w, l);
        }

        public void Init()
        {
            _next = Native.SetWindowsHookEx(Native.HookType.WH_KEYBOARD_LL, HookHandler, IntPtr.Zero, 0);
            if (_next == IntPtr.Zero)
                throw new Win32Exception(Native.GetLastError());
        }
    }
}
