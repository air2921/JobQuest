using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace domain.Abstractions;

public interface IS3Service
{
    Task Upload(Stream stream, string key, CancellationToken token = default);
    Task Delete(string key, CancellationToken token = default);
    Task<Dictionary<string, Stream>> DownloadCollection(IEnumerable<string> keys, CancellationToken token = default);
    Task<Stream> Download(string key, CancellationToken token = default);
}
