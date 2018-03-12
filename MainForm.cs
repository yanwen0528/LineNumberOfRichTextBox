using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication9
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

     

        private void MainForm_Shown(object sender, EventArgs e)
        {
            richTextBox1.showLineNumber();
           
        }

       
    }
}
