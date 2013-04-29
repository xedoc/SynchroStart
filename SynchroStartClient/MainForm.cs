using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SuperSocket.SocketBase;
using System.Configuration;
using WebSocket4Net;
using SynchroStartServer;
using System.Threading;
using System.Diagnostics;

namespace SynchroStartClient
{
    public partial class MainForm : Form
    {
        private const int port = 30303;
        //private const string host = "92.60.187.229";
        private const string host = "websocket.xedocproject.com";
        private const string game = "aces";
        private WebSocket client;
        private Properties.Settings settings;

        public MainForm()
        {
            settings = Properties.Settings.Default;
            InitializeComponent();
            statusBar.Text = "Disconnected";
            try
            {
                client = new WebSocket(String.Format("ws://{0}:{1}/", host, port));
                client.Error += new EventHandler<SuperSocket.ClientEngine.ErrorEventArgs>(client_Error);
                client.AllowUnstrustedCertificate = true;
                client.Opened += new EventHandler(client_Opened);
                client.Closed += new EventHandler(client_Closed);
                client.MessageReceived += new EventHandler<MessageReceivedEventArgs>(client_MessageReceived);
                client.Open();
            }
            catch { }
            
        }
        private string FormatTime(int seconds)
        {
            TimeSpan time = TimeSpan.FromSeconds(seconds);
            return String.Format("{0:D2}:{1:D2}", time.Minutes, time.Seconds);
        }
        void client_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            Console.WriteLine("Server message:" + e.Message);
            if (e.Message.Equals("ping"))
            {
                client.Send("pong");
            }
            else if (e.Message.Equals("start"))
            {
                if (!checkReady.Checked)
                    return;

                SetChecked(checkReady, false);

                FocusOnGame();
                Keyboard.PressKey('\n');
                
            }
            else if (e.Message.StartsWith("timer|"))
            {
                if (!checkReady.Checked)
                    return;

                int seconds;
                var strSeconds = int.TryParse( e.Message.Split('|')[1], out seconds);
                SetLabel(labelTimer, FormatTime(seconds));
            }

        }
        void Connect()
        {
            try
            {
                client.Open();
            }
            catch { }
        }
        void client_Closed(object sender, EventArgs e)
        {
            Thread.Sleep(3000);
            statusBar.Text = "Disconnected";
            Connect();
        }

        void client_Opened(object sender, EventArgs e)
        {
            toolStatusText.Text = "Connected!";
            UpdateNick();
        }

        void client_Error(object sender, SuperSocket.ClientEngine.ErrorEventArgs e)
        {
            toolStatusText.Text = "Connecting...";
            Thread.Sleep(3000);
            Connect();
        }

        private void textNick_TextChanged(object sender, EventArgs e)
        {
            UpdateNick();
            
        }
        private void SendReadyState()
        {
            if (checkReady.Checked)
                client.Send("ready");
            else
            {
                client.Send("notready");
                SetLabel(labelTimer, "00:00");
            }
        }
        private void UpdateNick()
        {
            try
            {
                client.Send(String.Format("nick|{0}", textNick.Text));
            }
            catch { }
            settings.Save();
        }
        delegate void SetLabelCallback(Label label, string text);
        public void SetLabel(Label label, string text)
        {
            if (label.InvokeRequired)
            {
                SetLabelCallback d = new SetLabelCallback(SetLabel);
                label.Parent.Invoke(d, new object[] { label, text });
            }
            else
            {
                label.Text = text;
            }
        }

        delegate void SetCheckedCallback(CheckBox checkbox, bool state);
        private void ResetReady()
        {
            SetChecked(checkReady, false);
            SendReadyState();

        }
        public void SetChecked(CheckBox checkbox, bool state)
        {
            if (checkbox.InvokeRequired)
            {
                SetCheckedCallback d = new SetCheckedCallback(SetChecked);
                checkbox.Parent.Invoke(d, new object[] { checkbox, state });
            }
            else
            {
                checkbox.Checked = state;
            }
        }

        
        private bool FocusOnGame()
        {
            var gameProcess = Process.GetProcessesByName(game);
            if (gameProcess.Length == 0)
                return false;

            IntPtr targetHwnd = gameProcess[0].MainWindowHandle;
            WindowsAPI.SetForegroundWindow(targetHwnd);
            WindowsAPI.SetFocus(targetHwnd);

            return true;
        }

        private void checkReady_CheckedChanged(object sender, EventArgs e)
        {
            SendReadyState();
        }
    }
}
