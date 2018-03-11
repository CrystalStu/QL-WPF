using System;

namespace Quick_Launcher
{
    [Serializable]
    public class Configuration
    {
        public string BackgroundSourceString;
        public Uri BackgroundSource { get { return new Uri(BackgroundSourceString); } }
    }
}
