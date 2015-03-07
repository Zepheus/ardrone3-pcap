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
            lblProject.Text = string.Format("Project: {0}", packet.Project);
            lblClass.Text = string.Format("Class: {0}", Packet.GetPacketClass(packet));
            lblCommand.Text = string.Format("Command: {0}", packet.Command);

            var ms = new MemoryStream(packet.Data);
            var prov = new Be.Windows.Forms.DynamicFileByteProvider(ms);

            hxData.ByteProvider = prov;
        }
    }
}
