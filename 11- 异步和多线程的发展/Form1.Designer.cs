namespace _11__异步和多线程的发展
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            button1 = new Button();
            button2 = new Button();
            button3 = new Button();
            label1 = new Label();
            button4 = new Button();
            button5 = new Button();
            button6 = new Button();
            button7 = new Button();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(12, 12);
            button1.Name = "button1";
            button1.Size = new Size(120, 45);
            button1.TabIndex = 0;
            button1.Text = "Thread";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Location = new Point(668, 325);
            button2.Name = "button2";
            button2.Size = new Size(120, 45);
            button2.TabIndex = 1;
            button2.Text = "+1";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button3
            // 
            button3.Location = new Point(668, 393);
            button3.Name = "button3";
            button3.Size = new Size(120, 45);
            button3.TabIndex = 2;
            button3.Text = "clear";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(719, 373);
            label1.Name = "label1";
            label1.Size = new Size(15, 17);
            label1.TabIndex = 3;
            label1.Text = "0";
            // 
            // button4
            // 
            button4.Location = new Point(12, 63);
            button4.Name = "button4";
            button4.Size = new Size(120, 45);
            button4.TabIndex = 4;
            button4.Text = "ThreadPool";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // button5
            // 
            button5.Location = new Point(12, 114);
            button5.Name = "button5";
            button5.Size = new Size(120, 45);
            button5.TabIndex = 5;
            button5.Text = "Task";
            button5.UseVisualStyleBackColor = true;
            button5.Click += button5_Click;
            // 
            // button6
            // 
            button6.Location = new Point(12, 165);
            button6.Name = "button6";
            button6.Size = new Size(120, 45);
            button6.TabIndex = 6;
            button6.Text = "TaskFactory";
            button6.UseVisualStyleBackColor = true;
            button6.Click += button6_Click;
            // 
            // button7
            // 
            button7.Location = new Point(12, 216);
            button7.Name = "button7";
            button7.Size = new Size(120, 45);
            button7.TabIndex = 7;
            button7.Text = "Parallel";
            button7.UseVisualStyleBackColor = true;
            button7.Click += button7_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(button7);
            Controls.Add(button6);
            Controls.Add(button5);
            Controls.Add(button4);
            Controls.Add(label1);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(button1);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private Button button2;
        private Button button3;
        private Label label1;
        private Button button4;
        private Button button5;
        private Button button6;
        private Button button7;
    }
}