using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Timers; 
using System.Xml.Linq;
using Topshelf;
using XMLProject;


class ReadingXML
{

    static void Main()
    {
        var exitCode = HostFactory.Run(x =>
        {
            x.Service<ServiceAdapter>(s =>
            {
                s.ConstructUsing(serviceAdapter => new ServiceAdapter());
                s.WhenStarted(serviceAdapter => serviceAdapter.Start());
                s.WhenStopped(serviceAdapter => serviceAdapter.Stop());
            });
            x.RunAsLocalSystem();
            x.SetServiceName("XML_Projesi");
            x.SetDescription("Service for collecting data independent of casting.");
            x.SetDisplayName("Görünen_XML_Projesi");
        });
        int exitCodeValue = (int)Convert.ChangeType(exitCode, exitCode.GetTypeCode());
        Environment.ExitCode = exitCodeValue;
    }

    
}
