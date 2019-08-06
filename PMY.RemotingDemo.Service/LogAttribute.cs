using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace PMY.RemotingDemo.Service
{
    public class LogAttribute : ContextAttribute, IContributeObjectSink
    {
        public LogAttribute()
            : base("Log")
        {

        }

        public IMessageSink GetObjectSink(MarshalByRefObject obj, IMessageSink nextSink)
        {
            return new MessageSink(nextSink);
        }

        public class MessageSink : IMessageSink
        {
            private IMessageSink nextSink = null;
            private static string IsWriteLog = ConfigurationManager.AppSettings["IsWriteInterfaceLog"];
            public MessageSink(IMessageSink messageSink)
            {
                nextSink = messageSink;
            }

            public IMessageCtrl AsyncProcessMessage(IMessage msg, IMessageSink replySink)
            {
                return null;
            }

            public IMessageSink NextSink
            {
                get { return nextSink; }
            }

            public IMessage SyncProcessMessage(IMessage msg)
            {

                IMessage returnMsg = nextSink.SyncProcessMessage(msg);

                if (IsWriteLog == null)
                    return returnMsg;

                if (IsWriteLog.ToLower() == "true")//默认是不记录日志，只有配置文件配置了true，则记录日志
                {
                    IMethodCallMessage call = msg as IMethodCallMessage;
                    //TrueLore.Bidding.Utility.ProjectInfo projectInfo = new Bidding.Utility.ProjectInfo();//V3版本数据库记录日志才用到，现在改用Log4Net记录日志，用不上了
                    String strTypeName = call.TypeName.Split(',')[1];//程序集名
                    String strClassName = call.TypeName.Split(',')[0];//类型名
                    String strMethodName = call.MethodName;//方法名
                    object[] args = call.Args;//方法参数
                    String[] argNames = new String[args.Length];//方法参数名
                    for (int i = 0; i < args.Length; i++)
                    {

                        argNames[i] = call.GetArgName(i);
                        if (args[i] == null)
                            continue;

                    }
                    IMethodReturnMessage returnMessage = returnMsg as System.Runtime.Remoting.Messaging.IMethodReturnMessage;
                    if (returnMessage.Exception != null)
                    {
                        this.WriteLog(strTypeName, strClassName, strMethodName, argNames, args, returnMessage.Exception);
                        //this.InsertInvokeLog(projectInfo,strBidProjId, strTypeName, strClassName, strMethodName, argNames, args, returnMessage.Exception.Message);
                    }
                    else
                    {
                        this.WriteLog(strTypeName, strClassName, strMethodName, argNames, args);
                        //this.InsertInvokeLog(projectInfo,strBidProjId, strTypeName, strClassName, strMethodName, argNames, args);
                    }
                }

                return returnMsg;
            }

            #region 将信息通过log4net记录到日志
            private void WriteLog(string typeName, string className, string methodName, string[] parametersName, object[] parametersValue, Exception exception = null)
            {
                string parameters = string.Empty;
                for (int i = 0; i < parametersValue.Length; i++)
                {
                    if (parametersValue[i] == null)
                    {
                        parameters += "***" + parametersName[i] + ":{}";
                        continue;
                    }
                    Type t = parametersValue[i].GetType();
                    parameters += "***" + parametersName[i] + ":{" + parametersValue[i] + "}";

                }

                String strInfo = String.Format("Type:{0} \r\n          ClassName:{1} \r\n          MethodName:{2} \r\n          Parameters:{3} \r\n          Result:{4} \r\n          IsError:{5}"
                    , typeName, className, methodName, parameters, exception != null ? exception.Message : "", exception != null ? "是" : "否");
                if (exception != null)
                {
                    PMY.Remoting.Common.LogHelper.WriteLog(strInfo, exception);
                }
                else
                {
                    PMY.Remoting.Common.LogHelper.WriteLog(strInfo);
                }
            }
            #endregion           
        }

    }

}
