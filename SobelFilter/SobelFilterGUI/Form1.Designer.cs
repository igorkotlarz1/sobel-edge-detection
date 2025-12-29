namespace SobelFilterGUI
{
    partial class Form1
    {
        /// <summary>
        /// Wymagana zmienna projektanta.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Wyczyśc wszystkie używane zasoby.
        /// </summary>
        /// <param name="disposing">prawda, jeżeli zarzadzane zasoby powinny zostac zlikwidowane; Falsz w przeciwnym wypadku.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Kod generowany przez Projektanta formularzy systemu Windows

        /// <summary>
        /// Metoda wymagana do obslugi projektanta — nie należy modyfikowac
        /// jej zawartości w edytorze kodu.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBoxImp = new System.Windows.Forms.GroupBox();
            this.radioButtonCPP = new System.Windows.Forms.RadioButton();
            this.radioButtonASM = new System.Windows.Forms.RadioButton();
            this.comboBoxThreads = new System.Windows.Forms.ComboBox();
            this.buttonRun = new System.Windows.Forms.Button();
            this.buttonLoad = new System.Windows.Forms.Button();
            this.labelThreads = new System.Windows.Forms.Label();
            this.labelImp = new System.Windows.Forms.Label();
            this.pictureBoxOriginal = new System.Windows.Forms.PictureBox();
            this.pictureBoxProcessed = new System.Windows.Forms.PictureBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.labelTime = new System.Windows.Forms.Label();
            this.groupBoxTime = new System.Windows.Forms.GroupBox();
            this.buttonSave = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonProbe = new System.Windows.Forms.Button();
            this.testLab = new System.Windows.Forms.Label();
            this.labelTimeSobel = new System.Windows.Forms.Label();
            this.groupBoxImp.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxOriginal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxProcessed)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBoxTime.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxImp
            // 
            this.groupBoxImp.Controls.Add(this.radioButtonCPP);
            this.groupBoxImp.Controls.Add(this.radioButtonASM);
            this.groupBoxImp.Location = new System.Drawing.Point(773, 468);
            this.groupBoxImp.Name = "groupBoxImp";
            this.groupBoxImp.Size = new System.Drawing.Size(143, 50);
            this.groupBoxImp.TabIndex = 0;
            this.groupBoxImp.TabStop = false;
            // 
            // radioButtonCPP
            // 
            this.radioButtonCPP.AutoSize = true;
            this.radioButtonCPP.Checked = true;
            this.radioButtonCPP.Location = new System.Drawing.Point(20, 19);
            this.radioButtonCPP.Name = "radioButtonCPP";
            this.radioButtonCPP.Size = new System.Drawing.Size(44, 17);
            this.radioButtonCPP.TabIndex = 0;
            this.radioButtonCPP.TabStop = true;
            this.radioButtonCPP.Text = "C++";
            this.radioButtonCPP.UseVisualStyleBackColor = true;
            this.radioButtonCPP.CheckedChanged += new System.EventHandler(this.radioButtonCPP_CheckedChanged);
            // 
            // radioButtonASM
            // 
            this.radioButtonASM.AutoSize = true;
            this.radioButtonASM.Location = new System.Drawing.Point(70, 19);
            this.radioButtonASM.Name = "radioButtonASM";
            this.radioButtonASM.Size = new System.Drawing.Size(48, 17);
            this.radioButtonASM.TabIndex = 1;
            this.radioButtonASM.Text = "ASM";
            this.radioButtonASM.UseVisualStyleBackColor = true;
            this.radioButtonASM.CheckedChanged += new System.EventHandler(this.radioButtonASM_CheckedChanged);
            // 
            // comboBoxThreads
            // 
            this.comboBoxThreads.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.comboBoxThreads.FormattingEnabled = true;
            this.comboBoxThreads.Items.AddRange(new object[] {
            "1",
            "2",
            "4",
            "8",
            "16",
            "32",
            "64"});
            this.comboBoxThreads.Location = new System.Drawing.Point(497, 484);
            this.comboBoxThreads.Name = "comboBoxThreads";
            this.comboBoxThreads.Size = new System.Drawing.Size(143, 24);
            this.comboBoxThreads.TabIndex = 0;
            this.comboBoxThreads.Text = "Number of Threads";
            this.comboBoxThreads.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // buttonRun
            // 
            this.buttonRun.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.buttonRun.Location = new System.Drawing.Point(792, 556);
            this.buttonRun.Name = "buttonRun";
            this.buttonRun.Size = new System.Drawing.Size(124, 33);
            this.buttonRun.TabIndex = 1;
            this.buttonRun.Text = "Run";
            this.buttonRun.UseVisualStyleBackColor = true;
            this.buttonRun.Click += new System.EventHandler(this.buttonRun_Click);
            // 
            // buttonLoad
            // 
            this.buttonLoad.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.buttonLoad.Location = new System.Drawing.Point(428, 556);
            this.buttonLoad.Name = "buttonLoad";
            this.buttonLoad.Size = new System.Drawing.Size(124, 33);
            this.buttonLoad.TabIndex = 2;
            this.buttonLoad.Text = "Load image";
            this.buttonLoad.UseVisualStyleBackColor = true;
            this.buttonLoad.Click += new System.EventHandler(this.buttonLoad_Click);
            // 
            // labelThreads
            // 
            this.labelThreads.AutoSize = true;
            this.labelThreads.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelThreads.Location = new System.Drawing.Point(425, 487);
            this.labelThreads.Name = "labelThreads";
            this.labelThreads.Size = new System.Drawing.Size(61, 16);
            this.labelThreads.TabIndex = 3;
            this.labelThreads.Text = "Threads:";
            this.labelThreads.Click += new System.EventHandler(this.label1_Click);
            // 
            // labelImp
            // 
            this.labelImp.AutoSize = true;
            this.labelImp.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelImp.Location = new System.Drawing.Point(657, 487);
            this.labelImp.Name = "labelImp";
            this.labelImp.Size = new System.Drawing.Size(101, 16);
            this.labelImp.TabIndex = 4;
            this.labelImp.Text = "Implementation:";
            // 
            // pictureBoxOriginal
            // 
            this.pictureBoxOriginal.Location = new System.Drawing.Point(20, 27);
            this.pictureBoxOriginal.Name = "pictureBoxOriginal";
            this.pictureBoxOriginal.Size = new System.Drawing.Size(360, 360);
            this.pictureBoxOriginal.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxOriginal.TabIndex = 5;
            this.pictureBoxOriginal.TabStop = false;
            // 
            // pictureBoxProcessed
            // 
            this.pictureBoxProcessed.Location = new System.Drawing.Point(20, 27);
            this.pictureBoxProcessed.Name = "pictureBoxProcessed";
            this.pictureBoxProcessed.Size = new System.Drawing.Size(360, 360);
            this.pictureBoxProcessed.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxProcessed.TabIndex = 6;
            this.pictureBoxProcessed.TabStop = false;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog1_FileOk);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.pictureBoxOriginal);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.groupBox1.Location = new System.Drawing.Point(48, 40);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(400, 405);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Before";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.pictureBoxProcessed);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.groupBox2.Location = new System.Drawing.Point(516, 40);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(400, 405);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "After";
            this.groupBox2.Enter += new System.EventHandler(this.groupBox2_Enter);
            // 
            // labelTime
            // 
            this.labelTime.AutoSize = true;
            this.labelTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelTime.Location = new System.Drawing.Point(6, 18);
            this.labelTime.Name = "labelTime";
            this.labelTime.Size = new System.Drawing.Size(122, 18);
            this.labelTime.TabIndex = 9;
            this.labelTime.Tag = "lab";
            this.labelTime.Text = "Run the program!";
            this.labelTime.Click += new System.EventHandler(this.labelTime_Click);
            // 
            // groupBoxTime
            // 
            this.groupBoxTime.Controls.Add(this.labelTimeSobel);
            this.groupBoxTime.Controls.Add(this.labelTime);
            this.groupBoxTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.groupBoxTime.Location = new System.Drawing.Point(68, 484);
            this.groupBoxTime.Name = "groupBoxTime";
            this.groupBoxTime.Size = new System.Drawing.Size(200, 66);
            this.groupBoxTime.TabIndex = 10;
            this.groupBoxTime.TabStop = false;
            this.groupBoxTime.Text = "Elapsed time";
            // 
            // buttonSave
            // 
            this.buttonSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.buttonSave.Location = new System.Drawing.Point(574, 556);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(124, 33);
            this.buttonSave.TabIndex = 11;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.FileName = "result_sobel";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label1.Location = new System.Drawing.Point(425, 521);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(136, 16);
            this.label1.TabIndex = 12;
            this.label1.Text = "Logical CPU Threads";
            this.label1.Click += new System.EventHandler(this.label1_Click_1);
            // 
            // buttonProbe
            // 
            this.buttonProbe.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.buttonProbe.Location = new System.Drawing.Point(68, 556);
            this.buttonProbe.Name = "buttonProbe";
            this.buttonProbe.Size = new System.Drawing.Size(124, 33);
            this.buttonProbe.TabIndex = 13;
            this.buttonProbe.Text = "Probe";
            this.buttonProbe.UseVisualStyleBackColor = true;
            this.buttonProbe.Click += new System.EventHandler(this.buttonProbe_Click);
            // 
            // testLab
            // 
            this.testLab.AutoSize = true;
            this.testLab.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.testLab.Location = new System.Drawing.Point(790, 521);
            this.testLab.Name = "testLab";
            this.testLab.Size = new System.Drawing.Size(126, 20);
            this.testLab.TabIndex = 14;
            this.testLab.Text = "implementaniton";
            this.testLab.Click += new System.EventHandler(this.label2_Click_1);
            // 
            // labelTimeSobel
            // 
            this.labelTimeSobel.AutoSize = true;
            this.labelTimeSobel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelTimeSobel.Location = new System.Drawing.Point(6, 39);
            this.labelTimeSobel.Name = "labelTimeSobel";
            this.labelTimeSobel.Size = new System.Drawing.Size(0, 18);
            this.labelTimeSobel.TabIndex = 15;
            this.labelTimeSobel.Tag = "lab";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(990, 625);
            this.Controls.Add(this.testLab);
            this.Controls.Add(this.buttonProbe);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.groupBoxTime);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.labelImp);
            this.Controls.Add(this.labelThreads);
            this.Controls.Add(this.buttonLoad);
            this.Controls.Add(this.buttonRun);
            this.Controls.Add(this.comboBoxThreads);
            this.Controls.Add(this.groupBoxImp);
            this.Name = "Form1";
            this.Text = "Sobel Edge Detector";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBoxImp.ResumeLayout(false);
            this.groupBoxImp.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxOriginal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxProcessed)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBoxTime.ResumeLayout(false);
            this.groupBoxTime.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxImp;
        private System.Windows.Forms.RadioButton radioButtonASM;
        private System.Windows.Forms.RadioButton radioButtonCPP;
        private System.Windows.Forms.ComboBox comboBoxThreads;
        private System.Windows.Forms.Button buttonRun;
        private System.Windows.Forms.Button buttonLoad;
        private System.Windows.Forms.Label labelThreads;
        private System.Windows.Forms.Label labelImp;
        private System.Windows.Forms.PictureBox pictureBoxOriginal;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label labelTime;
        private System.Windows.Forms.GroupBox groupBoxTime;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        public System.Windows.Forms.PictureBox pictureBoxProcessed;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonProbe;
        private System.Windows.Forms.Label testLab;
        private System.Windows.Forms.Label labelTimeSobel;
    }
}

