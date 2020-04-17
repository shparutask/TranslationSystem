using System;
using System.Drawing;
using System.Windows.Forms;

namespace TranslationSystem
{
    public partial class FormEnter : Form
    {
        public FormEnter()
        {
            InitializeComponent();
            textBox1.Font = new Font(FontFamily.GenericMonospace, 18, FontStyle.Regular);
            textBox2.Font = new Font(FontFamily.GenericMonospace, 18, FontStyle.Regular);
        }

        private void buttonEnter_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            Form1 translationForm = new Form1(textBox2.Text, textBox1.Text);
            translationForm.Visible = true;
        }
    }
}
