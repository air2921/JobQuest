using System;

namespace common.Exceptions;

public class EntityException(string message) : Exception(message)
{
}
