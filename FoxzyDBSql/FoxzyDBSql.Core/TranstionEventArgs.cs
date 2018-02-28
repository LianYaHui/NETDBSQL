using System;
using System.Collections.Generic;
using System.Text;

namespace FoxzyDBSql
{
    public class TranstionEventArgs : EventArgs
    {
        public dynamic TranstionData { set; get; }

        public TranstionEventArgs(dynamic data)
        {
            TranstionData = data;
        }
    }
}
