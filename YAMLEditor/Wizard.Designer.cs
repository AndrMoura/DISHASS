namespace YAMLEditor
{
    partial class Wizard
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
            this.wizardPage5 = new AeroWizard.WizardPage();
            this.wizardPage4 = new AeroWizard.WizardPage();
            this.wizardPage3 = new AeroWizard.WizardPage();
            this.wizardPage2 = new AeroWizard.WizardPage();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxComponentName = new System.Windows.Forms.TextBox();
            this.wizardPage1 = new AeroWizard.WizardPage();
            this.label1 = new System.Windows.Forms.Label();
            this.stepWizardControl1 = new AeroWizard.StepWizardControl();
            this.label3 = new System.Windows.Forms.Label();
            this.wizardPage3.SuspendLayout();
            this.wizardPage2.SuspendLayout();
            this.wizardPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.stepWizardControl1)).BeginInit();
            this.SuspendLayout();
            // 
            // wizardPage5
            // 
            this.wizardPage5.Name = "wizardPage5";
            this.wizardPage5.Size = new System.Drawing.Size(602, 296);
            this.stepWizardControl1.SetStepText(this.wizardPage5, "Page Title");
            this.wizardPage5.TabIndex = 6;
            this.wizardPage5.Text = "Page Title";
            // 
            // wizardPage4
            // 
            this.wizardPage4.Name = "wizardPage4";
            this.wizardPage4.Size = new System.Drawing.Size(602, 296);
            this.stepWizardControl1.SetStepText(this.wizardPage4, "Page 4");
            this.wizardPage4.TabIndex = 5;
            this.wizardPage4.Text = "Page Title";
            // 
            // wizardPage3
            // 
            this.wizardPage3.Controls.Add(this.label3);
            this.wizardPage3.Name = "wizardPage3";
            this.wizardPage3.Size = new System.Drawing.Size(602, 296);
            this.stepWizardControl1.SetStepText(this.wizardPage3, "Page 3");
            this.wizardPage3.TabIndex = 4;
            this.wizardPage3.Text = "Page Title";
            // 
            // wizardPage2
            // 
            this.wizardPage2.Controls.Add(this.textBoxComponentName);
            this.wizardPage2.Controls.Add(this.label2);
            this.wizardPage2.Name = "wizardPage2";
            this.wizardPage2.Size = new System.Drawing.Size(602, 296);
            this.stepWizardControl1.SetStepText(this.wizardPage2, "Page 2");
            this.wizardPage2.TabIndex = 3;
            this.wizardPage2.Text = "Page Title";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.World);
            this.label2.Location = new System.Drawing.Point(3, 128);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(155, 21);
            this.label2.TabIndex = 0;
            this.label2.Text = "Component name: ";
            // 
            // textBoxComponentName
            // 
            this.textBoxComponentName.Location = new System.Drawing.Point(174, 130);
            this.textBoxComponentName.Name = "textBoxComponentName";
            this.textBoxComponentName.Size = new System.Drawing.Size(235, 23);
            this.textBoxComponentName.TabIndex = 1;
            // 
            // wizardPage1
            // 
            this.wizardPage1.Controls.Add(this.label1);
            this.wizardPage1.Name = "wizardPage1";
            this.wizardPage1.Size = new System.Drawing.Size(602, 296);
            this.stepWizardControl1.SetStepText(this.wizardPage1, "Page 1");
            this.wizardPage1.TabIndex = 2;
            this.wizardPage1.Text = "New Component";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(42, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "label1";
            // 
            // stepWizardControl1
            // 
            this.stepWizardControl1.BackColor = System.Drawing.Color.White;
            this.stepWizardControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.stepWizardControl1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.stepWizardControl1.Location = new System.Drawing.Point(0, 0);
            this.stepWizardControl1.Name = "stepWizardControl1";
            this.stepWizardControl1.Pages.Add(this.wizardPage1);
            this.stepWizardControl1.Pages.Add(this.wizardPage2);
            this.stepWizardControl1.Pages.Add(this.wizardPage3);
            this.stepWizardControl1.Pages.Add(this.wizardPage4);
            this.stepWizardControl1.Pages.Add(this.wizardPage5);
            this.stepWizardControl1.Size = new System.Drawing.Size(800, 450);
            this.stepWizardControl1.StepListFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
            this.stepWizardControl1.TabIndex = 0;
            this.stepWizardControl1.Text = "Wizard Title";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
            this.label3.Location = new System.Drawing.Point(3, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(81, 21);
            this.label3.TabIndex = 0;
            this.label3.Text = "Properties";
            // 
            // Wizard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.stepWizardControl1);
            this.Name = "Wizard";
            this.Text = "Wizard";
            this.wizardPage3.ResumeLayout(false);
            this.wizardPage3.PerformLayout();
            this.wizardPage2.ResumeLayout(false);
            this.wizardPage2.PerformLayout();
            this.wizardPage1.ResumeLayout(false);
            this.wizardPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.stepWizardControl1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private AeroWizard.WizardPage wizardPage5;
        private AeroWizard.StepWizardControl stepWizardControl1;
        private AeroWizard.WizardPage wizardPage1;
        private System.Windows.Forms.Label label1;
        private AeroWizard.WizardPage wizardPage2;
        private System.Windows.Forms.TextBox textBoxComponentName;
        private System.Windows.Forms.Label label2;
        private AeroWizard.WizardPage wizardPage3;
        private System.Windows.Forms.Label label3;
        private AeroWizard.WizardPage wizardPage4;
    }
}