using System.Text;

namespace application.Helpers
{
    /// <summary>
    /// Крч алярма.
    /// Реализация хэш функции MurMurHash, с использованием unsafe, для более быстрых XOR операций.
    /// В целом этот код с использованием unsafe быстрее чем аналог без unsafe на 0.003 - 0.004 sec
    /// </summary>
    internal static class Hash
    {
        internal static string MurmurHash(params object[] objects)
        {
            var paramsBuilder = new StringBuilder(256);
            foreach (var obj in objects)
                paramsBuilder.Append(obj?.ToString() ?? "null").Append('_');

            if (paramsBuilder.Length > 0)
                paramsBuilder.Length--;

            byte[] inputBytes = Encoding.UTF8.GetBytes(paramsBuilder.ToString());
            return HashData(inputBytes).ToString();
        }

        private static unsafe uint HashData(byte[] data)
        {
            const uint seed = 0;
            const uint c1 = 0xcc9e2d51;
            const uint c2 = 0x1b873593;
            const int r1 = 15;
            const int r2 = 13;
            const uint m = 5;
            const uint n = 0xe6546b64;

            uint hash = seed;
            int length = data.Length / 4;

            fixed (byte* pData = data)
            {
                uint* pDataAsUint = (uint*)pData;

                for (int i = 0; i < length; i++)
                {
                    uint k = pDataAsUint[i];

                    k *= c1;
                    k = RotateLeft(k, r1);
                    k *= c2;

                    hash ^= k;
                    hash = RotateLeft(hash, r2);
                    hash = hash * m + n;
                }
            }

            hash ^= (uint)data.Length;
            hash ^= hash >> 16;
            hash *= 0x85ebca6b;
            hash ^= hash >> 13;
            hash *= 0xc2b2ae35;
            hash ^= hash >> 16;

            return hash;
        }

        private static uint RotateLeft(uint x, int r) => (x << r) | (x >> (32 - r));
    }
}
