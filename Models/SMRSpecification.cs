using Newtonsoft.Json;

namespace SNAMP.Models
{
    public class SMRSpecification
    {
        [JsonIgnore]
        public bool IsSave { get; set; }

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
        public string Value
        {
            get { return value; }
            set
            {
                if (value != this.value)
                    OnNotifyPropertyChanged();
                this.value = value;
            }
        }
        public string Unit
        {
            get { return unit; }
            set
            {
                if (value != unit)
                    OnNotifyPropertyChanged();
                unit = value;
            }
        }
        public string DecimalPrefixes
        {
            get { return decimalPrefixes; }
            set
            {
                if (value != decimalPrefixes)
                    OnNotifyPropertyChanged();
                decimalPrefixes = value;
            }
        }

        private string name;
        private string value;
        private string unit;
        private string decimalPrefixes;

        public SMRSpecification(string name = "", string value = "", string unit = "", string decimalPrefixes = "") 
        {
            Name = name;
            Value = value;
            Unit = unit;
            DecimalPrefixes = decimalPrefixes;
            IsSave = false;
        }

        private void OnNotifyPropertyChanged() => IsSave = false;
    }
}
