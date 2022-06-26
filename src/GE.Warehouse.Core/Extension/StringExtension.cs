using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using GE.Warehouse.Core.Helper;

namespace GE.Warehouse.Core.Extension
{
    public static class StringExtension
    {
        private static readonly string KeySecure = ConfigurationManager.AppSettings["KeySecure"].ToString();
        private static readonly Regex WebUrlExpression = new Regex(@"(http|https)://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?", RegexOptions.Singleline | RegexOptions.Compiled);
        private static readonly Regex EmailExpression = new Regex(@"^([0-9a-zA-Z]+[-._+&])*[0-9a-zA-Z]+@([-0-9a-zA-Z]+[.])+[a-zA-Z]{2,6}$", RegexOptions.Singleline | RegexOptions.Compiled);
        private static readonly Regex StripHTMLExpression = new Regex("<\\S[^><]*>", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.CultureInvariant | RegexOptions.Compiled);
        const string FindText = "ỵỹỷỳýựữửừứưụũủùúợỡởờớơộỗổồốôọõỏòóịĩỉìíệễểềếêẹẽẻèéđậẫẩầấâặẵẳằắăạãảàáỴỸỶỲÝỰỮỬỪỨƯỤŨỦÙÚỢỠỞỜỚƠỘỖỔỒỐÔỌÕỎÒÓỊĨỈÌÍỆỄỂỀẾÊẸẼẺÈÉĐẬẪẨẦẤÂẶẴẲẰẮĂẠÃẢÀÁ ’\".$`~!@'#%^&*()?/\\>,<;:–+";
        const string ReplText = "yyyyyuuuuuuuuuuuoooooooooooooooooiiiiieeeeeeeeeeedaaaaaaaaaaaaaaaaaYYYYYUUUUUUUUUUUOOOOOOOOOOOOOOOOOIIIIIEEEEEEEEEEEDAAAAAAAAAAAAAAAAA-";

        private static readonly char[] IllegalUrlCharacters = new[] { ';', '/', '\\', '?', ':', '@', '&', '=', '+', '$', ',', '<', '>', '#', '%', '.', '!', '*', '\'', '"', '(', ')', '[', ']', '{', '}', '|', '^', '`', '~', '–', '‘', '’', '“', '”', '»', '«' };

        [DebuggerStepThrough]
        public static bool IsWebUrl(this string target)
        {
            return !string.IsNullOrEmpty(target) && WebUrlExpression.IsMatch(target);
        }

        [DebuggerStepThrough]
        public static bool IsEmail(this string target)
        {
            return !string.IsNullOrEmpty(target) && EmailExpression.IsMatch(target);
        }

        [DebuggerStepThrough]
        public static string NullSafe(this string target)
        {
            return (target ?? string.Empty).Trim();
        }

        [DebuggerStepThrough]
        public static string FormatWith(this string target, params object[] args)
        {            
            return string.Format(Constants.CurrentCulture, target, args);
        }

        [DebuggerStepThrough]
        public static string Hash(this string target)
        {            
            using (MD5 md5 = MD5.Create())
            {
                byte[] data = Encoding.Unicode.GetBytes(target);
                byte[] hash = md5.ComputeHash(data);

                return Convert.ToBase64String(hash);
            }
        }

        [DebuggerStepThrough]
        public static string WrapAt(this string target, int index)
        {
            const int dotCount = 3;            

            return (target.Length <= index) ? target : string.Concat(target.Substring(0, index - dotCount), new string('.', dotCount));
        }

        [DebuggerStepThrough]
        public static string Sub(this string target, int length)
        {
            if(!string.IsNullOrEmpty(target))
            {
                if (target.Length < length)
                {
                    return target;
                }
                return target.Substring(0, length) + "...";    
            }
            return "";
        }

        [DebuggerStepThrough]
        public static string StripHtml(this string target)
        {
            return StripHTMLExpression.Replace(target, string.Empty);
        }

        [DebuggerStepThrough]
        public static Guid ToGuid(this string target)
        {
            Guid result = Guid.Empty;

            if ((!string.IsNullOrEmpty(target)) && (target.Trim().Length == 22))
            {
                string encoded = string.Concat(target.Trim().Replace("-", "+").Replace("_", "/"), "==");

                try
                {
                    byte[] base64 = Convert.FromBase64String(encoded);

                    result = new Guid(base64);
                }
                catch (FormatException)
                {
                }
            }

            return result;
        }

        [DebuggerStepThrough]
        public static int RandomNumber(int min, int max)
        {
            var random = new Random();
            return random.Next(min, max);
        }

        [DebuggerStepThrough]
        public static T ToEnum<T>(this string target, T defaultValue) where T : IComparable, IFormattable
        {
            T convertedValue = defaultValue;

            if (!string.IsNullOrEmpty(target))
            {
                try
                {
                    convertedValue = (T)Enum.Parse(typeof(T), target.Trim(), true);
                }
                catch (ArgumentException)
                {
                }
            }

            return convertedValue;
        }

        [DebuggerStepThrough]
        public static string ToLegalUrl(this string target)
        {
            if (string.IsNullOrEmpty(target))
            {
                return target;
            }

            target = target.Trim();

            if (target.IndexOfAny(IllegalUrlCharacters) > -1)
            {
                target = IllegalUrlCharacters.Aggregate(target, (current, character) => current.Replace(character.ToString(Constants.CurrentCulture), string.Empty));
            }

            target = target.Replace(" ", "-");

            while (target.Contains("--"))
            {
                target = target.Replace("--", "-");
            }

            return target;
        }

        [DebuggerStepThrough]
        public static string UrlEncode(this string target)
        {
            return HttpUtility.UrlEncode(target);
        }

        [DebuggerStepThrough]
        public static string UrlDecode(this string target)
        {
            return HttpUtility.UrlDecode(target);
        }

        [DebuggerStepThrough]
        public static string AttributeEncode(this string target)
        {
            return HttpUtility.HtmlAttributeEncode(target);
        }

        [DebuggerStepThrough]
        public static string HtmlEncode(this string target)
        {
            return HttpUtility.HtmlEncode(target);
        }

        [DebuggerStepThrough]
        public static string HtmlDecode(this string target)
        {
            return HttpUtility.HtmlDecode(target);
        }

        public static string Replace(this string target, ICollection<string> oldValues, string newValue)
        {
            oldValues.ForEach(oldValue => target = target.Replace(oldValue, newValue));
            return target;
        }

        [DebuggerStepThrough]
        public static string ToDecryptDes(this string target)
        {
            //get the byte code of the string

            //        byte[] toEncryptArray = Convert.FromBase64String(cipherString);
            byte[] toEncryptArray = HexToBytes(target);

            //if hashing was used get the hash code with regards to your key
            var hashmd5 = new MD5CryptoServiceProvider();
            byte[] keyArray = hashmd5.ComputeHash(Encoding.UTF8.GetBytes(KeySecure));
            //release any resource held by the MD5CryptoServiceProvider

            hashmd5.Clear();

            var tdes = new TripleDESCryptoServiceProvider
                           {
                               Key = keyArray,
                               Mode = CipherMode.ECB,
                               Padding = PaddingMode.PKCS7
                           };
            //set the secret key for the tripleDES algorithm
            //mode of operation. there are other 4 modes. 
            //We choose ECB(Electronic code Book)

            //padding mode(if any extra byte added)

            ICryptoTransform cTransform = tdes.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(
                                 toEncryptArray, 0, toEncryptArray.Length);
            //Release resources held by TripleDes Encryptor                
            tdes.Clear();
            //return the Clear decrypted TEXT
            return Encoding.UTF8.GetString(resultArray);
        }

        [DebuggerStepThrough]
        public static string ToEncryptDes(this string target)
        {
            byte[] toEncryptArray = Encoding.UTF8.GetBytes(target);

            //System.Windows.Forms.MessageBox.Show(key);
            //If hashing use get hashcode regards to your key

            var hashmd5 = new MD5CryptoServiceProvider();
            byte[] keyArray = hashmd5.ComputeHash(Encoding.UTF8.GetBytes(KeySecure));
            //Always release the resources and flush data
            // of the Cryptographic service provide. Best Practice

            hashmd5.Clear();


            var tdes = new TripleDESCryptoServiceProvider
                           {
                               Key = keyArray,
                               Mode = CipherMode.ECB,
                               Padding = PaddingMode.PKCS7
                           };
            //set the secret key for the tripleDES algorithm
            //mode of operation. there are other 4 modes.
            //We choose ECB(Electronic code Book)
            //padding mode(if any extra byte added)

            ICryptoTransform cTransform = tdes.CreateEncryptor();
            //transform the specified region of bytes array to resultArray
            byte[] resultArray =
              cTransform.TransformFinalBlock(toEncryptArray, 0,
              toEncryptArray.Length);
            //Release resources held by TripleDes Encryptor
            tdes.Clear();
            //Return the encrypted data into unreadable string format

            //StringBuilder hex = new StringBuilder();
            //for(int i =0 ; i < resultArray.Length -1; i++) hex.AppendFormat("{0:X2}", resultArray[i]);
            //return hex.ToString(); 
            return BytesToHex(resultArray);

            //return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        private static string BytesToHex(byte[] bytes)
        {
            var hex = new StringBuilder();
            for (int n = 0; n <= bytes.Length - 1; n++)
            {
                hex.AppendFormat("{0:X2}", bytes[n]);
            }
            return hex.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        private static Byte[] HexToBytes(string hex)
        {
            int numBytes = hex.Length / 2;
            var bytes = new Byte[numBytes];
            for (int n = 0; n <= numBytes - 1; n++)
            {
                string hexByte = hex.Substring(n * 2, 2);
                bytes[n] = Convert.ToByte(int.Parse(hexByte, System.Globalization.NumberStyles.HexNumber));
            }
            return bytes;
        }

        public static string FormatStringFullTextSearch(this string target)
        {
            target = target.ToStringSql();
            if (String.IsNullOrEmpty(target)) return target;
            var sb = new StringBuilder();
            string[] arrWords = target.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            int i = 0, length = 0;
            length = arrWords.Length;

            for (i = 0; i < length; i++)
            {
                if (arrWords[i].Length > 1)
                {
                    sb.Append(i < length - 1
                                  ? String.Format("\"{0}\" AND ", arrWords[i].Trim())
                                  : String.Format("\"{0}\" ", arrWords[i].Trim()));
                }
            }

            if (!String.IsNullOrEmpty(sb.ToString()))
            {
                sb.Insert(0, "N'");
                sb.Append("'");
                return sb.ToString().Trim();
            }
            return string.Empty;

        }

        public static string ToStringSql(this string target)
        {
            if (string.IsNullOrEmpty(target))
            {
                return target;
            }

            target = target.Trim();

            if (target.IndexOfAny(IllegalUrlCharacters) > -1)
            {
                target = IllegalUrlCharacters.Aggregate(target, (current, character) => current.Replace(character.ToString(), string.Empty));
            }
            return target;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strVietNamese"></param>
        /// <returns></returns>
        public static string ClearSign(this string strVietNamese)
        {
            if (string.IsNullOrEmpty(strVietNamese)) return "";
            int index = -1;
            while ((index = strVietNamese.IndexOfAny(FindText.ToCharArray())) != -1)
            {
                int index2 = FindText.IndexOf(strVietNamese[index]);
                strVietNamese = index2 > 134 ? strVietNamese.Remove(index, 1) : strVietNamese.Replace(strVietNamese[index], ReplText[index2]);
            }
            return strVietNamese.ToLower();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string ConverValue(this string text)
        {
            var regex = new Regex(@"\s{2,}");
            text = regex.Replace(text.Trim(), " ");//This line removes extra spaces and make space exactly one.
            //To remove the  space between the end of a word and a punctuation mark used in the text we will
            //be using following line of code
            regex = new Regex(@"\s(\!|\.|\?|\;|\,|\:)"); // “\s” whill check for space near all puntuation marks in side ( \!|\.|\?|\;|\,|\:)”); )
            text = regex.Replace(text, "$1");
            return text.Replace(' ', '-').Replace(".", "") + "";
        }

        public static string ToCamelCase(this string phrase)
        {
            if (phrase == null)
                return string.Empty;

            var sb = new StringBuilder(phrase.Length);

            // First letter is always upper case
            bool nextUpper = true;

            foreach (char ch in phrase)
            {
                if (char.IsWhiteSpace(ch) || char.IsPunctuation(ch) || char.IsSeparator(ch))
                {
                    nextUpper = true;
                    continue;
                }

                sb.Append(nextUpper ? char.ToUpper(ch) : char.ToLower(ch));

                nextUpper = false;
            }

            return sb.ToString();
        }
    }
}