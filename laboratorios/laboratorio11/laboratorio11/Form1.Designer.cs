﻿namespace laboratorio11
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
            lblHelloWorld = new Label();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(67, 223);
            button1.Name = "button1";
            button1.Size = new Size(131, 23);
            button1.TabIndex = 0;
            button1.Text = "btnClick this";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // lblHelloWorld
            // 
            lblHelloWorld.AutoSize = true;
            lblHelloWorld.Location = new Point(90, 262);
            lblHelloWorld.Name = "lblHelloWorld";
            lblHelloWorld.Size = new Size(80, 15);
            lblHelloWorld.TabIndex = 1;
            lblHelloWorld.Text = "lblHelloWorld";
            lblHelloWorld.Click += label1_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(lblHelloWorld);
            Controls.Add(button1);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private Label lblHelloWorld;
    }
}
