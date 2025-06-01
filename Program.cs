using System;
using System.IO;
using System.Security.Cryptography;

class Program
{
    static void Main()
    {
        Console.WriteLine("RSA File Encryptor/Decryptor");

        if (!File.Exists("private.key") || !File.Exists("public.key"))
        {
            Console.WriteLine("No key files found. Generating RSA key pair...");
            using (RSA rsa = RSA.Create(2048))
            {
                File.WriteAllText("private.key", rsa.ToXmlString(true));
                File.WriteAllText("public.key", rsa.ToXmlString(false));
            }
        }

        RSA rsaEnc = RSA.Create();
        rsaEnc.FromXmlString(File.ReadAllText("public.key"));

        RSA rsaDec = RSA.Create();
        rsaDec.FromXmlString(File.ReadAllText("private.key"));

        Console.Write("Enter path to file: ");
        string inputPath = Console.ReadLine();

        if (!File.Exists(inputPath))
        {
            Console.WriteLine("File not found.");
            return;
        }

        byte[] fileData = File.ReadAllBytes(inputPath);

        Console.Write("Encrypt or Decrypt? (e/d): ");
        string choice = Console.ReadLine().ToLower();

        if (choice == "e")
        {
            string encPath = inputPath + ".enc";
            byte[] encrypted = EncryptLargeData(fileData, rsaEnc);
            File.WriteAllBytes(encPath, encrypted);
            Console.WriteLine("Encrypted file saved: " + encPath);
        }
        else if (choice == "d")
        {
            string decPath = inputPath + ".dec";
            byte[] decrypted = DecryptLargeData(fileData, rsaDec);
            File.WriteAllBytes(decPath, decrypted);
            Console.WriteLine("Decrypted file saved: " + decPath);
        }
        else
        {
            Console.WriteLine("Invalid choice.");
        }
    }

    static byte[] EncryptLargeData(byte[] data, RSA rsa)
    {
        int blockSize = 214; // For 2048-bit key with PKCS#1
        using MemoryStream input = new MemoryStream(data);
        using MemoryStream output = new MemoryStream();
        byte[] buffer = new byte[blockSize];
        int bytesRead;
        while ((bytesRead = input.Read(buffer, 0, blockSize)) > 0)
        {
            byte[] chunk = new byte[bytesRead];
            Array.Copy(buffer, chunk, bytesRead);
            byte[] encrypted = rsa.Encrypt(chunk, RSAEncryptionPadding.Pkcs1);
            output.Write(encrypted, 0, encrypted.Length);
        }
        return output.ToArray();
    }

    static byte[] DecryptLargeData(byte[] data, RSA rsa)
    {
        int blockSize = 256; // 2048 bits = 256 bytes
        using MemoryStream input = new MemoryStream(data);
        using MemoryStream output = new MemoryStream();
        byte[] buffer = new byte[blockSize];
        int bytesRead;
        while ((bytesRead = input.Read(buffer, 0, blockSize)) > 0)
        {
            byte[] chunk = new byte[bytesRead];
            Array.Copy(buffer, chunk, bytesRead);
            byte[] decrypted = rsa.Decrypt(chunk, RSAEncryptionPadding.Pkcs1);
            output.Write(decrypted, 0, decrypted.Length);
        }
        return output.ToArray();
    }
}
