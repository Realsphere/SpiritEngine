using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using SharpDX;
using SharpDX.D3DCompiler;
using SharpDX.IO;

namespace Realsphere.Spirit.RenderingCommon
{
    internal class HLSLCodeIncludeHandler : CallbackBase, Include
    {
        internal readonly Stack<string> CurrentDirectory;
        internal readonly List<string> IncludeDirectories;
        string hlslCode;

        internal HLSLCodeIncludeHandler(string hlslCode)
        {
            IncludeDirectories = new List<string>();
            CurrentDirectory = new Stack<string>();
            this.hlslCode = hlslCode;
        }

        #region Include Members
        Stream Include.Open(IncludeType type, string fileName, Stream parentStream)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(hlslCode));
        }

        void Include.Close(Stream stream)
        {
            stream.Close();
            stream.Dispose();
        }
        #endregion
    }
}
