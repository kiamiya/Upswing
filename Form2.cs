using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Upswing
{
    public partial class UpswingApp : Form
    {
        Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\myApp\file.txt")

        private GeocodingEngine geocodingEngine;
        public UpswingApp()
        {
            InitializeComponent();
            geocodingEngine = new GeocodingEngine();
        }

        private void button1_Click(object sender, EventArgs e)
        {
           string street = textBox1.Text;
           string city = textBox2.Text;
           string zip = textBox3.Text;
           string state = textBox4.Text;

            Last.Items.Add($"{street} {city} {zip} {state}");
            


            var url = geocodingEngine.GetStaticMapUrl($"{street} {city} {zip} {state}");
            
            webBrowser1.DocumentText = url;
            try
            {
                //json
                //linq : list
                //File
                //wpf

                //transformer l'input en requête

                var valu = Properties.Settings.Default.LastRequest;
                var elements = valu.Split(',').ToList();

                //
                elements.Add("new request");

                List<string> requests = new List<string>();
                var settingValue = string.Join(",", elements);


                // File.AppendAllText("D:\\myfil.txt", "dnhwadhjwiladhjklwa");
                //var mytext = File.ReadAllText("D:\\myfil.txt");

                List<Record> records = new List<Record>();

                if (!File.Exists(FilePath))
                    File.WriteAllText(FilePath, "");

                var fileValue = File.ReadAllText(FilePath);
                if (fileValue != "")
                    records = JsonSerializer.Deserialize<List<Record>>(fileValue);


                var mupltiples = records.Where(r => r.Url == url).ToList();
                var single = records.SingleOrDefault(r => r.Url == url);

                //not present
                if (single == null)
                {
                    var record = new Record
                    {
                        Count = 1,
                        Url = url
                    };

                    records.Add(record);
                }
                else
                {
                    single.Count++;
                }

                var top = records.OrderByDescending(r => r.Count).ToList();


                var text = JsonSerializer.Serialize(records);

                File.WriteAllText(FilePath, text);

                Properties.Settings.Default.LastRequest = settingValue;
                Properties.Settings.Default.Save();


            }
            catch (Exception e)
            {
                //System.Windows.Forms.MessageBox.Show("Test", "OOps");
                //throw;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //récupérer recherche la plus faite

            //remplir les champs de From 2 ou 3 avec ta propriété

            Form3 most = new Form3();
            most.Show();
        }
        public class Record
        {
            public string Url { get; set; }
            public int Count { get; set; }
        }
    }
}
