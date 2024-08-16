﻿using System.IO;
using System.Threading.Tasks;

namespace domain.Abstractions;

public interface IS3Service
{
    Task Upload(Stream stream, string key);
    Task Delete(string key);
    Task<Stream> Download(string key);
}
