using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Security.Cryptography;

public class EncryptScript {

	private static readonly string SALT = "c6eahbq9sjuawhvdr9kvhpsm5qv393ga";
	private static readonly string PASSWORD = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

	/// <summary>
	/// AES暗号化
	/// </summary>
	public static string EncryptUTF8AesToBase64(string src) 
	{
		byte[] data = System.Text.Encoding.UTF8.GetBytes(src);

		RijndaelManaged rijndael = new RijndaelManaged ();
		rijndael.Padding = PaddingMode.PKCS7;
		rijndael.Mode = CipherMode.CBC;
		rijndael.KeySize = 256;
		rijndael.BlockSize = 128;

		byte[] bSalt = Encoding.UTF8.GetBytes (SALT);
		Rfc2898DeriveBytes deriveBytes = new Rfc2898DeriveBytes (PASSWORD, bSalt);
		deriveBytes.IterationCount = 1000;        // 反復回数

		rijndael.Key = deriveBytes.GetBytes (rijndael.KeySize / 8);
		rijndael.IV = deriveBytes.GetBytes (rijndael.BlockSize / 8);

		// 暗号化
		ICryptoTransform encryptor = rijndael.CreateEncryptor ();
		byte[] encrypted = encryptor.TransformFinalBlock (data, 0, src.Length);

		encryptor.Dispose ();

//		string result = System.Text.Encoding.UTF8.GetString(encrypted); 
		//暗号文をBase64化
		string result = System.Convert.ToBase64String(encrypted);
		Debug.Log ("Encoded: " + result);
		return result;
	}



	/// <summary>
	/// AES複合化
	/// </summary>
	public static string DecryptBase64AesToUTF8(string src)
	{
		Debug.Log ("decrypt:" + "src; " + src);

//		byte[] data = System.Text.Encoding.UTF8.GetBytes(src);
		//セーブデータをBase64化
		byte[] data = System.Convert.FromBase64String(src);

		RijndaelManaged rijndael = new RijndaelManaged ();
		rijndael.Padding = PaddingMode.PKCS7;
		rijndael.Mode = CipherMode.CBC;
		rijndael.KeySize = 256;
		rijndael.BlockSize = 128;


		byte[] bSalt = Encoding.UTF8.GetBytes (SALT);
		Rfc2898DeriveBytes deriveBytes = new Rfc2898DeriveBytes (PASSWORD, bSalt);
		deriveBytes.IterationCount = 1000;        // 反復回数

		rijndael.Key = deriveBytes.GetBytes (rijndael.KeySize / 8);
		rijndael.IV = deriveBytes.GetBytes (rijndael.BlockSize / 8);

		// 復号化
		ICryptoTransform decryptor = rijndael.CreateDecryptor ();
		byte[] plain = decryptor.TransformFinalBlock (data, 0, data.Length);

		decryptor.Dispose ();

		string result = System.Text.Encoding.UTF8.GetString(plain);
		return result;
	}
}
