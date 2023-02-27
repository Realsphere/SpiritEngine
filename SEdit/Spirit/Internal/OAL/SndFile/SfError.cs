﻿using JetBrains.Annotations;

namespace libsndfile.NET
{
    [PublicAPI]
    internal enum SfError
    {
        NoError = 0,
        UnrecognisedFormat = 1,
        System = 2,
        MalformedFile = 3,
        UnsupportedEncoding = 4
    }
}