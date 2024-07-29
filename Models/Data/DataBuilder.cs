using System;
using System.Collections.Generic;

namespace SNAMP
{
    public class DataBuilder : ICloneable, IDataSerialize
    {
        public List<string> directories;

        public object Clone() => new DataBuilder { directories = new List<string>(directories) };

        public void PrepareData() { }

        public void SetDataByDefalut()
        {
            directories = new List<string>(DataDefault.Directories);
        }
    }
}
