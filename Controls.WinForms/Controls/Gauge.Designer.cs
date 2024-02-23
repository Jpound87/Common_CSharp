
namespace Datam.WinForms.Controls
{
    partial class Gauge
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
            this.bgbGauge = new Common.Controls.BorderedGroupBox();
            this.tlpGauge = new System.Windows.Forms.TableLayoutPanel();
            this.txtValueDisplay = new System.Windows.Forms.TextBox();
            this.lblUnitDisplay = new System.Windows.Forms.Label();
            this.bgbGauge.SuspendLayout();
            this.tlpGauge.SuspendLayout();
            this.SuspendLayout();
            // 
            // bgbGauge
            // 
            this.bgbGauge.BorderColor = System.Drawing.Color.Chartreuse;
            this.bgbGauge.BorderRadius = 3;
            this.bgbGauge.BorderWidth = 1;
            this.bgbGauge.Controls.Add(this.tlpGauge);
            this.bgbGauge.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bgbGauge.Font = new System.Drawing.Font("Microsoft YaHei UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bgbGauge.ForeColor = System.Drawing.Color.Chartreuse;
            this.bgbGauge.LabelIndent = 10;
            this.bgbGauge.Location = new System.Drawing.Point(0, 0);
            this.bgbGauge.Name = "bgbGauge";
            this.bgbGauge.Size = new System.Drawing.Size(516, 170);
            this.bgbGauge.TabIndex = 1;
            this.bgbGauge.TabStop = false;
            this.bgbGauge.Text = "None - Click To Select";
            // 
            // tlpGauge
            // 
            this.tlpGauge.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tlpGauge.ColumnCount = 4;
            this.tlpGauge.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 4F));
            this.tlpGauge.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpGauge.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpGauge.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 4F));
            this.tlpGauge.Controls.Add(this.txtValueDisplay, 1, 1);
            this.tlpGauge.Controls.Add(this.lblUnitDisplay, 2, 1);
            this.tlpGauge.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpGauge.ForeColor = System.Drawing.Color.Chartreuse;
            this.tlpGauge.Location = new System.Drawing.Point(3, 26);
            this.tlpGauge.Name = "tlpGauge";
            this.tlpGauge.RowCount = 3;
            this.tlpGauge.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 4F));
            this.tlpGauge.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpGauge.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 4F));
            this.tlpGauge.Size = new System.Drawing.Size(510, 141);
            this.tlpGauge.TabIndex = 0;
            // 
            // txtValueDisplay
            // 
            this.txtValueDisplay.BackColor = System.Drawing.Color.Black;
            this.txtValueDisplay.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtValueDisplay.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtValueDisplay.Font = new System.Drawing.Font("Microsoft YaHei UI", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtValueDisplay.ForeColor = System.Drawing.Color.Chartreuse;
            this.txtValueDisplay.Location = new System.Drawing.Point(4, 35);
            this.txtValueDisplay.Margin = new System.Windows.Forms.Padding(0);
            this.txtValueDisplay.Name = "txtValueDisplay";
            this.txtValueDisplay.ReadOnly = true;
            this.txtValueDisplay.Size = new System.Drawing.Size(379, 102);
            this.txtValueDisplay.TabIndex = 4;
            this.txtValueDisplay.TabStop = false;
            this.txtValueDisplay.Text = "X.XXX";
            this.txtValueDisplay.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtValueDisplay.WordWrap = false;
            // 
            // lblUnitDisplay
            // 
            this.lblUnitDisplay.AutoSize = true;
            this.lblUnitDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblUnitDisplay.Font = new System.Drawing.Font("Microsoft YaHei UI", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUnitDisplay.Location = new System.Drawing.Point(383, 4);
            this.lblUnitDisplay.Margin = new System.Windows.Forms.Padding(0, 0, 0, 9);
            this.lblUnitDisplay.Name = "lblUnitDisplay";
            this.lblUnitDisplay.Size = new System.Drawing.Size(123, 124);
            this.lblUnitDisplay.TabIndex = 5;
            this.lblUnitDisplay.Text = "Units";
            this.lblUnitDisplay.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // Gauge
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.Controls.Add(this.bgbGauge);
            this.MaximumSize = new System.Drawing.Size(0, 170);
            this.MinimumSize = new System.Drawing.Size(0, 170);
            this.Name = "Gauge";
            this.Size = new System.Drawing.Size(516, 170);
            this.bgbGauge.ResumeLayout(false);
            this.tlpGauge.ResumeLayout(false);
            this.tlpGauge.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpGauge;
        private System.Windows.Forms.TextBox txtValueDisplay;
        private Common.Controls.BorderedGroupBox bgbGauge;
        private System.Windows.Forms.Label lblUnitDisplay;
    }
}
