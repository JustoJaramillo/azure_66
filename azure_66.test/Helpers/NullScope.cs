using System;
using System.Collections.Generic;
using System.Text;

namespace azure_66.test.Helpers
{
    public class NullScope : IDisposable
    {
        public void Dispose(){}
        public static NullScope Instance { get; } = new NullScope();
        private NullScope() { }
    }
}
