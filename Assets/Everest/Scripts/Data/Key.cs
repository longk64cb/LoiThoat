namespace Everest {
    internal class Key {
        //public static void GenerateKeyAndIV() {
        //    using (var rm = new System.Security.Cryptography.RijndaelManaged()) {
        //        rm.GenerateKey();
        //        rm.GenerateIV();
        //        Debug.Log(string.Join(", ", rm.Key));
        //        Debug.Log(string.Join(", ", rm.IV));
        //    }
        //}

        internal static readonly byte[] key = {
            251, 129, 38, 223, 49, 19, 61, 30, 44, 175, 201, 49, 161, 28, 78, 85, 29, 118, 63, 60, 85, 16, 143, 22, 52, 193, 241, 221, 170, 243, 215, 186
        };

        internal static readonly byte[] vector = {
            24, 234, 17, 13, 202, 61, 112, 0, 13, 224, 170, 171, 85, 198, 185, 35
        };
    }
}
