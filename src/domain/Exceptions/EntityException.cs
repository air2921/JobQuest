using System;

namespace domain.Exceptions
{
    public class EntityException(string message) : Exception(message)
    {
    }
}
