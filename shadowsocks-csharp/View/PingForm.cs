using Shadowsocks.Controller;
using Shadowsocks.Model;
using Shadowsocks.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Shadowsocks.View
{
    public partial class PingForm : Form
    {
        private Controller.ShadowsocksController controller;

        private Configuration _configuration;
        private BindingList<Server> _servers;

        public PingForm(Controller.ShadowsocksController controller)
        {
            this.Font = System.Drawing.SystemFonts.MessageBoxFont;
            InitializeComponent();

            UpdateTexts();
            this.Icon = Icon.FromHandle(Resources.ssw128.GetHicon());

            this.controller = controller;
            controller.ConfigChanged += controller_ConfigChanged;

            LoadCurrentConfiguration();
        }

        private void UpdateTexts()
        {
            this.Text = I18N.GetString("Find Fastest Server");
            FindButton.Text = I18N.GetString("&Find");
            ApplyButton.Text = I18N.GetString("&Apply");
            Server.HeaderText = I18N.GetString("Server");
            Delay.HeaderText = I18N.GetString("Delay");
        }

        private void controller_ConfigChanged(object sender, EventArgs e)
        {
            LoadCurrentConfiguration();
        }

        private void LoadConfiguration(Configuration configuration)
        {
            _servers =  new BindingList<Server>(configuration.configs);
            _servers.ListChanged += Servers_ListChanged;
            ServersDataGridView.DataSource = _servers;
        }

        private void LoadCurrentConfiguration()
        {
            _configuration = controller.GetConfiguration();
            LoadConfiguration(_configuration);
        }

        private void FindButton_Click(object sender, EventArgs e)
        {
            int fastestIndex = controller.Ping(_servers);
            ServersDataGridView.Rows[fastestIndex].Selected = true;

        }

        private void ApplyButton_Click(object sender, EventArgs e)
        {
            int index = ServersDataGridView.SelectedRows[0].Index;

            controller.ConfigChanged -= controller_ConfigChanged;
            controller.SelectServerIndex(index);

            this.Close();
        }

        private void PingForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            controller.ConfigChanged -= controller_ConfigChanged;
        }

        private void PingForm_Shown(object sender, EventArgs e)
        {
            ServersDataGridView.Rows[_configuration.index].Selected = true;
        }

        private void Servers_ListChanged(object sender, ListChangedEventArgs e)
        {
            ServersDataGridView.Refresh();
        }

    }
}
