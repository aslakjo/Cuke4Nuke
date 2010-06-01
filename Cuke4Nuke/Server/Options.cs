using System;
using System.Collections.ObjectModel;
using System.IO;

using NDesk.Options;
using System.Collections.Generic;

namespace Cuke4Nuke.Server
{
    public class Options
    {
        public static readonly int DefaultPort = 3901;

        public int Port { get; set; }
        public bool ShowHelp { get; set; }
        public ICollection<string> AssemblyPaths { get; set; }
        public bool WaitForDebuger { get; set; }

        private readonly OptionSet options;

        public Options(params string[] args)
        {
            Port = DefaultPort;
            AssemblyPaths = new Collection<string>();

            options = new OptionSet
                          {
                              {
                                  "p|port=",
                                  "the {PORT} the server should listen on (default=" + DefaultPort + ").",
                                  (int v) => Port = v
                                  },
                              {
                                  "a|assembly=",
                                  "an assembly to search for step definition methods.",
                                  v => AssemblyPaths.Add(v)
                                  },
                              {
                                  "h|?|help",
                                  "show this message and exit.",
                                  v => ShowHelp = v != null
                                  },
                              {
                                  "d|debuger",
                                  "Start the server and wait for a debuger to attache",
                                  v => WaitForDebuger = v != null
                                  }
                          };
            options.Parse(args);
        }

        public void Write(TextWriter textWriter)
        {
            textWriter.WriteLine("Options:");
            options.WriteOptionDescriptions(textWriter);
        }
    }
}