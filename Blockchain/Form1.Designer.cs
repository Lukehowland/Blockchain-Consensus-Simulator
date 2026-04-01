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
            btnReset = new Button();
            btnCompare = new Button();
            lstPendingTx = new ListBox();
            rtbLogs = new RichTextBox();
            picNetwork = new PictureBox();
            lblTitle = new Label();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            labelDifficulty = new Label();
            numDifficulty = new NumericUpDown();
            label5 = new Label();
            label6 = new Label();
            labelSpeed = new Label();
            cmbSpeed = new ComboBox();
            dgvNodes = new DataGridView();
            dgvChain = new DataGridView();
            dgvMetrics = new DataGridView();
            panelLeft = new Panel();
            panelRight = new Panel();
            panelCenter = new Panel();
            panelNetworkHeader = new Panel();
            panelBlockchain = new Panel();
            picBlockchain = new PictureBox();
            tabControlRight = new TabControl();
            tabLogs = new TabPage();
            tabData = new TabPage();
            tabMetrics = new TabPage();
            tabYisus = new TabPage();
            picYisus = new PictureBox();
            splitData = new SplitContainer();

            ((System.ComponentModel.ISupportInitialize)numNodes).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numMalicious).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numDifficulty).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picNetwork).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvNodes).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvChain).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvMetrics).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picBlockchain).BeginInit();
            panelLeft.SuspendLayout();
            panelRight.SuspendLayout();
            panelCenter.SuspendLayout();
            panelNetworkHeader.SuspendLayout();
            panelBlockchain.SuspendLayout();
            tabControlRight.SuspendLayout();
            tabLogs.SuspendLayout();
            tabData.SuspendLayout();
            tabMetrics.SuspendLayout();
            tabYisus.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picYisus).BeginInit();
            ((System.ComponentModel.ISupportInitialize)splitData).BeginInit();
            splitData.Panel1.SuspendLayout();
            splitData.Panel2.SuspendLayout();
            splitData.SuspendLayout();
            SuspendLayout();
            //
            // label1
            //
            label1.AutoSize = true;
            label1.Location = new Point(10, 10);
            label1.Name = "label1";
            label1.Size = new Size(105, 15);
            label1.Text = "Tipo de Consenso:";
            //
            // cmbConsensus
            //
            cmbConsensus.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbConsensus.Location = new Point(10, 30);
            cmbConsensus.Name = "cmbConsensus";
            cmbConsensus.Size = new Size(220, 23);
            //
            // label2
            //
            label2.AutoSize = true;
            label2.Location = new Point(10, 65);
            label2.Name = "label2";
            label2.Size = new Size(76, 15);
            label2.Text = "Cant. Nodos:";
            //
            // numNodes
            //
            numNodes.Location = new Point(10, 85);
            numNodes.Name = "numNodes";
            numNodes.Size = new Size(220, 23);
            numNodes.Minimum = 1;
            numNodes.Maximum = 20;
            //
            // label3
            //
            label3.AutoSize = true;
            label3.Location = new Point(10, 120);
            label3.Name = "label3";
            label3.Size = new Size(135, 15);
            label3.Text = "Cant. Nodos Maliciosos:";
            //
            // numMalicious
            //
            numMalicious.Location = new Point(10, 140);
            numMalicious.Name = "numMalicious";
            numMalicious.Size = new Size(220, 23);
            numMalicious.Minimum = 0;
            numMalicious.Maximum = 19;
            //
            // labelDifficulty (4.3)
            //
            labelDifficulty.AutoSize = true;
            labelDifficulty.Location = new Point(10, 175);
            labelDifficulty.Name = "labelDifficulty";
            labelDifficulty.Size = new Size(100, 15);
            labelDifficulty.Text = "Dificultad PoW:";
            //
            // numDifficulty (4.3)
            //
            numDifficulty.Location = new Point(120, 173);
            numDifficulty.Name = "numDifficulty";
            numDifficulty.Size = new Size(110, 23);
            numDifficulty.Minimum = 1;
            numDifficulty.Maximum = 6;
            numDifficulty.Value = 3;
            //
            // labelSpeed
            //
            labelSpeed.AutoSize = true;
            labelSpeed.Location = new Point(10, 205);
            labelSpeed.Name = "labelSpeed";
            labelSpeed.Text = "Velocidad Simulación:";
            //
            // cmbSpeed
            //
            cmbSpeed.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSpeed.Location = new Point(10, 225);
            cmbSpeed.Name = "cmbSpeed";
            cmbSpeed.Size = new Size(220, 23);
            //
            // btnSetup
            //
            btnSetup.Location = new Point(10, 260);
            btnSetup.Name = "btnSetup";
            btnSetup.Size = new Size(220, 30);
            btnSetup.Text = "Configurar Nodos";
            btnSetup.Click += btnSetup_Click;
            //
            // btnAddTx
            //
            btnAddTx.Location = new Point(10, 300);
            btnAddTx.Name = "btnAddTx";
            btnAddTx.Size = new Size(220, 30);
            btnAddTx.Text = "Agregar Transacción";
            btnAddTx.Click += btnAddTx_Click;
            //
            // btnStart
            //
            btnStart.Location = new Point(10, 340);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(220, 30);
            btnStart.Text = "Iniciar Consenso";
            btnStart.Click += btnStart_Click;
            //
            // btnReset (4.4)
            //
            btnReset.Location = new Point(10, 380);
            btnReset.Name = "btnReset";
            btnReset.Size = new Size(220, 30);
            btnReset.Text = "Reiniciar";
            btnReset.Click += btnReset_Click;
            //
            // btnCompare (4.5)
            //
            btnCompare.Location = new Point(10, 420);
            btnCompare.Name = "btnCompare";
            btnCompare.Size = new Size(220, 30);
            btnCompare.Text = "Comparar Algoritmos";
            btnCompare.Click += btnCompare_Click;
            //
            // label5
            //
            label5.AutoSize = true;
            label5.Location = new Point(10, 460);
            label5.Name = "label5";
            label5.Size = new Size(145, 15);
            label5.Text = "Transacciones Pendientes:";
            //
            // lstPendingTx (3.6 — tamaño corregido)
            //
            lstPendingTx.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lstPendingTx.Location = new Point(10, 480);
            lstPendingTx.Name = "lstPendingTx";
            lstPendingTx.Size = new Size(218, 240);
            //
            // rtbLogs
            //
            rtbLogs.BackColor = Color.Black;
            rtbLogs.Dock = DockStyle.Fill;
            rtbLogs.ForeColor = Color.Lime;
            rtbLogs.Name = "rtbLogs";
            rtbLogs.ReadOnly = true;
            rtbLogs.Text = "";
            //
            // dgvNodes (1.3 — configuración completa)
            //
            dgvNodes.Dock = DockStyle.Fill;
            dgvNodes.Name = "dgvNodes";
            dgvNodes.ReadOnly = true;
            dgvNodes.AllowUserToAddRows = false;
            dgvNodes.RowHeadersVisible = false;
            //
            // dgvChain (1.3)
            //
            dgvChain.Dock = DockStyle.Fill;
            dgvChain.Name = "dgvChain";
            dgvChain.ReadOnly = true;
            dgvChain.AllowUserToAddRows = false;
            dgvChain.RowHeadersVisible = false;
            //
            // dgvMetrics (4.6)
            //
            dgvMetrics.Dock = DockStyle.Fill;
            dgvMetrics.Name = "dgvMetrics";
            dgvMetrics.ReadOnly = true;
            dgvMetrics.AllowUserToAddRows = false;
            dgvMetrics.RowHeadersVisible = false;
            //
            // splitData (1.3 — SplitContainer para datos)
            //
            splitData.Dock = DockStyle.Fill;
            splitData.Orientation = Orientation.Horizontal;
            splitData.SplitterDistance = 200;
            splitData.Panel1.Controls.Add(dgvNodes);
            splitData.Panel2.Controls.Add(dgvChain);
            //
            // tabLogs
            //
            tabLogs.Text = "Registro";
            tabLogs.Controls.Add(rtbLogs);
            tabLogs.Padding = new Padding(3);
            //
            // tabData
            //
            tabData.Text = "Datos";
            tabData.Controls.Add(splitData);
            tabData.Padding = new Padding(3);
            //
            // tabMetrics
            //
            tabMetrics.Text = "Métricas";
            tabMetrics.Controls.Add(dgvMetrics);
            tabMetrics.Padding = new Padding(3);
            //
            // tabControlRight (1.3)
            //
            tabControlRight.Dock = DockStyle.Fill;
            tabControlRight.TabPages.Add(tabLogs);
            tabControlRight.TabPages.Add(tabData);
            tabControlRight.TabPages.Add(tabMetrics);
            //
            // tabYisus
            //
            tabYisus.Text = "yisus";
            tabYisus.Padding = new Padding(3);
            tabYisus.BackColor = Color.FromArgb(30, 30, 30);
            tabYisus.Controls.Add(picYisus);
            //
            // picYisus
            //
            picYisus.Dock = DockStyle.Fill;
            picYisus.SizeMode = PictureBoxSizeMode.Zoom;
            picYisus.BackColor = Color.FromArgb(30, 30, 30);
            picYisus.Name = "picYisus";
            picYisus.TabStop = false;
            //
            tabControlRight.TabPages.Add(tabYisus);
            //
            // picNetwork
            //
            picNetwork.BackColor = Color.White;
            picNetwork.Dock = DockStyle.Fill;
            picNetwork.Name = "picNetwork";
            picNetwork.TabStop = false;
            picNetwork.Paint += picNetwork_Paint;
            //
            // lblTitle (2.2 — ahora en panelNetworkHeader)
            //
            lblTitle.AutoSize = true;
            lblTitle.BackColor = Color.White;
            lblTitle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblTitle.Location = new Point(8, 4);
            lblTitle.Name = "lblTitle";
            lblTitle.Text = "Red Visual — Estado: Inactivo";
            //
            // panelNetworkHeader (2.2)
            //
            panelNetworkHeader.BackColor = Color.White;
            panelNetworkHeader.Dock = DockStyle.Top;
            panelNetworkHeader.Height = 28;
            panelNetworkHeader.Controls.Add(lblTitle);
            //
            // label6
            //
            label6.AutoSize = true;
            label6.Location = new Point(5, 3);
            label6.Name = "label6";
            label6.Text = "Cadena de Bloques (Blockchain):";
            //
            // picBlockchain (2.5 — sin Dock, tamaño dinámico)
            //
            picBlockchain.BackColor = Color.WhiteSmoke;
            picBlockchain.Location = new Point(0, 22);
            picBlockchain.Name = "picBlockchain";
            picBlockchain.Size = new Size(598, 120);
            picBlockchain.TabStop = false;
            picBlockchain.Paint += picBlockchain_Paint;
            //
            // panelBlockchain (2.5 — AutoScroll habilitado)
            //
            panelBlockchain.AutoScroll = true;
            panelBlockchain.BorderStyle = BorderStyle.FixedSingle;
            panelBlockchain.Controls.Add(label6);
            panelBlockchain.Controls.Add(picBlockchain);
            panelBlockchain.Dock = DockStyle.Bottom;
            panelBlockchain.Size = new Size(600, 150);
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
            panelLeft.Controls.Add(labelDifficulty);
            panelLeft.Controls.Add(labelSpeed);
            panelLeft.Controls.Add(cmbSpeed);
            panelLeft.Controls.Add(numDifficulty);
            panelLeft.Controls.Add(btnSetup);
            panelLeft.Controls.Add(btnAddTx);
            panelLeft.Controls.Add(btnStart);
            panelLeft.Controls.Add(btnReset);
            panelLeft.Controls.Add(btnCompare);
            panelLeft.Controls.Add(label5);
            panelLeft.Controls.Add(lstPendingTx);
            panelLeft.Dock = DockStyle.Left;
            panelLeft.Size = new Size(250, 700);
            //
            // panelRight (1.3 — ahora con TabControl)
            //
            panelRight.BorderStyle = BorderStyle.FixedSingle;
            panelRight.Controls.Add(tabControlRight);
            panelRight.Dock = DockStyle.Right;
            panelRight.Size = new Size(350, 700);
            //
            // panelCenter (2.2 — con panelNetworkHeader)
            //
            panelCenter.Controls.Add(picNetwork);
            panelCenter.Controls.Add(panelNetworkHeader);
            panelCenter.Controls.Add(panelBlockchain);
            panelCenter.Dock = DockStyle.Fill;
            //
            // Form1
            //
            ClientSize = new Size(1200, 700);
            Controls.Add(panelCenter);
            Controls.Add(panelLeft);
            Controls.Add(panelRight);
            Name = "Form1";
            Text = "Simulador Consenso Blockchain";
            WindowState = FormWindowState.Maximized;
            Load += Form1_Load;

            ((System.ComponentModel.ISupportInitialize)numNodes).EndInit();
            ((System.ComponentModel.ISupportInitialize)numMalicious).EndInit();
            ((System.ComponentModel.ISupportInitialize)numDifficulty).EndInit();
            ((System.ComponentModel.ISupportInitialize)picNetwork).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvNodes).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvChain).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvMetrics).EndInit();
            ((System.ComponentModel.ISupportInitialize)picBlockchain).EndInit();
            panelLeft.ResumeLayout(false);
            panelLeft.PerformLayout();
            panelRight.ResumeLayout(false);
            panelCenter.ResumeLayout(false);
            panelNetworkHeader.ResumeLayout(false);
            panelNetworkHeader.PerformLayout();
            panelBlockchain.ResumeLayout(false);
            panelBlockchain.PerformLayout();
            tabControlRight.ResumeLayout(false);
            tabLogs.ResumeLayout(false);
            tabData.ResumeLayout(false);
            tabMetrics.ResumeLayout(false);
            tabYisus.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)picYisus).EndInit();
            splitData.Panel1.ResumeLayout(false);
            splitData.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitData).EndInit();
            splitData.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.ComboBox cmbConsensus;
        private System.Windows.Forms.NumericUpDown numNodes;
        private System.Windows.Forms.NumericUpDown numMalicious;
        private System.Windows.Forms.NumericUpDown numDifficulty;
        private System.Windows.Forms.Button btnSetup;
        private System.Windows.Forms.Button btnAddTx;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnCompare;
        private System.Windows.Forms.ListBox lstPendingTx;
        private System.Windows.Forms.RichTextBox rtbLogs;
        private System.Windows.Forms.PictureBox picNetwork;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label labelDifficulty;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label labelSpeed;
        private System.Windows.Forms.ComboBox cmbSpeed;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DataGridView dgvNodes;
        private System.Windows.Forms.DataGridView dgvChain;
        private System.Windows.Forms.DataGridView dgvMetrics;
        private System.Windows.Forms.Panel panelLeft;
        private System.Windows.Forms.Panel panelRight;
        private System.Windows.Forms.Panel panelCenter;
        private System.Windows.Forms.Panel panelNetworkHeader;
        private System.Windows.Forms.Panel panelBlockchain;
        private System.Windows.Forms.PictureBox picBlockchain;
        private System.Windows.Forms.TabControl tabControlRight;
        private System.Windows.Forms.TabPage tabLogs;
        private System.Windows.Forms.TabPage tabData;
        private System.Windows.Forms.TabPage tabMetrics;
        private System.Windows.Forms.TabPage tabYisus;
        private System.Windows.Forms.PictureBox picYisus;
        private System.Windows.Forms.SplitContainer splitData;
    }
}
