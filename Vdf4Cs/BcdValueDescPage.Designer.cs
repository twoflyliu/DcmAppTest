﻿namespace Vdf4Cs
{
    partial class BcdValueDescPage
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBoxFill = new System.Windows.Forms.CheckBox();
            this.checkBoxCanFillAlpha = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxFactor = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxOffset = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxMinValue = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxMaxValue = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxSeperator = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // textBoxName
            // 
            this.textBoxName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxName.Location = new System.Drawing.Point(171, 27);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(546, 25);
            this.textBoxName.TabIndex = 1;
            this.textBoxName.TextChanged += new System.EventHandler(this.textBoxName_TextChanged);
            this.textBoxName.Validating += new System.ComponentModel.CancelEventHandler(this.textBoxName_Validating);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(101, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "名称：";
            // 
            // checkBoxFill
            // 
            this.checkBoxFill.AutoSize = true;
            this.checkBoxFill.Location = new System.Drawing.Point(376, 91);
            this.checkBoxFill.Name = "checkBoxFill";
            this.checkBoxFill.Size = new System.Drawing.Size(59, 19);
            this.checkBoxFill.TabIndex = 3;
            this.checkBoxFill.Text = "填充";
            this.checkBoxFill.UseVisualStyleBackColor = true;
            this.checkBoxFill.Visible = false;
            // 
            // checkBoxCanFillAlpha
            // 
            this.checkBoxCanFillAlpha.AutoSize = true;
            this.checkBoxCanFillAlpha.Location = new System.Drawing.Point(171, 91);
            this.checkBoxCanFillAlpha.Name = "checkBoxCanFillAlpha";
            this.checkBoxCanFillAlpha.Size = new System.Drawing.Size(119, 19);
            this.checkBoxCanFillAlpha.TabIndex = 2;
            this.checkBoxCanFillAlpha.Text = "可以填充字母";
            this.checkBoxCanFillAlpha.UseVisualStyleBackColor = true;
            this.checkBoxCanFillAlpha.Click += new System.EventHandler(this.checkBoxCanFillAlpha_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(26, 140);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(127, 15);
            this.label2.TabIndex = 4;
            this.label2.Text = "精度（正整数）：";
            // 
            // textBoxFactor
            // 
            this.textBoxFactor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxFactor.Location = new System.Drawing.Point(171, 137);
            this.textBoxFactor.Name = "textBoxFactor";
            this.textBoxFactor.Size = new System.Drawing.Size(546, 25);
            this.textBoxFactor.TabIndex = 5;
            this.textBoxFactor.Validating += new System.ComponentModel.CancelEventHandler(this.textBoxFactor_Validating);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 196);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(142, 15);
            this.label3.TabIndex = 6;
            this.label3.Text = "偏移值（正整数）：";
            // 
            // textBoxOffset
            // 
            this.textBoxOffset.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxOffset.Location = new System.Drawing.Point(171, 193);
            this.textBoxOffset.Name = "textBoxOffset";
            this.textBoxOffset.Size = new System.Drawing.Size(546, 25);
            this.textBoxOffset.TabIndex = 7;
            this.textBoxOffset.Validating += new System.ComponentModel.CancelEventHandler(this.textBoxOffset_Validating);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(26, 248);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(142, 15);
            this.label4.TabIndex = 8;
            this.label4.Text = "最小值（正整数）：";
            // 
            // textBoxMinValue
            // 
            this.textBoxMinValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxMinValue.Location = new System.Drawing.Point(171, 245);
            this.textBoxMinValue.Name = "textBoxMinValue";
            this.textBoxMinValue.Size = new System.Drawing.Size(546, 25);
            this.textBoxMinValue.TabIndex = 9;
            this.textBoxMinValue.Validating += new System.ComponentModel.CancelEventHandler(this.textBoxMinValue_Validating);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(11, 304);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(142, 15);
            this.label5.TabIndex = 10;
            this.label5.Text = "最大值（正整数）：";
            // 
            // textBoxMaxValue
            // 
            this.textBoxMaxValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxMaxValue.Location = new System.Drawing.Point(171, 301);
            this.textBoxMaxValue.Name = "textBoxMaxValue";
            this.textBoxMaxValue.Size = new System.Drawing.Size(546, 25);
            this.textBoxMaxValue.TabIndex = 11;
            this.textBoxMaxValue.Validating += new System.ComponentModel.CancelEventHandler(this.textBoxMaxValue_Validating);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(11, 356);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(142, 15);
            this.label6.TabIndex = 12;
            this.label6.Text = "分隔符（正整数）：";
            // 
            // textBoxSeperator
            // 
            this.textBoxSeperator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxSeperator.Location = new System.Drawing.Point(171, 353);
            this.textBoxSeperator.Name = "textBoxSeperator";
            this.textBoxSeperator.Size = new System.Drawing.Size(546, 25);
            this.textBoxSeperator.TabIndex = 13;
            this.textBoxSeperator.Validating += new System.ComponentModel.CancelEventHandler(this.textBoxSeperator_Validating);
            // 
            // BcdValueDescPage
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.Controls.Add(this.checkBoxCanFillAlpha);
            this.Controls.Add(this.checkBoxFill);
            this.Controls.Add(this.textBoxSeperator);
            this.Controls.Add(this.textBoxMaxValue);
            this.Controls.Add(this.textBoxOffset);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBoxMinValue);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBoxFactor);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxName);
            this.Controls.Add(this.label1);
            this.Name = "BcdValueDescPage";
            this.Size = new System.Drawing.Size(741, 415);
            this.Load += new System.EventHandler(this.BcdValueDescPage_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBoxFill;
        private System.Windows.Forms.CheckBox checkBoxCanFillAlpha;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxFactor;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxOffset;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxMinValue;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxMaxValue;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBoxSeperator;
    }
}
