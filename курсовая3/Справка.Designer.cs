namespace курсовая3
{
    partial class Справка
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(603, 294);
            this.label1.TabIndex = 0;
            this.label1.Text = "Вас приветствует приложение  автоматизированной системы центра по продаже автомоб" +
    "илей\r\n\r\n";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // Справка
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GrayText;
            this.ClientSize = new System.Drawing.Size(603, 294);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Source Code Pro", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.Name = "Справка";
            this.Text = "Справка";
            this.Load += new System.EventHandler(this.Справка_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Label label1;
    }
}