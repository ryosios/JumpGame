using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using UnityEngine;

public static class SaveManager
{
    private static readonly string FilePath =
        Path.Combine(Application.persistentDataPath, "save.dat");

    //適当でいいが「固定」にする
    private static readonly string Key = "MySecretKey12345"; // 16文字
    private static readonly string Iv = "MySecretIv123456"; // 16文字

    // --------------------
    // Save
    // --------------------
    public static void Save(SaveData data)
    {
        string json = JsonUtility.ToJson(data);
        byte[] encrypted = Encrypt(json);
        File.WriteAllBytes(FilePath, encrypted);
    }

    // --------------------
    // Load
    // --------------------
    public static SaveData Load()
    {
        Debug.Log("保存された"+Application.persistentDataPath);
        if (!File.Exists(FilePath))
            return null;

        byte[] encrypted = File.ReadAllBytes(FilePath);
        string json = Decrypt(encrypted);
        return JsonUtility.FromJson<SaveData>(json);
    }

    // --------------------
    // AES Encrypt
    // --------------------
    private static byte[] Encrypt(string plainText)
    {
        using var aes = Aes.Create();
        aes.Key = Encoding.UTF8.GetBytes(Key);
        aes.IV = Encoding.UTF8.GetBytes(Iv);

        using var encryptor = aes.CreateEncryptor();
        byte[] bytes = Encoding.UTF8.GetBytes(plainText);
        return encryptor.TransformFinalBlock(bytes, 0, bytes.Length);
    }

    // --------------------
    // AES Decrypt
    // --------------------
    private static string Decrypt(byte[] cipherText)
    {
        using var aes = Aes.Create();
        aes.Key = Encoding.UTF8.GetBytes(Key);
        aes.IV = Encoding.UTF8.GetBytes(Iv);

        using var decryptor = aes.CreateDecryptor();
        byte[] bytes = decryptor.TransformFinalBlock(cipherText, 0, cipherText.Length);
        return Encoding.UTF8.GetString(bytes);
    }
    /*
     保存
     SaveManager.Save(new SaveData
     {
         //C:\Users\taro\AppData\LocalLow\MyCompany\MyGame\save.dat
         gold = 100
     });

    　読み込み
      var data = SaveManager.Load();
      if (data != null)
      {
          int gold = data.gold;
      }
     */

}
