using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AnalogClock
{
    public partial class ClockSettingsForm : Form
    {
        public List<TimeZoneInfo> SelectedTimeZones { get; private set; } = new List<TimeZoneInfo>();

        public ClockSettingsForm(List<TimeZoneInfo> existingTimeZones)
        {
            InitializeComponent();

            // Заполнение ComboBox часовыми поясами
            foreach (var tz in TimeZoneInfo.GetSystemTimeZones())
            {
                comboTimeZones.Items.Add(tz);
            }
            comboTimeZones.DisplayMember = "DisplayName";

            if (comboTimeZones.Items.Count > 0)
                comboTimeZones.SelectedIndex = 0;

            // Добавляем уже выбранные часовые пояса в ListBox
            foreach (var tz in existingTimeZones)
            {
                SelectedTimeZones.Add(tz);
                lbSelectedTimeZones.Items.Add(tz);
            }

            lbSelectedTimeZones.DisplayMember = "DisplayName";

            // Обработчик удаления по Delete
            lbSelectedTimeZones.KeyDown += LbSelectedTimeZones_KeyDown;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (comboTimeZones.SelectedItem is TimeZoneInfo tz && !SelectedTimeZones.Contains(tz))
            {
                SelectedTimeZones.Add(tz);
                lbSelectedTimeZones.Items.Add(tz);
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        // Удаление выбранного из списка по клавише Delete
        private void LbSelectedTimeZones_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && lbSelectedTimeZones.SelectedItem != null)
            {
                var item = lbSelectedTimeZones.SelectedItem as TimeZoneInfo;
                if (item != null)
                {
                    SelectedTimeZones.Remove(item);
                    lbSelectedTimeZones.Items.Remove(item);
                }
            }
        }
    }
}
