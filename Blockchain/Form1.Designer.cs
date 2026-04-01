namespace Blockchain
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.cmbConsensus = new System.Windows.Forms.ComboBox();
            this.numNodes = new System.Windows.Forms.NumericUpDown();
            this.numMalicious = new System.Windows.Forms.NumericUpDown();
            this.btnSetup = new System.Windows.Forms.Button();
            this.btnAddTx = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.dgvNodes = new System.Windows.Forms.DataGridView();
            this.lstPendingTx = new System.Windows.Forms.ListBox();
            this.dgvChain = new System.Windows.Forms.DataGridView();
            this.rtbLogs = new System.Windows.Forms.RichTextBox();
            this.picNetwork = new System.Windows.Forms.PictureBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numNodes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMalicious)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvNodes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvChain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picNetwork)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbConsensus
            // 
            this.cmbConsensus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbConsensus.FormattingEnabled = true;
            this.cmbConsensus.Location = new System.Drawing.Point(12, 29);
            this.cmbConsensus.Name = "cmbConsensus";
            this.cmbConsensus.Size = new System.Drawing.Size(150, 23);
            this.cmbConsensus.TabIndex = 0;
            // 
            // numNodes
            // 
            this.numNodes.Location = new System.Drawing.Point(12, 73);
            this.numNodes.Name = "numNodes";
            this.numNodes.Size = new System.Drawing.Size(150, 23);
            this.numNodes.TabIndex = 1;
            // 
            // numMalicious
            // 
            this.numMalicious.Location = new System.Drawing.Point(12, 117);
            this.numMalicious.Name = "numMalicious";
            this.numMalicious.Size = new System.Drawing.Size(150, 23);
            this.numMalicious.TabIndex = 2;
            // 
            // btnSetup
            // 
            this.btnSetup.Location = new System.Drawing.Point(12, 146);
            this.btnSetup.Name = "btnSetup";
            this.btnSetup.Size = new System.Drawing.Size(150, 30);
            this.btnSetup.TabIndex = 3;
            this.btnSetup.Text = "Configurar Nodos";
            this.btnSetup.UseVisualStyleBackColor = true;
            this.btnSetup.Click += new System.EventHandler(this.btnSetup_Click);
            // 
            // btnAddTx
            // 
            this.btnAddTx.Location = new System.Drawing.Point(12, 182);
            this.btnAddTx.Name = "btnAddTx";
            this.btnAddTx.Size = new System.Drawing.Size(150, 30);
            this.btnAddTx.TabIndex = 4;
            this.btnAddTx.Text = "Agregar Transacción";
            this.btnAddTx.UseVisualStyleBackColor = true;
            this.btnAddTx.Click += new System.EventHandler(this.btnAddTx_Click);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(12, 218);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(150, 30);
            this.btnStart.TabIndex = 5;
            this.btnStart.Text = "Iniciar Consenso";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // dgvNodes
            // 
            this.dgvNodes.AllowUserToAddRows = false;
            this.dgvNodes.AllowUserToDeleteRows = false;
            this.dgvNodes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvNodes.Location = new System.Drawing.Point(12, 260);
            this.dgvNodes.Name = "dgvNodes";
            this.dgvNodes.ReadOnly = true;
            this.dgvNodes.RowHeadersVisible = false;
            this.dgvNodes.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvNodes.Size = new System.Drawing.Size(150, 100);
            this.dgvNodes.TabIndex = 6;
            this.dgvNodes.Visible = false;
            // 
            // lstPendingTx
            // 
            this.lstPendingTx.FormattingEnabled = true;
            this.lstPendingTx.ItemHeight = 15;
            this.lstPendingTx.Location = new System.Drawing.Point(200, 165);
            this.lstPendingTx.Name = "lstPendingTx";
            this.lstPendingTx.Size = new System.Drawing.Size(150, 79);
            this.lstPendingTx.TabIndex = 7;
            // 
            // dgvChain
            // 
            this.dgvChain.AllowUserToAddRows = false;
            this.dgvChain.AllowUserToDeleteRows = false;
            this.dgvChain.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvChain.Location = new System.Drawing.Point(400, 275);
            this.dgvChain.Name = "dgvChain";
            this.dgvChain.ReadOnly = true;
            this.dgvChain.RowHeadersVisible = false;
            this.dgvChain.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvChain.Size = new System.Drawing.Size(350, 169);
            this.dgvChain.TabIndex = 8;
            this.dgvChain.Visible = false;
            // 
            // rtbLogs
            // 
            this.rtbLogs.BackColor = System.Drawing.Color.Black;
            this.rtbLogs.ForeColor = System.Drawing.Color.Lime;
            this.rtbLogs.Location = new System.Drawing.Point(575, 450);
            this.rtbLogs.Name = "rtbLogs";
            this.rtbLogs.ReadOnly = true;
            this.rtbLogs.Size = new System.Drawing.Size(580, 200);
            this.rtbLogs.TabIndex = 9;
            this.rtbLogs.Text = "";
            // 
            // picNetwork
            // 
            this.picNetwork.BackColor = System.Drawing.Color.White;
            this.picNetwork.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picNetwork.Location = new System.Drawing.Point(380, 29);
            this.picNetwork.Name = "picNetwork";
            this.picNetwork.Size = new System.Drawing.Size(780, 400);
            this.picNetwork.TabIndex = 10;
            this.picNetwork.TabStop = false;
            this.picNetwork.Paint += new System.Windows.Forms.PaintEventHandler(this.picNetwork_Paint);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblTitle.Location = new System.Drawing.Point(380, 7);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Text = "Red Visual Simulación";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1184, 661);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.picNetwork);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.rtbLogs);
            this.Controls.Add(this.dgvChain);
            this.Controls.Add(this.lstPendingTx);
            this.Controls.Add(this.dgvNodes);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.btnAddTx);
            this.Controls.Add(this.btnSetup);
            this.Controls.Add(this.numMalicious);
            this.Controls.Add(this.numNodes);
            this.Controls.Add(this.cmbConsensus);
            this.Name = "Form1";
            this.Text = "Simulador Consenso Blockchain";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numNodes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMalicious)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvNodes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvChain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picNetwork)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ComboBox cmbConsensus;
        private System.Windows.Forms.NumericUpDown numNodes;
        private System.Windows.Forms.NumericUpDown numMalicious;
        private System.Windows.Forms.Button btnSetup;
        private System.Windows.Forms.Button btnAddTx;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.DataGridView dgvNodes;
        private System.Windows.Forms.ListBox lstPendingTx;
        private System.Windows.Forms.DataGridView dgvChain;
        private System.Windows.Forms.RichTextBox rtbLogs;
        private System.Windows.Forms.PictureBox picNetwork;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
    }
}
