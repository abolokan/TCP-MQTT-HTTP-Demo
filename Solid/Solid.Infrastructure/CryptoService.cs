using Solid.Application.Interfaces;
using System.Security.Cryptography;

namespace Solid.Application;

public class CryptoService : ICryptoService
{
    public string Hex(byte[] binary)
    {
        string hex = string.Empty;


        using (MD5 md5 = MD5.Create())
        {
            byte[] hash = md5.ComputeHash(binary);
            hex = BitConverter.ToString(hash).Replace("-", "").ToLower();
        }

        return hex;
    }
}
