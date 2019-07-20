using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Photo_Resize
{
    public partial class Form1 : Form
    {
        static List<String> files=new List<string>();
        static string folder;
        public Form1()
        {
            InitializeComponent();
        }

        private void listView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keys.Delete == e.KeyCode)
            {
                foreach (ListViewItem listViewItem in ((ListView)sender).SelectedItems)
                {
                    listViewItem.Remove();
                }
            }
        }

        private void listView1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        private void listView1_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (string file in files)
                listView1.Items.Add(file);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            files.Clear();
            folder = "";
            if(folderBrowserDialog1.ShowDialog()==DialogResult.OK)
            {
                 folder = folderBrowserDialog1.SelectedPath;
                foreach (ListViewItem item in listView1.Items)
                {
                    files.Add((String)item.Text);
                }
                button1.Enabled = false;
                new Thread(Convert).Start();
            }

        }

        void Convert()
        {
            int index = 0;
            foreach (string file in files)
            {
                string filePath = Path.GetFileName(file);
                Image imgPhoto = Image.FromFile(@file);
                Bitmap image = ResizeImage.Resize.Image(imgPhoto, 640, 480);
                image.Save(Path.Combine(folder,filePath));
                index++;
                this.BeginInvoke((MethodInvoker)(() => {
                    progressBar1.Value = (int)((((float)index) / ((float)files.Count))*100);
                }));
            }
            this.BeginInvoke((MethodInvoker)(() => {
                button1.Enabled = true;
                progressBar1.Value = 0;
                MessageBox.Show("Convertion Done");
            }));
        }
}
}
