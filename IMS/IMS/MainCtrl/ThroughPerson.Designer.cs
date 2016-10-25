namespace IMS.MainCtrl
{
    partial class ThroughPerson
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
            this.lbName = new DevComponents.DotNetBar.LabelX();
            this.pbPhoto = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbPhoto)).BeginInit();
            this.SuspendLayout();
            // 
            // lbName
            // 
            // 
            // 
            // 
            this.lbName.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lbName.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbName.Location = new System.Drawing.Point(14, 18);
            this.lbName.Name = "lbName";
            this.lbName.Size = new System.Drawing.Size(38, 23);
            this.lbName.TabIndex = 1;
            this.lbName.Text = "XX";
            // 
            // pbPhoto
            // 
            this.pbPhoto.BackgroundImage = global::IMS.Properties.Resources.zanwutupian;
            this.pbPhoto.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pbPhoto.Location = new System.Drawing.Point(67, 0);
            this.pbPhoto.Name = "pbPhoto";
            this.pbPhoto.Size = new System.Drawing.Size(69, 54);
            this.pbPhoto.TabIndex = 2;
            this.pbPhoto.TabStop = false;
            // 
            // ThroughPerson
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pbPhoto);
            this.Controls.Add(this.lbName);
            this.Name = "ThroughPerson";
            this.Size = new System.Drawing.Size(144, 57);
            ((System.ComponentModel.ISupportInitialize)(this.pbPhoto)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.LabelX lbName;
        private System.Windows.Forms.PictureBox pbPhoto;
    }
}
