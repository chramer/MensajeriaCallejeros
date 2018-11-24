namespace MensajeriaCallejeros
{
    partial class Form1
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.administraciónToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.administraciónDeClientesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.administraciónDeCadetesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.button1 = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.administraciónToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(781, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // administraciónToolStripMenuItem
            // 
            this.administraciónToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.administraciónDeClientesToolStripMenuItem,
            this.administraciónDeCadetesToolStripMenuItem});
            this.administraciónToolStripMenuItem.Name = "administraciónToolStripMenuItem";
            this.administraciónToolStripMenuItem.Size = new System.Drawing.Size(103, 20);
            this.administraciónToolStripMenuItem.Text = "Administración ";
            // 
            // administraciónDeClientesToolStripMenuItem
            // 
            this.administraciónDeClientesToolStripMenuItem.Name = "administraciónDeClientesToolStripMenuItem";
            this.administraciónDeClientesToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.administraciónDeClientesToolStripMenuItem.Text = "Clientes";
            this.administraciónDeClientesToolStripMenuItem.Click += new System.EventHandler(this.administraciónDeClientesToolStripMenuItem_Click);
            // 
            // administraciónDeCadetesToolStripMenuItem
            // 
            this.administraciónDeCadetesToolStripMenuItem.Name = "administraciónDeCadetesToolStripMenuItem";
            this.administraciónDeCadetesToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.administraciónDeCadetesToolStripMenuItem.Text = "Cadetes";
            this.administraciónDeCadetesToolStripMenuItem.Click += new System.EventHandler(this.administraciónDeCadetesToolStripMenuItem_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(694, 408);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Salir";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(781, 443);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem administraciónToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem administraciónDeClientesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem administraciónDeCadetesToolStripMenuItem;
        private System.Windows.Forms.Button button1;
    }
}

