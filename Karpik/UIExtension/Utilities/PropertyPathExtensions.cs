using System.Runtime.CompilerServices;
using Unity.Properties;

namespace Karpik.UIExtension
{
    public static class PropertyPathExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PropertyPath Add(this PropertyPath path, string add)
        {
            return PropertyPath.Combine(path, add);
        }
    }
}