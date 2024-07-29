using SNAMP.Models;
using System.Collections.Generic;

namespace SNAMP
{
    public class DataMeta : IDataSerialize
    {
        public List<LinkToFile> links;

        public void PrepareData() { }

        public void SetDataByDefalut()
        {
            links = new List<LinkToFile>();
        }
    }
}
