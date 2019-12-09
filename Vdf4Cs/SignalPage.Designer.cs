namespace Vdf4Cs
{
    partial class SignalPage
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
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxStartBit = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxBitLen = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBoxByteOrder = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.comboBoxValDesc = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(45, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "名称：";
            // 
            // textBoxName
            // 
            this.textBoxName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxName.Location = new System.Drawing.Point(103, 22);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(465, 25);
            this.textBoxName.TabIndex = 1;
            this.textBoxName.TextChanged += new System.EventHandler(this.textBoxName_TextChanged);
            this.textBoxName.Validating += new System.ComponentModel.CancelEventHandler(this.textBoxName_Validating);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(30, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "起始位：";
            // 
            // textBoxStartBit
            // 
            this.textBoxStartBit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxStartBit.Location = new System.Drawing.Point(103, 74);
            this.textBoxStartBit.Name = "textBoxStartBit";
            this.textBoxStartBit.Size = new System.Drawing.Size(465, 25);
            this.textBoxStartBit.TabIndex = 3;
            this.textBoxStartBit.Validating += new System.ComponentModel.CancelEventHandler(this.textBoxStartBit_Validating);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(30, 129);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 15);
            this.label3.TabIndex = 4;
            this.label3.Text = "位长度：";
            // 
            // textBoxBitLen
            // 
            this.textBoxBitLen.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxBitLen.Location = new System.Drawing.Point(103, 126);
            this.textBoxBitLen.Name = "textBoxBitLen";
            this.textBoxBitLen.Size = new System.Drawing.Size(465, 25);
            this.textBoxBitLen.TabIndex = 5;
            this.textBoxBitLen.Validating += new System.ComponentModel.CancelEventHandler(this.textBoxBitLen_Validating);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(30, 185);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 15);
            this.label4.TabIndex = 6;
            this.label4.Text = "字节序：";
            // 
            // comboBoxByteOrder
            // 
            this.comboBoxByteOrder.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxByteOrder.FormattingEnabled = true;
            this.comboBoxByteOrder.Location = new System.Drawing.Point(103, 182);
            this.comboBoxByteOrder.Name = "comboBoxByteOrder";
            this.comboBoxByteOrder.Size = new System.Drawing.Size(122, 23);
            this.comboBoxByteOrder.TabIndex = 7;
            this.comboBoxByteOrder.SelectedIndexChanged += new System.EventHandler(this.comboBoxByteOrder_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(30, 238);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(67, 15);
            this.label5.TabIndex = 8;
            this.label5.Text = "值描述：";
            // 
            // comboBoxValDesc
            // 
            this.comboBoxValDesc.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxValDesc.FormattingEnabled = true;
            this.comboBoxValDesc.Location = new System.Drawing.Point(103, 235);
            this.comboBoxValDesc.Name = "comboBoxValDesc";
            this.comboBoxValDesc.Size = new System.Drawing.Size(465, 23);
            this.comboBoxValDesc.TabIndex = 9;
            this.comboBoxValDesc.SelectedIndexChanged += new System.EventHandler(this.comboBoxValDesc_SelectedIndexChanged);
            // 
            // SignalPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.Controls.Add(this.comboBoxValDesc);
            this.Controls.Add(this.comboBoxByteOrder);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBoxBitLen);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBoxStartBit);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxName);
            this.Controls.Add(this.label1);
            this.Name = "SignalPage";
            this.Size = new System.Drawing.Size(616, 454);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxStartBit;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxBitLen;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboBoxByteOrder;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox comboBoxValDesc;
    }
}
