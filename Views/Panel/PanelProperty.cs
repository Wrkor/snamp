using System.IO;
using SNAMP.Models;
using System.Drawing;
using SNAMP.Properties;
using System.Windows.Forms;

namespace SNAMP.Views
{
    public class PanelProperty : Panel
    {
        public Button ButtonDelete { get; private set; }
        public FileInfo File { get; private set; }
        public MLinkLabel LinkLabel { get; private set; }
        public MLinkLabel PageLabel { get; private set; }
        public LinkToFile LinkToFile { get; private set; }

        private readonly ToolTip toolTipLinkLabel;

        public PanelProperty(LinkToFile linkToFile) : base() 
        {
            LinkToFile = linkToFile;
            File = new FileInfo(linkToFile.Link);
            Dock = DockStyle.Fill;
            Size = new Size(280, 70);
            toolTipLinkLabel = new ToolTip();

            InitializeElements();
        }

        public void SetData(LinkToFile linkToFile)
        {
            LinkToFile = linkToFile;
            File = new FileInfo(linkToFile.Link);

            LinkLabel.Text = File.Name;
            LinkLabel.Tag = File.FullName;

            PageLabel.Text = linkToFile.LinkPage.ToString();
            PageLabel.Visible = linkToFile.LinkPage != 0;

            toolTipLinkLabel.SetToolTip(LinkLabel, File.Name);
        }

        private void InitializeElements()
        {
            ButtonDelete = new MButton(new Size(30, 30), AnchorStyles.Right, Resources.minus24, new Point(0, 15)) { Tag = this };

            Panel panelBtn = new Panel { Dock = DockStyle.Right, Size = new Size(30, 60) };
            Panel panePage = new Panel { Dock = DockStyle.Right, Size = new Size(30, 60) };
            Panel panelLink = new Panel { Dock = DockStyle.Fill };
            PageLabel = new MLinkLabel(LinkToFile.LinkPage.ToString()) { Tag = File.FullName, Visible = LinkToFile.LinkPage != 0 };
            LinkLabel = new MLinkLabel(File.Name) { Tag = File.FullName };
            toolTipLinkLabel.SetToolTip(LinkLabel, File.Name);

            panePage.Controls.Add(PageLabel);
            panelBtn.Controls.Add(ButtonDelete);
            panelLink.Controls.Add(LinkLabel);

            Controls.Add(panePage);
            Controls.Add(panelBtn);
            Controls.Add(panelLink);
        }
    }
}
