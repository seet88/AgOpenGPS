namespace AgOpenGPS.Forms
{
    partial class CustomDataSender
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
            this.saveToConfig = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.txt_traccar_id = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lbl_traccar_id = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // saveToConfig
            // 
            this.saveToConfig.Location = new System.Drawing.Point(65, 129);
            this.saveToConfig.Name = "saveToConfig";
            this.saveToConfig.Size = new System.Drawing.Size(75, 23);
            this.saveToConfig.TabIndex = 0;
            this.saveToConfig.Text = "Save";
            this.saveToConfig.UseVisualStyleBackColor = true;
            this.saveToConfig.Click += new System.EventHandler(this.saveToConfig_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(65, 60);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 22);
            this.textBox1.TabIndex = 1;
            // 
            // txt_traccar_id
            // 
            this.txt_traccar_id.Location = new System.Drawing.Point(179, 60);
            this.txt_traccar_id.Name = "txt_traccar_id";
            this.txt_traccar_id.Size = new System.Drawing.Size(100, 22);
            this.txt_traccar_id.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(62, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 16);
            this.label1.TabIndex = 3;
            this.label1.Text = "label1";
            // 
            // lbl_traccar_id
            // 
            this.lbl_traccar_id.AutoSize = true;
            this.lbl_traccar_id.Location = new System.Drawing.Point(176, 41);
            this.lbl_traccar_id.Name = "lbl_traccar_id";
            this.lbl_traccar_id.Size = new System.Drawing.Size(62, 16);
            this.lbl_traccar_id.TabIndex = 4;
            this.lbl_traccar_id.Text = "traccar Id";
            // 
            // CustomDataSender
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.lbl_traccar_id);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txt_traccar_id);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.saveToConfig);
            this.Name = "CustomDataSender";
            this.Text = "CustomDataSender";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button saveToConfig;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox txt_traccar_id;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbl_traccar_id;
    }
}