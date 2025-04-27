namespace AgOpenGPS.Forms.Field
{
    partial class FormNewFieldCustom
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
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tboxFieldName = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnSerialCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.listOfRefFieldsCmb = new System.Windows.Forms.ComboBox();
            this.refFieldLabel = new System.Windows.Forms.Label();
            this.taskNameTxt = new System.Windows.Forms.TextBox();
            this.taskNameLbl = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.createNewTask = new System.Windows.Forms.TabPage();
            this.listOfToolsCmb = new System.Windows.Forms.ComboBox();
            this.listOfVehiclesCmb = new System.Windows.Forms.ComboBox();
            this.toolLbl = new System.Windows.Forms.Label();
            this.vehicleLbl = new System.Windows.Forms.Label();
            this.selectedRefFieldInfoRichTxtBox = new System.Windows.Forms.RichTextBox();
            this.CreateNewRefField = new System.Windows.Forms.TabPage();
            this.newRefFieldLbl = new System.Windows.Forms.Label();
            this.newRefFieldNameTxt = new System.Windows.Forms.TextBox();
            this.cancelBtn = new System.Windows.Forms.Button();
            this.createRefFieldBtn = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.createNewTask.SuspendLayout();
            this.CreateNewRefField.SuspendLayout();
            this.SuspendLayout();
            // 
            // tboxFieldName
            // 
            this.tboxFieldName.BackColor = System.Drawing.Color.AliceBlue;
            this.tboxFieldName.Enabled = false;
            this.tboxFieldName.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tboxFieldName.Location = new System.Drawing.Point(11, 63);
            this.tboxFieldName.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tboxFieldName.Name = "tboxFieldName";
            this.tboxFieldName.Size = new System.Drawing.Size(609, 36);
            this.tboxFieldName.TabIndex = 0;
            this.tboxFieldName.Click += new System.EventHandler(this.tboxFieldName_Click);
            this.tboxFieldName.TextChanged += new System.EventHandler(this.tboxFieldName_TextChanged);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.BackColor = System.Drawing.Color.Transparent;
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnSave.Image = global::AgOpenGPS.Properties.Resources.OK64;
            this.btnSave.Location = new System.Drawing.Point(1120, 365);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(83, 79);
            this.btnSave.TabIndex = 3;
            this.btnSave.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnSerialCancel
            // 
            this.btnSerialCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSerialCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnSerialCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnSerialCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnSerialCancel.FlatAppearance.BorderSize = 0;
            this.btnSerialCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSerialCancel.Font = new System.Drawing.Font("Tahoma", 12F);
            this.btnSerialCancel.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnSerialCancel.Image = global::AgOpenGPS.Properties.Resources.Cancel64;
            this.btnSerialCancel.Location = new System.Drawing.Point(1004, 364);
            this.btnSerialCancel.Name = "btnSerialCancel";
            this.btnSerialCancel.Size = new System.Drawing.Size(77, 79);
            this.btnSerialCancel.TabIndex = 4;
            this.btnSerialCancel.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnSerialCancel.UseVisualStyleBackColor = false;
            this.btnSerialCancel.Click += new System.EventHandler(this.btnSerialCancel_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label1.Location = new System.Drawing.Point(9, 35);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(174, 23);
            this.label1.TabIndex = 4;
            this.label1.Text = "Enter Field Name";
            // 
            // listOfRefFieldsCmb
            // 
            this.listOfRefFieldsCmb.FormattingEnabled = true;
            this.listOfRefFieldsCmb.Location = new System.Drawing.Point(13, 241);
            this.listOfRefFieldsCmb.Name = "listOfRefFieldsCmb";
            this.listOfRefFieldsCmb.Size = new System.Drawing.Size(381, 31);
            this.listOfRefFieldsCmb.TabIndex = 153;
            this.listOfRefFieldsCmb.SelectedIndexChanged += new System.EventHandler(this.listOfRefFieldsCmb_SelectedIndexChanged);
            // 
            // refFieldLabel
            // 
            this.refFieldLabel.AutoSize = true;
            this.refFieldLabel.Location = new System.Drawing.Point(19, 201);
            this.refFieldLabel.Name = "refFieldLabel";
            this.refFieldLabel.Size = new System.Drawing.Size(130, 23);
            this.refFieldLabel.TabIndex = 154;
            this.refFieldLabel.Text = "Select ref field";
            // 
            // taskNameTxt
            // 
            this.taskNameTxt.Location = new System.Drawing.Point(11, 150);
            this.taskNameTxt.Name = "taskNameTxt";
            this.taskNameTxt.Size = new System.Drawing.Size(522, 30);
            this.taskNameTxt.TabIndex = 155;
            this.taskNameTxt.TextChanged += new System.EventHandler(this.taskNameTxt_TextChanged);
            // 
            // taskNameLbl
            // 
            this.taskNameLbl.AutoSize = true;
            this.taskNameLbl.Location = new System.Drawing.Point(19, 113);
            this.taskNameLbl.Name = "taskNameLbl";
            this.taskNameLbl.Size = new System.Drawing.Size(101, 23);
            this.taskNameLbl.TabIndex = 156;
            this.taskNameLbl.Text = "Task name";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.createNewTask);
            this.tabControl1.Controls.Add(this.CreateNewRefField);
            this.tabControl1.Location = new System.Drawing.Point(21, 35);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1220, 485);
            this.tabControl1.TabIndex = 157;
            // 
            // createNewTask
            // 
            this.createNewTask.Controls.Add(this.listOfToolsCmb);
            this.createNewTask.Controls.Add(this.listOfVehiclesCmb);
            this.createNewTask.Controls.Add(this.toolLbl);
            this.createNewTask.Controls.Add(this.vehicleLbl);
            this.createNewTask.Controls.Add(this.selectedRefFieldInfoRichTxtBox);
            this.createNewTask.Controls.Add(this.listOfRefFieldsCmb);
            this.createNewTask.Controls.Add(this.taskNameLbl);
            this.createNewTask.Controls.Add(this.refFieldLabel);
            this.createNewTask.Controls.Add(this.taskNameTxt);
            this.createNewTask.Controls.Add(this.btnSerialCancel);
            this.createNewTask.Controls.Add(this.label1);
            this.createNewTask.Controls.Add(this.btnSave);
            this.createNewTask.Controls.Add(this.tboxFieldName);
            this.createNewTask.Location = new System.Drawing.Point(4, 32);
            this.createNewTask.Name = "createNewTask";
            this.createNewTask.Padding = new System.Windows.Forms.Padding(3);
            this.createNewTask.Size = new System.Drawing.Size(1212, 449);
            this.createNewTask.TabIndex = 0;
            this.createNewTask.Text = "Create new task";
            this.createNewTask.UseVisualStyleBackColor = true;
            // 
            // listOfToolsCmb
            // 
            this.listOfToolsCmb.FormattingEnabled = true;
            this.listOfToolsCmb.Location = new System.Drawing.Point(758, 236);
            this.listOfToolsCmb.Name = "listOfToolsCmb";
            this.listOfToolsCmb.Size = new System.Drawing.Size(206, 31);
            this.listOfToolsCmb.TabIndex = 161;
            // 
            // listOfVehiclesCmb
            // 
            this.listOfVehiclesCmb.FormattingEnabled = true;
            this.listOfVehiclesCmb.Location = new System.Drawing.Point(445, 236);
            this.listOfVehiclesCmb.Name = "listOfVehiclesCmb";
            this.listOfVehiclesCmb.Size = new System.Drawing.Size(206, 31);
            this.listOfVehiclesCmb.TabIndex = 160;
            // 
            // toolLbl
            // 
            this.toolLbl.AutoSize = true;
            this.toolLbl.Location = new System.Drawing.Point(780, 210);
            this.toolLbl.Name = "toolLbl";
            this.toolLbl.Size = new System.Drawing.Size(40, 23);
            this.toolLbl.TabIndex = 159;
            this.toolLbl.Text = "tool";
            // 
            // vehicleLbl
            // 
            this.vehicleLbl.AutoSize = true;
            this.vehicleLbl.Location = new System.Drawing.Point(454, 210);
            this.vehicleLbl.Name = "vehicleLbl";
            this.vehicleLbl.Size = new System.Drawing.Size(67, 23);
            this.vehicleLbl.TabIndex = 158;
            this.vehicleLbl.Text = "vehicle";
            // 
            // selectedRefFieldInfoRichTxtBox
            // 
            this.selectedRefFieldInfoRichTxtBox.Location = new System.Drawing.Point(13, 310);
            this.selectedRefFieldInfoRichTxtBox.Name = "selectedRefFieldInfoRichTxtBox";
            this.selectedRefFieldInfoRichTxtBox.ReadOnly = true;
            this.selectedRefFieldInfoRichTxtBox.Size = new System.Drawing.Size(381, 133);
            this.selectedRefFieldInfoRichTxtBox.TabIndex = 157;
            this.selectedRefFieldInfoRichTxtBox.Text = "";
            // 
            // CreateNewRefField
            // 
            this.CreateNewRefField.Controls.Add(this.newRefFieldLbl);
            this.CreateNewRefField.Controls.Add(this.newRefFieldNameTxt);
            this.CreateNewRefField.Controls.Add(this.cancelBtn);
            this.CreateNewRefField.Controls.Add(this.createRefFieldBtn);
            this.CreateNewRefField.Location = new System.Drawing.Point(4, 32);
            this.CreateNewRefField.Name = "CreateNewRefField";
            this.CreateNewRefField.Padding = new System.Windows.Forms.Padding(3);
            this.CreateNewRefField.Size = new System.Drawing.Size(1212, 449);
            this.CreateNewRefField.TabIndex = 1;
            this.CreateNewRefField.Text = "Create new ref field";
            this.CreateNewRefField.UseVisualStyleBackColor = true;
            // 
            // newRefFieldLbl
            // 
            this.newRefFieldLbl.AutoSize = true;
            this.newRefFieldLbl.Location = new System.Drawing.Point(38, 51);
            this.newRefFieldLbl.Name = "newRefFieldLbl";
            this.newRefFieldLbl.Size = new System.Drawing.Size(168, 23);
            this.newRefFieldLbl.TabIndex = 8;
            this.newRefFieldLbl.Text = "new ref field name";
            // 
            // newRefFieldNameTxt
            // 
            this.newRefFieldNameTxt.Location = new System.Drawing.Point(38, 85);
            this.newRefFieldNameTxt.Name = "newRefFieldNameTxt";
            this.newRefFieldNameTxt.Size = new System.Drawing.Size(479, 30);
            this.newRefFieldNameTxt.TabIndex = 7;
            // 
            // cancelBtn
            // 
            this.cancelBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelBtn.BackColor = System.Drawing.Color.Transparent;
            this.cancelBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.cancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelBtn.FlatAppearance.BorderSize = 0;
            this.cancelBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cancelBtn.Font = new System.Drawing.Font("Tahoma", 12F);
            this.cancelBtn.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.cancelBtn.Image = global::AgOpenGPS.Properties.Resources.Cancel64;
            this.cancelBtn.Location = new System.Drawing.Point(424, 364);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(77, 79);
            this.cancelBtn.TabIndex = 6;
            this.cancelBtn.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.cancelBtn.UseVisualStyleBackColor = false;
            this.cancelBtn.Click += new System.EventHandler(this.btnSerialCancel_Click);
            // 
            // createRefFieldBtn
            // 
            this.createRefFieldBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.createRefFieldBtn.BackColor = System.Drawing.Color.Transparent;
            this.createRefFieldBtn.FlatAppearance.BorderSize = 0;
            this.createRefFieldBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.createRefFieldBtn.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.createRefFieldBtn.Image = global::AgOpenGPS.Properties.Resources.OK64;
            this.createRefFieldBtn.Location = new System.Drawing.Point(540, 365);
            this.createRefFieldBtn.Name = "createRefFieldBtn";
            this.createRefFieldBtn.Size = new System.Drawing.Size(83, 79);
            this.createRefFieldBtn.TabIndex = 5;
            this.createRefFieldBtn.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.createRefFieldBtn.UseVisualStyleBackColor = false;
            this.createRefFieldBtn.Click += new System.EventHandler(this.createRefFieldBtn_Click);
            // 
            // FormNewFieldCustom
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(1258, 559);
            this.ControlBox = false;
            this.Controls.Add(this.tabControl1);
            this.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "FormNewFieldCustom";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Create New Field ";
            this.Load += new System.EventHandler(this.FormFieldDir_Load);
            this.tabControl1.ResumeLayout(false);
            this.createNewTask.ResumeLayout(false);
            this.createNewTask.PerformLayout();
            this.CreateNewRefField.ResumeLayout(false);
            this.CreateNewRefField.PerformLayout();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.TextBox tboxFieldName;
        private System.Windows.Forms.Button btnSerialCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label label1;
        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        //private void InitializeComponent()
        //{
        //    this.components = new System.ComponentModel.Container();
        //    this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        //    this.ClientSize = new System.Drawing.Size(800, 450);
        //    this.Text = "FormNewFieldCustom";
        //}

        #endregion

        private System.Windows.Forms.ComboBox listOfRefFieldsCmb;
        private System.Windows.Forms.Label refFieldLabel;
        private System.Windows.Forms.TextBox taskNameTxt;
        private System.Windows.Forms.Label taskNameLbl;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage createNewTask;
        private System.Windows.Forms.TabPage CreateNewRefField;
        private System.Windows.Forms.Button cancelBtn;
        private System.Windows.Forms.Button createRefFieldBtn;
        private System.Windows.Forms.TextBox newRefFieldNameTxt;
        private System.Windows.Forms.Label newRefFieldLbl;
        private System.Windows.Forms.RichTextBox selectedRefFieldInfoRichTxtBox;
        private System.Windows.Forms.ComboBox listOfToolsCmb;
        private System.Windows.Forms.ComboBox listOfVehiclesCmb;
        private System.Windows.Forms.Label toolLbl;
        private System.Windows.Forms.Label vehicleLbl;
    }
}