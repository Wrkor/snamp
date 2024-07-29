using SNAMP.Models;
using System.Collections.Generic;
using System.Linq;

namespace SNAMP
{
    public class DataProjects : IDataSerialize
    {
        public List<SMRProject> projects;

        public void PrepareData()
        {
            projects = projects.OrderByDescending(project => project.Date).ToList();
        }

        public void SetDataByDefalut()
        {
            projects = new List<SMRProject>();
        }
    }
}
