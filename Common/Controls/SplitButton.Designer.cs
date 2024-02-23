
namespace Common.Controls
{
    partial class SplitButton
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
            this.tlpSplitButton = new System.Windows.Forms.TableLayoutPanel();
            this.btnLeft = new System.Windows.Forms.Button();
            this.btnRight = new System.Windows.Forms.Button();
            this.tlpSplitButton.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpSplitButton
            // 
            this.tlpSplitButton.AutoSize = true;
            this.tlpSplitButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tlpSplitButton.ColumnCount = 4;
            this.tlpSplitButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpSplitButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 2F));
            this.tlpSplitButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpSplitButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 2F));
            this.tlpSplitButton.Controls.Add(this.btnLeft, 0, 1);
            this.tlpSplitButton.Controls.Add(this.btnRight, 2, 2);
            this.tlpSplitButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpSplitButton.Location = new System.Drawing.Point(0, 0);
            this.tlpSplitButton.Margin = new System.Windows.Forms.Padding(0);
            this.tlpSplitButton.MinimumSize = new System.Drawing.Size(30, 31);
            this.tlpSplitButton.Name = "tlpSplitButton";
            this.tlpSplitButton.RowCount = 5;
            this.tlpSplitButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 2F));
            this.tlpSplitButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpSplitButton.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpSplitButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpSplitButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 2F));
            this.tlpSplitButton.Size = new System.Drawing.Size(141, 31);
            this.tlpSplitButton.TabIndex = 0;
            // 
            // btnLeft
            // 
            this.btnLeft.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnLeft.FlatAppearance.BorderSize = 0;
            this.btnLeft.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.ControlDark;
            this.btnLeft.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ControlLight;
            this.btnLeft.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLeft.Location = new System.Drawing.Point(0, 2);
            this.btnLeft.Margin = new System.Windows.Forms.Padding(0);
            this.btnLeft.MinimumSize = new System.Drawing.Size(0, 31);
            this.btnLeft.Name = "btnLeft";
            this.btnLeft.Padding = new System.Windows.Forms.Padding(19, 0, 0, 0);
            this.tlpSplitButton.SetRowSpan(this.btnLeft, 3);
            this.btnLeft.Size = new System.Drawing.Size(110, 31);
            this.btnLeft.TabIndex = 0;
            this.btnLeft.Text = "SplitButton";
            this.btnLeft.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLeft.UseVisualStyleBackColor = true;
            // 
            // btnRight
            // 
            this.btnRight.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnRight.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnRight.FlatAppearance.BorderSize = 0;
            this.btnRight.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.ControlDark;
            this.btnRight.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ControlLight;
            this.btnRight.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRight.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRight.Location = new System.Drawing.Point(112, 0);
            this.btnRight.Margin = new System.Windows.Forms.Padding(0);
            this.btnRight.MaximumSize = new System.Drawing.Size(28, 31);
            this.btnRight.MinimumSize = new System.Drawing.Size(0, 31);
            this.btnRight.Name = "btnRight";
            this.btnRight.Size = new System.Drawing.Size(27, 31);
            this.btnRight.TabIndex = 1;
            this.btnRight.UseVisualStyleBackColor = true;
            // 
            // SplitButton
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.Controls.Add(this.tlpSplitButton);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.MinimumSize = new System.Drawing.Size(30, 31);
            this.Name = "SplitButton";
            this.Size = new System.Drawing.Size(141, 31);
            this.tlpSplitButton.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpSplitButton;
        private System.Windows.Forms.Button btnLeft;
        private System.Windows.Forms.Button btnRight;
    }
}
