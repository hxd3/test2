﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CentralControl
{
    public partial class MultiTunnelDeviceForm : Form
    {
        public DeviceInfoForm FatherForm;
        public MultiTunnelVirtualDevice DeviceInfo;

        private DataTable dt;

        public MultiTunnelDeviceForm()
        {
            InitializeComponent();
        }

        private void MultiTunnelDeviceForm_Load(object sender, EventArgs e)
        {
            FatherForm.Enabled = false;
            jianCeMoShiComboBox.SelectedIndex = 0;
            dt = new DataTable();
            DataColumn dc;
            for (int i = 1; i <= MultiTunnelVirtualDevice.JianCeLieShu; i++)
            {
                dc = new DataColumn(i.ToString());
                dt.Columns.Add(dc);
            }
            DataRow dr;
            for (int i = 1; i <= MultiTunnelVirtualDevice.JianCeHangShu; i++)
            {
                dr = dt.NewRow();
                for (int j = 0; j < MultiTunnelVirtualDevice.JianCeLieShu; j++)
                {
                    dr[i] = "";
                }
                dt.Rows.Add(dr);
            }
            dataGridView.DataSource = dt;
            foreach (DataGridViewColumn dgc in dataGridView.Columns)
            {
                dgc.Width = 44;
                dgc.Resizable = DataGridViewTriState.False;
            }
            timer1.Start();
        }

        private void MultiTunnelDeviceForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            FatherForm.Enabled = true;
        }

        private void runButton_Click(object sender, EventArgs e)
        {
            int index = jianCeMoShiComboBox.SelectedIndex;
            if (index == 0) DeviceInfo.MoShi = MultiTunnelVirtualDevice.JianCeMoShi.OD;
            if (index == 1) DeviceInfo.MoShi = MultiTunnelVirtualDevice.JianCeMoShi.YingGuang;
            if (index == 2) DeviceInfo.MoShi = MultiTunnelVirtualDevice.JianCeMoShi.HuaXueFaGuang;
            DeviceInfo.send_moshi();
            DeviceInfo.send_cmd("Start");
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            float[][] v = DeviceInfo.getDetectValues();
            String TiaoMaHao = DeviceInfo.getTiaoMaHao();
            if (v == null || TiaoMaHao == null) 
            {
                timer1.Start();
                return;
            }
            DataRow dr;
            for (int i = 0; i < MultiTunnelVirtualDevice.JianCeHangShu; i++)
            {
                dr = dt.Rows[i];
                for (int j = 0; j < MultiTunnelVirtualDevice.JianCeLieShu; j++) 
                {
                    dr[j] = v[i][j];
                }
            }
            dataGridView.DataSource = dt;
            dataGridView.Update();
            timer1.Start();
        }
    }
}
