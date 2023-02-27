using JetBrains.Annotations;

namespace libsndfile.NET
{
    [PublicAPI]
    internal enum SfLoopMode
    {
        None = 800,
        Forward,
        Backward,
        Alternating
    }
}