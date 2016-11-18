namespace IMS.Config
{
    partial class DBConfig
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
            this.btnSave = new DevComponents.DotNetBar.ButtonX();
            this.btnTestDataBase = new DevComponents.DotNetBar.ButtonX();
            this.lbMsg = new DevComponents.DotNetBar.LabelX();
            this.labelX5 = new DevComponents.DotNetBar.LabelX();
            this.cbVerType = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.comboItem1 = new DevComponents.Editors.ComboItem();
            this.comboItem2 = new DevComponents.Editors.ComboItem();
            this.labelX4 = new DevComponents.DotNetBar.LabelX();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.tbPwd = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.tbDatabase = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.tbUser = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.tbServerName = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSave.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnSave.Location = new System.Drawing.Point(364, 286);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(91, 32);
            this.btnSave.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnSave.TabIndex = 21;
            this.btnSave.Text = "保存";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnTestDataBase
            // 
            this.btnTestDataBase.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnTestDataBase.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnTestDataBase.Location = new System.Drawing.Point(267, 286);
            this.btnTestDataBase.Name = "btnTestDataBase";
            this.btnTestDataBase.Size = new System.Drawing.Size(91, 32);
            this.btnTestDataBase.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnTestDataBase.TabIndex = 19;
            this.btnTestDataBase.Text = "测试";
            this.btnTestDataBase.Click += new System.EventHandler(this.btnTestDataBase_Click);
            // 
            // lbMsg
            // 
            // 
            // 
            // 
            this.lbMsg.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lbMsg.ForeColor = System.Drawing.Color.Red;
            this.lbMsg.Location = new System.Drawing.Point(73, 240);
            this.lbMsg.Name = "lbMsg";
            this.lbMsg.Size = new System.Drawing.Size(399, 40);
            this.lbMsg.TabIndex = 16;
            this.lbMsg.WordWrap = true;
            // 
            // labelX5
            // 
            // 
            // 
            // 
            this.labelX5.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX5.Location = new System.Drawing.Point(111, 202);
            this.labelX5.Name = "labelX5";
            this.labelX5.Size = new System.Drawing.Size(75, 23);
            this.labelX5.TabIndex = 17;
            this.labelX5.Text = "数  据  库";
            // 
            // cbVerType
            // 
            this.cbVerType.DisplayMember = "Text";
            this.cbVerType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbVerType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbVerType.FormattingEnabled = true;
            this.cbVerType.ItemHeight = 17;
            this.cbVerType.Items.AddRange(new object[] {
            this.comboItem1,
            this.comboItem2});
            this.cbVerType.Location = new System.Drawing.Point(192, 79);
            this.cbVerType.Name = "cbVerType";
            this.cbVerType.Size = new System.Drawing.Size(227, 23);
            this.cbVerType.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cbVerType.TabIndex = 15;
            this.cbVerType.SelectedIndexChanged += new System.EventHandler(this.cbVerType_SelectedIndexChanged);
            // 
            // comboItem1
            // 
            this.comboItem1.Text = "Windows身份验证";
            // 
            // comboItem2
            // 
            this.comboItem2.Text = "用户名密码验证";
            // 
            // labelX4
            // 
            // 
            // 
            // 
            this.labelX4.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX4.Location = new System.Drawing.Point(111, 161);
            this.labelX4.Name = "labelX4";
            this.labelX4.Size = new System.Drawing.Size(75, 23);
            this.labelX4.TabIndex = 12;
            this.labelX4.Text = "密  码";
            // 
            // labelX3
            // 
            // 
            // 
            // 
            this.labelX3.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX3.Location = new System.Drawing.Point(111, 120);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(75, 23);
            this.labelX3.TabIndex = 13;
            this.labelX3.Text = "用  户  名";
            // 
            // labelX2
            // 
            // 
            // 
            // 
            this.labelX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX2.Location = new System.Drawing.Point(111, 79);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(75, 23);
            this.labelX2.TabIndex = 14;
            this.labelX2.Text = "身 份 验 证";
            // 
            // tbPwd
            // 
            // 
            // 
            // 
            this.tbPwd.Border.Class = "TextBoxBorder";
            this.tbPwd.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.tbPwd.Location = new System.Drawing.Point(192, 159);
            this.tbPwd.Name = "tbPwd";
            this.tbPwd.PasswordChar = '*';
            this.tbPwd.Size = new System.Drawing.Size(227, 23);
            this.tbPwd.TabIndex = 8;
            this.tbPwd.Text = "123456";
            // 
            // tbDatabase
            // 
            // 
            // 
            // 
            this.tbDatabase.Border.Class = "TextBoxBorder";
            this.tbDatabase.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.tbDatabase.Location = new System.Drawing.Point(192, 199);
            this.tbDatabase.Name = "tbDatabase";
            this.tbDatabase.ReadOnly = true;
            this.tbDatabase.Size = new System.Drawing.Size(227, 21);
            this.tbDatabase.TabIndex = 9;
            this.tbDatabase.Text = "SmartAccess";
            // 
            // tbUser
            // 
            // 
            // 
            // 
            this.tbUser.Border.Class = "TextBoxBorder";
            this.tbUser.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.tbUser.Location = new System.Drawing.Point(192, 119);
            this.tbUser.Name = "tbUser";
            this.tbUser.Size = new System.Drawing.Size(227, 21);
            this.tbUser.TabIndex = 10;
            this.tbUser.Text = "sa";
            // 
            // tbServerName
            // 
            // 
            // 
            // 
            this.tbServerName.Border.Class = "TextBoxBorder";
            this.tbServerName.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.tbServerName.Location = new System.Drawing.Point(192, 39);
            this.tbServerName.Name = "tbServerName";
            this.tbServerName.Size = new System.Drawing.Size(227, 21);
            this.tbServerName.TabIndex = 11;
            this.tbServerName.Text = "(local)";
            // 
            // labelX1
            // 
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Location = new System.Drawing.Point(111, 38);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(75, 23);
            this.labelX1.TabIndex = 7;
            this.labelX1.Text = "服务器地址";
            // 
            // DBConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnTestDataBase);
            this.Controls.Add(this.lbMsg);
            this.Controls.Add(this.labelX5);
            this.Controls.Add(this.cbVerType);
            this.Controls.Add(this.labelX4);
            this.Controls.Add(this.labelX3);
            this.Controls.Add(this.labelX2);
            this.Controls.Add(this.tbPwd);
            this.Controls.Add(this.tbDatabase);
            this.Controls.Add(this.tbUser);
            this.Controls.Add(this.tbServerName);
            this.Controls.Add(this.labelX1);
            this.Name = "DBConfig";
            this.Size = new System.Drawing.Size(539, 342);
            this.Load += new System.EventHandler(this.DBConfig_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.ButtonX btnSave;
        private DevComponents.DotNetBar.ButtonX btnTestDataBase;
        private DevComponents.DotNetBar.LabelX lbMsg;
        private DevComponents.DotNetBar.LabelX labelX5;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cbVerType;
        private DevComponents.Editors.ComboItem comboItem1;
        private DevComponents.Editors.ComboItem comboItem2;
        private DevComponents.DotNetBar.LabelX labelX4;
        private DevComponents.DotNetBar.LabelX labelX3;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.Controls.TextBoxX tbPwd;
        private DevComponents.DotNetBar.Controls.TextBoxX tbDatabase;
        private DevComponents.DotNetBar.Controls.TextBoxX tbUser;
        private DevComponents.DotNetBar.Controls.TextBoxX tbServerName;
        private DevComponents.DotNetBar.LabelX labelX1;
    }
}
