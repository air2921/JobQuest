﻿using System;

namespace common.Exceptions;

public class SmtpClientException : Exception
{
    public SmtpClientException() { }
    public SmtpClientException(string? message = "Ошибка при отправке сообщения") : base(message) { }
}