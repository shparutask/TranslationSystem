namespace TranslationSystem
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.richTextQuest = new System.Windows.Forms.RichTextBox();
            this.Result = new System.Windows.Forms.Label();
            this.Question = new System.Windows.Forms.Label();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // richTextQuest
            // 
            this.richTextQuest.Location = new System.Drawing.Point(12, 60);
            this.richTextQuest.Name = "richTextQuest";
            this.richTextQuest.Size = new System.Drawing.Size(894, 46);
            this.richTextQuest.TabIndex = 1;
            this.richTextQuest.Text = "";
            // 
            // Result
            // 
            this.Result.AutoSize = true;
            this.Result.Location = new System.Drawing.Point(25, 152);
            this.Result.Name = "Result";
            this.Result.Size = new System.Drawing.Size(59, 13);
            this.Result.TabIndex = 2;
            this.Result.Text = "Результат";

            // 
            // Question
            // 
            this.Question.AutoSize = true;
            this.Question.Location = new System.Drawing.Point(12, 32);
            this.Question.Name = "Question";
            this.Question.Size = new System.Drawing.Size(203, 13);
            this.Question.TabIndex = 2;
            this.Question.Text = "Введите свой вопрос (на английском):";
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(15, 180);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(891, 263);
            this.richTextBox1.TabIndex = 3;
            this.richTextBox1.Text = "";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(712, 112);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(175, 41);
            this.button1.TabIndex = 4;
            this.button1.Text = "Выполнить";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(918, 455);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.Question);
            this.Controls.Add(this.Result);
            this.Controls.Add(this.richTextQuest);
            this.Name = "Form1";
            this.Text = "MIGRATION";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextQuest;
        private System.Windows.Forms.Label Result;
        private System.Windows.Forms.Label Question;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button button1;
    }
}

