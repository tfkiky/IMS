namespace IMS
{
    partial class CompareInfo
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
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.labelX4 = new DevComponents.DotNetBar.LabelX();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.pbBlack = new System.Windows.Forms.PictureBox();
            this.pbLocalPhoto = new System.Windows.Forms.PictureBox();
            this.pbRealPhoto = new System.Windows.Forms.PictureBox();
            this.tbPassTime = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.tbIDCard = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.tbDepart = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.tbName = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX9 = new DevComponents.DotNetBar.LabelX();
            this.labelX8 = new DevComponents.DotNetBar.LabelX();
            this.labelX5 = new DevComponents.DotNetBar.LabelX();
            this.labelX13 = new DevComponents.DotNetBar.LabelX();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbBlack)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLocalPhoto)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbRealPhoto)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.ForeColor = System.Drawing.Color.White;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.labelX4);
            this.splitContainer1.Panel1.Controls.Add(this.labelX3);
            this.splitContainer1.Panel1.Controls.Add(this.labelX2);
            this.splitContainer1.Panel1.Controls.Add(this.pbBlack);
            this.splitContainer1.Panel1.Controls.Add(this.pbLocalPhoto);
            this.splitContainer1.Panel1.Controls.Add(this.pbRealPhoto);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tbPassTime);
            this.splitContainer1.Panel2.Controls.Add(this.tbIDCard);
            this.splitContainer1.Panel2.Controls.Add(this.tbDepart);
            this.splitContainer1.Panel2.Controls.Add(this.tbName);
            this.splitContainer1.Panel2.Controls.Add(this.labelX9);
            this.splitContainer1.Panel2.Controls.Add(this.labelX8);
            this.splitContainer1.Panel2.Controls.Add(this.labelX5);
            this.splitContainer1.Panel2.Controls.Add(this.labelX13);
            this.splitContainer1.Size = new System.Drawing.Size(509, 465);
            this.splitContainer1.SplitterDistance = 224;
            this.splitContainer1.SplitterWidth = 1;
            this.splitContainer1.TabIndex = 0;
            // 
            // labelX4
            // 
            // 
            // 
            // 
            this.labelX4.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX4.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelX4.Location = new System.Drawing.Point(382, 3);
            this.labelX4.Name = "labelX4";
            this.labelX4.Size = new System.Drawing.Size(75, 15);
            this.labelX4.TabIndex = 0;
            this.labelX4.Text = "黑名单";
            // 
            // labelX3
            // 
            // 
            // 
            // 
            this.labelX3.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX3.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelX3.Location = new System.Drawing.Point(221, 3);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(75, 15);
            this.labelX3.TabIndex = 0;
            this.labelX3.Text = "库内照片";
            // 
            // labelX2
            // 
            // 
            // 
            // 
            this.labelX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelX2.Location = new System.Drawing.Point(56, 3);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(75, 15);
            this.labelX2.TabIndex = 0;
            this.labelX2.Text = "实时照片";
            // 
            // pbBlack
            // 
            this.pbBlack.BackgroundImage = global::IMS.Properties.Resources.暂无图片;
            this.pbBlack.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pbBlack.Location = new System.Drawing.Point(333, 24);
            this.pbBlack.Name = "pbBlack";
            this.pbBlack.Size = new System.Drawing.Size(150, 198);
            this.pbBlack.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbBlack.TabIndex = 0;
            this.pbBlack.TabStop = false;
            // 
            // pbLocalPhoto
            // 
            this.pbLocalPhoto.BackgroundImage = global::IMS.Properties.Resources.暂无图片;
            this.pbLocalPhoto.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pbLocalPhoto.Location = new System.Drawing.Point(175, 24);
            this.pbLocalPhoto.Name = "pbLocalPhoto";
            this.pbLocalPhoto.Size = new System.Drawing.Size(150, 198);
            this.pbLocalPhoto.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbLocalPhoto.TabIndex = 0;
            this.pbLocalPhoto.TabStop = false;
            // 
            // pbRealPhoto
            // 
            this.pbRealPhoto.BackgroundImage = global::IMS.Properties.Resources.暂无图片;
            this.pbRealPhoto.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pbRealPhoto.Location = new System.Drawing.Point(17, 24);
            this.pbRealPhoto.Name = "pbRealPhoto";
            this.pbRealPhoto.Size = new System.Drawing.Size(150, 198);
            this.pbRealPhoto.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbRealPhoto.TabIndex = 0;
            this.pbRealPhoto.TabStop = false;
            // 
            // tbPassTime
            // 
            // 
            // 
            // 
            this.tbPassTime.Border.Class = "TextBoxBorder";
            this.tbPassTime.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.tbPassTime.Enabled = false;
            this.tbPassTime.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbPassTime.Location = new System.Drawing.Point(142, 168);
            this.tbPassTime.Name = "tbPassTime";
            this.tbPassTime.Size = new System.Drawing.Size(310, 26);
            this.tbPassTime.TabIndex = 35;
            // 
            // tbIDCard
            // 
            // 
            // 
            // 
            this.tbIDCard.Border.Class = "TextBoxBorder";
            this.tbIDCard.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.tbIDCard.Enabled = false;
            this.tbIDCard.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbIDCard.Location = new System.Drawing.Point(142, 125);
            this.tbIDCard.Name = "tbIDCard";
            this.tbIDCard.Size = new System.Drawing.Size(310, 26);
            this.tbIDCard.TabIndex = 34;
            // 
            // tbDepart
            // 
            // 
            // 
            // 
            this.tbDepart.Border.Class = "TextBoxBorder";
            this.tbDepart.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.tbDepart.Enabled = false;
            this.tbDepart.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbDepart.Location = new System.Drawing.Point(142, 82);
            this.tbDepart.Name = "tbDepart";
            this.tbDepart.Size = new System.Drawing.Size(310, 26);
            this.tbDepart.TabIndex = 30;
            // 
            // tbName
            // 
            // 
            // 
            // 
            this.tbName.Border.Class = "TextBoxBorder";
            this.tbName.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.tbName.Enabled = false;
            this.tbName.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbName.Location = new System.Drawing.Point(142, 39);
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(310, 26);
            this.tbName.TabIndex = 29;
            // 
            // labelX9
            // 
            // 
            // 
            // 
            this.labelX9.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX9.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelX9.Location = new System.Drawing.Point(52, 171);
            this.labelX9.Name = "labelX9";
            this.labelX9.Size = new System.Drawing.Size(110, 23);
            this.labelX9.TabIndex = 26;
            this.labelX9.Text = "通行时间：";
            // 
            // labelX8
            // 
            // 
            // 
            // 
            this.labelX8.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX8.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelX8.Location = new System.Drawing.Point(52, 128);
            this.labelX8.Name = "labelX8";
            this.labelX8.Size = new System.Drawing.Size(110, 23);
            this.labelX8.TabIndex = 25;
            this.labelX8.Text = "身份证号：";
            // 
            // labelX5
            // 
            // 
            // 
            // 
            this.labelX5.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX5.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelX5.Location = new System.Drawing.Point(51, 42);
            this.labelX5.Name = "labelX5";
            this.labelX5.Size = new System.Drawing.Size(111, 23);
            this.labelX5.TabIndex = 23;
            this.labelX5.Text = "姓      名：";
            // 
            // labelX13
            // 
            // 
            // 
            // 
            this.labelX13.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX13.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelX13.Location = new System.Drawing.Point(51, 85);
            this.labelX13.Name = "labelX13";
            this.labelX13.Size = new System.Drawing.Size(110, 23);
            this.labelX13.TabIndex = 22;
            this.labelX13.Text = "部      门：";
            // 
            // CompareInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.splitContainer1);
            this.Name = "CompareInfo";
            this.Size = new System.Drawing.Size(509, 465);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbBlack)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLocalPhoto)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbRealPhoto)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private DevComponents.DotNetBar.LabelX labelX4;
        private DevComponents.DotNetBar.LabelX labelX3;
        private DevComponents.DotNetBar.LabelX labelX2;
        private System.Windows.Forms.PictureBox pbBlack;
        private System.Windows.Forms.PictureBox pbLocalPhoto;
        private System.Windows.Forms.PictureBox pbRealPhoto;
        private DevComponents.DotNetBar.Controls.TextBoxX tbPassTime;
        private DevComponents.DotNetBar.Controls.TextBoxX tbIDCard;
        private DevComponents.DotNetBar.Controls.TextBoxX tbDepart;
        private DevComponents.DotNetBar.Controls.TextBoxX tbName;
        private DevComponents.DotNetBar.LabelX labelX9;
        private DevComponents.DotNetBar.LabelX labelX8;
        private DevComponents.DotNetBar.LabelX labelX5;
        private DevComponents.DotNetBar.LabelX labelX13;
    }
}
