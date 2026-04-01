using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Blockchain.Models;

namespace Blockchain
{
    public partial class Form1 : Form
    {
        private SimulatorEngine engine;

        public Form1()
        {
            InitializeComponent();
            engine = new SimulatorEngine();
            engine.OnLogEvent += Engine_OnLogEvent;
            engine.OnStateChanged += UpdateUI;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cmbConsensus.DataSource = Enum.GetValues(typeof(ConsensusType));
            cmbConsensus.SelectedIndex = 0;
            numNodes.Value = 5;
            numMalicious.Value = 1;

            ConfigurarGrillas();
        }

        private void ConfigurarGrillas()
        {
            dgvNodes.Columns.Add("Id", "ID");
            dgvNodes.Columns.Add("Stake", "Stake");
            dgvNodes.Columns.Add("Power", "Poder");
            dgvNodes.Columns.Add("Malicious", "Malicioso");
            dgvNodes.Columns["Id"].Width = 40;
            dgvNodes.Columns["Stake"].Width = 60;
            dgvNodes.Columns["Power"].Width = 60;
            dgvNodes.Columns["Malicious"].Width = 80;

            dgvChain.Columns.Add("Index", "#");
            dgvChain.Columns.Add("Hash", "Hash");
            dgvChain.Columns.Add("Validator", "Cated. / Validator");
            dgvChain.Columns.Add("TxCount", "Transacciones");
            dgvChain.Columns.Add("Nonce", "Nonce");
            dgvChain.Columns["Index"].Width = 30;
            dgvChain.Columns["Hash"].Width = 100;
            dgvChain.Columns["Validator"].Width = 120;
            dgvChain.Columns["TxCount"].Width = 90;
            dgvChain.Columns["Nonce"].Width = 60;
        }

        private void Engine_OnLogEvent(string message)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => Engine_OnLogEvent(message)));
                return;
            }

            Color logColor = Color.LimeGreen;
            if (message.Contains("RECHAZADO") || message.Contains("inválido") || message.Contains("Malicioso") || message.Contains("rob") || message.Contains("miente") || message.Contains("FALLIDO") || message.Contains("inconsistente"))
                logColor = Color.Tomato;
            else if (message.Contains("validado") || message.Contains("correctamente") || message.Contains("ALCANZADO"))
                logColor = Color.Cyan;
            else if (message.Contains("Líder") || message.Contains("Validador seleccionado"))
                logColor = Color.Yellow;
            else if (message.Contains("Iniciando Consenso"))
                logColor = Color.Magenta;

            rtbLogs.SelectionStart = rtbLogs.TextLength;
            rtbLogs.SelectionLength = 0;
            rtbLogs.SelectionColor = logColor;
            rtbLogs.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}{Environment.NewLine}");
            rtbLogs.ScrollToCaret();
        }

        private void UpdateUI()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(UpdateUI));
                return;
            }

            dgvNodes.Rows.Clear();
            foreach (var node in engine.Nodes)
            {
                int rowIndex = dgvNodes.Rows.Add(node.Id, node.Stake, node.ComputationalPower, node.IsMalicious ? "Sí" : "No");
                if (node.IsMalicious)
                {
                    dgvNodes.Rows[rowIndex].DefaultCellStyle.BackColor = Color.MistyRose;
                    dgvNodes.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.DarkRed;
                }
                else
                {
                    dgvNodes.Rows[rowIndex].DefaultCellStyle.BackColor = Color.White;
                    dgvNodes.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Black;
                }
            }

            lstPendingTx.Items.Clear();
            foreach (var tx in engine.PendingTransactions)
            {
                lstPendingTx.Items.Add(tx.ToString());
            }

            dgvChain.Rows.Clear();
            foreach (var block in engine.Chain)
            {
                string shortHash = block.Hash.Length > 10 ? block.Hash.Substring(0, 10) + "..." : block.Hash;
                dgvChain.Rows.Add(block.Index, shortHash, block.Validator, block.Transactions?.Count ?? 0, block.Nonce);
            }
            if (dgvChain.Rows.Count > 0)
                dgvChain.FirstDisplayedScrollingRowIndex = dgvChain.Rows.Count - 1;

            picNetwork.Invalidate(); // Triggers redraw
        }

        private void btnSetup_Click(object sender, EventArgs e)
        {
            engine.SetupNodes((int)numNodes.Value, (int)numMalicious.Value);
        }

        private void btnAddTx_Click(object sender, EventArgs e)
        {
            var tx = new Transaction
            {
                Sender = $"Usuario_{new Random().Next(100)}",
                Receiver = $"Usuario_{new Random().Next(100)}",
                Amount = (decimal)(new Random().NextDouble() * 100)
            };
            engine.AddTransaction(tx);
        }

        private async void btnStart_Click(object sender, EventArgs e)
        {
            engine.Consensus = (ConsensusType)cmbConsensus.SelectedItem;
            btnStart.Enabled = false;
            btnAddTx.Enabled = false;
            btnSetup.Enabled = false;

            await engine.StartConsensusAsync();
            
            btnStart.Enabled = true;
            btnAddTx.Enabled = true;
            btnSetup.Enabled = true;
        }

        private void picNetwork_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            int nodeCount = engine.Nodes.Count;
            if (nodeCount == 0) return;

            // Center of the PictureBox
            int cx = picNetwork.Width / 2;
            int cy = picNetwork.Height / 2;
            int radius = Math.Min(cx, cy) - 50;

            // Header state text
            g.DrawString($"Estado: {engine.ActiveAction}", new Font("Arial", 12, FontStyle.Bold), Brushes.DarkBlue, 10, 10);

            // Calculate positions
            Dictionary<int, PointF> positions = new ();
            for (int i = 0; i < nodeCount; i++)
            {
                var node = engine.Nodes[i];
                double angle = (2 * Math.PI * i) / nodeCount;
                float x = cx + (float)(radius * Math.Cos(angle));
                float y = cy + (float)(radius * Math.Sin(angle));
                positions[node.Id] = new PointF(x, y);
            }

            // Draw connections (if there's a proposer or targeted nodes)
            if (engine.CurrentProposer != null && positions.ContainsKey(engine.CurrentProposer.Id))
            {
                PointF pStart = positions[engine.CurrentProposer.Id];
                foreach (var target in engine.ParticipatingNodes)
                {
                    if (positions.ContainsKey(target.Id))
                    {
                        PointF pEnd = positions[target.Id];
                        // Draw animated line
                        Pen p = new Pen(Color.Orange, 2) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dot };
                        if (target.IsMalicious || engine.CurrentProposer.IsMalicious) 
                            p.Color = Color.Red;
                        
                        g.DrawLine(p, pStart, pEnd);
                    }
                }
            }

            // Draw nodes
            int nodeRadius = 25;
            foreach (var node in engine.Nodes)
            {
                PointF pos = positions[node.Id];
                RectangleF rect = new RectangleF(pos.X - nodeRadius, pos.Y - nodeRadius, nodeRadius * 2, nodeRadius * 2);
                
                // Color logic
                Brush b = Brushes.LightGray;
                if (node.IsMalicious) b = Brushes.IndianRed;
                if (engine.CurrentProposer == node) b = Brushes.Gold;
                else if (engine.ParticipatingNodes.Contains(node)) b = Brushes.LightSkyBlue;

                g.FillEllipse(b, rect);
                g.DrawEllipse(Pens.Black, rect);

                // Draw Text inside node
                StringFormat sf = new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                g.DrawString($"N{node.Id}", new Font("Arial", 9, FontStyle.Bold), Brushes.Black, pos, sf);

                // Draw external label (Stake/Power)
                g.DrawString($"S={node.Stake}\nP={node.ComputationalPower}", new Font("Arial", 8), Brushes.DarkGray, pos.X - nodeRadius, pos.Y + nodeRadius + 2);
            }
        }
    }
}
