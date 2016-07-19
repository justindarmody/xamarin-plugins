using System;
using Xamarin.Forms;

namespace CardView.Forms.Plugin.Abstractions
{
    internal abstract class DeviceInfo : BindableObject, IDisposable
    {
        private bool disposed;

        public abstract Size PixelScreenSize { get; }

        public abstract Size ScaledScreenSize { get; }

        public abstract double ScalingFactor { get; }

        public void Dispose()
        {
            this.Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed)
                return;
            this.disposed = true;
        }
    }
}

