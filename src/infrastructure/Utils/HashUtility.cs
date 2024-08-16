using domain.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using BC = BCrypt.Net;

namespace infrastructure.Utils;

public class HashUtility(ILogger<HashUtility> logger) : IHashUtility
{
    public string Hash(string password) => BC.BCrypt.EnhancedHashPassword(password, BC.HashType.SHA512);

    public bool Verify(string input, string src)
    {
        try
        {
            return BC.BCrypt.EnhancedVerify(input, src, BC.HashType.SHA512);
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex.ToString());

            return false;
        }
    }
}
