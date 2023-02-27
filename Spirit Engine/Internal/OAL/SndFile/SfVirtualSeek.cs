using System;
using System.Runtime.InteropServices;

namespace libsndfile.NET
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate long SfVirtualSeek(long offset, SfSeek seek, IntPtr userData);
}