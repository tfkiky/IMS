namespace IMS.MainCtrl
{
    partial class FaceRecord
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FaceRecord));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.lbValidate = new DevComponents.DotNetBar.LabelX();
            this.lbIsAllow = new DevComponents.DotNetBar.LabelX();
            this.labelX6 = new DevComponents.DotNetBar.LabelX();
            this.labelX8 = new DevComponents.DotNetBar.LabelX();
            this.lbForward = new DevComponents.DotNetBar.LabelX();
            this.labelX12 = new DevComponents.DotNetBar.LabelX();
            this.lbValue = new DevComponents.DotNetBar.LabelX();
            this.lbSwipe = new DevComponents.DotNetBar.LabelX();
            this.labelX7 = new DevComponents.DotNetBar.LabelX();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.lbTime = new DevComponents.DotNetBar.LabelX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.pbCaptrue = new System.Windows.Forms.PictureBox();
            this.peopleIDCard1 = new IMS.PeopleIDCard();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbCaptrue)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.lbValidate);
            this.splitContainer1.Panel1.Controls.Add(this.lbIsAllow);
            this.splitContainer1.Panel1.Controls.Add(this.labelX6);
            this.splitContainer1.Panel1.Controls.Add(this.labelX8);
            this.splitContainer1.Panel1.Controls.Add(this.lbForward);
            this.splitContainer1.Panel1.Controls.Add(this.labelX12);
            this.splitContainer1.Panel1.Controls.Add(this.lbValue);
            this.splitContainer1.Panel1.Controls.Add(this.lbSwipe);
            this.splitContainer1.Panel1.Controls.Add(this.labelX7);
            this.splitContainer1.Panel1.Controls.Add(this.labelX3);
            this.splitContainer1.Panel1.Controls.Add(this.lbTime);
            this.splitContainer1.Panel1.Controls.Add(this.labelX1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(1039, 481);
            this.splitContainer1.SplitterDistance = 150;
            this.splitContainer1.SplitterWidth = 1;
            this.splitContainer1.TabIndex = 0;
            // 
            // lbValidate
            // 
            // 
            // 
            // 
            this.lbValidate.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lbValidate.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbValidate.Location = new System.Drawing.Point(768, 82);
            this.lbValidate.Name = "lbValidate";
            this.lbValidate.Size = new System.Drawing.Size(99, 23);
            this.lbValidate.TabIndex = 1;
            this.lbValidate.Text = "60";
            // 
            // lbIsAllow
            // 
            // 
            // 
            // 
            this.lbIsAllow.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lbIsAllow.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbIsAllow.Location = new System.Drawing.Point(499, 82);
            this.lbIsAllow.Name = "lbIsAllow";
            this.lbIsAllow.Size = new System.Drawing.Size(102, 23);
            this.lbIsAllow.TabIndex = 2;
            this.lbIsAllow.Text = "门禁刷卡";
            // 
            // labelX6
            // 
            // 
            // 
            // 
            this.labelX6.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX6.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelX6.Location = new System.Drawing.Point(672, 82);
            this.labelX6.Name = "labelX6";
            this.labelX6.Size = new System.Drawing.Size(99, 23);
            this.labelX6.TabIndex = 3;
            this.labelX6.Text = "验证结果：";
            // 
            // labelX8
            // 
            // 
            // 
            // 
            this.labelX8.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX8.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelX8.Location = new System.Drawing.Point(403, 82);
            this.labelX8.Name = "labelX8";
            this.labelX8.Size = new System.Drawing.Size(102, 23);
            this.labelX8.TabIndex = 4;
            this.labelX8.Text = "通行结果：";
            // 
            // lbForward
            // 
            // 
            // 
            // 
            this.lbForward.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lbForward.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbForward.Location = new System.Drawing.Point(154, 82);
            this.lbForward.Name = "lbForward";
            this.lbForward.Size = new System.Drawing.Size(167, 23);
            this.lbForward.TabIndex = 6;
            this.lbForward.Text = "XXXX";
            // 
            // labelX12
            // 
            // 
            // 
            // 
            this.labelX12.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX12.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelX12.Location = new System.Drawing.Point(58, 82);
            this.labelX12.Name = "labelX12";
            this.labelX12.Size = new System.Drawing.Size(90, 23);
            this.labelX12.TabIndex = 8;
            this.labelX12.Text = "通行方向：";
            // 
            // lbValue
            // 
            // 
            // 
            // 
            this.lbValue.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lbValue.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbValue.Location = new System.Drawing.Point(768, 31);
            this.lbValue.Name = "lbValue";
            this.lbValue.Size = new System.Drawing.Size(99, 23);
            this.lbValue.TabIndex = 0;
            this.lbValue.Text = "60";
            // 
            // lbSwipe
            // 
            // 
            // 
            // 
            this.lbSwipe.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lbSwipe.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbSwipe.Location = new System.Drawing.Point(499, 31);
            this.lbSwipe.Name = "lbSwipe";
            this.lbSwipe.Size = new System.Drawing.Size(102, 23);
            this.lbSwipe.TabIndex = 0;
            this.lbSwipe.Text = "门禁刷卡";
            // 
            // labelX7
            // 
            // 
            // 
            // 
            this.labelX7.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX7.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelX7.Location = new System.Drawing.Point(672, 31);
            this.labelX7.Name = "labelX7";
            this.labelX7.Size = new System.Drawing.Size(99, 23);
            this.labelX7.TabIndex = 0;
            this.labelX7.Text = "相似度：";
            // 
            // labelX3
            // 
            // 
            // 
            // 
            this.labelX3.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX3.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelX3.Location = new System.Drawing.Point(403, 31);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(102, 23);
            this.labelX3.TabIndex = 0;
            this.labelX3.Text = "刷卡方式：";
            // 
            // lbTime
            // 
            // 
            // 
            // 
            this.lbTime.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lbTime.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbTime.Location = new System.Drawing.Point(154, 31);
            this.lbTime.Name = "lbTime";
            this.lbTime.Size = new System.Drawing.Size(193, 23);
            this.lbTime.TabIndex = 0;
            this.lbTime.Text = "XXXX";
            // 
            // labelX1
            // 
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelX1.Location = new System.Drawing.Point(58, 31);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(90, 23);
            this.labelX1.TabIndex = 0;
            this.labelX1.Text = "通行时间：";
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.pbCaptrue);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.peopleIDCard1);
            this.splitContainer2.Size = new System.Drawing.Size(1039, 330);
            this.splitContainer2.SplitterDistance = 518;
            this.splitContainer2.SplitterWidth = 1;
            this.splitContainer2.TabIndex = 1;
            // 
            // pbCaptrue
            // 
            this.pbCaptrue.BackgroundImage = global::IMS.Properties.Resources.zanwutupian;
            this.pbCaptrue.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pbCaptrue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbCaptrue.Location = new System.Drawing.Point(0, 0);
            this.pbCaptrue.Name = "pbCaptrue";
            this.pbCaptrue.Size = new System.Drawing.Size(518, 330);
            this.pbCaptrue.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbCaptrue.TabIndex = 0;
            this.pbCaptrue.TabStop = false;
            // 
            // peopleIDCard1
            // 
            this.peopleIDCard1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("peopleIDCard1.BackgroundImage")));
            this.peopleIDCard1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.peopleIDCard1.Location = new System.Drawing.Point(5, 3);
            this.peopleIDCard1.Name = "peopleIDCard1";
            this.peopleIDCard1.Size = new System.Drawing.Size(513, 324);
            this.peopleIDCard1.TabIndex = 0;
            // 
            // FaceRecord
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1039, 481);
            this.Controls.Add(this.splitContainer1);
            this.DoubleBuffered = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FaceRecord";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "通行记录";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbCaptrue)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private DevComponents.DotNetBar.LabelX lbSwipe;
        private DevComponents.DotNetBar.LabelX labelX3;
        private DevComponents.DotNetBar.LabelX lbTime;
        private DevComponents.DotNetBar.LabelX labelX1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.PictureBox pbCaptrue;
        private DevComponents.DotNetBar.LabelX lbValue;
        private DevComponents.DotNetBar.LabelX labelX7;
        private DevComponents.DotNetBar.LabelX lbValidate;
        private DevComponents.DotNetBar.LabelX lbIsAllow;
        private DevComponents.DotNetBar.LabelX labelX6;
        private DevComponents.DotNetBar.LabelX labelX8;
        private DevComponents.DotNetBar.LabelX lbForward;
        private DevComponents.DotNetBar.LabelX labelX12;
        private PeopleIDCard peopleIDCard1;

    }
}