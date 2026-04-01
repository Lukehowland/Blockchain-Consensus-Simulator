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
            cmbConsensus = new ComboBox();
            numNodes = new NumericUpDown();
            numMalicious = new NumericUpDown();
            btnSetup = new Button();
            btnAddTx = new Button();
            btnStart = new Button();
            lstPendingTx = new ListBox();
            rtbLogs = new RichTextBox();
            picNetwork = new PictureBox();
            lblTitle = new Label();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label5 = new Label();
            label6 = new Label();
            label7 = new Label();
            dgvNodes = new DataGridView();
            dgvChain = new DataGridView();
            panelLeft = new Panel();
            panelRight = new Panel();
            panelCenter = new Panel();
            panelBlockchain = new Panel();
            picBlockchain = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)numNodes).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numMalicious).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picNetwork).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvNodes).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvChain).BeginInit();
            panelLeft.SuspendLayout();
            panelRight.SuspendLayout();
            panelCenter.SuspendLayout();
            panelBlockchain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picBlockchain).BeginInit();
            SuspendLayout();
            // 
            // cmbConsensus
            // 
            cmbConsensus.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbConsensus.Location = new Point(10, 30);
            cmbConsensus.Name = "cmbConsensus";
            cmbConsensus.Size = new Size(220, 23);
            cmbConsensus.TabIndex = 1;
            // 
            // numNodes
            // 
            numNodes.Location = new Point(10, 85);
            numNodes.Name = "numNodes";
            numNodes.Size = new Size(220, 23);
            numNodes.TabIndex = 3;
            // 
            // numMalicious
            // 
            numMalicious.Location = new Point(10, 140);
            numMalicious.Name = "numMalicious";
            numMalicious.Size = new Size(220, 23);
            numMalicious.TabIndex = 5;
            // 
            // btnSetup
            // 
            btnSetup.Location = new Point(10, 180);
            btnSetup.Name = "btnSetup";
            btnSetup.Size = new Size(220, 30);
            btnSetup.TabIndex = 6;
            btnSetup.Text = "Configurar Nodos";
            btnSetup.Click += btnSetup_Click;
            // 
            // btnAddTx
            // 
            btnAddTx.Location = new Point(10, 220);
            btnAddTx.Name = "btnAddTx";
            btnAddTx.Size = new Size(220, 30);
            btnAddTx.TabIndex = 7;
            btnAddTx.Text = "Agregar Transacción";
            btnAddTx.Click += btnAddTx_Click;
            // 
            // btnStart
            // 
            btnStart.Location = new Point(10, 260);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(220, 30);
            btnStart.TabIndex = 8;
            btnStart.Text = "Iniciar Consenso";
            btnStart.Click += btnStart_Click;
            // 
            // lstPendingTx
            // 
            lstPendingTx.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lstPendingTx.Location = new Point(10, 330);
            lstPendingTx.Name = "lstPendingTx";
            lstPendingTx.Size = new Size(218, 874);
            lstPendingTx.TabIndex = 10;
            // 
            // rtbLogs
            // 
            rtbLogs.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            rtbLogs.BackColor = Color.Black;
            rtbLogs.ForeColor = Color.Lime;
            rtbLogs.Location = new Point(10, 30);
            rtbLogs.Name = "rtbLogs";
            rtbLogs.ReadOnly = true;
            rtbLogs.Size = new Size(323, 1188);
            rtbLogs.TabIndex = 1;
            rtbLogs.Text = "";
            // 
            // picNetwork
            // 
            picNetwork.BackColor = Color.White;
            picNetwork.Dock = DockStyle.Fill;
            picNetwork.Location = new Point(0, 0);
            picNetwork.Name = "picNetwork";
            picNetwork.Size = new Size(600, 530);
            picNetwork.TabIndex = 1;
            picNetwork.TabStop = false;
            picNetwork.Paint += picNetwork_Paint;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.BackColor = Color.White;
            lblTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblTitle.Location = new Point(10, 10);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(180, 21);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Red Visual Simulación";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(10, 10);
            label1.Name = "label1";
            label1.Size = new Size(105, 15);
            label1.TabIndex = 0;
            label1.Text = "Tipo de Consenso:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(10, 65);
            label2.Name = "label2";
            label2.Size = new Size(76, 15);
            label2.TabIndex = 2;
            label2.Text = "Cant. Nodos:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(10, 120);
            label3.Name = "label3";
            label3.Size = new Size(135, 15);
            label3.TabIndex = 4;
            label3.Text = "Cant. Nodos Maliciosos:";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(10, 305);
            label5.Name = "label5";
            label5.Size = new Size(145, 15);
            label5.TabIndex = 9;
            label5.Text = "Transacciones Pendientes:";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(5, 5);
            label6.Name = "label6";
            label6.Size = new Size(180, 15);
            label6.TabIndex = 0;
            label6.Text = "Cadena de Bloques (Blockchain):";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(10, 10);
            label7.Name = "label7";
            label7.Size = new Size(113, 15);
            label7.TabIndex = 0;
            label7.Text = "Registro de Eventos:";
            // 
            // dgvNodes
            // 
            dgvNodes.Location = new Point(0, 0);
            dgvNodes.Name = "dgvNodes";
            dgvNodes.Size = new Size(240, 150);
            dgvNodes.TabIndex = 0;
            // 
            // dgvChain
            // 
            dgvChain.Location = new Point(0, 0);
            dgvChain.Name = "dgvChain";
            dgvChain.Size = new Size(240, 150);
            dgvChain.TabIndex = 0;
            // 
            // panelLeft
            // 
            panelLeft.BorderStyle = BorderStyle.FixedSingle;
            panelLeft.Controls.Add(label1);
            panelLeft.Controls.Add(cmbConsensus);
            panelLeft.Controls.Add(label2);
            panelLeft.Controls.Add(numNodes);
            panelLeft.Controls.Add(label3);
            panelLeft.Controls.Add(numMalicious);
            panelLeft.Controls.Add(btnSetup);
            panelLeft.Controls.Add(btnAddTx);
            panelLeft.Controls.Add(btnStart);
            panelLeft.Controls.Add(label5);
            panelLeft.Controls.Add(lstPendingTx);
            panelLeft.Dock = DockStyle.Left;
            panelLeft.Location = new Point(0, 0);
            panelLeft.Name = "panelLeft";
            panelLeft.Size = new Size(250, 680);
            panelLeft.TabIndex = 1;
            // 
            // panelRight
            // 
            panelRight.BorderStyle = BorderStyle.FixedSingle;
            panelRight.Controls.Add(label7);
            panelRight.Controls.Add(rtbLogs);
            panelRight.Dock = DockStyle.Right;
            panelRight.Location = new Point(850, 0);
            panelRight.Name = "panelRight";
            panelRight.Size = new Size(350, 680);
            panelRight.TabIndex = 2;
            // 
            // panelCenter
            // 
            panelCenter.Controls.Add(lblTitle);
            panelCenter.Controls.Add(picNetwork);
            panelCenter.Controls.Add(panelBlockchain);
            panelCenter.Dock = DockStyle.Fill;
            panelCenter.Location = new Point(250, 0);
            panelCenter.Name = "panelCenter";
            panelCenter.Size = new Size(600, 680);
            panelCenter.TabIndex = 0;
            // 
            // panelBlockchain
            // 
            panelBlockchain.BorderStyle = BorderStyle.FixedSingle;
            panelBlockchain.Controls.Add(label6);
            panelBlockchain.Controls.Add(picBlockchain);
            panelBlockchain.Dock = DockStyle.Bottom;
            panelBlockchain.Location = new Point(0, 530);
            panelBlockchain.Name = "panelBlockchain";
            panelBlockchain.Size = new Size(600, 150);
            panelBlockchain.TabIndex = 2;
            // 
            // picBlockchain
            // 
            picBlockchain.BackColor = Color.WhiteSmoke;
            picBlockchain.Dock = DockStyle.Bottom;
            picBlockchain.Location = new Point(0, 25);
            picBlockchain.Name = "picBlockchain";
            picBlockchain.Size = new Size(598, 123);
            picBlockchain.TabIndex = 1;
            picBlockchain.TabStop = false;
            picBlockchain.Paint += picBlockchain_Paint;
            // 
            // Form1
            // 
            ClientSize = new Size(1200, 680);
            Controls.Add(panelCenter);
            Controls.Add(panelLeft);
            Controls.Add(panelRight);
            Name = "Form1";
            Text = "Simulador Consenso Blockchain";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)numNodes).EndInit();
            ((System.ComponentModel.ISupportInitialize)numMalicious).EndInit();
            ((System.ComponentModel.ISupportInitialize)picNetwork).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvNodes).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvChain).EndInit();
            panelLeft.ResumeLayout(false);
            panelLeft.PerformLayout();
            panelRight.ResumeLayout(false);
            panelRight.PerformLayout();
            panelCenter.ResumeLayout(false);
            panelCenter.PerformLayout();
            panelBlockchain.ResumeLayout(false);
            panelBlockchain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)picBlockchain).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.ComboBox cmbConsensus;
        private System.Windows.Forms.NumericUpDown numNodes;
        private System.Windows.Forms.NumericUpDown numMalicious;
        private System.Windows.Forms.Button btnSetup;
        private System.Windows.Forms.Button btnAddTx;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.ListBox lstPendingTx;
        private System.Windows.Forms.RichTextBox rtbLogs;
        private System.Windows.Forms.PictureBox picNetwork;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.DataGridView dgvNodes;
        private System.Windows.Forms.DataGridView dgvChain;
        
        private System.Windows.Forms.Panel panelLeft;
        private System.Windows.Forms.Panel panelRight;
        private System.Windows.Forms.Panel panelCenter;
        private System.Windows.Forms.Panel panelBlockchain;
        private System.Windows.Forms.PictureBox picBlockchain;
    }
}
