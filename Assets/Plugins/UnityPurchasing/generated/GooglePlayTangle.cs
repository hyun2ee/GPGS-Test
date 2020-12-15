#if UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_OSX || UNITY_TVOS
// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("oKMtFoa8rqPGi1t5ajZqOe4W/3Sfck9DHlST0t3B6zync0wamdLt7MFzmVcVQD+kliSax3yzASMeyHJdBPRWVWoEw/Pg6Yx+Guekh0U4eSi/kFoFSVIehLvbGuezuHIgpSNOmQUamL5aN51TZ2+Cw7ZE053AVavLMLO9soIws7iwMLOzsnRSjS7mBY/YILtmh7nzfUUJXgSi+cb2gy4FWwYgZq76CN78dQ1aAjGO2RLPCnfcL8e5flZm8lQmBRblB1sKApR8PVPQQ5vvFFjtmJfiZ3JrPPQ2gmKggIIws5CCv7S7mDT6NEW/s7Ozt7KxQibtqcZGGA6N4SRhYZYlfkdUDvJLBsksdjkH3sotCjxfTyg9xGcip0wMF6IZcOWdR7Cxs7Kz");
        private static int[] order = new int[] { 3,4,6,6,7,10,7,10,13,11,12,12,12,13,14 };
        private static int key = 178;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
#endif
