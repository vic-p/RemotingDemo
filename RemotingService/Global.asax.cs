using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace RemotingService
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.Load("PMY.RemotingDemo.Service");
            Type[] arrType = assembly.GetTypes();
            foreach (Type type in arrType)
            {
                System.Runtime.Remoting.RemotingConfiguration.RegisterActivatedServiceType(type);
                System.Runtime.Remoting.RemotingConfiguration.RegisterWellKnownServiceType
                        (
                        type,
                        type.FullName + ".soap",
                        System.Runtime.Remoting.WellKnownObjectMode.Singleton
                        );
            }         
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}