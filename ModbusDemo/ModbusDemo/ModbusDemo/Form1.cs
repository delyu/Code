using System;
using System.Windows.Forms;
using HslCommunication.ModBus;
using HslCommunication;

namespace ModbusDemo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 初始化类
        /// </summary>
        private ModbusTcpNet busTcpClient =null;
        /// <summary>
        /// 监听状态
        /// </summary>
        private bool IsEnable = false;
        private void Form1_Load(object sender, EventArgs e)
        {
            txtPort.Text = "502";
            txtIp.Text = "172.30.16.220";
            textBox2.Text = "00141";
        }
        /// <summary>
        /// 开启服务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button5_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsEnable)
                {
                    MessageBox.Show("请勿重复建立连接!");
                    return;
                }
                string ip = txtIp.Text.Trim();
                int port = Convert.ToInt32(txtPort.Text);
                if (ip==null || ip=="")
                {
                    MessageBox.Show("ip不能为空!");
                    return;
                }
                busTcpClient = new ModbusTcpNet(ip, port, 0x01);
                OperateResult res = busTcpClient.ConnectServer();
                if (res.IsSuccess==true) //接收状态返回值
                {
                    IsEnable = true;
                    MessageBox.Show("开启连接成功");
                }
                else
                {
                    MessageBox.Show("开启连接失败");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("开启连接失败!", ex.Message.ToString());
            }
        }        
        private void Button6_Click(object sender, EventArgs e)
        {
            try
            {
                if (!IsEnable)
                {
                    MessageBox.Show("尚未建立连接!");
                    return;
                }
                busTcpClient.ConnectClose();
                IsEnable = false;
                MessageBox.Show("关闭连接成功！");
            }
            catch (Exception ex)
            {
                MessageBox.Show("关闭连接失败!", ex.Message.ToString());
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (!IsEnable)
                {
                    MessageBox.Show("尚未建立连接!");
                    return;
                }
                if (busTcpClient == null)
                {
                    MessageBox.Show("尚未初始化对象!");
                    return;
                }
                string txt = textBox2.Text.Trim();
                if (txt=="")
                {
                    MessageBox.Show("地址不能为空!");
                    return;
                }
                bool coil100 = busTcpClient.ReadCoil(txt).Content;   // 读取线圈100的通断
                textBox1.Text = "";
                MessageBox.Show("监听成功!");
                textBox1.Text = coil100 == true ? "true" : "false";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
    }
}