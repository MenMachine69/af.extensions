using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace af.extensions
{
    /// <summary>
    /// Extension methods for type System.String
    /// </summary>
    public static class StringEx
    {
        /// <summary>
        /// Checks if string is null, empty or only whitspace characters.
        /// </summary>
        /// <param name="data">string</param>
        /// <returns>true if string IS empty</returns>
        public static bool IsEmpty(this string data) { return string.IsNullOrWhiteSpace(data); }

        /// <summary>
        /// Checks if string is null, empty or only whitspace characters.
        /// </summary>
        /// <param name="data">string</param>
        /// <returns>true if string is NOT empty</returns>
        public static bool IsNotEmpty(this string data) { return !string.IsNullOrWhiteSpace(data); }


        /// <summary>
        /// Checks that string does not contain any unallowed character
        /// </summary>
        /// <param name="data">string</param>
        /// <param name="notallowed">string with characters which are not allowed</param>
        /// <returns>true if string contains any of the not allowed cahracters</returns>
        public static bool ContainsNotAllowed(this string data, string notallowed) 
        {
            bool ret = false;

            foreach (var chr in notallowed.ToCharArray())
            {
                if (data.Contains(chr))
                {
                    ret = true;
                    break;
                }
            }

            return ret;
        }

        /// <summary>
        /// Checks that string does contain only allowed characters
        /// </summary>
        /// <param name="data">string</param>
        /// <param name="allowed">string with characters that are allowed</param>
        /// <returns>true if string contains any of the not allowed cahracters</returns>
        public static bool ContainsOnlyAllowed(this string data, string allowed)
        {
            bool ret = true;

            foreach (var chr in data.ToCharArray())
            {
                if (!allowed.Contains(chr))
                {
                    ret = false;
                    break;
                }
            }

            return ret;
        }


        /// <summary>
        /// Berechnet den Hash-Wert eines Strings 
        /// Über den Hashwert können Kennwörter indirekt in einer Datenbank
        /// und ähnlichen Orten gespeichert werden. Diese geben dann keinen 
        /// Rückschluss auf das tatsächlich zu verwendende Kennwort
        /// </summary>
        /// <param name="text">der Text zu dem ein Hash gebildet werden soll</param>
        /// <param name="pack">Ergebnis komprimieren (entfernt die Bindestriche aus dem Hash um Platz zu sparen)</param>
        /// <returns>der Hash für den übergebenen Text oder string.empty wenn kein Text übergeben wurde oder ein Fehler auftrat</returns>
        public static string GetHash(this string data, bool pack)
        {
            string ret = String.Empty;

            if (data.IsEmpty())
                return ret;

            using (MD5 md5 = new MD5CryptoServiceProvider())
            {
                //MD5 Hash aus dem String berechnen. Dazu muss der string in ein Byte[]
                //zerlegt werden. Danach muss das Resultat wieder zurück in ein string.
                ret = BitConverter.ToString(md5.ComputeHash(Encoding.Default.GetBytes(data)));

                if (pack)
                    ret = ret.Replace("-", "");
            }

            return ret;
        }

        /// <summary>
        /// Konvertiert einen String in ein ByteArray
        /// <example>
        /// string test="Das ist ein Test";
        /// byte[] bytes=test.ToByteArray();
        /// </example>
        /// </summary>
        /// <param name="str">der zu konvertierende String</param>
        /// <returns>ByteArray</returns>
        public static byte[] ToByteArray(this string data)
        {
            ASCIIEncoding enc = new ASCIIEncoding();
            return enc.GetBytes(data);
        }

        /// <summary>
        /// Konvertiert ein ByteArray in einen String
        /// Erweitert die Klasse String
        /// <example>
        /// string test="";
        /// test=test.FromByteArray(new byte[] {});
        /// </example>
        /// </summary>
        /// <param name="text">Stringvariable, die erweitert wird</param>
        /// <param name="arr">das zu konvertierende ByteArray</param>
        /// <returns>string</returns>
        public static string FromByteArray(this string data, byte[] arr)
        {
            data = "";
            ASCIIEncoding enc = new ASCIIEncoding();
            return enc.GetString(arr);
        }

        /// <summary>
        /// Zählt, wie oft ein String in einem anderen vorkommt
        /// Erweitert die Klasse String
        /// <example>
        /// string test="Das ist ein Test";
        /// int leerzeichen=test.Count(" ");
        /// </example>
        /// </summary>
        /// <param name="data">String in dem gesucht wird</param>
        /// <param name="search">zu suchender String</param>
        /// <returns>Anzahl der vorkommen von sSearch in sSource</returns>
        public static int Count(this string data, string search)
        {
            return new Regex(Regex.Escape(search)).Matches(data).Count;
        }

        /// <summary>
        /// Liefert einen Substring über Start- und Endposition
        /// </summary>
        /// <param name="data">String, dessen SUbstring ermittelt werden soll</param>
        /// <param name="startpos">Position des ersten Zeichens</param>
        /// <param name="endpos">Position des letzten Zeichens</param>
        /// <returns>SubString</returns>
        public static string Substring(this string data, int startpos, int endpos)
        {
            return data.Substring(startpos, endpos - startpos + 1);
        }

        /// <summary>
        /// Liefert eine Anzahl von Zeichen aus dem String von rechts beginnend
        /// Erweitert die Klasse String
        /// <example>
        /// string test="Das ist ein Test";
        /// test=test.Right(4);  // Ergebnis: Test
        /// </example>
        /// </summary>
        /// <param name="data">String aus dem die Zeichen ermittelt werden sollen</param>
        /// <param name="chars">Anzahl der Zeichen</param>
        /// <returns>Ergebnis</returns>
        public static string Right(this string data, int chars)
        {
            if (data.Length <= chars)
                return data;

            return data.Substring(data.Length - chars, chars);
        }

        /// <summary>
        /// Liefert eine Anzahl von Zeichen aus dem String von links beginnend
        /// Erweitert die Klasse String
        /// <example>
        /// string test="Das ist ein Test";
        /// test=test.Left(3);  // Ergebnis: Das
        /// </example>
        /// </summary>
        /// <param name="sSource">String aus dem die Zeichen ermittelt werden sollen</param>
        /// <param name="iChars">Anzahl der Zeichen</param>
        /// <returns>Ergebnis</returns>
        public static string Left(this string data, int chars)
        {
            if (data.Length <= chars)
                return data;

            return data.Substring(0, chars);
        }

        /// <summary>
        /// Prüft, ob das erste Zeichen eine Ziffer ist.
        /// </summary>
        /// <param name="source">der zu prüfende Text</param>
        /// <returns>true wenn das erste Zeichen eine Ziffer ist, sonst false (Leerzeichen werden ignoriert)</returns>
        public static bool IsDigit(this string data)
        {
            return ("0123456789".Contains(data.Trim().Left(1)));
        }

        /// <summary>
        /// Liefert alle Ziffern am Anfang eines Strings.
        /// Kann z.B. benutzt werden um eine PLZ unbekannter Länge aus einem String zu extrahieren, wenn dieser mit der PLZ beginnt.
        /// <example>
        /// string test="01309 Dresden";
        /// string plz=test.GetDigits();
        /// </example>
        /// </summary>
        /// <param name="source">der zu prüfende Text</param>
        /// <returns>Alle Ziffern mit denen ein String beginnt (Leerzeichen werden ignoriert)</returns>
        public static string GetDigits(this string data)
        {
            StringBuilder sb = new StringBuilder();
            char[] digits = "0123456789".ToCharArray();

            foreach (char chr in data.TrimStart())
            {
                if (digits.Contains(chr))
                    sb.Append(chr);
                else
                    break;
            }

            return sb.ToString();
        }

        public static string Join(this string data, string[] tojoin, string separator)
        {
            if (separator.IsEmpty())
                return string.Concat(tojoin);

            return string.Join(separator, tojoin);
        }

        /// <summary>
        /// extrahier aus dem String einen Teil, der dem Pattern entspricht (z.B. eine EMailadresse aus einem Text)
        /// 
        /// ? = genau ein beliebiges Zeichen 
        /// % = kein oder beleibig viele Zeichen (ein beliebiger String)
        /// * = kein oder beliebig viele Zeichen, aber kein Leerzeichen (=ein Wort)
        /// # = genau eine der Ziffern 0-9
        /// 
        /// eine Guid : ????????-????-????-????-????????????
        /// eine EMailadresse : *?@?*.?*
        /// ein einfaches Datum : ##.##.####
        /// </summary>
        public static string Search(this string s, string simplepattern)
        {
            var match = simplepattern.GetRegexFromPattern().Match(s);
            if (match.Success)
                return s.Substring(match.Index, match.Length);

            return null;
        }

        /// <summary>
        /// Vergleich mit einem vereinfachten Pattern
        /// 
        /// ? = genau ein beliebiges Zeichen 
        /// % = kein oder beleibig viele Zeichen (ein beliebiger String)
        /// * = kein oder beliebig viele Zeichen, aber kein Leerzeichen (=ein Wort)
        /// # = genau eine der Ziffern 0-9
        /// 
        /// eine Guid : ????????-????-????-????-????????????
        /// eine EMailadresse : %?@?%.?%
        /// ein einfaches Datum : ##.##.####
        /// </summary>
        public static bool Like(this string s, string simplepattern)
        {
            return simplepattern.GetRegexFromPattern().IsMatch(s);
        }

        /// <summary>
        /// Wandelt ein einfaches Pattern in einen RegEx-Ausdruck um, der dann für Vergleiche etc. verwendet werden kann
        /// 
        /// Das vereinfachte Pattern kann folgende Zeichen enthalten:
        /// 
        /// ? = genau ein beliebiges Zeichen 
        /// % = kein oder beleibig viele Zeichen (ein beliebiger String)
        /// * = kein oder beliebig viele Zeichen, aber kein Leerzeichen (= ein Wort)
        /// # = genau eine der Ziffern 0-9
        /// 
        /// eine Guid : ????????-????-????-????-????????????
        /// eine EMailadresse : %?@?%.?%
        /// ein einfaches Datum : ##.##.####
        /// </summary>
        /// <param name="pattern">vereinfachtes Pattern für die Suche</param>
        /// <returns>RegEx für das vereinfachte Pattern</returns>
        public static Regex GetRegexFromPattern(this string pattern)
        {
            pattern = "^" + pattern;
            return new Regex(pattern
                    .Replace("\\", "\\\\")
                    .Replace(".", "\\.")
                    .Replace("{", "\\{")
                    .Replace("}", "\\}")
                    .Replace("[", "\\[")
                    .Replace("]", "\\]")
                    .Replace("+", "\\+")
                    .Replace("$", "\\$")
                    .Replace(" ", "\\s")
                    .Replace("#", "[0-9]")
                    .Replace("?", ".")
                    .Replace("*", "\\w*")
                    .Replace("%", ".*")
                    , RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// Überprüft, ob es sich um eine Emailadresse handelt
        /// </summary>
        /// <param name="adress">zu prüfende Adresse</param>
        /// <returns>true, wenn gültige ADresse, sonst false</returns>
        public static bool IsEmailAddress(this string adress)
        {
            bool ret = true;

            try { MailAddress m = new MailAddress(adress); }
            catch (FormatException) { ret = false; }

            return ret;
        }

        /// <summary>
        /// Verschlüsselt einen String nach MD5 anhand eines Passworts
        /// Erweitert die Klasse String
        /// <example>
        /// string test="Das ist ein Test";
        /// string pass="Kennwort";
        /// string neu=test.EncryptString(pass);
        /// </example>
        /// </summary>
        /// <param name="text">String der verschlüsselt, werden soll</param>
        /// <param name="password">Passwort das den String verschlüsseln soll</param>
        /// <returns>Gibt den verschlüsselten String zurück, String.Empty wenn ein Fehler auftrat</returns>
        public static string EncryptString(this string text, string password)
        {
            string ret = String.Empty;
            MemoryStream ms = null;
            CryptoStream encStream = null;
            TripleDESCryptoServiceProvider des = null;

            try
            {
                des = new TripleDESCryptoServiceProvider();
                des.IV = new byte[8];
                PasswordDeriveBytes pdb = new PasswordDeriveBytes(password, new byte[0]);
                des.Key = pdb.CryptDeriveKey("RC2", "MD5", 128, new byte[8]);
                ms = new MemoryStream(text.Length * 2);
                encStream = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
                byte[] plainBytes = Encoding.UTF8.GetBytes(text);
                encStream.Write(plainBytes, 0, plainBytes.Length);
                encStream.FlushFinalBlock();
                byte[] encryptedBytes = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(encryptedBytes, 0, (int)ms.Length);

                ret = Convert.ToBase64String(encryptedBytes);
            }
            catch { throw; }
            finally
            {
                if (encStream != null)
                    encStream.Close();

                if (ms != null)
                    ms.Close();

                if (des != null)
                    des.Clear();
            }

            return ret;
        }

        /// <summary>
        /// Entschlüsselt einen String anhand eines Passworts
        /// Erweitert die Klasse String
        /// <example>
        /// string test="Das ist ein Test";
        /// string pass="Kennwort";
        /// string neu=test.DecryptString(pass);
        /// </example>
        /// </summary>
        /// <param name="text">Verschlüsselter String der entschlüsselt werden soll</param>
        /// <param name="password">Passwort mit dem der String verschlüsselt wurde</param>
        /// <returns>Gibt den entschlüsselten String zurück</returns>
        public static string DecryptString(this string text, string password)
        {
            string ret = String.Empty;
            MemoryStream ms = null;
            CryptoStream decStream = null;
            TripleDESCryptoServiceProvider des = null;

            try
            {
                des = new TripleDESCryptoServiceProvider();
                des.IV = new byte[8];
                des.Key = new PasswordDeriveBytes(password, new byte[0]).CryptDeriveKey("RC2", "MD5", 128, new byte[8]);
                
                byte[] encryptedBytes = Convert.FromBase64String(text);
                
                ms = new MemoryStream(text.Length);
                
                decStream = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
                decStream.Write(encryptedBytes, 0, encryptedBytes.Length);
                decStream.FlushFinalBlock();

                byte[] plainBytes = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(plainBytes, 0, (int)ms.Length);

                ret = Encoding.UTF8.GetString(plainBytes);
            }
            catch { throw; }
            finally
            {
                if (decStream != null) decStream.Close();

                if (ms != null) ms.Close();

                if (des != null) des.Clear();
            }

            return ret;
        }


    }
}
