/*
 * Created by SharpDevelop.
 * User: tanuki
 * Date: 23.10.2021
 * Time: 22:15
 *
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security;
using System.Security.Cryptography;
using System.Text;


using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

using System.Reflection;
using System.Text.RegularExpressions;



namespace PostConfiguratorLibrarySource
{

  public class GeneralMethods
  {
    //[DllImport("Lib_pc_commands.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static IntPtr PtrReturnAPI01(string i)
    {
      //	string ii = "a8a3cz1f82g3r4l" ;
      int val = Convert.ToInt32(i, 16); //Парсим шестнадцатеричное число
      IntPtr ptr = new IntPtr(val); //получаем указатель
      return(ptr);
    }

    //[DllImport("Lib_pc_commands.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static IntPtr PtrReturnAPI02(string i)
    {
      //	string ii = "a2a4sdf93nbx4j4" ;
      int val = Convert.ToInt32(i, 16); //Парсим шестнадцатеричное число
      IntPtr ptr = new IntPtr(val); //получаем указатель
      return(ptr);
    }

    public static byte[] ObjectToByteArray(object obj)
    {
      using (MemoryStream memoryStream = new MemoryStream())
      {
        new BinaryFormatter().Serialize((Stream) memoryStream, obj);
        return memoryStream.ToArray();
      }
    }

    public static object ByteArrayToObject(byte[] arrBytes)
    {
      if (arrBytes == null)
        return (object) null;
      using (MemoryStream memoryStream = new MemoryStream())
      {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        memoryStream.Write(arrBytes, 0, arrBytes.Length);
        memoryStream.Seek(0L, SeekOrigin.Begin);
        return binaryFormatter.Deserialize((Stream) memoryStream);
      }
    }

    public static void DecryptData(
      byte[] ToDecrypt,
      out byte[] DecryptedBytes,
      string v1,
      string v2)
    {
      //IntPtr ptr1 = GeneralMethods.PtrReturnAPI01(v1);
      //IntPtr ptr2 = GeneralMethods.PtrReturnAPI02(v2);
      try
      {
        Rijndael rijndael = Rijndael.Create();
        //rijndael.Key = Encoding.UTF8.GetBytes(Marshal.PtrToStringUni(ptr1));
        //rijndael.IV = Encoding.UTF8.GetBytes(Marshal.PtrToStringUni(ptr2));
        rijndael.Key = Encoding.UTF8.GetBytes(v1);
        rijndael.IV = Encoding.UTF8.GetBytes(v2);

        MemoryStream memoryStream = new MemoryStream();
        CryptoStream cryptoStream = new CryptoStream((Stream) memoryStream, rijndael.CreateDecryptor(), CryptoStreamMode.Write);
        cryptoStream.Write(ToDecrypt, 0, ToDecrypt.Length);
        cryptoStream.Close();
        DecryptedBytes = memoryStream.ToArray();
        memoryStream.Close();
      }
      catch (Exception ex)
      {
        DecryptedBytes = (byte[]) null;
      }
    }
  }

	class Program
	{
		public static void Main(string[] args)
		{
			Console.WriteLine("Hello World!");

			// TODO: Implement Functionality Here
			LoadFile() ;
			
			Console.WriteLine("finish!");

			Console.Write("Press any key to continue . . . ");
			Console.ReadKey(true);
		}


    public static void LoadFile()
    {

      byte[] DecryptedBytes;
      string tb3str = "8a3cz1f82g3r4lo8y2t9w6po52rn7xa9" ;
      string tb4str = "2a4sdf93nbx4j41h" ;
      GeneralMethods.DecryptData(File.ReadAllBytes("LibraryData_v5_4_0.ngh"),
                                 out DecryptedBytes, tb3str, tb4str);
      //object UIData = GeneralMethods.ByteArrayToObject(DecryptedBytes);

      //Преобразование расшифрованных данных в строку.
      string plainText = Encoding.UTF8.GetString(DecryptedBytes);

      File.WriteAllBytes("LibraryData_v5_4_01.txt",DecryptedBytes);
      File.WriteAllText("LibraryData_v5_4_02.txt",plainText);
      //File.WriteAllText("LibraryData_v5_4_03.txt",UIData);
    }







	}
}