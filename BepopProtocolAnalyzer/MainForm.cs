using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BepopProtocolAnalyzer
{
    public partial class MainForm : Form
    {
        private PacketReader _reader;
        private FileStream _videoFile;

        private BepopServer _s;

        public MainForm()
        {
            InitializeComponent();
        }

        void reader_OnVideoFrameReceived(object sender, VideoFrameReceived e)
        {
            if (_videoFile == null)
            {
                _videoFile = File.Create("video.h264");
            }
            _videoFile.Write(e.Data, 0, e.Data.Length);
        }

        void reader_OnStreamFinished(object sender, StreamFinishedEventArgs e)
        {
            if (_videoFile != null)
            {
                _videoFile.Flush();
                _videoFile.Close();
            }

            var a = new Action(() =>
            {
                lblStatus.Text = e.FoundDiscovery
                    ? "Finished."
                    : "No protocol discovery found. Some parts of capture are missing.";
                SetControls(true);
            });

            if (statusStrip1.InvokeRequired)
                statusStrip1.Invoke(a);
            else
                a();
        }

        private void AddFrameToList(Frame f)
        {
            var a = new Action(() =>
            {
                var i = new ListViewItem(f.Time.ToString("0.###"));
                i.SubItems.Add(f.Direction.ToString());
                i.SubItems.Add(f.Type.ToString());

                if (f.Type == FrameType.ACK)
                {
                    i.ForeColor = Color.Purple;
                }
                else if (f.Direction == Frame.FrameDirection.ToDrone)
                {
                    i.ForeColor = Color.Blue;
                }
                else if (f.Direction == Frame.FrameDirection.ToController)
                {
                    i.ForeColor = Color.Black;
                }

                if (f.Id == 0)
                {
                    i.SubItems.Add("PING");
                    i.ForeColor = Color.Orange;
                }
                else if (f.Id == 1)
                {
                    i.SubItems.Add("PONG");
                    i.ForeColor = Color.DarkGreen;
                }
                else
                {
                    i.SubItems.Add(f.Id.ToString());
                }

                i.SubItems.Add(f.Seq.ToString());
                i.SubItems.Add(f.Data.Length.ToString());
                if (f.Id >= 2 && (f.Type == FrameType.DATA_W_ACK
                    || f.Type == FrameType.DATA_LL
                    || f.Type == FrameType.DATA))
                {
                    var proj = (PacketType)f.Data[0];
                    var c = Packet.GetPacketClass(proj, f.Data[1]);

                    ushort command = 0;
                    command = f.Data[2];
                    command |= (ushort)(f.Data[3] << 8);

                    var cmd = Packet.GetPacketCommand(proj, f.Data[1], command);
                    i.SubItems.Add(proj.ToString());
                    i.SubItems.Add(c);
                    i.SubItems.Add(cmd);
                }
                i.Tag = f;
                lstPackets.Items.Add(i);
            });

            if (lstPackets.InvokeRequired)
                lstPackets.Invoke(a);
            else
                a();
        }

        void reader_OnFrameReceived(object sender, FrameReceivedEventArgs e)
        {
            if (e.Frame.Type != FrameType.DATA_LL)
            {
                AddFrameToList(e.Frame);
            }
        }

        private void lstPackets_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstPackets.SelectedItems.Count > 0)
            {
                var selectedFrame = (Frame)lstPackets.SelectedItems[0].Tag;

                if (hxBox.ByteProvider != null)
                {
                    var p = (Be.Windows.Forms.DynamicFileByteProvider)hxBox.ByteProvider;
                    p.Dispose();
                }

                var ms = new MemoryStream(selectedFrame.Data);
                var prov = new Be.Windows.Forms.DynamicFileByteProvider(ms);

                hxBox.ByteProvider = prov;
            }
        }

        private void lstPackets_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (lstPackets.SelectedItems.Count > 0)
            {
                var selectedFrame = (Frame)lstPackets.SelectedItems[0].Tag;
                if (selectedFrame.Type == FrameType.DATA
                    || selectedFrame.Type == FrameType.DATA_LL
                    || selectedFrame.Type == FrameType.DATA_W_ACK)
                {
                    var packet = Packet.Parse(selectedFrame.Data);
                    var form = new PacketInspectorForm(packet);
                    form.ShowDialog();
                }

            }
        }

        private void SetControls(bool enabled)
        {
            simulateDroneToolStripMenuItem.Enabled = enabled;
            openToolStripMenuItem.Enabled = enabled;
            startToolStripMenuItem.Enabled = enabled;
            dumpVideoToolStripMenuItem.Enabled = enabled;
            stopToolStripMenuItem.Enabled = !enabled;
        }

        private void StartCapture(Func<PacketReader> reader)
        {
            // Close previous reader if necessary
            if (_reader != null)
            {
                _reader.Stop();
                _reader.Close();
            }

            lstPackets.Items.Clear();
            Task.Factory.StartNew(() =>
            {
                _reader = reader();
                _reader.OnFrameReceived += reader_OnFrameReceived;
                _reader.OnStreamFinished += reader_OnStreamFinished;
                _reader.OnVideoFrameReceived += reader_OnVideoFrameReceived;
                _reader.Open();
                _reader.Start();
            });
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.Filter = "PCAP File (*.pcap)|*.pcap|CAP file (*.cap)|*.cap";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                SetControls(false);

                StartCapture(() => new PacketReader(ofd.FileName, dumpVideoToolStripMenuItem.Checked));
                lblStatus.Text = "Parsing PCAP...";
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void simulateDroneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_s == null)
            {
                SetControls(false);

                _s = new BepopServer(44444);
                lblStatus.Text = "Started ArDrone3 discovery on port 44444";
                _s.OnFrameReceived += reader_OnFrameReceived;
                _s.Start();
            }
        }

        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var selector = new AdapterSelectionForm();
            if (selector.ShowDialog() == DialogResult.OK)
            {
                var device = selector.Device;
                SetControls(false);
                stopToolStripMenuItem.Enabled = true;
                StartCapture(() =>  new PacketReader(device, dumpVideoToolStripMenuItem.Checked));
                lblStatus.Text = "Started capture on " + device.Interface.FriendlyName;
            }
        }

        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_reader != null)
            {
                _reader.Stop();
                _reader.Close();
                SetControls(true);
                lblStatus.Text = "Stopped capture.";
            }
        }
    }
}
