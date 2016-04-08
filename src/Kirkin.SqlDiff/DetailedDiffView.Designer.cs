namespace Kirkin.SqlDiff
{
    partial class DetailedDiffView
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DetailedDiffView));
            this.TextBox1 = new FastColoredTextBoxNS.FastColoredTextBox();
            this.TextBox2 = new FastColoredTextBoxNS.FastColoredTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.TextBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TextBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // TextBox1
            // 
            this.TextBox1.AutoCompleteBracketsList = new char[] {
        '(',
        ')',
        '{',
        '}',
        '[',
        ']',
        '\"',
        '\"',
        '\'',
        '\''};
            this.TextBox1.AutoScrollMinSize = new System.Drawing.Size(27, 14);
            this.TextBox1.BackBrush = null;
            this.TextBox1.CharHeight = 14;
            this.TextBox1.CharWidth = 8;
            this.TextBox1.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.TextBox1.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.TextBox1.Font = new System.Drawing.Font("Courier New", 9.75F);
            this.TextBox1.IsReplaceMode = false;
            this.TextBox1.Location = new System.Drawing.Point(12, 12);
            this.TextBox1.Name = "TextBox1";
            this.TextBox1.Paddings = new System.Windows.Forms.Padding(0);
            this.TextBox1.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.TextBox1.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("TextBox1.ServiceColors")));
            this.TextBox1.Size = new System.Drawing.Size(496, 711);
            this.TextBox1.TabIndex = 0;
            this.TextBox1.Zoom = 100;
            // 
            // TextBox2
            // 
            this.TextBox2.AutoCompleteBracketsList = new char[] {
        '(',
        ')',
        '{',
        '}',
        '[',
        ']',
        '\"',
        '\"',
        '\'',
        '\''};
            this.TextBox2.AutoScrollMinSize = new System.Drawing.Size(27, 14);
            this.TextBox2.BackBrush = null;
            this.TextBox2.CharHeight = 14;
            this.TextBox2.CharWidth = 8;
            this.TextBox2.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.TextBox2.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.TextBox2.Font = new System.Drawing.Font("Courier New", 9.75F);
            this.TextBox2.IsReplaceMode = false;
            this.TextBox2.Location = new System.Drawing.Point(514, 12);
            this.TextBox2.Name = "TextBox2";
            this.TextBox2.Paddings = new System.Windows.Forms.Padding(0);
            this.TextBox2.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.TextBox2.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("TextBox2.ServiceColors")));
            this.TextBox2.Size = new System.Drawing.Size(496, 711);
            this.TextBox2.TabIndex = 0;
            this.TextBox2.Zoom = 100;
            // 
            // DetailedDiffView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1032, 735);
            this.Controls.Add(this.TextBox2);
            this.Controls.Add(this.TextBox1);
            this.Name = "DetailedDiffView";
            this.Text = "DetailedDiffView";
            this.Load += new System.EventHandler(this.DetailedDiffView_Load);
            ((System.ComponentModel.ISupportInitialize)(this.TextBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TextBox2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private FastColoredTextBoxNS.FastColoredTextBox TextBox1;
        private FastColoredTextBoxNS.FastColoredTextBox TextBox2;
    }
}