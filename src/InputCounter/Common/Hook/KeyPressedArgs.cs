using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace InputCounter.Common.Hook;

public class KeyPressedArgs : EventArgs
{
    public Key Key { get; }

    public KeyPressedArgs(Key key)
    {
        Key = key;
    }
}