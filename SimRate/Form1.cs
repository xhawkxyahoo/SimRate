using Microsoft.FlightSimulator.SimConnect;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimRate
{

    public partial class Form1 : Form
    {
        SimConnect simconnect = null;
        const int WM_USER_SIMCONNECT = 0x0402;
        private NotifyIcon trayIcon;
        private ContextMenuStrip trayMenu;

        public Form1()
        {
            InitializeComponent();

            // Create context menu
            trayMenu = new ContextMenuStrip();
            //trayMenu.Items.Add("Open", null, Open_Click);
            trayMenu.Items.Add("Exit", null, Exit_Click);

            // Set up the tray icon
            trayIcon = new NotifyIcon();
            trayIcon.Icon = SystemIcons.Information;
            trayIcon.Visible = true;
            trayIcon.Text = "SimRate";
            trayIcon.ContextMenuStrip = trayMenu;

            // Ensure the tray icon reacts to right-clicks
            trayIcon.MouseClick += TrayIcon_MouseClick;
            timer1.Enabled = true;
            //ConfigureSimConnect();
        }

        private void TrayIcon_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                trayMenu.Show(Control.MousePosition); // Show context menu at mouse position
            }
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void ConfigureSimConnect()
        {
            //if (simconnect == null)
            //{
            //    bool connected = false;
                //while (!connected)
                    try
                    {
                        simconnect = new SimConnect("Managed Data Request", this.Handle, WM_USER_SIMCONNECT, null, 0);
                        //connected = true;
                        simconnect.AddToDataDefinition(DEFINITIONS.Struct1, "SIMULATION RATE", null, SIMCONNECT_DATATYPE.FLOAT32, 0.0f, SimConnect.SIMCONNECT_UNUSED);
                        simconnect.RegisterDataDefineStruct<Struct1>(DEFINITIONS.Struct1);
                        simconnect.OnRecvSimobjectDataBytype += new SimConnect.RecvSimobjectDataBytypeEventHandler(simconnect_OnRecvSimobjectDataBytype);
                        simconnect.OnRecvQuit += new SimConnect.RecvQuitEventHandler(simconnect_OnRecvQuit);
                    }
                    catch (COMException ex)
                    {
                        System.Threading.Thread.Sleep(1000);
                    }
            //}
        }

        protected override void DefWndProc(ref Message m)
        {
            if (m.Msg == WM_USER_SIMCONNECT)
            {
                if (simconnect != null)
                {
                    try
                    {
                        simconnect.ReceiveMessage();
                    }
                    catch
                    {
                        StoppListeningAndReconnect();
                    }
                }
            }
            else
            {
                base.DefWndProc(ref m);
            }
        }

        private void simconnect_OnRecvSimobjectDataBytype(SimConnect sender, SIMCONNECT_RECV_SIMOBJECT_DATA_BYTYPE data)
        {

            if (data.dwRequestID == 0)
            {
                Struct1 struct1 = (Struct1)data.dwData[0];
                var simRateTxt = struct1.SimRate.ToString();
                switch (struct1.SimRate)
                {
                    case .5F:
                        simRateTxt = "1/2";
                        break;
                    case .25F:
                        simRateTxt = "1/4";
                        break;
                    case .125F:
                        simRateTxt = "1/8";
                        break;
                    case .0625F:
                        simRateTxt = "1/16";
                        break;
                    default:
                        break;
                }
                if (textBox1.InvokeRequired)
                {
                    textBox1.Invoke(new Action(() => textBox1.Text = simRateTxt + "x"));
                }
                else
                {
                    textBox1.Text = simRateTxt + "x";
                }
                Console.WriteLine(simRateTxt);
            }
        }

        private void Timer1_Tick_1(object sender, EventArgs e)
        {
            if (simconnect != null)
            {
                try
                {
                    simconnect.RequestDataOnSimObjectType(DATA_REQUESTS.REQUEST_1, DEFINITIONS.Struct1, 0, SIMCONNECT_SIMOBJECT_TYPE.USER);
                }
                catch
                {
                    StoppListeningAndReconnect();
                }
            }
            else
            {
                ConfigureSimConnect();
            }
        }

        private enum DATA_REQUESTS
        {
            REQUEST_1
        }

        private void simconnect_OnRecvOpen(SimConnect sender, SIMCONNECT_RECV_OPEN data)
        {
            //timer1.Enabled = true;
        }

        private void simconnect_OnRecvQuit(SimConnect sender, SIMCONNECT_RECV data)
        {
            StoppListeningAndReconnect();
        }

        public void StoppListeningAndReconnect()
        {
            textBox1.Text = "---";
            simconnect.Dispose();
            simconnect = null;
            this.Update();
        }

        private void form1_Shown(object sender, EventArgs e)
        {
            this.Refresh();
            ConfigureSimConnect();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer1.Enabled = false;
            timer1.Dispose();
            if (simconnect != null)
            {
                simconnect.Dispose();
                simconnect = null;
            }
            trayIcon.Dispose();
        }
    }
}
