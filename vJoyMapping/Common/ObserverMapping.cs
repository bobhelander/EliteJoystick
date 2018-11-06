using System;
using System.Collections.Generic;
using System.Text;

namespace vJoyMapping.Common
{
    public class ObserverMapping <T> 
    {
        public IObserver<T> Observer { get;set; }
        public IDisposable Unsubscriber { get; set; }
    }
}
