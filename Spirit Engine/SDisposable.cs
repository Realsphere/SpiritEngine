using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Realsphere.Spirit
{
    /// <summary>
    /// Any class extending this, will be automatically Disposed when the Game throws an exception, or closes. 
    /// </summary>
    public abstract class SDisposable : IDisposable
    {
        public SDisposable()
        {
            Game.ToDispose.Add(this);
        }

        public void Dispose()
        {
            SDispose();
        }

        public abstract void SDispose();
    }
}
