using System;

namespace domain.Exceptions
{
    public class S3Exception(string message) : Exception(message)
    {
    }
}
