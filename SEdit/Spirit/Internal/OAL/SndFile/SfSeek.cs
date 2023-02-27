using JetBrains.Annotations;

namespace libsndfile.NET
{
    [PublicAPI]
    internal enum SfSeek
    {
        Begin = 0,
        Current = 1,
        End = 2
    }
}