using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SuperSocket.SocketBase;
using SuperSocket.SocketEngine;
using System.Configuration;
using WebSocket4Net;
using SuperWebSocket;
using SuperWebSocket.SubProtocol;
using System.Diagnostics;
using SuperSocket.SocketBase.Config;
using System.Threading;


namespace SynchroStartServer
{
    public partial class MainForm : Form
    {
        private JsonWebSocket socket;

        private WebSocketServer server;
        private const int port = 30303;
        private int clientCount = 0;
        private int currentSeconds = 0;
        private bool countDownStarted;
        private BindingSource playersBinding;
        private Properties.Settings settings;
        List<ListElement> sessions;
        public MainForm()
        {
            
            settings = Properties.Settings.Default;
            InitializeComponent();


            sessions = new List<ListElement>();
            playersBinding = new BindingSource();
            playersBinding.DataSource = sessions;
            playersBinding.DataMember = "nickName";

            server = new WebSocketServer();
            server.NewSessionConnected +=new SessionEventHandler<WebSocketSession>(server_NewSessionConnected);
            server.NewMessageReceived +=new SessionEventHandler<WebSocketSession,string>(server_NewMessageReceived);
            server.SessionClosed += new SessionEventHandler<WebSocketSession, SuperSocket.SocketBase.CloseReason>(server_SessionClosed);
            if( !server.Setup(new RootConfig(), new ServerConfig
            {
                Port = port,
                Ip = "Any",
                MaxConnectionNumber = 100,
                Mode = SocketMode.Async,
                Name = "SynchroStart Server"
            }, SocketServerFactory.Instance)
            ||
            !server.Start())
            {
                MessageBox.Show(String.Format("Error starting the server on port #{0}", port));
                return;
            }
            CurrentSeconds = settings.timerSeconds;

        }

        void server_SessionClosed(WebSocketSession session, SuperSocket.SocketBase.CloseReason e)
        {
            ClientCount--;
            if (ClientCount < 0)
                ClientCount = 0;
            sessions.RemoveAll( s => s.Session.Equals(session));
            UpdateList();
        }
        void  server_NewMessageReceived(WebSocketSession session, string e)
        {
            Debug.Print("Client message: " + e);
            if (e.StartsWith("nick|"))
            {
                sessions.ForEach(s => { if (s.Session.SessionID == session.SessionID) s.nickName = String.IsNullOrEmpty(e.Split('|')[1]) ? "UnknownPlayer" : e.Split('|')[1]; });
                UpdateList();
                listPlayers.Invalidate();
            }
            else if (e.StartsWith("ready"))
            {
                sessions.ForEach(s => { if (s.Session.SessionID == session.SessionID) { s.isReady = true; s.Session.SendResponse(String.Format("timer|{0}", CurrentSeconds)); } });
                listPlayers.Invalidate();
            }
            else if (e.StartsWith("notready"))
            {
                sessions.ForEach(s => { if (s.Session.SessionID == session.SessionID) s.isReady = false; });
                listPlayers.Invalidate();
            }
        }

        void  server_NewSessionConnected(WebSocketSession session)
        {
            ClientCount++;
            UpdateList();

            session.SendResponse(String.Format("timer|{0}", CurrentSeconds));
            sessions.Add(new ListElement(session));

        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            server.Stop();
        }

        private void labelTimer_Click(object sender, EventArgs e)
        {
            SwitchTimer();
        }
        private void SwitchTimer()
        {
            if (countDownStarted)
            {
                StopCountDown();
            }
            else
            {
                StartCountDown();
            }
        }
        private void StartCountDown()
        {
            CurrentSeconds = settings.timerSeconds;
            startCountdownToolStripMenuItem.Text = "Stop countdown";
            countDownStarted = true;
            timerCountdown.Start();
        }
        private void StopCountDown(bool resetTimer = true)
        {
            CurrentSeconds = settings.timerSeconds;
            startCountdownToolStripMenuItem.Text = "Start countdown";
            countDownStarted = false;
            timerCountdown.Stop();
        }
        private int CurrentSeconds
        {
            get { return currentSeconds; }
            set
            {
                currentSeconds = value;
                labelTimer.Text = FormatTime(CurrentSeconds);
                sessions.ForEach(s => ThreadPool.QueueUserWorkItem(t => s.Session.SendResponse(String.Format("timer|{0}", CurrentSeconds))));
            }
        }
        private string FormatTime(int seconds)
        {
            TimeSpan time = TimeSpan.FromSeconds(seconds);
            return String.Format("{0:D2}:{1:D2}", time.Minutes, time.Seconds);
        }

        private void timerCountdown_Tick(object sender, EventArgs e)
        {

           
            CurrentSeconds -= 1;

            if (CurrentSeconds == 0)
            {
                TimerAction();
                timerCountdown.Stop();
            }
        }
        private void TimerAction()
        {
            sessions.ForEach(s => s.Session.SendResponse("start"));
            FocusOnGame();
            Keyboard.PressKey('\n');
        }

        private bool FocusOnGame()
        {
            var gameProcess = Process.GetProcessesByName(settings.gameName);
            if (gameProcess.Length == 0)
                return false;

            IntPtr targetHwnd = gameProcess[0].MainWindowHandle;
            WindowsAPI.SetForegroundWindow(targetHwnd);
            WindowsAPI.SetFocus(targetHwnd);

            return true;
        }

        private int ClientCount
        {
            get
            {
                return clientCount;
            }
            set
            {
                clientCount = value;
                statusClientCount.Text = String.Format("{0}", clientCount);
            }
        }
        private void UpdateList()
        {
            listPlayers.SetDataSource(null);
            listPlayers.SetDataSource(playersBinding, "nickName", "nickName");
        }
        private void listPlayers_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();


            int index = e.Index;
            if (index >= 0 && index < listPlayers.Items.Count)
            {

                if (sessions[index] == null)
                {
                    sessions.RemoveAt(index);
                    return;
                }

                var item = listPlayers.Items[index] as ListElement;
                var client = sessions.FirstOrDefault(s => s.Session.SessionID == item.Session.SessionID);
                if (client == null)
                    return;

                bool selected = client.isReady;

                string text = item.ToString();
                Graphics g = e.Graphics;

                Color color = (selected) ? Color.LightGreen : Color.White;
                g.FillRectangle(new SolidBrush(color), e.Bounds);

                g.DrawString(text, e.Font, Brushes.Black, listPlayers.GetItemRectangle(index).Location);
            }

            e.DrawFocusRectangle();
        }

        private void timerPing_Tick(object sender, EventArgs e)
        {
            sessions.ForEach(s => s.Session.SendResponse("ping"));
        }
    }
    public class ListElement
    {
        public ListElement(WebSocketSession session)
        {
            Session = session;
        }
        public WebSocketSession Session
        {
            get;
            set;
        }
        public String nickName
        {
            get;
            set;
        }
        public bool isReady
        {
            get;
            set;
        }
        public override string ToString()
        {
            return nickName;
        }

    }

}
