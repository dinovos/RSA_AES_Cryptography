using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace OS2_RSA_AES_DigSig
{
    class AESKriptiranjeDekriptiranje
    {

        public static RijndaelManaged tajniKljuc = new RijndaelManaged() { BlockSize = 256, Mode = CipherMode.CBC, Padding = PaddingMode.PKCS7};

        public RijndaelManaged VratiTajniKljuc()
        {
            return tajniKljuc;
        }


        private const int Keysize = 256;
        private const int brojIteracija = 1000;
        private const string lozinka = "random";
     

        private static byte[] SlucajnoGeneriraj256Bitova()
        {
            var slucajnoGeneriranoPolje = new byte[32];
            using (var RNG = new RNGCryptoServiceProvider())
            {
                RNG.GetBytes(slucajnoGeneriranoPolje);
            }
            return slucajnoGeneriranoPolje;
        }

        public string KriptirajAES(string txtZaKriptiranje)
        {
            var salt = SlucajnoGeneriraj256Bitova();
            var vektor = SlucajnoGeneriraj256Bitova();
            var cistiTekst = Encoding.UTF8.GetBytes(txtZaKriptiranje);

            using(var password = new Rfc2898DeriveBytes(lozinka, salt, brojIteracija))
            {
                var bitoviKljuca = password.GetBytes(Keysize / 8);
                using (tajniKljuc)
                {
                    using (var enkriptor = tajniKljuc.CreateEncryptor(bitoviKljuca, vektor))
                    {
                        using(var memory = new MemoryStream())
                        {
                            using(var kriptiraj = new CryptoStream(memory, enkriptor, CryptoStreamMode.Write))
                            {
                                kriptiraj.Write(cistiTekst, 0, cistiTekst.Length);
                                kriptiraj.FlushFinalBlock();
                                var kriptiraniBitovi = salt;
                                kriptiraniBitovi = kriptiraniBitovi.Concat(vektor).ToArray();
                                kriptiraniBitovi = kriptiraniBitovi.Concat(memory.ToArray()).ToArray();
                                memory.Close();
                                kriptiraj.Close();

                                return Convert.ToBase64String(kriptiraniBitovi);
                            }
                        }
                    }
                }
            }
        }


        public string DekriptirajAES(string txtZaKriptiranje)
        {
            var kriptiraniTekstSaSvim = Convert.FromBase64String(txtZaKriptiranje);
            var saltBitovi = kriptiraniTekstSaSvim.Take(Keysize / 8).ToArray();
            var vektorBitovi = kriptiraniTekstSaSvim.Skip(Keysize / 8).Take(Keysize / 8).ToArray();
            var kriptiraniTekstBitovi = kriptiraniTekstSaSvim.Skip((Keysize / 8) * 2).Take(kriptiraniTekstSaSvim.Length - ((Keysize / 8) * 2)).ToArray();

            using (var password = new Rfc2898DeriveBytes(lozinka, saltBitovi, brojIteracija))
            {
                var keyBytes = password.GetBytes(Keysize / 8);
                using (tajniKljuc)
                {
                    using (var dekriptor = tajniKljuc.CreateDecryptor(keyBytes, vektorBitovi))
                    {
                        using (var memory = new MemoryStream(kriptiraniTekstBitovi))
                        {
                            using (var dekriptiranje = new CryptoStream(memory, dekriptor, CryptoStreamMode.Read))
                            {
                                var plainTextBytes = new byte[kriptiraniTekstBitovi.Length];
                                var decryptedByteCount = dekriptiranje.Read(plainTextBytes, 0, plainTextBytes.Length);
                                memory.Close();
                                dekriptiranje.Close();
                                return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
                            }
                        }
                    }
                }
            }

        }


    }
}
