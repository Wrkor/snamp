using System.Windows.Forms;

namespace SNAMP
{
    public interface ISMRData
    {
        DataDefault.SMRState SMRState { get; set; }

        TreeNode Node { get; set; }
        ListViewItem ListViewItem { get; set; }

        string FullPathToSMRData { get; set; }
        string Name { get; set; }

        int IndexImg { get; set; }
    }
}
