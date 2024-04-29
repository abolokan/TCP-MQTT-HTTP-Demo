namespace TCPClientApp
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
            textBox1 = new TextBox();
            button3 = new Button();
            buttonChangeDeviceId = new Button();
            buttonSendRaw = new Button();
            buttonError = new Button();
            buttonOnline = new Button();
            buttonDisconnect = new Button();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(145, 38);
            button1.Name = "button1";
            button1.Size = new Size(91, 43);
            button1.TabIndex = 0;
            button1.Text = "Connect";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click_1;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(45, 266);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(163, 23);
            textBox1.TabIndex = 2;
            textBox1.Text = "test";
            // 
            // button3
            // 
            button3.Location = new Point(45, 105);
            button3.Name = "button3";
            button3.Size = new Size(299, 23);
            button3.TabIndex = 3;
            button3.Text = "Send [data]";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click_1;
            // 
            // buttonChangeDeviceId
            // 
            buttonChangeDeviceId.Location = new Point(45, 211);
            buttonChangeDeviceId.Name = "buttonChangeDeviceId";
            buttonChangeDeviceId.Size = new Size(299, 23);
            buttonChangeDeviceId.TabIndex = 3;
            buttonChangeDeviceId.Text = "Change Device ID [device_id]";
            buttonChangeDeviceId.UseVisualStyleBackColor = true;
            buttonChangeDeviceId.Click += buttonChangeDeviceId_Click;
            // 
            // buttonSendRaw
            // 
            buttonSendRaw.Location = new Point(45, 168);
            buttonSendRaw.Name = "buttonSendRaw";
            buttonSendRaw.Size = new Size(299, 23);
            buttonSendRaw.TabIndex = 4;
            buttonSendRaw.Text = "Send [raw]";
            buttonSendRaw.UseVisualStyleBackColor = true;
            buttonSendRaw.Click += buttonSendRaw_Click;
            // 
            // buttonError
            // 
            buttonError.Location = new Point(45, 306);
            buttonError.Name = "buttonError";
            buttonError.Size = new Size(299, 23);
            buttonError.TabIndex = 5;
            buttonError.Text = "Send [error]";
            buttonError.UseVisualStyleBackColor = true;
            buttonError.Click += buttonError_Click;
            // 
            // buttonOnline
            // 
            buttonOnline.Location = new Point(245, 266);
            buttonOnline.Name = "buttonOnline";
            buttonOnline.Size = new Size(99, 23);
            buttonOnline.TabIndex = 6;
            buttonOnline.Text = "Send [online]";
            buttonOnline.UseVisualStyleBackColor = true;
            buttonOnline.Click += buttonOnline_Click;
            // 
            // buttonDisconnect
            // 
            buttonDisconnect.Location = new Point(45, 352);
            buttonDisconnect.Name = "buttonDisconnect";
            buttonDisconnect.Size = new Size(299, 23);
            buttonDisconnect.TabIndex = 5;
            buttonDisconnect.Text = "Send [disconnected]";
            buttonDisconnect.UseVisualStyleBackColor = true;
            buttonDisconnect.Click += buttonDisconnect_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(426, 410);
            Controls.Add(buttonOnline);
            Controls.Add(buttonDisconnect);
            Controls.Add(buttonError);
            Controls.Add(buttonSendRaw);
            Controls.Add(buttonChangeDeviceId);
            Controls.Add(button3);
            Controls.Add(textBox1);
            Controls.Add(button1);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private TextBox textBox1;
        private Button button3;
        private Button buttonChangeDeviceId;
        private Button buttonSendRaw;
        private Button buttonError;
        private Button buttonOnline;
        private Button buttonDisconnect;
    }
}
