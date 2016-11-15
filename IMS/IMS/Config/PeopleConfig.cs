using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.Editors;
using IMS.Collecter;
using IMS.Common.Config;

namespace IMS
{
    public partial class PeopleConfig : UserControl
    {
        private Maticsoft.BLL.SMT_CONTROLLER_INFO ctrlBll = new Maticsoft.BLL.SMT_CONTROLLER_INFO();
        private List<Maticsoft.Model.SMT_CONTROLLER_INFO> ctrlList = new List<Maticsoft.Model.SMT_CONTROLLER_INFO>();
        private Maticsoft.BLL.SMT_DOOR_INFO doorBll = new Maticsoft.BLL.SMT_DOOR_INFO();
        private List<Maticsoft.Model.SMT_DOOR_INFO> doorList = new List<Maticsoft.Model.SMT_DOOR_INFO>();
        private Maticsoft.BLL.IMS_FACE_CAMERA cameraBll = new Maticsoft.BLL.IMS_FACE_CAMERA();
        private List<Maticsoft.Model.IMS_FACE_CAMERA> cameraList = new List<Maticsoft.Model.IMS_FACE_CAMERA>();

        public PeopleConfig()
        {
            InitializeComponent();
            StyleManager.Style = eStyle.Office2007Black;
            LoadComboBox();
        }

        private void LoadComboBox()
        {
            try
            {
                ctrlList = ctrlBll.GetModelList("1=1");
                foreach (Maticsoft.Model.SMT_CONTROLLER_INFO ctrl in ctrlList)
                {
                    ComboItem ci = new ComboItem();
                    ci.Text = ctrl.NAME;
                    ci.Tag = ctrl;
                    cbCtrl.Items.Add(ci);
                    if (ctrl.ID == decimal.Parse(SysConfigClass.GetIMSConfig("IMS_CONFIG", "Controller")))
                    {
                        cbCtrl.SelectedItem = ci;
                    }
                }

                cameraList = cameraBll.GetModelList("1=1");
                foreach (Maticsoft.Model.IMS_FACE_CAMERA cam in cameraList)
                {
                    ipAddressInput1.Value = cameraList[0].CameraIP;
                    tbPort.Text = cameraList[0].CameraPort;
                    tbUserName.Text = cameraList[0].CameraUser;
                    tbPwd.Text = cameraList[0].CameraPwd;
                }
            }
            catch (System.Exception ex)
            {
            	
            }
        }

        private void cbCtrl_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbDoor.Items.Clear();
            doorList = doorBll.GetModelList("CTRL_ID=" + ((cbCtrl.SelectedItem as ComboItem).Tag as Maticsoft.Model.SMT_CONTROLLER_INFO).ID);
            foreach (Maticsoft.Model.SMT_DOOR_INFO door in doorList)
            {
                ComboItem ci = new ComboItem();
                ci.Text = door.DOOR_NAME;
                ci.Tag = door;
                cbDoor.Items.Add(ci);
                if (door.ID == decimal.Parse(SysConfigClass.GetIMSConfig("IMS_CONFIG", "Door")))
                {
                    cbDoor.SelectedItem = ci;
                }
            }
        }


        private void btnEdit_Click(object sender, EventArgs e)
        {
            cbCtrl.Enabled = true;
            cbDoor.Enabled = true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if ((cbCtrl.SelectedItem as ComboItem).Tag is Maticsoft.Model.SMT_CONTROLLER_INFO)
                {
                    Maticsoft.Model.SMT_CONTROLLER_INFO ctrl = (cbCtrl.SelectedItem as ComboItem).Tag as Maticsoft.Model.SMT_CONTROLLER_INFO;
                    SysConfigClass.SetIMSConfig("IMS_CONFIG", "Controller", ctrl.ID.ToString());
                    AccessCollect.Instance.FaceControllerID = (int)ctrl.ID;
                }
                if ((cbDoor.SelectedItem as ComboItem).Tag is Maticsoft.Model.SMT_DOOR_INFO)
                {
                    Maticsoft.Model.SMT_DOOR_INFO door = (cbDoor.SelectedItem as ComboItem).Tag as Maticsoft.Model.SMT_DOOR_INFO;
                    SysConfigClass.SetIMSConfig("IMS_CONFIG", "Door", door.ID.ToString());
                    AccessCollect.Instance.FaceDoorID = (int)door.ID;
                }

                MessageBox.Show("保存成功");
                cbCtrl.Enabled = false;
                cbDoor.Enabled = false;
            }
            catch (Exception ex)
            {

            }

        }

        private void btnTest_Click(object sender, EventArgs e)
        {

        }

        private void btnEdit1_Click(object sender, EventArgs e)
        {
            ipAddressInput1.Enabled = true;
            tbPort.Enabled = true;
            tbUserName.Enabled = true;
            tbPwd.Enabled = true;
        }

        private void btnSave1_Click(object sender, EventArgs e)
        {
            
            Maticsoft.BLL.IMS_FACE_CAMERA cameraBll=new Maticsoft.BLL.IMS_FACE_CAMERA();
            Maticsoft.Model.IMS_FACE_CAMERA camera=cameraBll.GetModel(1);
            camera.CameraIP=ipAddressInput1.Value;
            camera.CameraPort=tbPort.Text;
            camera.CameraUser=tbUserName.Text;
            camera.CameraPwd=tbPwd.Text;
            cameraBll.Update(camera);
            MainForm.Instance.FaceCamera = camera;

            MessageBox.Show("保存成功");
            ipAddressInput1.Enabled = false;
            tbPort.Enabled = false;
            tbUserName.Enabled = false;
            tbPwd.Enabled = false;
        }

        private void btnTest1_Click(object sender, EventArgs e)
        {

        }

        

    }
}
