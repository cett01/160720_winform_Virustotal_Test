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
using VirusTotalNET;
using VirusTotalNET.Objects;

namespace Virustotal_Test
{
    public partial class Form1 : Form
    {
        string path = "";
        VirusTotal virustotal = new VirusTotal("4f557e00e17d03c104aca2ab42245d2bfe9b488410289e6fdaf2c31143f83922");
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnChoose_Click(object sender, EventArgs e)
        {

            OpenFileDialog ofd = new OpenFileDialog();
            if(ofd.ShowDialog() == DialogResult.OK)
            {
                path = ofd.FileName;
            }
            lblPath.Text = path;


        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(path))
            {
      
                ScanResult myScanResult = virustotal.ScanFile(new System.IO.FileInfo(path));
                MessageBox.Show(myScanResult.VerboseMsg);
         
            }
            else
            {
                MessageBox.Show("You must select a file");
            }
            
        }

        private void btnGetReport_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(path))
            {
                lstViewScanResult.Items.Clear();
                FileReport fileReport = virustotal.GetFileReport(File.ReadAllBytes(path));
                ReportResponseCode x= fileReport.ResponseCode;
         
                if ((long)x==-2)
                {
                    MessageBox.Show("Wait Please.  The requested item is still queued for analysis.");
                }
                  else if ((long)x==-1)
                {
                    MessageBox.Show("The item you searched for was not present in VirusTotal's dataset.");
                }
                     else if ((long)x==0)
                {
                    MessageBox.Show("The item you searched for was not present in VirusTotal's dataset.");
                }
                else if ((long)x ==1)
                {
                    foreach (var item in fileReport.Scans.Reverse())
                    {
                        ListViewItem temp = new ListViewItem();
                        temp.Text = item.Key;
                        temp.SubItems.Add(item.Value.Detected.ToString());
                        temp.SubItems.Add(item.Value.Result);
                        lstViewScanResult.Items.Add(temp);

                    }
                }
                else
                    MessageBox.Show("Again later.");
            
            }
            else
            {
                MessageBox.Show("You must select a file");
            }
        }
    }
}
