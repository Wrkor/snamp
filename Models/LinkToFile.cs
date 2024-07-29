using Newtonsoft.Json;

namespace SNAMP.Models
{
    public class LinkToFile
    {
        [JsonIgnore]
        public string Link { get; set; }
        public string LinkTreeNode { get; set; }
        
        public int LinkPage { get; set; }
        
        public LinkToFile(string linkTreeNode, string link, int linkPage = 0) 
        { 
            Link = link;
            LinkTreeNode = linkTreeNode;
            LinkPage = linkPage;
        }
    }
}
