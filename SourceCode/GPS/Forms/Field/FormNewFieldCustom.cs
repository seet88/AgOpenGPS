using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Diagnostics;

namespace AgOpenGPS.Forms.Field
{
    public partial class FormNewFieldCustom : Form
    {
        //class variables
        private readonly FormGPS mf = null;
        public FormNewFieldCustom(Form _callingForm)
        {
            mf = _callingForm as FormGPS;
            InitializeComponent();
            InitRefFields();
        }

        private void InitRefFields()
        {
            listOfRefFieldsCmb.DisplayMember = "desc";
            listOfRefFieldsCmb.Items.Clear();
            listOfRefFieldsCmb.Items.AddRange(GetListOfRefFields().ToArray());
        }

        private void FormNewFieldCustom_Load(object sender, EventArgs e)
        {
            // Set the form to the top left corner of the screen
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(0, 0);
            this.Size = new Size(300, 200);
        }


        private void FormFieldDir_Load(object sender, EventArgs e)
        {
            btnSave.Enabled = false;

            if (!mf.IsOnScreen(Location, Size, 1))
            {
                Top = 0;
                Left = 0;
            }
        }

        private void tboxFieldName_TextChanged(object sender, EventArgs e)
        {
            TextBox textboxSender = (TextBox)sender;
            int cursorPosition = textboxSender.SelectionStart;
            textboxSender.Text = Regex.Replace(textboxSender.Text, glm.fileRegex, "");
            textboxSender.SelectionStart = cursorPosition;

            if (String.IsNullOrEmpty(tboxFieldName.Text.Trim()))
            {
                btnSave.Enabled = false;
            }
            else
            {
                btnSave.Enabled = true;
            }
        }

        private void btnSerialCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnAddDate_Click(object sender, EventArgs e)
        {
            tboxFieldName.Text += " " + DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        }

        private void btnAddTime_Click(object sender, EventArgs e)
        {
            tboxFieldName.Text += " " + DateTime.Now.ToString("HH-mm", CultureInfo.InvariantCulture);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //fill something in
            if (String.IsNullOrEmpty(tboxFieldName.Text.Trim()))
            {
                return;
            }

            if (mf.isJobStarted) mf.FileSaveEverythingBeforeClosingField();

            //append date time to name
            var taskName = tboxFieldName.Text.Trim();
            mf.currentFieldDirectory = taskName;

            //get the directory and make sure it exists, create if not
            string dirNewField = mf.fieldsDirectory + mf.currentFieldDirectory + "\\";

            mf.menustripLanguage.Enabled = false;
            //if no template set just make a new file.
            try
            {
                var selectedRefField = listOfRefFieldsCmb.SelectedItem as RefField;
                if (selectedRefField == null)
                {
                    MessageBox.Show("No selected ref field, try again", gStr.gsError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                CreateTaskDir(taskName);
                CreateTaskFiles(selectedRefField, dirNewField);
                mf.FileOpenField(dirNewField + "\\Field.txt");

            }
            catch (Exception ex)
            {
                mf.WriteErrorLog("Creating new field " + ex);

                MessageBox.Show(gStr.gsError, ex.ToString());
                mf.currentFieldDirectory = "";
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void tboxFieldName_Click(object sender, EventArgs e)
        {
            if (mf.isKeyboardOn)
            {
                mf.KeyboardToText((TextBox)sender, this);
                btnSerialCancel.Focus();
            }
        }

        private void tboxTask_Click(object sender, EventArgs e)
        {
            if (mf.isKeyboardOn)
            {
                mf.KeyboardToText((TextBox)sender, this);
                btnSerialCancel.Focus();
            }
        }

        private void tboxVehicle_Click(object sender, EventArgs e)
        {
            if (mf.isKeyboardOn)
            {
                mf.KeyboardToText((TextBox)sender, this);
                btnSerialCancel.Focus();
            }
        }

        private string GetDBFieldConnectionString()
        {
            string subPathToDbs = "dbs\\fields.db";
            var pathToDb = $"{mf.baseDirectory}{subPathToDbs}";
            string connectionString = $"Data Source={pathToDb};Version=3;";
            return connectionString;
        }

        private List<RefField> GetListOfRefFields()
        {
            List<RefField> list = new List<RefField>();
            try
            {

                using (SQLiteConnection connection = new SQLiteConnection(GetDBFieldConnectionString()))
                {
                    connection.Open();
                    var lat = mf.pn.latitude;
                    var lon = mf.pn.longitude;

                    string sql = $"SELECT *, ((lat - {lat})*(lat - {lat}) + (lon - {lon})*(lon - {lon})) AS distance FROM ref_fields ORDER BY distance ASC;";
                    using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var rf = new RefField()
                            {
                                name = reader["name"].ToString(),
                                id = Convert.ToInt32(reader["id"]),
                                distance = Convert.ToDouble(reader["distance"]),
                                lat = Convert.ToDouble(reader["lat"]),
                                lon = Convert.ToDouble(reader["lon"]),
                                boundary = reader["boundary"].ToString(),
                                contour = reader["contour"].ToString(),
                                elevation = reader["elevation"].ToString(),
                                field = reader["field"].ToString(),
                                flags = reader["flags"].ToString(),
                                recPath = reader["recPath"].ToString(),
                                sections = reader["sections"].ToString(),
                                abLines = reader["abLines"].ToString(),
                                curveLines = reader["curveLines"].ToString()
                            };
                            rf.desc = $"#{rf.id} {rf.name} d:{rf.distance.ToString("0.00", CultureInfo.InvariantCulture)} m";
                            list.Add(rf);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error: " + ex.Message);
                return list;
            }

            return list;
        }

        private void SetTaskName()
        {
            var time = DateTime.Now.ToString("HH-mm", CultureInfo.InvariantCulture);
            var date = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            var fieldName = listOfRefFieldsCmb.SelectedItem as RefField;

            tboxFieldName.Text = $"{fieldName?.name}_{date}_{time}_{taskNameTxt.Text}";
        }

        private void taskNameTxt_TextChanged(object sender, EventArgs e)
        {

            SetTaskName();
        }

        private void listOfRefFieldsCmb_SelectedIndexChanged(object sender, EventArgs e)
        {

            SetTaskName();
        }

        private string CreateTaskDir(string taskName)
        {
            string dirField = mf.fieldsDirectory + taskName + "\\";
            string directoryName = Path.GetDirectoryName(dirField);

            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }
            return dirField;
        }

        private void SplitAndWriteStringToFile(StreamWriter writer, string str)
        {
            string[] lines = str.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

            foreach (string line in lines)
            {
                if (line.Length == 0) continue;
                writer.WriteLine(line);
            }

        }

        private void CreateTaskFiles(RefField refField, string taskPathDir)
        {
            string myFileName = "Field.txt";
            using (StreamWriter writer = new StreamWriter(taskPathDir + myFileName))
            {
                SplitAndWriteStringToFile(writer, refField.field);
            }

            myFileName = "Elevation.txt";
            using (StreamWriter writer = new StreamWriter(taskPathDir + myFileName))
            {
                SplitAndWriteStringToFile(writer, refField.elevation);
            }

            myFileName = "Sections.txt";
            using (StreamWriter writer = new StreamWriter(taskPathDir + myFileName))
            {
                SplitAndWriteStringToFile(writer, refField.sections);
            }

            myFileName = "Boundary.txt";
            using (StreamWriter writer = new StreamWriter(taskPathDir + myFileName))
            {
                SplitAndWriteStringToFile(writer, refField.boundary);
            }

            myFileName = "Flags.txt";
            using (StreamWriter writer = new StreamWriter(taskPathDir + myFileName))
            {
                SplitAndWriteStringToFile(writer, refField.flags);
            }

            myFileName = "Contour.txt";
            using (StreamWriter writer = new StreamWriter(taskPathDir + myFileName))
            {
                SplitAndWriteStringToFile(writer, refField.contour);
            }

            myFileName = "RecPath.txt";
            using (StreamWriter writer = new StreamWriter(taskPathDir + myFileName))
            {
                SplitAndWriteStringToFile(writer, refField.recPath);
            }

            myFileName = "CurveLines.txt";
            using (StreamWriter writer = new StreamWriter(taskPathDir + myFileName))
            {
                SplitAndWriteStringToFile(writer, refField.curveLines);
            }

            myFileName = "ABLines.txt";
            using (StreamWriter writer = new StreamWriter(taskPathDir + myFileName))
            {
                SplitAndWriteStringToFile(writer, refField.abLines);
            }
        }


        private string GetFileString(string pathToFile)
        {
            string line = "";
            using (StreamReader reader = new StreamReader(pathToFile))
            {
                while (!reader.EndOfStream)
                {
                    line += reader.ReadLine() + "\r\n";
                }
            }
            return line;
        }

        private RefField ParseTaskIntoRefField(string pathToTaskDir)
        {
            RefField rf = new RefField();
            string fileName = "\\Field.txt";
            if (File.Exists(pathToTaskDir + fileName))
            {
                rf.field = GetFileString(pathToTaskDir + fileName);
            }
            fileName = "\\Elevation.txt";
            if (File.Exists(pathToTaskDir + fileName))
            {
                rf.elevation = GetFileString(pathToTaskDir + fileName);
            }
            fileName = "\\Sections.txt";
            if (File.Exists(pathToTaskDir + fileName))
            {
                rf.sections = GetFileString(pathToTaskDir + fileName);
            }
            fileName = "\\Boundary.txt";
            if (File.Exists(pathToTaskDir + fileName))
            {
                rf.boundary = GetFileString(pathToTaskDir + fileName);
            }
            fileName = "\\Flags.txt";
            if (File.Exists(pathToTaskDir + fileName))
            {
                rf.flags = GetFileString(pathToTaskDir + fileName);
            }
            fileName = "\\Contour.txt";
            if (File.Exists(pathToTaskDir + fileName))
            {
                rf.contour = GetFileString(pathToTaskDir + fileName);
            }
            fileName = "\\RecPath.txt";
            if (File.Exists(pathToTaskDir + fileName))
            {
                rf.recPath = GetFileString(pathToTaskDir + fileName);
            }
            fileName = "\\CurveLines.txt";
            if (File.Exists(pathToTaskDir + fileName))
            {
                rf.curveLines = GetFileString(pathToTaskDir + fileName);
            }
            fileName = "\\ABLines.txt";
            if (File.Exists(pathToTaskDir + fileName))
            {
                rf.abLines = GetFileString(pathToTaskDir + fileName);
            }


            return rf;
        }

        private void SaveRefFieldIntoDB(RefField rf)
        {

            using (SQLiteConnection connection = new SQLiteConnection(GetDBFieldConnectionString()))
            {
                connection.Open();
                var lat = mf.pn.latitude;
                var lon = mf.pn.longitude;

                string insertSql = "INSERT INTO ref_fields (lat, lon, name, field, boundary, contour, elevation, flags, recPath, sections, abLines, curveLines) " +
                    "VALUES (@lat, @lon, @name, @field, @boundary, @contour, @elevation, @flags, @recPath, @sections, @abLines, @curveLines)";
                using (var command = new SQLiteCommand(insertSql, connection))
                {
                    command.Parameters.AddWithValue("@lat", lat);
                    command.Parameters.AddWithValue("@lon", lon);
                    command.Parameters.AddWithValue("@name", rf.name);
                    command.Parameters.AddWithValue("@field", rf.field);
                    command.Parameters.AddWithValue("@boundary", rf.boundary);
                    command.Parameters.AddWithValue("@contour", rf.contour);
                    command.Parameters.AddWithValue("@elevation", rf.elevation);
                    command.Parameters.AddWithValue("@flags", rf.flags);
                    command.Parameters.AddWithValue("@recPath", rf.recPath);
                    command.Parameters.AddWithValue("@sections", rf.sections);
                    command.Parameters.AddWithValue("@abLines", rf.abLines);
                    command.Parameters.AddWithValue("@curveLines", rf.curveLines);

                    command.ExecuteNonQuery();
                }
            }
        }



        private void createRefFieldBtn_Click(object sender, EventArgs e)
        {
            try
            {
                string newRefFieldName = newRefFieldNameTxt.Text.Trim();
                if (String.IsNullOrEmpty(newRefFieldName))
                {
                    MessageBox.Show("No name for new ref field, try again", gStr.gsError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    if (!mf.isJobStarted)
                    {
                        MessageBox.Show("No active task, create one", gStr.gsError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else
                    {
                        mf.FileSaveEverythingBeforeClosingField();

                        var pathToTaskDictionary = mf.fieldsDirectory + mf.currentFieldDirectory;

                        var rf= ParseTaskIntoRefField(pathToTaskDictionary);
                        rf.name = newRefFieldName;
                        SaveRefFieldIntoDB(rf);

                    }


                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error: " + ex.Message);
                MessageBox.Show("Error creating ref field: " + ex.Message, gStr.gsError, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    public class RefField
    {
        public string name { get; set; }
        public int id { get; set; }
        public double distance { get; set; }
        public string desc { get; set; }
        public double lat { get; set; }
        public double lon { get; set; }
        public string boundary { get; set; }
        public string contour { get; set; }
        public string elevation { get; set; }
        public string field { get; set; }
        public string flags { get; set; }
        public string recPath { get; set; }
        public string sections { get; set; }
        public string abLines { get; set; }
        public string curveLines { get; set; }

    }
}
