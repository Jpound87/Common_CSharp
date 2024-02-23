
namespace Common.Controls
{
    partial class ToggleButton
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ToggleButton));
            this.tlpSplitButton = new System.Windows.Forms.TableLayoutPanel();
            this.rdoRight = new System.Windows.Forms.RadioButton();
            this.rdoLeft = new System.Windows.Forms.RadioButton();
            this.tlpSplitButton.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpSplitButton
            // 
            this.tlpSplitButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tlpSplitButton.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.OutsetDouble;
            this.tlpSplitButton.ColumnCount = 2;
            this.tlpSplitButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpSplitButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpSplitButton.Controls.Add(this.rdoRight, 1, 0);
            this.tlpSplitButton.Controls.Add(this.rdoLeft, 0, 0);
            this.tlpSplitButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpSplitButton.Location = new System.Drawing.Point(0, 0);
            this.tlpSplitButton.Margin = new System.Windows.Forms.Padding(0);
            this.tlpSplitButton.MaximumSize = new System.Drawing.Size(320, 160);
            this.tlpSplitButton.MinimumSize = new System.Drawing.Size(80, 40);
            this.tlpSplitButton.Name = "tlpSplitButton";
            this.tlpSplitButton.RowCount = 1;
            this.tlpSplitButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpSplitButton.Size = new System.Drawing.Size(226, 97);
            this.tlpSplitButton.TabIndex = 0;
            // 
            // rdoRight
            // 
            this.rdoRight.Appearance = System.Windows.Forms.Appearance.Button;
            this.rdoRight.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("rdoRight.BackgroundImage")));
            this.rdoRight.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.rdoRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rdoRight.FlatAppearance.BorderSize = 0;
            this.rdoRight.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.rdoRight.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Lime;
            this.rdoRight.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.rdoRight.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rdoRight.Location = new System.Drawing.Point(114, 3);
            this.rdoRight.Margin = new System.Windows.Forms.Padding(0);
            this.rdoRight.Name = "rdoRight";
            this.rdoRight.Padding = new System.Windows.Forms.Padding(6);
            this.rdoRight.Size = new System.Drawing.Size(109, 91);
            this.rdoRight.TabIndex = 1;
            this.rdoRight.UseVisualStyleBackColor = true;
            // 
            // rdoLeft
            // 
            this.rdoLeft.Appearance = System.Windows.Forms.Appearance.Button;
            this.rdoLeft.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("rdoLeft.BackgroundImage")));
            this.rdoLeft.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.rdoLeft.Checked = true;
            this.rdoLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rdoLeft.FlatAppearance.BorderSize = 0;
            this.rdoLeft.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.rdoLeft.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Red;
            this.rdoLeft.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.rdoLeft.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rdoLeft.Location = new System.Drawing.Point(3, 3);
            this.rdoLeft.Margin = new System.Windows.Forms.Padding(0);
            this.rdoLeft.Name = "rdoLeft";
            this.rdoLeft.Padding = new System.Windows.Forms.Padding(6);
            this.rdoLeft.Size = new System.Drawing.Size(108, 91);
            this.rdoLeft.TabIndex = 0;
            this.rdoLeft.TabStop = true;
            this.rdoLeft.UseVisualStyleBackColor = true;
            // 
            // ToggleButton
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.tlpSplitButton);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.MaximumSize = new System.Drawing.Size(320, 160);
            this.MinimumSize = new System.Drawing.Size(80, 40);
            this.Name = "ToggleButton";
            this.Size = new System.Drawing.Size(226, 97);
            this.tlpSplitButton.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpSplitButton;
        private System.Windows.Forms.RadioButton rdoLeft;
        private System.Windows.Forms.RadioButton rdoRight;
    }
}
