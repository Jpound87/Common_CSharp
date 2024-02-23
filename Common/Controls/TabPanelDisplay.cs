using System;
using System.Windows.Forms;

namespace Common.Controls
{
    /// <summary>
    /// This class contains the split container that hosts the input
    /// and output group panels that themselves host the polymorphic controls.
    /// </summary>
    public class TabPanelDisplay : IValidate
    {
        #region Identity
        public const String ClassName = nameof(TabPanelDisplay);
        #endregion

        #region Accessors
        public bool Valid { get; private set; }
        public FlowLayoutPanel InputControlPanel { get; private set; }
        public FlowLayoutPanel Outputs { get; private set; }
        public SplitContainer HostSplitContainer { get; private set; }
        #endregion /Accessors

        #region Constructor
        public TabPanelDisplay()
        {
            HostSplitContainer = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Visible = false
            };
            // Inputs
            InputControlPanel = new FlowLayoutPanel()
            {
                AutoScroll = true,
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight
            };
            GroupBox inBox = new GroupBox
            {
                AutoSize = true,
                Text = "Read/Write"
            };
            inBox.Controls.Add(InputControlPanel);
            HostSplitContainer.Panel1.Controls.Add(inBox);
            inBox.Dock = DockStyle.Fill;// Set to fill after its 'docked'
            InputControlPanel.Dock = DockStyle.Fill;
            //inPanel.SizeChanged += updateSizeEventHandler;
            // Outputs
            Outputs = new FlowLayoutPanel()
            {
                AutoScroll = true,
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight
            };
            GroupBox outBox = new GroupBox
            {
                AutoSize = true,
                Text = "Read-Only"
            };
            outBox.Controls.Add(Outputs);
            HostSplitContainer.Panel2.Controls.Add(outBox);
            outBox.Dock = DockStyle.Fill;// Set to fill after its 'docked'
            Outputs.Dock = DockStyle.Fill;
            Valid = true;
        }
        #endregion /Contstructor
    }
}
