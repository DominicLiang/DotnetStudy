namespace _10_异步和多线程
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
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(24, 26);
            button1.Name = "button1";
            button1.Size = new Size(144, 46);
            button1.TabIndex = 0;
            button1.Text = "同步方法";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Location = new Point(24, 130);
            button2.Name = "button2";
            button2.Size = new Size(144, 46);
            button2.TabIndex = 1;
            button2.Text = "异步方法+主线程长耗时";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button3
            // 
            button3.Location = new Point(644, 323);
            button3.Name = "button3";
            button3.Size = new Size(144, 46);
            button3.TabIndex = 2;
            button3.Text = "其他操作";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(710, 372);
            label1.Name = "label1";
            label1.Size = new Size(15, 17);
            label1.TabIndex = 3;
            label1.Text = "0";
            // 
            // button4
            // 
            button4.Location = new Point(24, 78);
            button4.Name = "button4";
            button4.Size = new Size(144, 46);
            button4.TabIndex = 4;
            button4.Text = "异步方法";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // button5
            // 
            button5.Location = new Point(644, 392);
            button5.Name = "button5";
            button5.Size = new Size(144, 46);
            button5.TabIndex = 5;
            button5.Text = "归零";
            button5.UseVisualStyleBackColor = true;
            button5.Click += button5_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
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
    }
}