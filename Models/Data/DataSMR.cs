using Newtonsoft.Json;
using SNAMP.Models;
using System.Collections.Generic;
using System.ComponentModel;

namespace SNAMP
{
    public class DataSMR : IDataSerialize
    {
        public List<SMRSpecification> SMRSpecifications { get; set; }

        public string Name
        {
            get { return name; }
            set
            {
                if (value != name)
                    OnNotifyPropertyChanged();
                name = value;
            }
        }
        public string Position
        {
            get { return position; }
            set
            {
                if (value != position)
                    OnNotifyPropertyChanged();
                position = value;
            }
        }
        public string Description
        {
            get { return description; }
            set
            {
                if (value != description)
                    OnNotifyPropertyChanged();
                description = value;
            }
        }
        public string PathToImage
        {
            get { return pathToImage; }
            set
            {
                if (value != pathToImage)
                    OnNotifyPropertyChanged();
                pathToImage = value;
            }
        }

        [JsonIgnore]
        public bool isSave;

        private string name;
        private string position;
        private string description;
        private string pathToImage;

        public void PrepareData() 
        {
            isSave = true;
            SMRSpecifications.ForEach(smrSpecification => smrSpecification.IsSave = true);
        }

        public void SetDataByDefalut()
        {
            SMRSpecifications = new List<SMRSpecification>(1) { new SMRSpecification() };
            Name = string.Empty;
            Position = string.Empty;
            PathToImage = string.Empty;
            Description = string.Empty;
        }

        public bool IsSaveData() => isSave && SMRSpecifications.Find(smrSpecification => !smrSpecification.IsSave) == null;

        private void OnNotifyPropertyChanged() => isSave = false;
    }
}
