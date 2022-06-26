using System;
using System.Diagnostics;

namespace GE.Warehouse.Core.Helper
{
    public abstract class DisposableResource : IDisposable
    {
        private bool _isDisposed;

        ~DisposableResource()
        {
            Dispose(false);
        }

        [DebuggerStepThrough]
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        

        [DebuggerStepThrough]
        protected virtual void DisposeCore()
        {
        }

        [DebuggerStepThrough]
        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed && disposing)
            {
                DisposeCore();
            }

            _isDisposed = true;
        }
    }
}