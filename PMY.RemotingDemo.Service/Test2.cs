using PMY.RemotingDemo.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMY.RemotingDemo.Service
{
    [Serializable]
    public class Test2 : System.ContextBoundObject, ITest2
    {
        public string Test()
        {
            return "这是Test2实例";
        }
    }
}
