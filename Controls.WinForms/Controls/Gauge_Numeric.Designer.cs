
namespace Datam.WinForms.Controls
{
    partial class Gauge_Numeric
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
            this.lblMinimumValue = new System.Windows.Forms.Label();
            this.lblUnitDisplay = new System.Windows.Forms.Label();
            this.lblMinimum = new System.Windows.Forms.Label();
            this.lblMaximum = new System.Windows.Forms.Label();
            this.lblMaximumValue = new System.Windows.Forms.Label();
            this.lblAverage = new System.Windows.Forms.Label();
            this.lblAverageValue = new System.Windows.Forms.Label();
            this.bgbGauge.SuspendLayout();
            this.tlpGauge.SuspendLayout();
            this.SuspendLayout();
            // 
            // bgbGauge
            // 
            this.bgbGauge.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
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
            this.bgbGauge.Size = new System.Drawing.Size(519, 170);
            this.bgbGauge.TabIndex = 1;
            this.bgbGauge.TabStop = false;
            this.bgbGauge.Text = "None - Click To Select";
            // 
            // tlpGauge
            // 
            this.tlpGauge.AutoSize = true;
            this.tlpGauge.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tlpGauge.ColumnCount = 6;
            this.tlpGauge.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 4F));
            this.tlpGauge.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpGauge.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpGauge.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpGauge.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpGauge.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 4F));
            this.tlpGauge.Controls.Add(this.txtValueDisplay, 1, 1);
            this.tlpGauge.Controls.Add(this.lblMinimumValue, 4, 3);
            this.tlpGauge.Controls.Add(this.lblUnitDisplay, 2, 1);
            this.tlpGauge.Controls.Add(this.lblMinimum, 3, 3);
            this.tlpGauge.Controls.Add(this.lblMaximum, 3, 1);
            this.tlpGauge.Controls.Add(this.lblMaximumValue, 4, 1);
            this.tlpGauge.Controls.Add(this.lblAverage, 3, 2);
            this.tlpGauge.Controls.Add(this.lblAverageValue, 4, 2);
            this.tlpGauge.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpGauge.Location = new System.Drawing.Point(3, 26);
            this.tlpGauge.Name = "tlpGauge";
            this.tlpGauge.RowCount = 5;
            this.tlpGauge.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 4F));
            this.tlpGauge.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tlpGauge.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tlpGauge.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tlpGauge.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 4F));
            this.tlpGauge.Size = new System.Drawing.Size(513, 141);
            this.tlpGauge.TabIndex = 1;
            // 
            // txtValueDisplay
            // 
            this.txtValueDisplay.BackColor = System.Drawing.Color.Black;
            this.txtValueDisplay.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtValueDisplay.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtValueDisplay.Font = new System.Drawing.Font("Microsoft YaHei UI", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtValueDisplay.ForeColor = System.Drawing.Color.Chartreuse;
            this.txtValueDisplay.Location = new System.Drawing.Point(4, 34);
            this.txtValueDisplay.Margin = new System.Windows.Forms.Padding(0);
            this.txtValueDisplay.Name = "txtValueDisplay";
            this.txtValueDisplay.ReadOnly = true;
            this.tlpGauge.SetRowSpan(this.txtValueDisplay, 3);
            this.txtValueDisplay.Size = new System.Drawing.Size(227, 102);
            this.txtValueDisplay.TabIndex = 4;
            this.txtValueDisplay.TabStop = false;
            this.txtValueDisplay.Text = "X.XXX";
            this.txtValueDisplay.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtValueDisplay.WordWrap = false;
            // 
            // lblMinimumValue
            // 
            this.lblMinimumValue.AutoSize = true;
            this.lblMinimumValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblMinimumValue.Font = new System.Drawing.Font("Microsoft YaHei", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMinimumValue.Location = new System.Drawing.Point(459, 92);
            this.lblMinimumValue.Name = "lblMinimumValue";
            this.lblMinimumValue.Size = new System.Drawing.Size(47, 44);
            this.lblMinimumValue.TabIndex = 2;
            this.lblMinimumValue.Text = "X.XX";
            this.lblMinimumValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblUnitDisplay
            // 
            this.lblUnitDisplay.AutoSize = true;
            this.lblUnitDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblUnitDisplay.Font = new System.Drawing.Font("Microsoft YaHei UI", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUnitDisplay.Location = new System.Drawing.Point(231, 4);
            this.lblUnitDisplay.Margin = new System.Windows.Forms.Padding(0, 0, 0, 9);
            this.lblUnitDisplay.Name = "lblUnitDisplay";
            this.tlpGauge.SetRowSpan(this.lblUnitDisplay, 3);
            this.lblUnitDisplay.Size = new System.Drawing.Size(123, 123);
            this.lblUnitDisplay.TabIndex = 5;
            this.lblUnitDisplay.Text = "Units";
            this.lblUnitDisplay.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // lblMinimum
            // 
            this.lblMinimum.AutoSize = true;
            this.lblMinimum.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblMinimum.Font = new System.Drawing.Font("Microsoft YaHei", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMinimum.Location = new System.Drawing.Point(357, 92);
            this.lblMinimum.Name = "lblMinimum";
            this.lblMinimum.Size = new System.Drawing.Size(96, 44);
            this.lblMinimum.TabIndex = 0;
            this.lblMinimum.Text = "Minimum:";
            this.lblMinimum.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblMaximum
            // 
            this.lblMaximum.AutoSize = true;
            this.lblMaximum.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblMaximum.Font = new System.Drawing.Font("Microsoft YaHei", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMaximum.Location = new System.Drawing.Point(357, 4);
            this.lblMaximum.Name = "lblMaximum";
            this.lblMaximum.Size = new System.Drawing.Size(96, 44);
            this.lblMaximum.TabIndex = 1;
            this.lblMaximum.Text = "Maximum:";
            this.lblMaximum.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblMaximumValue
            // 
            this.lblMaximumValue.AutoSize = true;
            this.lblMaximumValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblMaximumValue.Font = new System.Drawing.Font("Microsoft YaHei", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMaximumValue.Location = new System.Drawing.Point(459, 4);
            this.lblMaximumValue.Name = "lblMaximumValue";
            this.lblMaximumValue.Size = new System.Drawing.Size(47, 44);
            this.lblMaximumValue.TabIndex = 1;
            this.lblMaximumValue.Text = "X.XX";
            this.lblMaximumValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblAverage
            // 
            this.lblAverage.AutoSize = true;
            this.lblAverage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAverage.Font = new System.Drawing.Font("Microsoft YaHei UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAverage.Location = new System.Drawing.Point(357, 48);
            this.lblAverage.Name = "lblAverage";
            this.lblAverage.Size = new System.Drawing.Size(96, 44);
            this.lblAverage.TabIndex = 6;
            this.lblAverage.Text = "Average:";
            this.lblAverage.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblAverageValue
            // 
            this.lblAverageValue.AutoSize = true;
            this.lblAverageValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAverageValue.Font = new System.Drawing.Font("Microsoft YaHei UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAverageValue.Location = new System.Drawing.Point(459, 48);
            this.lblAverageValue.Name = "lblAverageValue";
            this.lblAverageValue.Size = new System.Drawing.Size(47, 44);
            this.lblAverageValue.TabIndex = 7;
            this.lblAverageValue.Text = "X.XX";
            this.lblAverageValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Gauge_Numeric
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.Controls.Add(this.bgbGauge);
            this.MaximumSize = new System.Drawing.Size(0, 170);
            this.MinimumSize = new System.Drawing.Size(0, 170);
            this.Name = "Gauge_Numeric";
            this.Size = new System.Drawing.Size(519, 170);
            this.bgbGauge.ResumeLayout(false);
            this.bgbGauge.PerformLayout();
            this.tlpGauge.ResumeLayout(false);
            this.tlpGauge.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label lblMinimum;
        private System.Windows.Forms.Label lblMaximumValue;
        private System.Windows.Forms.Label lblMaximum;
        private System.Windows.Forms.Label lblMinimumValue;
        private System.Windows.Forms.TextBox txtValueDisplay;
        private System.Windows.Forms.Label lblUnitDisplay;
        private Common.Controls.BorderedGroupBox bgbGauge;
        private System.Windows.Forms.Label lblAverage;
        private System.Windows.Forms.Label lblAverageValue;
        private System.Windows.Forms.TableLayoutPanel tlpGauge;
    }
}
