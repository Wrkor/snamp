using System;
using Newtonsoft.Json;

namespace SNAMP.Models
{
    public class SMRProject
    {
        public DateTime Date { get; private set; }

        public string Name { get; private set; }
        public string Path { get; private set; }

        [JsonIgnore]
        public bool isActive;

        public SMRProject(string name, string path, DateTime date)
        {
            Name = name;
            Path = path;
            Date = date;
        }

        public string GetFullName() => Name + DataDefault.SMR_PROJECT_EXT;

        public string GetFullPath() => System.IO.Path.Combine(Path, GetFullName());

        public override int GetHashCode() => GetFullPath().GetHashCode();

        public override bool Equals(object obj) => obj is SMRProject smrProject && smrProject.GetFullPath() == GetFullPath();

        public void UpdateTime() => Date = DateTime.Now;
    }
}
