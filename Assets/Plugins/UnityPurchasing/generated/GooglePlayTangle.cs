#if UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_OSX || UNITY_TVOS
// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("iqoVoVfK673M4KueCijXDvgD9LPNTkBPf81ORU3NTk5P2bMJs7DpuyQD0jxpn1teA9DWL80Q2A+wdP3ZttrAUzgWdYdewQ6++dOFMdE6pSSw3h58aZo27w1zfamP/+RSF46Wqer0EPoICKH4hxBCLHGUdLtcDAAGmxKDlicAiyu7UJJOhUdUxfrLmOd42FmcGj0WsIXhHhNNLtzmwZ8+YIhEAW06x0iWstUP6vyIBFeDzrTYIqZbMktmdAJU52jCNW69E92z83YsGHPdXQsbMsvEPy6orgDZ1q9cVn/NTm1/QklGZckHybhCTk5OSk9MTM4Z+IIzqrB7OLgsJzn1srMNbvTLQIo9XYiW3IGQ0S3REkgwuPaz29zuNdHPRWn2XE1MTk9O");
        private static int[] order = new int[] { 12,1,3,6,13,5,8,9,8,13,10,12,13,13,14 };
        private static int key = 79;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
#endif
