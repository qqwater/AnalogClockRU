namespace AnalogClock
{
    partial class ClockSettingsForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.ComboBox comboTimeZones;
        private System.Windows.Forms.ListBox lbSelectedTimeZones;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            comboTimeZones = new ComboBox();
            lbSelectedTimeZones = new ListBox();
            btnAdd = new Button();
            btnOk = new Button();
            label1 = new Label();
            label2 = new Label();
            SuspendLayout();
            // 
            // comboTimeZones
            // 
            comboTimeZones.DropDownStyle = ComboBoxStyle.DropDownList;
            comboTimeZones.FormattingEnabled = true;
            comboTimeZones.Location = new Point(15, 25);
            comboTimeZones.Name = "comboTimeZones";
            comboTimeZones.Size = new Size(350, 23);
            comboTimeZones.TabIndex = 0;
            // 
            // lbSelectedTimeZones
            // 
            lbSelectedTimeZones.FormattingEnabled = true;
            lbSelectedTimeZones.ItemHeight = 15;
            lbSelectedTimeZones.Location = new Point(15, 75);
            lbSelectedTimeZones.Name = "lbSelectedTimeZones";
            lbSelectedTimeZones.Size = new Size(350, 139);
            lbSelectedTimeZones.TabIndex = 2;
            // 
            // btnAdd
            // 
            btnAdd.Location = new Point(370, 24);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(75, 25);
            btnAdd.TabIndex = 1;
            btnAdd.Text = "Добавить";
            btnAdd.UseVisualStyleBackColor = true;
            btnAdd.Click += btnAdd_Click;
            // 
            // btnOk
            // 
            btnOk.Location = new Point(370, 189);
            btnOk.Name = "btnOk";
            btnOk.Size = new Size(75, 25);
            btnOk.TabIndex = 3;
            btnOk.Text = "ОК";
            btnOk.UseVisualStyleBackColor = true;
            btnOk.Click += btnOk_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(15, 7);
            label1.Name = "label1";
            label1.Size = new Size(139, 15);
            label1.TabIndex = 4;
            label1.Text = "Выберите часовой пояс";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(15, 57);
            label2.Name = "label2";
            label2.Size = new Size(253, 15);
            label2.TabIndex = 5;
            label2.Text = "Выбранные пояса (для удаления кнопка Del)";
            // 
            // ClockSettingsForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(460, 230);
            Controls.Add(comboTimeZones);
            Controls.Add(btnAdd);
            Controls.Add(lbSelectedTimeZones);
            Controls.Add(btnOk);
            Controls.Add(label1);
            Controls.Add(label2);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ClockSettingsForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Настройки часовых поясов";
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
