using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace AnalogClock
{
    public partial class Form1 : Form
    {
        private System.Windows.Forms.Timer timer;
        private List<TimeZoneInfo> additionalTimeZones = new List<TimeZoneInfo>();
        private Button btnSettings;

        public Form1()
        {
            InitializeComponent();

            // Открываем окно по центру экрана
            this.StartPosition = FormStartPosition.CenterScreen;

            this.MinimumSize = new Size(300, 400);
            this.ClientSize = new Size(300, 400);
            this.DoubleBuffered = true;

            timer = new System.Windows.Forms.Timer();
            timer.Interval = 1000;
            timer.Tick += (s, e) => Invalidate();
            timer.Start();

            // Кнопка "Добавить"
            btnSettings = new Button();
            btnSettings.Text = "Добавить";
            btnSettings.Size = new Size(100, 30);
            btnSettings.Location = new Point(10, 10);
            btnSettings.Click += BtnSettings_Click;
            this.Controls.Add(btnSettings);

            // Загружаем сохранённые настройки часовых поясов
            LoadSettings();
        }

        private void BtnSettings_Click(object sender, EventArgs e)
        {
            using (var settingsForm = new ClockSettingsForm(additionalTimeZones))
            {
                if (settingsForm.ShowDialog() == DialogResult.OK)
                {
                    additionalTimeZones = settingsForm.SelectedTimeZones;
                    AdjustFormHeight();
                    Invalidate();
                }
            }
        }

        private void AdjustFormHeight()
        {
            int miniClockHeight = 100;   // высота одного мини-часового пояса с цифровым временем и отступами
            int baseClientHeight = 400;  // базовая высота клиентской области (под основной аналоговый циферблат)

            int totalClientHeight = baseClientHeight + additionalTimeZones.Count * miniClockHeight;

            this.ClientSize = new Size(this.ClientSize.Width, totalClientHeight);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            int w = this.ClientSize.Width;
            int h = this.ClientSize.Height;

            // Основной аналоговый циферблат — внизу
            int baseClockRadius = Math.Min(w, 300) / 2 - 10;
            Point baseCenter = new Point(w / 2, h - baseClockRadius - 60);

            // Фон и циферблат
            using (Brush brush = new SolidBrush(Color.LightGray))
            {
                g.FillEllipse(brush, baseCenter.X - baseClockRadius, baseCenter.Y - baseClockRadius, baseClockRadius * 2, baseClockRadius * 2);
            }
            g.DrawEllipse(Pens.Black, baseCenter.X - baseClockRadius, baseCenter.Y - baseClockRadius, baseClockRadius * 2, baseClockRadius * 2);

            // Цифры и риски
            using (Font font = new Font("Arial", 14, FontStyle.Bold))
            using (StringFormat format = new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
            {
                float offset = font.Size / 2;
                float additionalInset = 10;
                float gapBetweenNumberAndTick = 10;
                float tickLength = 15;

                for (int i = 1; i <= 12; i++)
                {
                    double angle = (i - 3) * Math.PI / 6;

                    int numberRadius = (int)(baseClockRadius - offset - additionalInset);
                    int xNum = baseCenter.X + (int)(numberRadius * Math.Cos(angle));
                    int yNum = baseCenter.Y + (int)(numberRadius * Math.Sin(angle));
                    g.DrawString(i.ToString(), font, Brushes.Black, new PointF(xNum, yNum), format);

                    float lineStartRadius = numberRadius - gapBetweenNumberAndTick;
                    float lineEndRadius = lineStartRadius - tickLength;

                    int xStart = baseCenter.X + (int)(lineStartRadius * Math.Cos(angle));
                    int yStart = baseCenter.Y + (int)(lineStartRadius * Math.Sin(angle));
                    int xEnd = baseCenter.X + (int)(lineEndRadius * Math.Cos(angle));
                    int yEnd = baseCenter.Y + (int)(lineEndRadius * Math.Sin(angle));

                    g.DrawLine(Pens.Black, xStart, yStart, xEnd, yEnd);
                }

                // Минутные риски
                float minuteTickLength = 7;
                float minuteTickThickness = 1;
                float minuteTickRadiusStart = baseClockRadius - offset - additionalInset - 0.2f;
                float minuteTickRadiusEnd = minuteTickRadiusStart - minuteTickLength;

                using (Pen minutePen = new Pen(Color.Black, minuteTickThickness))
                {
                    for (int i = 0; i < 60; i++)
                    {
                        if (i % 5 == 0)
                            continue;

                        double angle = (i - 15) * Math.PI / 30;

                        int xStart = baseCenter.X + (int)(minuteTickRadiusStart * Math.Cos(angle));
                        int yStart = baseCenter.Y + (int)(minuteTickRadiusStart * Math.Sin(angle));
                        int xEnd = baseCenter.X + (int)(minuteTickRadiusEnd * Math.Cos(angle));
                        int yEnd = baseCenter.Y + (int)(minuteTickRadiusEnd * Math.Sin(angle));

                        g.DrawLine(minutePen, xStart, yStart, xEnd, yEnd);
                    }
                }
            }

            DateTime now = DateTime.Now;

            double hourAngle = ((now.Hour % 12) + now.Minute / 60.0) * Math.PI / 6;
            DrawHand(g, baseCenter, hourAngle, baseClockRadius * 0.5f, 6, Brushes.Black);

            double minuteAngle = (now.Minute + now.Second / 60.0) * Math.PI / 30;
            DrawHand(g, baseCenter, minuteAngle, baseClockRadius * 0.7f, 4, Brushes.Blue);

            double secondAngle = now.Second * Math.PI / 30;
            DrawHand(g, baseCenter, secondAngle, baseClockRadius * 0.9f, 2, Brushes.Red);

            string digitalTime = now.ToString("HH:mm:ss");
            using (Font digitalFont = new Font("Arial", 16, FontStyle.Bold))
            using (Brush digitalBrush = new SolidBrush(Color.Black))
            using (StringFormat digitalFormat = new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Near })
            {
                PointF digitalPos = new PointF(baseCenter.X, baseCenter.Y + baseClockRadius + 20);
                g.DrawString(digitalTime, digitalFont, digitalBrush, digitalPos, digitalFormat);
            }

            // Дополнительные мини-циферблаты сверху, под кнопкой
            int miniClockRadius = 40;
            int spacing = 10;
            int miniTop = 50;
            int miniLeft = w / 2;

            for (int i = 0; i < additionalTimeZones.Count; i++)
            {
                var tz = additionalTimeZones[i];
                DateTime timeInZone = TimeZoneInfo.ConvertTime(now, tz);
                Point miniCenter = new Point(miniLeft, miniTop + i * (miniClockRadius * 2 + spacing + 30));

                DrawMiniClock(g, miniCenter, miniClockRadius, timeInZone, tz.DisplayName);
            }
        }

        private void DrawHand(Graphics g, Point center, double angle, float length, int thickness, Brush brush)
        {
            float x = center.X + (float)(length * Math.Sin(angle));
            float y = center.Y - (float)(length * Math.Cos(angle));

            using (Pen pen = new Pen(brush, thickness))
            {
                pen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
                g.DrawLine(pen, center.X, center.Y, x, y);
            }
        }

        private void DrawMiniClock(Graphics g, Point center, int radius, DateTime time, string label)
        {
            using (Brush b = new SolidBrush(Color.White))
                g.FillEllipse(b, center.X - radius, center.Y - radius, radius * 2, radius * 2);

            g.DrawEllipse(Pens.Black, center.X - radius, center.Y - radius, radius * 2, radius * 2);

            double hourAngle = ((time.Hour % 12) + time.Minute / 60.0) * Math.PI / 6;
            double minuteAngle = (time.Minute + time.Second / 60.0) * Math.PI / 30;
            double secondAngle = time.Second * Math.PI / 30;

            DrawHand(g, center, hourAngle, radius * 0.5f, 2, Brushes.Black);
            DrawHand(g, center, minuteAngle, radius * 0.7f, 2, Brushes.Blue);
            DrawHand(g, center, secondAngle, radius * 0.9f, 1, Brushes.Red);

            using (Font f = new Font("Arial", 8))
            using (StringFormat sf = new StringFormat { Alignment = StringAlignment.Center })
            {
                g.DrawString(label, f, Brushes.Black, center.X, center.Y + radius + 5, sf);
                string digital = time.ToString("HH:mm:ss");
                g.DrawString(digital, f, Brushes.Black, center.X, center.Y + radius + 20, sf);
            }
        }

        // Загрузка списка часовых поясов из настроек
        private void LoadSettings()
        {
            string tzList = Properties.Settings.Default.TimeZones;
            additionalTimeZones.Clear();

            if (!string.IsNullOrEmpty(tzList))
            {
                var ids = tzList.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var id in ids)
                {
                    try
                    {
                        var tz = TimeZoneInfo.FindSystemTimeZoneById(id);
                        additionalTimeZones.Add(tz);
                    }
                    catch
                    {
                        // Игнорируем невалидные или удалённые часовые пояса
                    }
                }
            }
            AdjustFormHeight();
        }

        // Сохранение списка часовых поясов в настройки
        private void SaveSettings()
        {
            var ids = new List<string>();
            foreach (var tz in additionalTimeZones)
                ids.Add(tz.Id);

            Properties.Settings.Default.TimeZones = string.Join(";", ids);
            Properties.Settings.Default.Save();
        }

        // При закрытии формы сохраняем настройки
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            SaveSettings();
        }
    }
}
