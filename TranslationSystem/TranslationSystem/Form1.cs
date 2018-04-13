using System;
using TranslationLib;
using System.Windows.Forms;
using System.Drawing;

namespace TranslationSystem
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            richTextBox1.Font = new Font(FontFamily.GenericMonospace, 18, FontStyle.Italic);
            richTextBox2.Font = new Font(FontFamily.GenericMonospace, 18, FontStyle.Italic);
            richTextQuest.Font = new Font(FontFamily.GenericMonospace, 18, FontStyle.Italic);
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            var t = new Translation();
            var q = new QueryExecution();

            string result = t.ToQuery(richTextQuest.Text);            
            q.ExecuteQuery(result);
            richTextBox1.Text = q.Result;
            richTextBox2.Text = q.ResultQuery;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!label1.Visible)
            {
                richTextBox2.Visible = true;
                label1.Visible = true;
                button2.Text = "Спрятать запрос";
            }
            else
            {
                richTextBox2.Visible = false;
                label1.Visible = false;
                button2.Text = "Показать запрос";
            }
        }
    }
}
