using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Upswing
{
    public partial class UpswingApp : Form
    {
        const string AppDirectory = "Upswing";
        const string AppFilename = "records.json";

        private GeocodingEngine geocodingEngine;
        private readonly string _appDir;
        private readonly string _filePath;
        private List<Record> _records = new List<Record>();

        public UpswingApp()
        {
            InitializeComponent();
            geocodingEngine = new GeocodingEngine();
            _appDir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + $@"\{AppDirectory}";
            _filePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + $@"\{AppDirectory}\{AppFilename}";

            //set most requested at loading
            LoadRecords();
            SetTopRequested();
        }

        private void LoadRecords()
        {
            //create a directory 
            Directory.CreateDirectory(_appDir);
            if (!File.Exists(_filePath))
                File.WriteAllText(_filePath, "");
            //read the directory
            var fileValue = File.ReadAllText(_filePath);
            if (fileValue != "")
                _records = JsonConvert.DeserializeObject<List<Record>>(fileValue);
        }

        private void SaveRecords()
        {
            //record request to Json
            var text = JsonConvert.SerializeObject(_records);
            File.WriteAllText(_filePath, text);
        }

        private void SetTopRequested()
        {
            //Display the most requested
            var top = _records.OrderByDescending(r => r.Count).FirstOrDefault();
            if (top != null)
                label6.Text = top.Address;
            else
                label6.Text = "No most Requested";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string street = textBox1.Text;
                string city = textBox2.Text;
                string zip = textBox3.Text;
                string state = textBox4.Text;
                string address = $"{street} {city} {zip} {state}";
                Last.Items.Add($"{street} {city} {zip} {state}");

                var url = geocodingEngine.GetStaticMapUrl(address);
                webBrowser1.DocumentText = url;

                var single = _records.SingleOrDefault(r => r.Url == url);
                //not present
                if (single == null)
                {
                    var record = new Record
                    {
                        Address = address,
                        Count = 1,
                        Url = url
                    };

                    _records.Add(record);
                }
                else
                {
                    single.Count++;
                }

                SetTopRequested();
                SaveRecords();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error\r\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public class Record
        {
            public string Url { get; set; }
            public int Count { get; set; }
            public string Address { get; set; }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            Last.Items.Clear();
        }
    }
}
