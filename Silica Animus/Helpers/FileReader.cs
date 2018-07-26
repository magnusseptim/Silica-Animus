using Newtonsoft.Json;
using NLog;
using System;
using System.IO;

namespace Silica_Animus.Helpers
{
    public static class FileReader
    {
        public static ConfFile ReadAsJSONFromFile<ConfFile>(string filepath) where ConfFile : class, new()   
        {
            try
            {
                using (StreamReader reader = new StreamReader(filepath))
                {
                    return JsonConvert.DeserializeObject<ConfFile>(reader.ReadToEnd());
                }
            }
            catch(Exception)
            {
                throw;
            }
        }
    }
}
