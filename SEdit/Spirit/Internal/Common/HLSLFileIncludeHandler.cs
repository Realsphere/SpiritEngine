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
    internal class HLSLFileIncludeHandler: CallbackBase, Include
    {
        internal readonly Stack<string> CurrentDirectory;
        internal readonly List<string> IncludeDirectories;


        internal HLSLFileIncludeHandler(string initialDirectory)
        {
            IncludeDirectories = new List<string>();
            CurrentDirectory = new Stack<string>();
            CurrentDirectory.Push(initialDirectory);
        }

        #region Include Members

        internal Stream Open(IncludeType type, string fileName, Stream parentStream)
        {
            var currentDirectory = CurrentDirectory.Peek();
            if (currentDirectory == null)
#if NETFX_CORE
                currentDirectory = Windows.ApplicationModel.Package.Current.InstalledLocation.Path;
#else
                currentDirectory = Environment.CurrentDirectory;
#endif

            var filePath = fileName;

            if (!Path.IsPathRooted(filePath))
            {
                var directoryToSearch = new List<string> {currentDirectory};
                directoryToSearch.AddRange(IncludeDirectories);
                foreach (var dirPath in directoryToSearch)
                {
                    var selectedFile = Path.Combine(dirPath, fileName);
                    if (NativeFile.Exists(selectedFile))
                    {
                        filePath = selectedFile;
                        break;
                    }
                }
            }

            if (filePath == null || !NativeFile.Exists(filePath))
            {
                throw new FileNotFoundException(String.Format("Unable to find file [{0}]", filePath ?? fileName));
            }

            NativeFileStream fs = new NativeFileStream(filePath, NativeFileMode.Open, NativeFileAccess.Read);
            CurrentDirectory.Push(Path.GetDirectoryName(filePath));
            return fs;
        }

        internal void Close(Stream stream)
        {
            stream.Dispose();
            CurrentDirectory.Pop();
        }

        Stream Include.Open(IncludeType type, string fileName, Stream parentStream)
        {
            return Open(type, fileName, parentStream);
        }

        void Include.Close(Stream stream)
        {
            Close(stream);
        }

        #endregion
    }
}
