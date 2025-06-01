# RSA File Encryptor

A simple C# console application for file encryption and decryption using RSA 2048-bit keys.

## Features

- Automatically generates RSA key pair (public/private) on first run
- Encrypts any file using RSA and saves as `.enc`
- Decrypts encrypted files and restores original as `.dec`
- Keys are saved as XML in `public.key` and `private.key`

## Requirements

- .NET 7 SDK
- Windows or Linux terminal

## Usage

1. Build and run the application.
2. Provide the full or relative path to a file you want to encrypt or decrypt.
3. Choose:
   - `e` to encrypt
   - `d` to decrypt

### Example

Assume you have a file named `test.txt` in the same folder as the executable:

```
Enter path to file: test.txt
Encrypt or Decrypt? (e/d): e
Encrypted file saved: test.txt.enc
```

Then:

```
Enter path to file: test.txt.enc
Encrypt or Decrypt? (e/d): d
Decrypted file saved: test.txt.enc.dec
```

## Key Management

- On first run, the app creates:
  - `private.key` (used for decryption)
  - `public.key` (used for encryption)
- Keys are reused on subsequent runs.
- Keys are saved in XML format compatible with `RSA.ToXmlString` / `FromXmlString`.

## Notes

- Due to RSA limitations, data is processed in 214-byte chunks (encryption) and 256-byte chunks (decryption).
- Only files encrypted with this app and the same key pair can be decrypted successfully.
