using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace SNAMP
{
    public class DataInterface : ICloneable, IDataSerialize
    {
        public int arhViewPanelsSD;
        public int viewPropertPanelsSD;
        public int viewManagerPanelsSD;

        public List<string> treeNodesExpanded;
        public List<string> tabPagesSMROpened;
        public string pathActived;

        public Size windowSize;
        public Point windowLocation;
        public FormWindowState windowState;

        public object Clone() => new DataInterface { 
            treeNodesExpanded = new List<string>(treeNodesExpanded), 
            tabPagesSMROpened = new List<string>(tabPagesSMROpened) 
        };

        public void PrepareData() { }

        public void SetDataByDefalut()
        {
            windowSize = DataDefault.windowSize;
            windowState = FormWindowState.Maximized;
            windowLocation = new Point();
            treeNodesExpanded = new List<string>();
            tabPagesSMROpened = new List<string>();
            pathActived = string.Empty;
            arhViewPanelsSD = DataDefault.ARH_VIEW_PANELS_SD;
            viewPropertPanelsSD = DataDefault.VIEW_PROPERTY_PANELS_SD;
            viewManagerPanelsSD = DataDefault.VIEW_MANAGER_PANELS_SD;
        }
    }
}
