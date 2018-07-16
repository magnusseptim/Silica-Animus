using Abominable_Intelligence.Model;
using Newtonsoft.Json;
using System;
using System.IO;

namespace Abominable_Intelligence.Extension
{
    public static class BuildInConfigExtension<T> where T : ConfigData, new()
    {
        public static T ReadAsObject(string path)
        {
            T result;
            try
            {
                using (StreamReader file = File.OpenText(path))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    result = (T)serializer.Deserialize(file, typeof(T));
                }
               
            }
            catch (Exception ex)
            {
                result = new T();
                result.ExceptionMessage = ex.Message;
            }
            return result;
        }
    }
}
