
namespace BeautySalonAdmin
{
    partial class MainMenu
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainMenu));
            this.ExitButton = new System.Windows.Forms.Button();
            this.CabButton = new System.Windows.Forms.Button();
            this.SprManagement = new System.Windows.Forms.Button();
            this.DocManagement = new System.Windows.Forms.Button();
            this.Reports = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ExitButton
            // 
            this.ExitButton.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ExitButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.ExitButton.Font = new System.Drawing.Font("Georgia", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ExitButton.Location = new System.Drawing.Point(12, 209);
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.Size = new System.Drawing.Size(122, 33);
            this.ExitButton.TabIndex = 0;
            this.ExitButton.Text = "Выход";
            this.ExitButton.UseVisualStyleBackColor = false;
            this.ExitButton.Click += new System.EventHandler(this.ExitButton_Click);
            // 
            // CabButton
            // 
            this.CabButton.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.CabButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CabButton.Font = new System.Drawing.Font("Georgia", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.CabButton.Location = new System.Drawing.Point(78, 12);
            this.CabButton.Name = "CabButton";
            this.CabButton.Size = new System.Drawing.Size(229, 33);
            this.CabButton.TabIndex = 1;
            this.CabButton.Text = "Личный кабинет";
            this.CabButton.UseVisualStyleBackColor = false;
            this.CabButton.Click += new System.EventHandler(this.CabButton_Click);
            // 
            // SprManagement
            // 
            this.SprManagement.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.SprManagement.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SprManagement.Font = new System.Drawing.Font("Georgia", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.SprManagement.Location = new System.Drawing.Point(78, 51);
            this.SprManagement.Name = "SprManagement";
            this.SprManagement.Size = new System.Drawing.Size(229, 33);
            this.SprManagement.TabIndex = 2;
            this.SprManagement.Text = "Управление справочниками";
            this.SprManagement.UseVisualStyleBackColor = false;
            this.SprManagement.Click += new System.EventHandler(this.SprManagement_Click);
            // 
            // DocManagement
            // 
            this.DocManagement.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.DocManagement.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.DocManagement.Font = new System.Drawing.Font("Georgia", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.DocManagement.Location = new System.Drawing.Point(78, 90);
            this.DocManagement.Name = "DocManagement";
            this.DocManagement.Size = new System.Drawing.Size(229, 33);
            this.DocManagement.TabIndex = 3;
            this.DocManagement.Text = "Управление документами";
            this.DocManagement.UseVisualStyleBackColor = false;
            this.DocManagement.Click += new System.EventHandler(this.DocManagement_Click);
            // 
            // Reports
            // 
            this.Reports.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.Reports.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Reports.Font = new System.Drawing.Font("Georgia", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Reports.Location = new System.Drawing.Point(78, 129);
            this.Reports.Name = "Reports";
            this.Reports.Size = new System.Drawing.Size(229, 33);
            this.Reports.TabIndex = 4;
            this.Reports.Text = "Формирование отчетов";
            this.Reports.UseVisualStyleBackColor = false;
            this.Reports.Click += new System.EventHandler(this.Reports_Click);
            // 
            // MainMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(365, 250);
            this.Controls.Add(this.Reports);
            this.Controls.Add(this.DocManagement);
            this.Controls.Add(this.SprManagement);
            this.Controls.Add(this.CabButton);
            this.Controls.Add(this.ExitButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainMenu";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "АИС \"Рабочее место администратора салона красоты\" - Главное меню";
            this.Load += new System.EventHandler(this.MainMenu_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button ExitButton;
        private System.Windows.Forms.Button CabButton;
        private System.Windows.Forms.Button SprManagement;
        private System.Windows.Forms.Button DocManagement;
        private System.Windows.Forms.Button Reports;
    }
}