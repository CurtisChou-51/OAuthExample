namespace OAuthExample.AppClient
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
            btnGithub = new Button();
            btnMicrosoft = new Button();
            btnLine = new Button();
            btnGoogle = new Button();
            SuspendLayout();
            // 
            // btnGithub
            // 
            btnGithub.Location = new Point(83, 158);
            btnGithub.Name = "btnGithub";
            btnGithub.Size = new Size(130, 34);
            btnGithub.TabIndex = 0;
            btnGithub.Text = "Github登入";
            btnGithub.UseVisualStyleBackColor = true;
            btnGithub.Click += btnGithub_Click;
            // 
            // btnMicrosoft
            // 
            btnMicrosoft.Location = new Point(83, 225);
            btnMicrosoft.Name = "btnMicrosoft";
            btnMicrosoft.Size = new Size(130, 34);
            btnMicrosoft.TabIndex = 1;
            btnMicrosoft.Text = "Microsoft登入";
            btnMicrosoft.UseVisualStyleBackColor = true;
            btnMicrosoft.Click += btnMicrosoft_Click;
            // 
            // btnLine
            // 
            btnLine.Location = new Point(83, 90);
            btnLine.Name = "btnLine";
            btnLine.Size = new Size(130, 34);
            btnLine.TabIndex = 2;
            btnLine.Text = "Line登入";
            btnLine.UseVisualStyleBackColor = true;
            btnLine.Click += btnLine_Click;
            // 
            // btnGoogle
            // 
            btnGoogle.Location = new Point(83, 28);
            btnGoogle.Name = "btnGoogle";
            btnGoogle.Size = new Size(130, 34);
            btnGoogle.TabIndex = 3;
            btnGoogle.Text = "Google登入";
            btnGoogle.UseVisualStyleBackColor = true;
            btnGoogle.Click += btnGoogle_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(9F, 19F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(504, 376);
            Controls.Add(btnGoogle);
            Controls.Add(btnLine);
            Controls.Add(btnMicrosoft);
            Controls.Add(btnGithub);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
        }

        #endregion

        private Button btnGithub;
        private Button btnMicrosoft;
        private Button btnLine;
        private Button btnGoogle;
    }
}
