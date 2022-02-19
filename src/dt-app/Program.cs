using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace dt
{
    public class DtTrayApp : Form
    {
        [STAThread]
        public static void Main()
        {
            Application.Run(new DtTrayApp());
        }

        private readonly NotifyIcon _trayIcon;
        private readonly ContextMenu _trayMenu;

        public DtTrayApp()
        {
            _trayMenu = new ContextMenu();
            _trayMenu.MenuItems.Add("Exit", OnExit);

            _trayIcon = new NotifyIcon();
            _trayIcon.Text = "Copy current timestamp";
            _trayIcon.Icon = new Icon(SystemIcons.Application, 40, 40);
            _trayIcon.ContextMenu = _trayMenu;
            _trayIcon.Visible = true;

            InitializeComponent();
            _trayIcon.MouseClick += trayIcon_MouseClick;
            _trayIcon.Icon = this.Icon;

            InitFormatMenu();
        }

        private void InitFormatMenu()
        {
            var formats = ReadFormats();
            foreach (var format in formats)
            {
                var item = new MenuItem(format, OnMenuItemChecked);
                item.Tag = format;
                item.RadioCheck = true;
                _trayMenu.MenuItems.Add(item);
            }

            if (!ApplyLastUsedFormat())
                _trayMenu.MenuItems[1].Checked = true;
        }

        private static string TryReadAllText(string path)
        {
            try
            {
                return File.ReadAllText(path);
            }
            catch
            {
                return string.Empty;
            }
        }

        private bool ApplyLastUsedFormat()
        {
            var lastUsedFormat = TryReadAllText(_lastUsedFormatPath);
            foreach (MenuItem m in _trayMenu.MenuItems)
            {
                if (m.Text != lastUsedFormat)
                    continue;
                m.Checked = true;
                return true;
            }
            return false;
        }

        private string _lastUsedFormatPath = "lastusedformat";
        private void SaveFormatAsLastUsed(string format)
        {
            File.WriteAllText(_lastUsedFormatPath, format);
        }

        private List<string> ReadFormats()
        {
            string filename = "timeformats";
            var lines = File.ReadLines(filename);
            var result = new List<string>();
            foreach (var line in lines)
            {
                if (ValidateFormat(line))
                    result.Add(line);
            }
            Debug.Assert(result.Count > 0);
            return result;
        }

        protected override void OnLoad(EventArgs e)
        {
            Visible = false;
            ShowInTaskbar = false;
            base.OnLoad(e);
        }

        private void OnMenuItemChecked(object sender, EventArgs e)
        {
            var item = sender as MenuItem;
            item.Checked = true;
            foreach (MenuItem m in _trayMenu.MenuItems)
            {
                if (m.Text == item.Text)
                    continue;
                m.Checked = false;
            }

            SaveFormatAsLastUsed(item.Tag as string);
            CopyToClipboard();
        }

        private void OnExit(object sender, EventArgs e)
        {
            Application.Exit();
        }

        #region designer
        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                _trayIcon.Dispose();
            }

            base.Dispose(isDisposing);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DtTrayApp));
            this.SuspendLayout();
            // 
            // DtTrayApp
            // 
            this.ClientSize = new System.Drawing.Size(282, 253);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("dt-square")));
            this.Name = "dt-app";
            this.ResumeLayout(false);
        }

        #endregion

        private void trayIcon_MouseClick(object sender, MouseEventArgs e)
        {
            CopyToClipboard();
        }

        private void CopyToClipboard()
        {
            string format = string.Empty;
            foreach (var item in _trayMenu.MenuItems)
            {
                var m = item as MenuItem;
                if (m.Checked)
                    format = m.Tag as string;
            }
            Clipboard.SetText($"{DateTime.Now.ToString(format)}");
        }

        private bool ValidateFormat(string format)
        {
            try
            {
                var sample = DateTime.Now.ToString(format);
                DateTime.TryParseExact(sample, format, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out var date);
                return !date.Equals(DateTime.MinValue);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error");
                return false;
            }
        }
    }
}