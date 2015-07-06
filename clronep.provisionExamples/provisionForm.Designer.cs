namespace clronep.provisionExamples
{
    partial class provisionForm
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
            this.vendorNameTextBox = new System.Windows.Forms.TextBox();
            this.vendorTokenTextBox = new System.Windows.Forms.TextBox();
            this.cloneCIKTextBox = new System.Windows.Forms.TextBox();
            this.clonePortalCIKTextBox = new System.Windows.Forms.TextBox();
            this.portalCIKTextBox = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // vendorNameTextBox
            // 
            this.vendorNameTextBox.Location = new System.Drawing.Point(12, 29);
            this.vendorNameTextBox.Name = "vendorNameTextBox";
            this.vendorNameTextBox.Size = new System.Drawing.Size(260, 20);
            this.vendorNameTextBox.TabIndex = 0;
            // 
            // vendorTokenTextBox
            // 
            this.vendorTokenTextBox.Location = new System.Drawing.Point(12, 68);
            this.vendorTokenTextBox.Name = "vendorTokenTextBox";
            this.vendorTokenTextBox.Size = new System.Drawing.Size(260, 20);
            this.vendorTokenTextBox.TabIndex = 1;
            // 
            // cloneCIKTextBox
            // 
            this.cloneCIKTextBox.Location = new System.Drawing.Point(11, 107);
            this.cloneCIKTextBox.Name = "cloneCIKTextBox";
            this.cloneCIKTextBox.Size = new System.Drawing.Size(259, 20);
            this.cloneCIKTextBox.TabIndex = 2;
            // 
            // clonePortalCIKTextBox
            // 
            this.clonePortalCIKTextBox.Location = new System.Drawing.Point(11, 150);
            this.clonePortalCIKTextBox.Name = "clonePortalCIKTextBox";
            this.clonePortalCIKTextBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.clonePortalCIKTextBox.Size = new System.Drawing.Size(260, 20);
            this.clonePortalCIKTextBox.TabIndex = 3;
            this.clonePortalCIKTextBox.TextChanged += new System.EventHandler(this.clonePortalCIKTextBox_TextChanged);
            // 
            // portalCIKTextBox
            // 
            this.portalCIKTextBox.Location = new System.Drawing.Point(12, 189);
            this.portalCIKTextBox.Name = "portalCIKTextBox";
            this.portalCIKTextBox.Size = new System.Drawing.Size(259, 20);
            this.portalCIKTextBox.TabIndex = 4;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 215);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(111, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "Submit and Run";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Vendor Name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Vendor Token Here";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 91);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(54, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Clone CIK";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 134);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(211, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Clone Portal CIK (if managing by sharecode";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 173);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(54, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Portal CIK";
            // 
            // provisionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.portalCIKTextBox);
            this.Controls.Add(this.clonePortalCIKTextBox);
            this.Controls.Add(this.cloneCIKTextBox);
            this.Controls.Add(this.vendorTokenTextBox);
            this.Controls.Add(this.vendorNameTextBox);
            this.Name = "provisionForm";
            this.Text = "Provision Example Form";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox vendorNameTextBox;
        private System.Windows.Forms.TextBox vendorTokenTextBox;
        private System.Windows.Forms.TextBox cloneCIKTextBox;
        private System.Windows.Forms.TextBox clonePortalCIKTextBox;
        private System.Windows.Forms.TextBox portalCIKTextBox;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
    }
}