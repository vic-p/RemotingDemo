using PMY.RemotingDemo.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMY.RemotingDemo.Service
{
    [Serializable]
    public class Test1 : System.ContextBoundObject, ITest1 //AOP记录日志，需要把System.MarshalByRefObject改为System.ContextBoundObject才能实现AOP功能
    {
        public string Test()
        {
            return "这是Test1实例";
        }
    }
}
