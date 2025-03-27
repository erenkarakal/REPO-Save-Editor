using System.Security.Cryptography;
using System.Text;

string whyWouldYouCheat = "Why would you want to cheat?... :o It's no fun. :') :'D";

void Main()
{
    if (args.Length == 0)
        Exit("Missing file path argument.");

    string filePath = args[0];

    if (filePath.EndsWith(".es3")) // decrypt
    {
        byte[] saveData = File.ReadAllBytes(filePath);
        byte[] decryptedData = DecryptSave(saveData, whyWouldYouCheat);
        string decryptedString = Encoding.UTF8.GetString(decryptedData);
        string decryptedFilePath = Path.ChangeExtension(filePath, ".json");

        if (!confirmFile(decryptedFilePath))
            Exit("Cancelled.");

        File.WriteAllText(decryptedFilePath, decryptedString);
    }
    else if (filePath.EndsWith(".json")) // encrypt
    {
        byte[] data = File.ReadAllBytes(filePath);
        string encryptedFilePath = Path.ChangeExtension(filePath, ".es3");
        byte[] encryptedData = EncryptSave(data, whyWouldYouCheat);

        if (!confirmFile(encryptedFilePath))
            Exit("Cancelled.");

        File.WriteAllBytes(encryptedFilePath, encryptedData);
    }
    else
        Exit("File format must be .es3 or .json");

    Exit("Done!");
}
void Exit(string message)
{
    Console.WriteLine(message);
    Console.ReadKey();
    Environment.Exit(0);
}

bool confirmFile(string filePath)
{
    if (File.Exists(filePath))
    {
        Console.WriteLine($"File '{filePath}' already exists.\nType 'confirm' if you want to overwrite it.");
        if (!Console.ReadLine().ToLower().Equals("confirm"))
            return false;
    }
    return true;
}

byte[] EncryptSave(byte[] data, string password)
{
    using MemoryStream input = new MemoryStream(data);
    using MemoryStream output = new MemoryStream();
    using Aes aes = Aes.Create();

    aes.GenerateIV();
    output.Write(aes.IV, 0, aes.IV.Length);

    Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, aes.IV, 100);
    aes.Key = rfc2898DeriveBytes.GetBytes(16);

    using ICryptoTransform cryptoTransform = aes.CreateEncryptor();
    using CryptoStream cryptoStream = new CryptoStream(output, cryptoTransform, CryptoStreamMode.Write);

    input.CopyTo(cryptoStream);
    cryptoStream.FlushFinalBlock();

    return output.ToArray();
}

byte[] DecryptSave(byte[] saveData, string password)
{
    using MemoryStream input = new MemoryStream(saveData);
    using MemoryStream output = new MemoryStream();
    using Aes aes = Aes.Create();

    byte[] array = new byte[16];
    input.Read(array, 0, 16);
    aes.IV = array;

    Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, aes.IV, 100);
    aes.Key = rfc2898DeriveBytes.GetBytes(16);

    using ICryptoTransform cryptoTransform = aes.CreateDecryptor();
    using CryptoStream cryptoStream = new CryptoStream(input, cryptoTransform, CryptoStreamMode.Read);

    CopyStream(cryptoStream, output);

    output.Position = 0L;

    return output.ToArray();
}

void CopyStream(Stream input, Stream output)
{
    int bufferSize = 2048;
    byte[] buffer = new byte[bufferSize];
    int count;
    while ((count = input.Read(buffer, 0, bufferSize)) > 0)
        output.Write(buffer, 0, count);
}

try
{
    Main();
}
catch (Exception e)
{
    Exit("\n\n\nError:\n" + e.Message);
}
