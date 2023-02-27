using System;
using System.Runtime.InteropServices;

namespace libsndfile.NET
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate long SfVirtualWrite(IntPtr ptr, long count, IntPtr userData);
}