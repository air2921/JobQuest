using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tests.application;

internal class ObjectValueGetter
{
    public static string GetData(object data, string name)
    {
        var resultData = JObject.FromObject(data);
        return resultData[name]?.ToString()!;
    }
}
