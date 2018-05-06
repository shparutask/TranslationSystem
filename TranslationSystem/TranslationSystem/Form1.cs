using System;
using TranslationLib;
using System.Windows.Forms;
using System.Drawing;

namespace TranslationSystem
{
    public partial class Form1 : Form
    {
        QueryExecution q;

        public Form1(string db, string server)
        {
            InitializeComponent();
            q = new QueryExecution(db, server);
            richTextQuest.Font = new Font(FontFamily.GenericMonospace, 18, FontStyle.Regular);
            richTextBox1.Font = new Font(FontFamily.GenericMonospace, 18, FontStyle.Regular);
            richTextBox2.Font = new Font(FontFamily.GenericMonospace, 18, FontStyle.Regular);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            q.ExecuteQuery(richTextQuest.Text);
            richTextBox2.Text = q.Result;
            richTextBox1.Text = q.ResultQuery;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!label1.Visible)
            {
                richTextBox1.Visible = true;
                label1.Visible = true;
                button2.Text = "Спрятать запрос";
            }
            else
            {
                richTextBox1.Visible = false;
                label1.Visible = false;
                button2.Text = "Показать запрос";
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
