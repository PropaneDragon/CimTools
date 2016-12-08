namespace TranslationHelper
{
    partial class TranslationHelperWindow
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
            this.referenceTranslation = new System.Windows.Forms.TextBox();
            this.convertedTranslation = new System.Windows.Forms.TextBox();
            this.neatenButton = new System.Windows.Forms.Button();
            this.copyButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // referenceTranslation
            // 
            this.referenceTranslation.AcceptsReturn = true;
            this.referenceTranslation.AcceptsTab = true;
            this.referenceTranslation.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.referenceTranslation.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.referenceTranslation.Location = new System.Drawing.Point(12, 12);
            this.referenceTranslation.Multiline = true;
            this.referenceTranslation.Name = "referenceTranslation";
            this.referenceTranslation.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.referenceTranslation.Size = new System.Drawing.Size(499, 619);
            this.referenceTranslation.TabIndex = 0;
            this.referenceTranslation.WordWrap = false;
            // 
            // convertedTranslation
            // 
            this.convertedTranslation.AcceptsReturn = true;
            this.convertedTranslation.AcceptsTab = true;
            this.convertedTranslation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.convertedTranslation.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.convertedTranslation.Location = new System.Drawing.Point(517, 12);
            this.convertedTranslation.Multiline = true;
            this.convertedTranslation.Name = "convertedTranslation";
            this.convertedTranslation.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.convertedTranslation.Size = new System.Drawing.Size(577, 619);
            this.convertedTranslation.TabIndex = 0;
            this.convertedTranslation.WordWrap = false;
            // 
            // neatenButton
            // 
            this.neatenButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.neatenButton.Location = new System.Drawing.Point(1019, 637);
            this.neatenButton.Name = "neatenButton";
            this.neatenButton.Size = new System.Drawing.Size(75, 23);
            this.neatenButton.TabIndex = 1;
            this.neatenButton.Text = "Combine";
            this.neatenButton.UseVisualStyleBackColor = true;
            this.neatenButton.Click += new System.EventHandler(this.neatenButton_Click);
            // 
            // copyButton
            // 
            this.copyButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.copyButton.Location = new System.Drawing.Point(938, 637);
            this.copyButton.Name = "copyButton";
            this.copyButton.Size = new System.Drawing.Size(75, 23);
            this.copyButton.TabIndex = 1;
            this.copyButton.Text = "Copy";
            this.copyButton.UseVisualStyleBackColor = true;
            this.copyButton.Click += new System.EventHandler(this.copyButton_Click);
            // 
            // TranslationHelperWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1108, 672);
            this.Controls.Add(this.copyButton);
            this.Controls.Add(this.neatenButton);
            this.Controls.Add(this.convertedTranslation);
            this.Controls.Add(this.referenceTranslation);
            this.Name = "TranslationHelperWindow";
            this.Text = "Translation Helper";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox referenceTranslation;
        private System.Windows.Forms.TextBox convertedTranslation;
        private System.Windows.Forms.Button neatenButton;
        private System.Windows.Forms.Button copyButton;
    }
}

