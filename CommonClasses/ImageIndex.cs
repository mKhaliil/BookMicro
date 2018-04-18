using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;
/// <summary>
namespace Outsourcing_System
{
    public class ImageIndex
    {
        public ImageIndex()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public  string GetHeader(string imageStream)
        {
            StreamReader sr = new StreamReader(imageStream);
            string inputStream = sr.ReadToEnd();
            string output = GetCaption(inputStream);
            return output;
        }
        public Stream GetHeader(string imageStream, bool loadFile)
        {
            StreamReader sr = new StreamReader(imageStream);
            string inputStream = sr.ReadToEnd();
            sr.Close();
            string output = "";
            try
            {
                output = GetCaption(inputStream);
            }
            catch { }

            System.IO.MemoryStream mstrm = new System.IO.MemoryStream();
            System.IO.StreamWriter sw = new System.IO.StreamWriter(mstrm);

            System.IO.Stream strm = sw.BaseStream;

            StringReader srr = new StringReader(output);
            char[] buffs = new char[output.Length];
            srr.Read(buffs, 0, output.Length);

            int length = UnicodeEncoding.UTF8.GetBytes(output).Length;
            byte[] outBytes = UnicodeEncoding.UTF8.GetBytes(buffs);
            if (length != output.Length)
            {
                length = System.Text.UnicodeEncoding.Unicode.GetBytes(output).Length;
                outBytes = UnicodeEncoding.Unicode.GetBytes(buffs);
            }
            mstrm.Write(outBytes, 0, outBytes.Length);
            strm.Position = 0;
            mstrm.WriteTo(strm);
            return strm;
        }
        public  string GetHeaderAsString(string imageStream, bool loadFile)
        {
            StreamReader sr = new StreamReader(imageStream);
            string inputStream = sr.ReadToEnd();
            sr.Close();
            string output = "";
            try
            {
                output = GetCaption(inputStream);
            }
            catch { }

            System.IO.MemoryStream mstrm = new System.IO.MemoryStream();
            System.IO.StreamWriter sw = new System.IO.StreamWriter(mstrm);

            System.IO.Stream strm = sw.BaseStream;

            StringReader srr = new StringReader(output);
            char[] buffs = new char[output.Length];
            srr.Read(buffs, 0, output.Length);

            int length = UnicodeEncoding.UTF8.GetBytes(output).Length;
            byte[] outBytes = UnicodeEncoding.UTF8.GetBytes(buffs);
            if (length != output.Length)
            {
                length = System.Text.UnicodeEncoding.Unicode.GetBytes(output).Length;
                outBytes = UnicodeEncoding.Unicode.GetBytes(buffs);
            }
            mstrm.Write(outBytes, 0, outBytes.Length);
            strm.Position = 0;
            mstrm.WriteTo(strm);
            //return strm;
            //Converting stream into string
            StreamReader reader = new StreamReader(strm);
            byte[] bytes1 = new byte[strm.Length];
            strm.Position = 0;
            strm.Read(bytes1, 0, (int)strm.Length);
            string text = System.Text.Encoding.Unicode.GetString(bytes1);
            //~Converting stream into string
            return text;
        }

        public  void SetHeader(string imageStream, string fileName)
        {
            string final = SetCaption(imageStream, "You are coder and my email iss axy19poorcode.com and i take 30 for code mess");
            StreamWriter sw = new StreamWriter(fileName);
            sw.Write(final);
            sw.Close();
        }

        public  string SetHeader(string imageStream)
        {
            //string final = SetCaption(imageStream, "You are coder and my email iss axy19poorcode.com and i take 30 for code mess");
            string final = SetCaption(imageStream, "You are coder and my email iss axy19poorcode.com and i take 30 for code mess");
            
            return final;
        }

        private  string SetCaption(string Message, string key)
        {
            byte[] Results;
            System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();

            // Step 1. We hash the passphrase using MD5
            // We use the MD5 hash generator as the result is a 128 bit byte array
            // which is a valid length for the TripleDES encoder we use below

            MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
            byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(key));

            // Step 2. Create a new TripleDESCryptoServiceProvider object
            TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();

            // Step 3. Setup the encoder
            TDESAlgorithm.Key = TDESKey;
            TDESAlgorithm.Mode = CipherMode.ECB;
            TDESAlgorithm.Padding = PaddingMode.PKCS7;

            // Step 4. Convert the input string to a byte[]
            byte[] DataToEncrypt = UTF8.GetBytes(Message);

            // Step 5. Attempt to encrypt the string
            try
            {
                ICryptoTransform Encryptor = TDESAlgorithm.CreateEncryptor();
                Results = Encryptor.TransformFinalBlock(DataToEncrypt, 0, DataToEncrypt.Length);
            }
            finally
            {
                // Clear the TripleDes and Hashprovider services of any sensitive information
                TDESAlgorithm.Clear();
                HashProvider.Clear();
            }

            // Step 6. Return the encrypted string as a base64 encoded string
            return Convert.ToBase64String(Results);
        }
        private  string GetCaption(string imageStream)
        {
            byte[] Results;
            System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();

            // Step 1. We hash the passphrase using MD5
            // We use the MD5 hash generator as the result is a 128 bit byte array
            // which is a valid length for the TripleDES encoder we use below

            MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
            byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes("You are coder and my email iss axy19poorcode.com and i take 30 for code mess"));

            // Step 2. Create a new TripleDESCryptoServiceProvider object
            TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();

            // Step 3. Setup the decoder
            TDESAlgorithm.Key = TDESKey;
            TDESAlgorithm.Mode = CipherMode.ECB;
            TDESAlgorithm.Padding = PaddingMode.PKCS7;

            // Step 4. Convert the input string to a byte[]
            byte[] DataToDecrypt = Convert.FromBase64String(imageStream);

            // Step 5. Attempt to decrypt the string
            try
            {
                ICryptoTransform Decryptor = TDESAlgorithm.CreateDecryptor();
                Results = Decryptor.TransformFinalBlock(DataToDecrypt, 0, DataToDecrypt.Length);
            }
            finally
            {
                // Clear the TripleDes and Hashprovider services of any sensitive information
                TDESAlgorithm.Clear();
                HashProvider.Clear();
            }

            // Step 6. Return the decrypted string in UTF8 format
            string userNameIs = UTF8.GetString(Results);
            return userNameIs;
        }
    }
}
