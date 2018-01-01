using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quick_Launcher
{
    [Serializable]
    public class Configuration
    {
        public string BackgroundSourceString;
        public Uri BackgroundSource { get { return new Uri(BackgroundSourceString); } }
    }
}
