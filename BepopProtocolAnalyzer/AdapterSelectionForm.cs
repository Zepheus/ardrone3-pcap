using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharpPcap;
using SharpPcap.WinPcap;

namespace BepopProtocolAnalyzer
{
    public partial class AdapterSelectionForm : Form
    {
        public WinPcapDevice Device { get; private set; }

        private readonly WinPcapDeviceList _devices;

        public AdapterSelectionForm()
        {
            InitializeComponent();
            _devices = WinPcapDeviceList.Instance;
            LoadAdapters();
        }

        private void LoadAdapters()
        {
            cmdAdapters.Items.Clear();
            foreach (WinPcapDevice device in _devices)
            {
                cmdAdapters.Items.Add(device.Interface.FriendlyName);
            }
            if (cmdAdapters.Items.Count > 0)
                cmdAdapters.SelectedIndex = 0;
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            if (cmdAdapters.SelectedItem != null)
            {
                Device = _devices[cmdAdapters.SelectedIndex];
                DialogResult = DialogResult.OK;
                Close();
            }
        }
    }
}
