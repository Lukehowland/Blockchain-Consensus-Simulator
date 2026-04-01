using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using Blockchain.Models;

namespace Blockchain
{
    public partial class Form1 : Form
    {
        private SimulatorEngine engine;
        private static readonly Random _rng = new(); // 3.3

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

            // Velocidad de simulación
            cmbSpeed.Items.AddRange(new object[] { "Muy Lenta (x3)", "Lenta (x2)", "Normal (x1)", "Rápida (x0.5)", "Muy Rápida (x0.25)" });
            cmbSpeed.SelectedIndex = 2; // Normal por defecto

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
            dgvChain.Columns.Add("Validator", "Validador");
            dgvChain.Columns.Add("TxCount", "Txs");
            dgvChain.Columns.Add("Nonce", "Nonce");
            dgvChain.Columns["Index"].Width = 30;
            dgvChain.Columns["Hash"].Width = 100;
            dgvChain.Columns["Validator"].Width = 120;
            dgvChain.Columns["TxCount"].Width = 50;
            dgvChain.Columns["Nonce"].Width = 60;

            // 4.6 — Columnas de métricas
            dgvMetrics.Columns.Add("Type", "Algoritmo");
            dgvMetrics.Columns.Add("Block", "Bloque");
            dgvMetrics.Columns.Add("Time", "Tiempo (ms)");
            dgvMetrics.Columns.Add("Hashes", "Intentos Hash");
            dgvMetrics.Columns.Add("Messages", "Mensajes");
            dgvMetrics.Columns.Add("Success", "Éxito");
            dgvMetrics.Columns.Add("Energy", "Energía Est.");
        }

        private void Engine_OnLogEvent(string message)
        {
            if (InvokeRequired) { Invoke(new Action(() => Engine_OnLogEvent(message))); return; }

            Color logColor = Color.LimeGreen;
            if (message.Contains("RECHAZADO") || message.Contains("inválido") || message.Contains("Malicioso") ||
                message.Contains("rob") || message.Contains("miente") || message.Contains("FALLIDO") ||
                message.Contains("inconsistente") || message.Contains("alterado") || message.Contains("contra"))
                logColor = Color.Tomato;
            else if (message.Contains("validado") || message.Contains("correctamente") || message.Contains("ALCANZADO") || message.Contains("favor"))
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
            if (InvokeRequired) { Invoke(new Action(UpdateUI)); return; }

            // 2.2 — Actualizar título
            string status = string.IsNullOrEmpty(engine.ActiveAction) ? "Inactivo" : engine.ActiveAction;
            lblTitle.Text = $"Red Visual — Estado: {status}";

            dgvNodes.Rows.Clear();
            foreach (var node in engine.Nodes)
            {
                int ri = dgvNodes.Rows.Add(node.Id, node.Stake, node.ComputationalPower, node.IsMalicious ? "Sí" : "No");
                if (node.IsMalicious)
                {
                    dgvNodes.Rows[ri].DefaultCellStyle.BackColor = Color.MistyRose;
                    dgvNodes.Rows[ri].DefaultCellStyle.ForeColor = Color.DarkRed;
                }
            }

            lstPendingTx.Items.Clear();
            foreach (var tx in engine.PendingTransactions)
                lstPendingTx.Items.Add(tx.ToString());

            dgvChain.Rows.Clear();
            foreach (var block in engine.Chain)
            {
                string shortHash = block.Hash.Length > 10 ? block.Hash.Substring(0, 10) + "..." : block.Hash;
                dgvChain.Rows.Add(block.Index, shortHash, block.Validator, block.Transactions?.Count ?? 0, block.Nonce);
            }
            if (dgvChain.Rows.Count > 0)
                dgvChain.FirstDisplayedScrollingRowIndex = dgvChain.Rows.Count - 1;

            // 4.6 — Actualizar métricas
            dgvMetrics.Rows.Clear();
            foreach (var m in engine.MetricsHistory)
            {
                dgvMetrics.Rows.Add(m.Type, $"#{m.BlockIndex}", m.ElapsedMs, m.HashAttempts,
                    m.MessagesExchanged, m.Success ? "✓" : "✗", m.EnergyEstimate);
            }

            // 2.5 — Ajustar ancho de picBlockchain para scroll
            int neededWidth = Math.Max(panelBlockchain.ClientSize.Width, engine.Chain.Count * 140 + 30);
            picBlockchain.Size = new Size(neededWidth, Math.Max(100, panelBlockchain.ClientSize.Height - 28));

            picNetwork.Invalidate();
            picBlockchain.Invalidate();
        }

        // 3.4 — Validar maliciosos < total
        private void btnSetup_Click(object sender, EventArgs e)
        {
            int total = (int)numNodes.Value;
            int malicious = (int)numMalicious.Value;
            if (malicious >= total)
            {
                MessageBox.Show("La cantidad de nodos maliciosos debe ser menor al total de nodos.",
                    "Configuración inválida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            engine.SetupNodes(total, malicious);
        }

        // 3.3 — Único Random
        private void btnAddTx_Click(object sender, EventArgs e)
        {
            var tx = new Transaction
            {
                Sender = $"Usuario_{_rng.Next(100)}",
                Receiver = $"Usuario_{_rng.Next(100)}",
                Amount = (decimal)(_rng.NextDouble() * 100)
            };
            engine.AddTransaction(tx);
        }

        // 3.2 — try/catch/finally + 4.3 dificultad
        private async void btnStart_Click(object sender, EventArgs e)
        {
            engine.Consensus = (ConsensusType)cmbConsensus.SelectedItem;
            engine.PoWDifficulty = (int)numDifficulty.Value;
            engine.SpeedMultiplier = cmbSpeed.SelectedIndex switch
            {
                0 => 3.0,   // Muy Lenta
                1 => 2.0,   // Lenta
                2 => 1.0,   // Normal
                3 => 0.5,   // Rápida
                4 => 0.25,  // Muy Rápida
                _ => 1.0
            };
            btnStart.Enabled = false;
            btnAddTx.Enabled = false;
            btnSetup.Enabled = false;

            try
            {
                await engine.StartConsensusAsync();
            }
            catch (Exception ex)
            {
                Engine_OnLogEvent($"Error: {ex.Message}");
            }
            finally
            {
                btnStart.Enabled = true;
                btnAddTx.Enabled = true;
                btnSetup.Enabled = true;
            }
        }

        // 4.4 — Reiniciar
        private void btnReset_Click(object sender, EventArgs e)
        {
            engine = new SimulatorEngine();
            engine.OnLogEvent += Engine_OnLogEvent;
            engine.OnStateChanged += UpdateUI;
            rtbLogs.Clear();
            dgvNodes.Rows.Clear();
            dgvChain.Rows.Clear();
            dgvMetrics.Rows.Clear();
            lstPendingTx.Items.Clear();
            lblTitle.Text = "Red Visual — Estado: Inactivo";
            picNetwork.Invalidate();
            picBlockchain.Invalidate();
            Engine_OnLogEvent("Simulador reiniciado.");
        }

        // 4.5 — Comparar algoritmos
        private void btnCompare_Click(object sender, EventArgs e)
        {
            if (engine.MetricsHistory.Count == 0)
            {
                MessageBox.Show("No hay métricas registradas. Ejecuta al menos una ronda de consenso.",
                    "Sin datos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var groups = engine.MetricsHistory.GroupBy(m => m.Type);
            string result = "═══ COMPARACIÓN DE ALGORITMOS ═══\n\n";

            foreach (var g in groups)
            {
                var items = g.ToList();
                result += $"▶ {g.Key}\n";
                result += $"  Rondas: {items.Count}\n";
                result += $"  Éxitos: {items.Count(m => m.Success)} / {items.Count}\n";
                result += $"  Tiempo prom.: {items.Average(m => m.ElapsedMs):F0} ms\n";
                result += $"  Hash intentos prom.: {items.Average(m => m.HashAttempts):F0}\n";
                result += $"  Mensajes prom.: {items.Average(m => m.MessagesExchanged):F0}\n";
                result += $"  Energía último: {items.Last().EnergyEstimate}\n\n";
            }

            MessageBox.Show(result, "Comparación de Algoritmos", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // 2.6 snapshots + 2.1 mesh + 1.4 colores + 3.5 dispose + 4.7 leyenda
        private void picNetwork_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // 2.6 — Snapshots
            var nodes = engine.Nodes.ToList();
            var participating = engine.ParticipatingNodes.ToList();
            var proposer = engine.CurrentProposer;

            int nodeCount = nodes.Count;
            if (nodeCount == 0) return;

            int cx = picNetwork.Width / 2;
            int cy = picNetwork.Height / 2;
            int radius = Math.Min(cx, cy) - 60;

            var positions = new System.Collections.Generic.Dictionary<int, PointF>();
            for (int i = 0; i < nodeCount; i++)
            {
                double angle = (2 * Math.PI * i) / nodeCount;
                float x = cx + (float)(radius * Math.Cos(angle));
                float y = cy + (float)(radius * Math.Sin(angle));
                positions[nodes[i].Id] = new PointF(x, y);
            }

            // 2.1 — Mesh P2P
            using var meshPen = new Pen(Color.FromArgb(40, Color.Gray), 1);
            for (int i = 0; i < nodeCount; i++)
                for (int j = i + 1; j < nodeCount; j++)
                    g.DrawLine(meshPen, positions[nodes[i].Id], positions[nodes[j].Id]);

            // Líneas proposer → participantes
            if (proposer != null && positions.ContainsKey(proposer.Id))
            {
                PointF pStart = positions[proposer.Id];
                foreach (var target in participating)
                {
                    if (!positions.ContainsKey(target.Id)) continue;
                    using var connPen = new Pen(target.IsMalicious || proposer.IsMalicious ? Color.Red : Color.Orange, 2)
                    { DashStyle = DashStyle.Dot };
                    g.DrawLine(connPen, pStart, positions[target.Id]);
                }
            }

            // Dibujar nodos
            int nr = 25;
            using var nodeFont = new Font("Segoe UI", 9, FontStyle.Bold);
            using var labelFont = new Font("Segoe UI", 7);
            using var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };

            foreach (var node in nodes)
            {
                PointF pos = positions[node.Id];
                RectangleF rect = new(pos.X - nr, pos.Y - nr, nr * 2, nr * 2);

                // 1.4 + 4.1 — Color de relleno
                Brush fill;
                if (node.IsMalicious)
                    fill = node.VisualState == NodeVisualState.Slashed ? Brushes.DarkRed : Brushes.IndianRed;
                else
                {
                    fill = node.VisualState switch
                    {
                        NodeVisualState.Mining => Brushes.Gold,
                        NodeVisualState.Proposing => Brushes.LightGreen,
                        NodeVisualState.Voting => Brushes.LightSkyBlue,
                        _ => Brushes.LightGray
                    };
                }
                g.FillEllipse(fill, rect);

                // 1.4 — Borde según rol
                if (proposer != null && proposer.Id == node.Id)
                {
                    using var bp = new Pen(Color.Goldenrod, 4);
                    g.DrawEllipse(bp, rect);
                }
                else if (participating.Any(p => p.Id == node.Id))
                {
                    using var bp = new Pen(Color.CornflowerBlue, 2);
                    g.DrawEllipse(bp, rect);
                }
                else
                    g.DrawEllipse(Pens.Black, rect);

                g.DrawString($"N{node.Id}", nodeFont, Brushes.Black, pos, sf);
                g.DrawString($"S={node.Stake:F0}\nP={node.ComputationalPower}", labelFont, Brushes.DarkGray,
                    pos.X - nr, pos.Y + nr + 3);
            }

            // 4.7 — Leyenda
            DrawLegend(g, picNetwork.Width, picNetwork.Height);
        }

        private void DrawLegend(Graphics g, int w, int h)
        {
            int lw = 165, lh = 128, m = 8;
            int x = w - lw - m, y = h - lh - m;

            using var bg = new SolidBrush(Color.FromArgb(210, 255, 255, 255));
            g.FillRectangle(bg, x, y, lw, lh);
            g.DrawRectangle(Pens.Gray, x, y, lw, lh);

            using var f = new Font("Segoe UI", 7.5f);
            int iy = y + 6, cs = 12, tx = x + 22;

            g.FillEllipse(Brushes.LightGray, x + 6, iy, cs, cs);
            g.DrawString("Honesto (Idle)", f, Brushes.Black, tx, iy); iy += 18;

            g.FillEllipse(Brushes.IndianRed, x + 6, iy, cs, cs);
            g.DrawString("Malicioso", f, Brushes.Black, tx, iy); iy += 18;

            g.FillEllipse(Brushes.Gold, x + 6, iy, cs, cs);
            g.DrawString("Minando (PoW)", f, Brushes.Black, tx, iy); iy += 18;

            g.FillEllipse(Brushes.LightSkyBlue, x + 6, iy, cs, cs);
            g.DrawString("Votando/Atestiguando", f, Brushes.Black, tx, iy); iy += 18;

            using var gp = new Pen(Color.Goldenrod, 3);
            g.DrawEllipse(gp, x + 6, iy, cs, cs);
            g.DrawString("Proponente (borde)", f, Brushes.Black, tx, iy); iy += 18;

            g.DrawString("S=Stake  P=Poder Comp.", f, Brushes.DarkGray, x + 6, iy);
        }

        // 2.6 snapshots + 4.8 bloques coloreados + 3.5 dispose
        private void picBlockchain_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            var chain = engine?.Chain?.ToList();
            if (chain == null || chain.Count == 0) return;

            int bw = 110, bh = 70, sp = 20, sx = 10, sy = 10;

            for (int i = 0; i < chain.Count; i++)
            {
                var block = chain[i];
                int x = sx + i * (bw + sp);

                // Flecha
                if (i > 0)
                {
                    int px = x - sp, ay = sy + bh / 2;
                    using var ap = new Pen(Color.Gray, 2);
                    g.DrawLine(ap, px, ay, x, ay);
                    g.DrawLine(ap, x - 8, ay - 4, x, ay);
                    g.DrawLine(ap, x - 8, ay + 4, x, ay);
                }

                // 4.8 — Color por tipo de consenso
                Color blockColor = block.Index == 0 ? Color.LightBlue :
                    block.ConsensusUsed switch
                    {
                        ConsensusType.ProofOfWork => Color.FromArgb(255, 220, 180),
                        ConsensusType.ProofOfStake => Color.FromArgb(180, 255, 180),
                        ConsensusType.PBFT => Color.FromArgb(220, 180, 255),
                        _ => Color.White
                    };

                Rectangle rect = new(x, sy, bw, bh);
                using var bb = new SolidBrush(blockColor);
                g.FillRectangle(bb, rect);
                g.DrawRectangle(Pens.DarkGray, rect);

                string hash = string.IsNullOrEmpty(block.Hash) ? "" : block.Hash.Substring(0, Math.Min(6, block.Hash.Length)) + "...";
                string val = block.Validator ?? "?";
                if (val.Length > 14) val = val.Substring(0, 14);
                string txt = $"Blk #{block.Index}\n{val}\n{hash}";

                using var bf = new Font("Segoe UI", 7.5f);
                using var bsf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                g.DrawString(txt, bf, Brushes.Black, rect, bsf);
            }
        }
    }
}
