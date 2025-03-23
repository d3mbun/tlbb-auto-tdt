using System;
using System.Collections.Generic;
using System.Text;

namespace Update
{
    interface IMirrorSelector
    {
        void Init(Downloader downloader);

        ResourceLocation GetNextResourceLocation(); 
    }
}
