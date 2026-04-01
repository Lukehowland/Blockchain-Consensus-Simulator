using System;
using System.Collections.Generic;
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
        private static readonly Random _rng = new();

        // === Sistema de Animación ===
        private readonly List<Particle> particles = new();
        private double pulsePhase = 0;
        private int displayNonce = 0;
        private float blockFlashOpacity = 0;
        private int blockFlashNodeId = -1;
        private float newBlockSlide = 0; // 0→1 para slide-in del nuevo bloque
        private bool isMining = false;

        // Colores target para transición suave
        private readonly Dictionary<int, float> nodeGlowIntensity = new();

        public Form1()
        {
            InitializeComponent();
            SetupEngine(new SimulatorEngine());
        }

        private void SetupEngine(SimulatorEngine eng)
        {
            engine = eng;
            engine.OnLogEvent += Engine_OnLogEvent;
            engine.OnStateChanged += () =>
            {
                // Detectar si estamos minando
                isMining = engine.Nodes.Any(n => n.VisualState == NodeVisualState.Mining);

                // Trigger block flash cuando se encuentra bloque
                if (engine.ActiveAction?.Contains("Encontrado") == true ||
                    engine.ActiveAction?.Contains("Aceptado") == true)
                {
                    blockFlashOpacity = 1.0f;
                    var proposer = engine.CurrentProposer;
                    if (proposer != null) blockFlashNodeId = proposer.Id;
                    newBlockSlide = 0;
                }

                UpdateUI();
            };
            engine.OnMessageSent += (fromId, toId, isPositive) =>
            {
                Action addParticle = () => particles.Add(new Particle
                {
                    FromNodeId = fromId,
                    ToNodeId = toId,
                    Progress = 0,
                    Speed = 0.04f,
                    ParticleColor = isPositive ? Color.FromArgb(0, 220, 100) : Color.FromArgb(255, 60, 60),
                    Symbol = isPositive ? "✓" : "✗"
                });

                if (InvokeRequired) Invoke(addParticle);
                else addParticle();
            };
        }

        // === Timer de Animación (30 FPS) ===
        private void animationTimer_Tick(object sender, EventArgs e)
        {
            pulsePhase += 0.12;
            if (pulsePhase > Math.PI * 2) pulsePhase -= Math.PI * 2;

            // Avanzar partículas
            for (int i = particles.Count - 1; i >= 0; i--)
            {
                particles[i].Progress += particles[i].Speed;
                if (particles[i].IsComplete)
                    particles.RemoveAt(i);
            }

            // Mining nonce animado
            if (isMining)
                displayNonce += _rng.Next(80, 500);

            // Decaer flash de bloque
            if (blockFlashOpacity > 0)
                blockFlashOpacity = Math.Max(0, blockFlashOpacity - 0.025f);

            // Slide-in del nuevo bloque
            if (newBlockSlide < 1.0f)
                newBlockSlide = Math.Min(1.0f, newBlockSlide + 0.04f);

            // Actualizar glow de nodos activos
            foreach (var node in engine.Nodes.ToList())
            {
                if (!nodeGlowIntensity.ContainsKey(node.Id))
                    nodeGlowIntensity[node.Id] = 0;

                float target = node.VisualState != NodeVisualState.Idle ? 1.0f : 0f;
                float current = nodeGlowIntensity[node.Id];
                nodeGlowIntensity[node.Id] = current + (target - current) * 0.15f;
            }

            picNetwork.Invalidate();
            picBlockchain.Invalidate();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cmbConsensus.DataSource = Enum.GetValues(typeof(ConsensusType));
            cmbConsensus.SelectedIndex = 0;
            numNodes.Value = 5;
            numMalicious.Value = 1;

            cmbSpeed.Items.AddRange(new object[] { "Muy Lenta (x3)", "Lenta (x2)", "Normal (x1)", "Rápida (x0.5)", "Muy Rápida (x0.25)" });
            cmbSpeed.SelectedIndex = 2;

            try
            {
                string imgPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "satoshi.png");
                if (!File.Exists(imgPath))
                    imgPath = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath)!, "..", "..", "..", "satoshi.png");
                if (File.Exists(imgPath))
                    picYisus.Image = Image.FromFile(imgPath);
            }
            catch { }

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

            dgvMetrics.Rows.Clear();
            foreach (var m in engine.MetricsHistory)
            {
                dgvMetrics.Rows.Add(m.Type, $"#{m.BlockIndex}", m.ElapsedMs, m.HashAttempts,
                    m.MessagesExchanged, m.Success ? "✓" : "✗", m.EnergyEstimate);
            }

            int neededWidth = Math.Max(panelBlockchain.ClientSize.Width, engine.Chain.Count * 140 + 30);
            picBlockchain.Size = new Size(neededWidth, Math.Max(100, panelBlockchain.ClientSize.Height - 28));
        }

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

        private async void btnStart_Click(object sender, EventArgs e)
        {
            engine.Consensus = (ConsensusType)cmbConsensus.SelectedItem;
            engine.PoWDifficulty = (int)numDifficulty.Value;
            engine.SpeedMultiplier = cmbSpeed.SelectedIndex switch
            {
                0 => 3.0, 1 => 2.0, 2 => 1.0, 3 => 0.5, 4 => 0.25, _ => 1.0
            };
            btnStart.Enabled = false;
            btnAddTx.Enabled = false;
            btnSetup.Enabled = false;

            try { await engine.StartConsensusAsync(); }
            catch (Exception ex) { Engine_OnLogEvent($"Error: {ex.Message}"); }
            finally
            {
                btnStart.Enabled = true;
                btnAddTx.Enabled = true;
                btnSetup.Enabled = true;
                isMining = false;
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            particles.Clear();
            nodeGlowIntensity.Clear();
            blockFlashOpacity = 0;
            isMining = false;
            SetupEngine(new SimulatorEngine());
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

        // ========== PAINT: Red de Nodos ==========
        private void picNetwork_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            var nodes = engine.Nodes.ToList();
            var participating = engine.ParticipatingNodes.ToList();
            var proposer = engine.CurrentProposer;

            int nodeCount = nodes.Count;
            if (nodeCount == 0) return;

            int cx = picNetwork.Width / 2;
            int cy = picNetwork.Height / 2;
            int radius = Math.Min(cx, cy) - 65;

            // Calcular posiciones
            var positions = new Dictionary<int, PointF>();
            for (int i = 0; i < nodeCount; i++)
            {
                double angle = (2 * Math.PI * i) / nodeCount - Math.PI / 2;
                float x = cx + (float)(radius * Math.Cos(angle));
                float y = cy + (float)(radius * Math.Sin(angle));
                positions[nodes[i].Id] = new PointF(x, y);
            }

            // 1) Mesh P2P
            using var meshPen = new Pen(Color.FromArgb(30, Color.Gray), 1);
            for (int i = 0; i < nodeCount; i++)
                for (int j = i + 1; j < nodeCount; j++)
                    g.DrawLine(meshPen, positions[nodes[i].Id], positions[nodes[j].Id]);

            // 2) Líneas del proposer (animadas con dash offset)
            if (proposer != null && positions.ContainsKey(proposer.Id))
            {
                PointF pStart = positions[proposer.Id];
                foreach (var target in participating)
                {
                    if (!positions.ContainsKey(target.Id)) continue;
                    using var connPen = new Pen(target.IsMalicious || proposer.IsMalicious ? Color.FromArgb(180, Color.Red) : Color.FromArgb(180, Color.Orange), 2.5f)
                    {
                        DashStyle = DashStyle.Dash,
                        DashOffset = (float)(pulsePhase * 3 % 20)
                    };
                    g.DrawLine(connPen, pStart, positions[target.Id]);
                }
            }

            // 3) Dibujar partículas viajando
            foreach (var particle in particles.ToList())
            {
                if (!positions.ContainsKey(particle.FromNodeId) || !positions.ContainsKey(particle.ToNodeId)) continue;
                var from = positions[particle.FromNodeId];
                var to = positions[particle.ToNodeId];
                float px = from.X + (to.X - from.X) * particle.Progress;
                float py = from.Y + (to.Y - from.Y) * particle.Progress;

                // Estela (trail)
                for (int tr = 3; tr >= 0; tr--)
                {
                    float trailProgress = particle.Progress - tr * 0.03f;
                    if (trailProgress < 0) continue;
                    float tx = from.X + (to.X - from.X) * trailProgress;
                    float ty = from.Y + (to.Y - from.Y) * trailProgress;
                    int alpha = 200 - tr * 50;
                    float sz = 8 - tr * 1.5f;
                    using var trailBrush = new SolidBrush(Color.FromArgb(alpha, particle.ParticleColor));
                    g.FillEllipse(trailBrush, tx - sz / 2, ty - sz / 2, sz, sz);
                }

                // Símbolo
                using var symFont = new Font("Segoe UI", 8, FontStyle.Bold);
                using var symBrush = new SolidBrush(particle.ParticleColor);
                g.DrawString(particle.Symbol, symFont, symBrush, px - 6, py - 8);
            }

            // 4) Dibujar nodos con animación
            int baseRadius = 25;
            using var nodeFont = new Font("Segoe UI", 9, FontStyle.Bold);
            using var labelFont = new Font("Segoe UI", 7);
            using var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };

            foreach (var node in nodes)
            {
                PointF pos = positions[node.Id];

                // Pulsación para nodos activos
                float glow = nodeGlowIntensity.ContainsKey(node.Id) ? nodeGlowIntensity[node.Id] : 0;
                float pulseScale = 1.0f + glow * 0.15f * (float)Math.Sin(pulsePhase * 1.5);
                int nr = (int)(baseRadius * pulseScale);

                RectangleF rect = new(pos.X - nr, pos.Y - nr, nr * 2, nr * 2);

                // Glow exterior para nodos activos
                if (glow > 0.1f)
                {
                    int glowRadius = nr + (int)(8 * glow);
                    using var glowBrush = new SolidBrush(Color.FromArgb((int)(30 * glow),
                        node.IsMalicious ? Color.Red : Color.Gold));
                    g.FillEllipse(glowBrush, pos.X - glowRadius, pos.Y - glowRadius, glowRadius * 2, glowRadius * 2);
                }

                // Color de relleno con transición suave
                Color fillColor;
                if (node.IsMalicious)
                    fillColor = node.VisualState == NodeVisualState.Slashed
                        ? Color.FromArgb(100, 0, 0)
                        : Color.IndianRed;
                else
                {
                    fillColor = node.VisualState switch
                    {
                        NodeVisualState.Mining => Color.Gold,
                        NodeVisualState.Proposing => Color.FromArgb(120, 230, 120),
                        NodeVisualState.Voting => Color.LightSkyBlue,
                        _ => Color.FromArgb(210, 210, 210)
                    };
                }

                using var fillBrush = new SolidBrush(fillColor);
                g.FillEllipse(fillBrush, rect);

                // Borde según rol
                if (proposer != null && proposer.Id == node.Id)
                {
                    using var bp = new Pen(Color.Goldenrod, 4);
                    g.DrawEllipse(bp, rect);
                }
                else if (participating.Any(p => p.Id == node.Id))
                {
                    using var bp = new Pen(Color.CornflowerBlue, 2.5f);
                    g.DrawEllipse(bp, rect);
                }
                else
                {
                    using var bp = new Pen(Color.FromArgb(100, Color.Gray), 1);
                    g.DrawEllipse(bp, rect);
                }

                // Texto del nodo
                g.DrawString($"N{node.Id}", nodeFont, Brushes.Black, pos, sf);
                g.DrawString($"S={node.Stake:F0}\nP={node.ComputationalPower}", labelFont, Brushes.DarkGray,
                    pos.X - baseRadius, pos.Y + baseRadius + 5);

                // 5) Mining nonce animado sobre el nodo minero
                if (node.VisualState == NodeVisualState.Mining && proposer != null && proposer.Id == node.Id)
                {
                    using var mf = new Font("Consolas", 8, FontStyle.Bold);
                    string nonceHex = $"0x{displayNonce:X6}";
                    g.DrawString(nonceHex, mf, Brushes.Orange, pos.X - 28, pos.Y - baseRadius - 18);

                    // Mini hash cambiante
                    using var hf = new Font("Consolas", 6);
                    string fakeHash = $"{_rng.Next(0x1000):X3}...";
                    g.DrawString(fakeHash, hf, Brushes.DarkOrange, pos.X - 15, pos.Y - baseRadius - 6);
                }
            }

            // 6) Flash dorado cuando se encuentra bloque
            if (blockFlashOpacity > 0 && blockFlashNodeId >= 0 && positions.ContainsKey(blockFlashNodeId))
            {
                var fpos = positions[blockFlashNodeId];
                int flashR = (int)(50 * (1.5f - blockFlashOpacity) + 30);
                int alpha = (int)(blockFlashOpacity * 80);
                using var flashBrush = new SolidBrush(Color.FromArgb(alpha, Color.Gold));
                g.FillEllipse(flashBrush, fpos.X - flashR, fpos.Y - flashR, flashR * 2, flashR * 2);

                // Anillo expansivo
                int ringR = (int)(70 * (1 - blockFlashOpacity) + 30);
                using var ringPen = new Pen(Color.FromArgb(alpha, Color.Goldenrod), 3);
                g.DrawEllipse(ringPen, fpos.X - ringR, fpos.Y - ringR, ringR * 2, ringR * 2);
            }

            // 7) Leyenda
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

        // ========== PAINT: Blockchain ==========
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
                int targetX = sx + i * (bw + sp);

                // Slide-in para el último bloque
                int x = targetX;
                if (i == chain.Count - 1 && newBlockSlide < 1.0f)
                {
                    float ease = (float)(1 - Math.Pow(1 - newBlockSlide, 3)); // ease-out cubic
                    x = targetX + (int)((1 - ease) * 150);
                }

                // Flecha
                if (i > 0)
                {
                    int prevX = sx + (i - 1) * (bw + sp) + bw;
                    int ay = sy + bh / 2;
                    using var ap = new Pen(Color.Gray, 2);
                    g.DrawLine(ap, prevX, ay, x, ay);
                    g.DrawLine(ap, x - 8, ay - 4, x, ay);
                    g.DrawLine(ap, x - 8, ay + 4, x, ay);
                }

                // Color por tipo de consenso
                Color blockColor = block.Index == 0 ? Color.LightBlue :
                    block.ConsensusUsed switch
                    {
                        ConsensusType.ProofOfWork => Color.FromArgb(255, 220, 180),
                        ConsensusType.ProofOfStake => Color.FromArgb(180, 255, 180),
                        ConsensusType.PBFT => Color.FromArgb(220, 180, 255),
                        _ => Color.White
                    };

                // Glow para último bloque nuevo
                if (i == chain.Count - 1 && newBlockSlide < 0.8f)
                {
                    int glowAlpha = (int)((1 - newBlockSlide) * 60);
                    using var glowBrush = new SolidBrush(Color.FromArgb(glowAlpha, blockColor));
                    g.FillRectangle(glowBrush, x - 5, sy - 5, bw + 10, bh + 10);
                }

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
