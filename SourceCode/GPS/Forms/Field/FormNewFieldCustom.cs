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
                                distance = String.IsNullOrEmpty(reader["distance"]?.ToString()) ? 999999 : Convert.ToDouble(reader["distance"]),
                                lat = String.IsNullOrEmpty(reader["lat"]?.ToString()) ? 0 : Convert.ToDouble(reader["lat"]),
                                lon = String.IsNullOrEmpty(reader["lon"]?.ToString()) ? 0 : Convert.ToDouble(reader["lon"]),
                                boundary = reader["boundary"]?.ToString(),
                                contour = reader["contour"]?.ToString(),
                                elevation = reader["elevation"]?.ToString(),
                                field = reader["field"]?.ToString(),
                                flags = reader["flags"]?.ToString(),
                                recPath = reader["recPath"]?.ToString(),
                                sections = reader["sections"]?.ToString(),
                                abLines = reader["abLines"]?.ToString(),
                                curveLines = reader["curveLines"]?.ToString(),
                                tram = reader["tram"]?.ToString(),
                                headlines = reader["headlines"]?.ToString(),
                                headland = reader["headland"]?.ToString(),
                                backPic = reader["backPic"]?.ToString(),
                                rateMap = reader["rateMap"]?.ToString(),
                                createDate = reader["create_date"]?.ToString(),
                                modDate = reader["mod_date"]?.ToString(),
                                description = reader["description"]?.ToString(),
                                area = String.IsNullOrEmpty(reader["area"]?.ToString()) ? 0 : Convert.ToDouble(reader["area"])

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

        private string GetInfoFromSelectedRefField()
        {
            var selectedRefField = listOfRefFieldsCmb.SelectedItem as RefField;
            if (selectedRefField != null)
            {
                string info = $"Name: {selectedRefField.name}\r\n" +
                    $"Distance: {selectedRefField.distance.ToString("0.00", CultureInfo.InvariantCulture)} m\r\n" +
                    $"Area: {selectedRefField.area.ToString("0.00", CultureInfo.InvariantCulture)} ha\r\n" +
                    $"Description: {selectedRefField.desc}\r\n" +
                    $"Create date: {selectedRefField.createDate}\r\n" +
                    $"Modify date: {selectedRefField.modDate}\r\n" +
                    $"ID: {selectedRefField.id}\r\n" +
                    $"Lat: {selectedRefField.lat.ToString("0.000000", CultureInfo.InvariantCulture)}\r\n" +
                    $"Lon: {selectedRefField.lon.ToString("0.000000", CultureInfo.InvariantCulture)}\r\n";
                return info;
            }
            return "";
        }

        private void listOfRefFieldsCmb_SelectedIndexChanged(object sender, EventArgs e)
        {

            SetTaskName();
            selectedRefFieldInfoRichTxtBox.Text = GetInfoFromSelectedRefField(); 
            
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
            if (refField.field != null && refField.field.Length > 0)
            {
                using (StreamWriter writer = new StreamWriter(taskPathDir + myFileName))
                {
                    SplitAndWriteStringToFile(writer, refField.field);
                }
            }

            if (refField.tram != null && refField.tram.Length > 0)
            {
                myFileName = "Tram.txt";
                using (StreamWriter writer = new StreamWriter(taskPathDir + myFileName))
                {
                    SplitAndWriteStringToFile(writer, refField.tram);
                }
            }

            if (refField.flags != null && refField.flags.Length > 0)
            {
                myFileName = "Flags.txt";
                using (StreamWriter writer = new StreamWriter(taskPathDir + myFileName))
                {
                    SplitAndWriteStringToFile(writer, refField.flags);
                }
            }
            if (refField.recPath != null && refField.recPath.Length > 0)
            {
                myFileName = "RecPath.txt";
                using (StreamWriter writer = new StreamWriter(taskPathDir + myFileName))
                {
                    SplitAndWriteStringToFile(writer, refField.recPath);
                }
            }

            if (refField.sections != null && refField.sections.Length > 0)
            {
                myFileName = "Sections.txt";
                using (StreamWriter writer = new StreamWriter(taskPathDir + myFileName))
                {
                    SplitAndWriteStringToFile(writer, refField.sections);
                }
            }
            if (refField.elevation != null && refField.elevation.Length > 0)
            {
                myFileName = "Elevation.txt";
                using (StreamWriter writer = new StreamWriter(taskPathDir + myFileName))
                {
                    SplitAndWriteStringToFile(writer, refField.elevation);
                }
            }
            if (refField.boundary != null && refField.boundary.Length > 0)
            {
                myFileName = "Boundary.txt";
                using (StreamWriter writer = new StreamWriter(taskPathDir + myFileName))
                {
                    SplitAndWriteStringToFile(writer, refField.boundary);
                }
            }
            if (refField.contour != null && refField.contour.Length > 0)
            {
                myFileName = "Contour.txt";
                using (StreamWriter writer = new StreamWriter(taskPathDir + myFileName))
                {
                    SplitAndWriteStringToFile(writer, refField.contour);
                }
            }
            if (refField.curveLines != null && refField.curveLines.Length > 0)
            {
                myFileName = "CurveLines.txt";
                using (StreamWriter writer = new StreamWriter(taskPathDir + myFileName))
                {
                    SplitAndWriteStringToFile(writer, refField.curveLines);
                }
            }
            if (refField.abLines != null && refField.abLines.Length > 0)
            {
                myFileName = "ABLines.txt";
                using (StreamWriter writer = new StreamWriter(taskPathDir + myFileName))
                {
                    SplitAndWriteStringToFile(writer, refField.abLines);
                }
            }
            if (refField.headlines != null && refField.headlines.Length > 0)
            {
                myFileName = "Headlines.txt";
                using (StreamWriter writer = new StreamWriter(taskPathDir + myFileName))
                {
                    SplitAndWriteStringToFile(writer, refField.headlines);
                }
            }
            if (refField.headland != null && refField.headland.Length > 0)
            {
                myFileName = "Headland.txt";
                using (StreamWriter writer = new StreamWriter(taskPathDir + myFileName))
                {
                    SplitAndWriteStringToFile(writer, refField.headland);
                }
            }
            if (refField.backPic != null && refField.backPic.Length > 0)
            {
                myFileName = "BackPic.txt";
                using (StreamWriter writer = new StreamWriter(taskPathDir + myFileName))
                {
                    SplitAndWriteStringToFile(writer, refField.backPic);
                }
            }
            if (refField.rateMap != null && refField.rateMap.Length > 0)
            {
                myFileName = "RateMap.txt";
                using (StreamWriter writer = new StreamWriter(taskPathDir + myFileName))
                {
                    SplitAndWriteStringToFile(writer, refField.rateMap);
                }
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
            fileName = "\\Headlines.txt";
            if (File.Exists(pathToTaskDir + fileName))
            {
                rf.headlines = GetFileString(pathToTaskDir + fileName);
            }
            fileName = "\\Headland.txt";
            if (File.Exists(pathToTaskDir + fileName))
            {
                rf.headland = GetFileString(pathToTaskDir + fileName);
            }
            fileName = "\\BackPic.txt";
            if (File.Exists(pathToTaskDir + fileName))
            {
                rf.backPic = GetFileString(pathToTaskDir + fileName);
            }
            fileName = "\\RateMap.txt";
            if (File.Exists(pathToTaskDir + fileName))
            {
                rf.rateMap = GetFileString(pathToTaskDir + fileName);
            }
            fileName = "\\Tram.txt";
            if (File.Exists(pathToTaskDir + fileName))
            {
                rf.tram = GetFileString(pathToTaskDir + fileName);
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

                string insertSql = "INSERT INTO ref_fields (lat, lon, name, field, boundary, contour, elevation, flags, recPath, sections, abLines, curveLines, tram, headlines, headland, backPic, rateMap, create_date,  description, area) " +
                    "VALUES (@lat, @lon, @name, @field, @boundary, @contour, @elevation, @flags, @recPath, @sections, @abLines, @curveLines,@tram,@headlines,@headland,@backPic,@rateMap, @createDate, @description, @area)";

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
                    command.Parameters.AddWithValue("@tram", rf.tram);
                    command.Parameters.AddWithValue("@headlines", rf.headlines);
                    command.Parameters.AddWithValue("@headland", rf.headland);
                    command.Parameters.AddWithValue("@backPic", rf.backPic);
                    command.Parameters.AddWithValue("@rateMap", rf.rateMap);
                    command.Parameters.AddWithValue("@createDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
                    //command.Parameters.AddWithValue("@modDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
                    command.Parameters.AddWithValue("@description", rf.description);
                    command.Parameters.AddWithValue("@area", rf.area);

                    command.ExecuteNonQuery();
                }
            }
        }


        private (double, double) GetBoundaryPointInWGS84(List<CBoundaryList> bndList)
        {
            double lat = 0;
            double lon = 0;
            if (bndList.First() == null || bndList.First().fenceLine.Count == 0)
            {
                return (lat, lon);
            }
            mf.pn.ConvertLocalToWGS84(bndList.First().fenceLine.First().northing, bndList.First().fenceLine.First().easting, out lat, out lon);

            return (lat, lon);
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
                        //get bounadary info before closing
                        var bndList = mf.bnd.bndList;
                        var point = GetBoundaryPointInWGS84(bndList);
                        var area = mf.bnd.bndList.FirstOrDefault()?.area / 10000 ?? 0;

                        //close field/task
                        mf.FileSaveEverythingBeforeClosingField();

                        var pathToTaskDictionary = mf.fieldsDirectory + mf.currentFieldDirectory;


                        var rf = ParseTaskIntoRefField(pathToTaskDictionary);
                        rf.name = newRefFieldName;
                        rf.area = area;
                        rf.lat = point.Item1;
                        rf.lon = point.Item2;
                        SaveRefFieldIntoDB(rf);

                    }


                }
                var form = new FormTimedMessage(2000, "Field created", "Success");

                form.Show(this);
                Close();

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
        public string tram { get; set; }
        public string headlines { get; set; }
        public string headland { get; set; }
        public string backPic { get; set; }
        public string rateMap { get; set; }
        public string createDate { get; set; }
        public string modDate { get; set; }
        public string description { get; set; }
        public double area { get; set; }



    }
}
