using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BepopProtocolAnalyzer
{
    public partial class PacketInspectorForm : Form
    {
        private Packet packet;

        public PacketInspectorForm(Packet packet)
        {
            this.packet = packet;
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            var cl = Packet.GetPacketClass(packet.Project, packet.Class);
            var cmd = Packet.GetPacketCommand(packet.Project, packet.Class, packet.Command);

            lblProject.Text = string.Format("Project: {0}", packet.Project);
            lblClass.Text = string.Format("Class: {0}", cl);
            lblCommand.Text = string.Format("Command: {0} ({1})", cmd, packet.Command);
            lblLen.Text = string.Format("Length: {0}", packet.Data.Length);

            var ms = new MemoryStream(packet.Data);
            var prov = new Be.Windows.Forms.DynamicFileByteProvider(ms);

            hxData.ByteProvider = prov;
        }
    }
}
