using System;
using System.Collections.Generic;
using System.Text;

namespace Update
{
    interface IExtension
    {
        string Name { get; }

        IUIExtension UIExtension { get; }
    }
}
