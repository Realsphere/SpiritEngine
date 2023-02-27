using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Realsphere.Spirit.Exceptions
{
    public class ObjectCorruptedException : CorruptedException
    {
        public ObjectCorruptedException(string msg) : base(msg) { }
    }
    public class CorruptedException : Exception
    {
        public CorruptedException(string msg) : base(msg) { }
    }

    public class SpiritDXShaderException : Exception
    {
        public SpiritDXShaderException(string msg) : base(msg) { }
    }
    public class FlareShaderCorruptedException : Exception
    {
        public FlareShaderCorruptedException(string msg) : base(msg) { }
    }
}
