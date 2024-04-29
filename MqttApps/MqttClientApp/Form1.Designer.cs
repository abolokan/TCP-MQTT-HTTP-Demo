namespace MqttClientApp
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
            buttonConnect = new Button();
            buttonDisconnect = new Button();
            buttonSendMessage = new Button();
            richTextBoxMessage = new RichTextBox();
            richTextBoxSubcribtions = new RichTextBox();
            label1 = new Label();
            label2 = new Label();
            buttonClearHistory = new Button();
            SuspendLayout();
            // 
            // buttonConnect
            // 
            buttonConnect.Location = new Point(318, 22);
            buttonConnect.Name = "buttonConnect";
            buttonConnect.Size = new Size(129, 23);
            buttonConnect.TabIndex = 0;
            buttonConnect.Text = "Connect";
            buttonConnect.UseVisualStyleBackColor = true;
            buttonConnect.Click += buttonConnect_Click_1;
            // 
            // buttonDisconnect
            // 
            buttonDisconnect.Location = new Point(453, 22);
            buttonDisconnect.Name = "buttonDisconnect";
            buttonDisconnect.Size = new Size(137, 23);
            buttonDisconnect.TabIndex = 1;
            buttonDisconnect.Text = "Disconnect";
            buttonDisconnect.UseVisualStyleBackColor = true;
            buttonDisconnect.Click += buttonDisconnect_Click_1;
            // 
            // buttonSendMessage
            // 
            buttonSendMessage.Location = new Point(453, 255);
            buttonSendMessage.Name = "buttonSendMessage";
            buttonSendMessage.Size = new Size(137, 23);
            buttonSendMessage.TabIndex = 2;
            buttonSendMessage.Text = "Send Message";
            buttonSendMessage.UseVisualStyleBackColor = true;
            buttonSendMessage.Click += buttonSendMessage_Click;
            // 
            // richTextBoxMessage
            // 
            richTextBoxMessage.Enabled = false;
            richTextBoxMessage.Location = new Point(3, 84);
            richTextBoxMessage.Name = "richTextBoxMessage";
            richTextBoxMessage.Size = new Size(587, 155);
            richTextBoxMessage.TabIndex = 3;
            richTextBoxMessage.Text = "test";
            // 
            // richTextBoxSubcribtions
            // 
            richTextBoxSubcribtions.Location = new Point(3, 283);
            richTextBoxSubcribtions.Name = "richTextBoxSubcribtions";
            richTextBoxSubcribtions.Size = new Size(587, 126);
            richTextBoxSubcribtions.TabIndex = 4;
            richTextBoxSubcribtions.Text = "";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(3, 66);
            label1.Name = "label1";
            label1.Size = new Size(53, 15);
            label1.TabIndex = 5;
            label1.Text = "Message";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(3, 265);
            label2.Name = "label2";
            label2.Size = new Size(73, 15);
            label2.TabIndex = 5;
            label2.Text = "Subscribtion";
            // 
            // buttonClearHistory
            // 
            buttonClearHistory.Location = new Point(453, 415);
            buttonClearHistory.Name = "buttonClearHistory";
            buttonClearHistory.Size = new Size(137, 23);
            buttonClearHistory.TabIndex = 6;
            buttonClearHistory.Text = "Clear";
            buttonClearHistory.UseVisualStyleBackColor = true;
            buttonClearHistory.Click += buttonClearHistory_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(604, 450);
            Controls.Add(buttonClearHistory);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(richTextBoxSubcribtions);
            Controls.Add(richTextBoxMessage);
            Controls.Add(buttonSendMessage);
            Controls.Add(buttonDisconnect);
            Controls.Add(buttonConnect);
            Name = "Form1";
            Text = "MQQT Client";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button buttonConnect;
        private Button buttonDisconnect;
        private Button buttonSendMessage;
        private RichTextBox richTextBoxMessage;
        private RichTextBox richTextBoxSubcribtions;
        private Label label1;
        private Label label2;
        private Button buttonClearHistory;
    }
}
