using System;

namespace PhotoSi.Interfaces.Configuration
{
    public class GeneralConfig
    {
        public CORSConfig CORS { get; set; }
    }

    public class CORSConfig
    {
        public bool Enable { get; set; }
    }

}
