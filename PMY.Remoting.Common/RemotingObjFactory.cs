using PMY.RemotingDemo.IService;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Http;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading.Tasks;

namespace PMY.Remoting.Common
{
    public class RemotingObjFactory
    {
        private static readonly string server = ConfigurationManager.AppSettings["ServerAddress"];
        private static readonly string port = ConfigurationManager.AppSettings["Port"];
        private static readonly string site = ConfigurationManager.AppSettings["VirtualPath"];
        static RemotingObjFactory()
        {
            if (ChannelServices.RegisteredChannels == null || ChannelServices.RegisteredChannels.Length == 0)
            {
                BinaryClientFormatterSinkProvider binaryClient = new BinaryClientFormatterSinkProvider();
                BinaryServerFormatterSinkProvider binaryServer = new BinaryServerFormatterSinkProvider
                {
                    TypeFilterLevel = TypeFilterLevel.Full
                };
                IDictionary diction = new Hashtable();
                diction["name"] = "http";
                diction["port"] = 0;
                HttpChannel http = new HttpChannel(diction, binaryClient, binaryServer);
                ChannelServices.RegisterChannel(http, false);
            }
        }
        public static ITest1 Test1 = (ITest1)Activator.GetObject(typeof(ITest1), string.Format(@"http://{0}:{1}/{2}/PMY.RemotingDemo.Service.Test1.soap", server, port, site));
        public static ITest2 Test2 = (ITest2)Activator.GetObject(typeof(ITest2), string.Format(@"http://{0}:{1}/{2}/PMY.RemotingDemo.Service.Test2.soap", server, port, site));
    }
}
