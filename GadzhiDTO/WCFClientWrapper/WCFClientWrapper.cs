using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WcfClientProxyGenerator;

namespace GadzhiDTO.WCFClientWrapper
{
    public class WCFClientWrapper<TContract, TResult> where TContract: class
                                                      where TResult : class
    {
        public TResult ExecuteAsyncFunction(Func<TContract, TResult> asyncAction)
        {
            TContract proxy = WcfClientProxy.Create<TContract>(
                c => c.SetEndpoint("FileConvertingService"));

           return asyncAction.Invoke(proxy);
        }
    }
}
