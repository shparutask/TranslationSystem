using System;
using TranslationLib;
using System.Windows.Forms;

namespace TranslationSystem
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        

        private void button1_Click(object sender, EventArgs e)
        {
            var t = new Translation();
            string result = t.ToQuery(richTextQuest.Text);
            var q = new QueryExecution();
            richTextBox1.Text = q.ExecuteQuery(result);
        }
    }
}
