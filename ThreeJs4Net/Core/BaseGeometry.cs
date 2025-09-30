using System;
using ThreeJs4Net.Math;

namespace ThreeJs4Net.Core
{
    public abstract class BaseGeometry : IDisposable
    {
        public Guid Uuid = Guid.NewGuid();
        private bool _disposed = false;
        public int Id;
        public string Name;
        public string type;
        public Box3 BoundingBox = null;
        public Sphere BoundingSphere = null;
        public object UserData;

        public virtual void ComputeBoundingSphere() { }
        public virtual void ComputeBoundingBox() { }
        public virtual void ComputeVertexNormals(bool areaWeighted = false) { }
        public virtual void ApplyMatrix4(Matrix4 matrix) { }


        public event EventHandler<EventArgs> Disposed;

        protected virtual void RaiseDisposed()
        {
            var handler = this.Disposed;
            if (handler != null)
            {
                handler(this, new EventArgs());
            }
        }

        #region IDisposable Members
        /// <summary>
        /// Implement the IDisposable interface
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            // This object will be cleaned up by the Dispose method.
            // Therefore, you should call GC.SupressFinalize to
            // take this object off the finalization queue 
            // and prevent finalization code for this object
            // from executing A second time.
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!this._disposed)
            {
                try
                {
                    this._disposed = true;

                    this.RaiseDisposed();
                }
                finally
                {
                    //base.Dispose(true);           // call any base classes
                }
            }
        }
        #endregion
    }
}
