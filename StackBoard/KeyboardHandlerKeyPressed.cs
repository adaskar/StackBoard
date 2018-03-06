using System.Windows.Forms;

namespace StackBoard
{
    class KeyboardHandlerKeyPressed
    {
        public Keys Keys { get; private set; }
        public uint Time { get; private set; }

        public KeyboardHandlerKeyPressed(Keys keys, uint time)
        {
            this.Keys = keys;
            this.Time = time;
        }
    }
}
