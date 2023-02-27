﻿using JetBrains.Annotations;

namespace libsndfile.NET
{
    [PublicAPI]
    internal enum SfMode
    {
        Read = 0x10,
        Write = 0x20,
        ReadWrite = 0x30
    }
}