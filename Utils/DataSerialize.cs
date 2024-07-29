using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace SNAMP.Utils
{
    public static class DataSerialize
    {
        public static T ReadData<T>(string path, bool isWriteByDefault = true) where T : IDataSerialize, new()
        {
            T newData = new T();

            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                byte[] buffer = new byte[fs.Length];
                fs.Read(buffer, 0, buffer.Length);
                string dataString = Encoding.UTF8.GetString(buffer);
                newData = JsonConvert.DeserializeObject<T>(dataString);
            }

            if (newData == null)
            {
                if (!isWriteByDefault)
                    return newData;

                newData = new T();
                newData.SetDataByDefalut();
                WriteData(newData, path);
                return newData;
            }

            newData.PrepareData();
            return newData;
        }

        public static void WriteData<T>(T data, string path) where T : IDataSerialize
        {
            data.PrepareData();
            File.Create(path).Close();

            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite))
            {
                string dataString = JsonConvert.SerializeObject(data, Formatting.Indented);
                byte[] buffer = Encoding.UTF8.GetBytes(dataString);
                fs.Write(buffer, 0, buffer.Length);
            }
        }
    }
}
