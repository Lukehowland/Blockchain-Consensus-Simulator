using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Blockchain.Models
{
    public class SimulatorEngine
    {
        public List<Node> Nodes { get; private set; } = new();
        public List<Block> Chain { get; private set; } = new();
        public List<Transaction> PendingTransactions { get; private set; } = new();

        public ConsensusType Consensus { get; set; }

        // Settings
        public int PoWDifficulty { get; set; } = 3;
        public double SpeedMultiplier { get; set; } = 1.0; // 1.0=normal, 2.0=lenta, 0.5=rápida

        private Task SimDelay(int baseMs) => Task.Delay((int)(baseMs * SpeedMultiplier));

        // Visual State
        public Node CurrentProposer { get; private set; }
        public List<Node> ParticipatingNodes { get; private set; } = new();
        public string ActiveAction { get; private set; } = "";

        // 4.2 — Metrics
        public List<ConsensusMetrics> MetricsHistory { get; private set; } = new();
        public ConsensusMetrics CurrentMetrics { get; private set; }

        public event Action<string> OnLogEvent;
        public event Action OnStateChanged;

        public SimulatorEngine()
        {
            InitializeGenesisBlock();
        }

        private void InitializeGenesisBlock()
        {
            var genesis = new Block(0, "0", new List<Transaction>());
            genesis.Validator = "Genesis";
            genesis.Hash = genesis.CalculateHash();
            Chain.Add(genesis);
        }

        // 3.7 — SetupNodes limpia cadena y transacciones
        public void SetupNodes(int count, int maliciousCount)
        {
            Nodes.Clear();
            Chain.Clear();
            PendingTransactions.Clear();
            InitializeGenesisBlock();

            Random rnd = new Random();

            // Crear todos los nodos honestos primero
            for (int i = 0; i < count; i++)
            {
                Nodes.Add(new Node
                {
                    Id = i + 1,
                    Stake = rnd.Next(10, 100),
                    ComputationalPower = rnd.Next(1, 10),
                    IsMalicious = false
                });
            }

            // 1.2 — Marcar maliciosos aleatoriamente
            foreach (var node in Nodes.OrderBy(_ => rnd.Next()).Take(maliciousCount))
            {
                node.IsMalicious = true;
            }

            OnLogEvent?.Invoke($"Configurados {count} nodos ({maliciousCount} maliciosos).");
            OnStateChanged?.Invoke();
        }

        public void AddTransaction(Transaction t)
        {
            PendingTransactions.Add(t);
            OnLogEvent?.Invoke($"Nueva Tx: {t}");
            OnStateChanged?.Invoke();
        }

        private void ResetVisualStates()
        {
            foreach (var node in Nodes)
                node.VisualState = NodeVisualState.Idle;
        }

        public async Task StartConsensusAsync()
        {
            if (PendingTransactions.Count == 0)
            {
                OnLogEvent?.Invoke("No hay transacciones pendientes para procesar.");
                return;
            }
            if (Nodes.Count == 0)
            {
                OnLogEvent?.Invoke("No hay nodos en la red. Por favor, configura los nodos primero.");
                return;
            }

            OnLogEvent?.Invoke($"\n--- Iniciando Consenso: {Consensus} ---");

            var txToProcess = PendingTransactions.ToList();
            PendingTransactions.Clear();
            OnStateChanged?.Invoke();

            var prevBlock = Chain.Last();
            var newBlock = new Block(prevBlock.Index + 1, prevBlock.Hash, txToProcess);
            newBlock.ConsensusUsed = Consensus;

            // 4.2 — Start metrics
            var sw = Stopwatch.StartNew();
            CurrentMetrics = new ConsensusMetrics
            {
                Type = Consensus,
                BlockIndex = newBlock.Index,
                NodesParticipating = Nodes.Count
            };

            bool success = false;
            if (Consensus == ConsensusType.ProofOfWork)
                success = await SimulatePoW(newBlock);
            else if (Consensus == ConsensusType.ProofOfStake)
                success = await SimulatePoS(newBlock);
            else if (Consensus == ConsensusType.PBFT)
                success = await SimulatePBFT(newBlock);

            sw.Stop();
            CurrentMetrics.ElapsedMs = sw.ElapsedMilliseconds;
            CurrentMetrics.Success = success;

            // Energy estimate
            CurrentMetrics.EnergyEstimate = Consensus switch
            {
                ConsensusType.ProofOfWork => $"{CurrentMetrics.HashAttempts * 0.05:F2} kWh",
                ConsensusType.ProofOfStake => $"{CurrentMetrics.NodesParticipating * 0.001:F4} kWh",
                _ => $"{CurrentMetrics.MessagesExchanged * 0.0005:F4} kWh"
            };
            MetricsHistory.Add(CurrentMetrics);

            if (success)
            {
                Chain.Add(newBlock);
                ActiveAction = "Bloque Aceptado";
                OnLogEvent?.Invoke($"Bloque #{newBlock.Index} validado y agregado a la cadena.\nHash: {newBlock.Hash.Substring(0, 10)}...");
            }
            else
            {
                ActiveAction = "Bloque Rechazado";
                OnLogEvent?.Invoke($"Bloque #{newBlock.Index} RECHAZADO.");
                PendingTransactions.InsertRange(0, txToProcess);
            }

            CurrentProposer = null;
            ParticipatingNodes.Clear();
            ResetVisualStates();
            OnStateChanged?.Invoke();
            await SimDelay(1000);
            ActiveAction = "";
            OnStateChanged?.Invoke();
        }

        private async Task<bool> SimulatePoW(Block block)
        {
            ActiveAction = "Compitiendo (Minería PoW)...";
            ParticipatingNodes = Nodes.ToList();
            foreach (var n in Nodes) n.VisualState = NodeVisualState.Mining;
            OnStateChanged?.Invoke();

            OnLogEvent?.Invoke("Los nodos están compitiendo por minar el bloque...");
            Random rnd = new();
            string prefix = new string('0', PoWDifficulty);

            // 2.3 — Selección probabilística por ComputationalPower
            double totalPower = Nodes.Sum(n => (double)n.ComputationalPower);
            double randVal = rnd.NextDouble() * totalPower;
            double sum = 0;
            Node winner = Nodes.Last();
            foreach (var node in Nodes)
            {
                sum += node.ComputationalPower;
                if (randVal <= sum) { winner = node; break; }
            }

            CurrentProposer = winner;
            winner.VisualState = NodeVisualState.Proposing;
            ActiveAction = $"Minando (PoW) - {winner.Name}";
            OnStateChanged?.Invoke();

            OnLogEvent?.Invoke($"{winner.Name} está intentando resolver el puzzle (dificultad {PoWDifficulty}).");
            await SimDelay(500);

            if (winner.IsMalicious && rnd.Next(100) < 50)
            {
                winner.VisualState = NodeVisualState.Attacking;
                ActiveAction = $"Bloque Inválido Propuesto - {winner.Name}";
                ParticipatingNodes = Nodes.Where(n => n != winner).ToList();
                OnStateChanged?.Invoke();

                OnLogEvent?.Invoke($"¡{winner.Name} (Malicioso) intentó inyectar un bloque inválido!");
                block.Validator = winner.Name;
                block.Hash = "invalidhash123";
                CurrentMetrics.HashAttempts = 0;
                CurrentMetrics.MessagesExchanged = 1;

                OnLogEvent?.Invoke("La red verifica el bloque...");
                await SimDelay(1000);
                OnLogEvent?.Invoke("La red RECHAZA el bloque inválido.");
                return false;
            }

            // 1.1 — Asignar Validator ANTES del loop de minería
            block.Validator = winner.Name;

            // 3.1 — Mining loop en background thread
            await Task.Run(() =>
            {
                while (!block.Hash.StartsWith(prefix))
                {
                    block.Nonce++;
                    block.Hash = block.CalculateHash();
                }
            });

            CurrentMetrics.HashAttempts = block.Nonce;
            CurrentMetrics.MessagesExchanged = Nodes.Count;

            ActiveAction = $"Bloque Encontrado - {winner.Name}";
            ParticipatingNodes.Clear();
            OnStateChanged?.Invoke();

            OnLogEvent?.Invoke($"{winner.Name} encontró el hash: {block.Hash.Substring(0, 15)}... tras {block.Nonce} intentos.");
            await SimDelay(1000);
            return true;
        }

        private async Task<bool> SimulatePoS(Block block)
        {
            ActiveAction = "Seleccionando Validador (PoS)...";
            OnStateChanged?.Invoke();

            OnLogEvent?.Invoke("Seleccionando validador proporcional al Stake...");
            await SimDelay(1000);

            double totalStake = Nodes.Sum(n => n.Stake);
            Random rnd = new();
            double randVal = rnd.NextDouble() * totalStake;
            double sum = 0;
            Node selected = Nodes.Last();
            foreach (var node in Nodes)
            {
                sum += node.Stake;
                if (randVal <= sum) { selected = node; break; }
            }

            CurrentProposer = selected;
            selected.VisualState = NodeVisualState.Proposing;
            ActiveAction = $"Validador Elegido - {selected.Name}";
            OnStateChanged?.Invoke();

            OnLogEvent?.Invoke($"Validador seleccionado: {selected.Name} (Stake: {selected.Stake})");
            await SimDelay(1000);

            int messages = 1;

            if (selected.IsMalicious)
            {
                selected.VisualState = NodeVisualState.Attacking;
                ActiveAction = $"Ataque PoS - {selected.Name}";
                ParticipatingNodes = Nodes.Where(n => n != selected).ToList();
                OnStateChanged?.Invoke();

                OnLogEvent?.Invoke($"¡{selected.Name} es malicioso e intentó robar fondos!");
                block.Hash = "invalid_pos_hash";
                OnLogEvent?.Invoke("Los atestiguadores (attestors) revisan el bloque...");
                await SimDelay(1500);

                selected.VisualState = NodeVisualState.Slashed;
                ActiveAction = $"Slashing a {selected.Name}!";
                OnStateChanged?.Invoke();

                OnLogEvent?.Invoke($"El bloque fue RECHAZADO por la red. ¡Parte del Stake de {selected.Name} ha sido destruido (Slashing)!");
                selected.Stake *= 0.5;
                CurrentMetrics.MessagesExchanged = Nodes.Count;
                await SimDelay(1000);
                return false;
            }

            block.Validator = selected.Name;
            block.Hash = block.CalculateHash();

            ActiveAction = $"Bloque Propuesto - {selected.Name}";
            ParticipatingNodes = Nodes.Where(n => n != selected).ToList();
            OnStateChanged?.Invoke();
            await SimDelay(500);

            // 2.4 — Fase de atestiguación
            OnLogEvent?.Invoke("Fase de atestiguación: los demás nodos verifican el bloque...");
            int yesVotes = 1;
            int noVotes = 0;
            int requiredVotes = (int)Math.Ceiling(2.0 * Nodes.Count / 3.0);

            foreach (var node in Nodes.Where(n => n != selected))
            {
                node.VisualState = NodeVisualState.Voting;
                ActiveAction = $"Atestiguando: {node.Name}";
                ParticipatingNodes.Clear();
                ParticipatingNodes.Add(node);
                OnStateChanged?.Invoke();
                messages++;

                if (node.IsMalicious)
                {
                    noVotes++;
                    node.VisualState = NodeVisualState.Attacking;
                    OnLogEvent?.Invoke($"- {node.Name} (M) vota en contra del bloque.");
                }
                else
                {
                    yesVotes++;
                    OnLogEvent?.Invoke($"- {node.Name} atestigua a favor del bloque.");
                }
                await SimDelay(300);
            }

            CurrentMetrics.MessagesExchanged = messages;
            OnLogEvent?.Invoke($"Resultados atestiguación: {yesVotes} Favor, {noVotes} Contra (requeridos: {requiredVotes}).");

            if (yesVotes >= requiredVotes)
            {
                OnLogEvent?.Invoke($"{selected.Name} atestiguó el bloque correctamente.");
                return true;
            }
            else
            {
                OnLogEvent?.Invoke("El bloque fue RECHAZADO (no alcanzó 2/3 de atestiguaciones).");
                return false;
            }
        }

        private async Task<bool> SimulatePBFT(Block block)
        {
            ActiveAction = "Fase PBFT: Pre-Prepare";
            OnStateChanged?.Invoke();

            OnLogEvent?.Invoke("PBFT: Iniciando fases Pre-prepare, Prepare y Commit...");
            if (Nodes.Count < 4)
            {
                OnLogEvent?.Invoke("Se necesitan al menos 4 nodos para PBFT (f = 1).");
                return false;
            }

            // 1.2 — Rotación round-robin del líder
            var leader = Nodes[Chain.Count % Nodes.Count];
            CurrentProposer = leader;
            leader.VisualState = NodeVisualState.Proposing;
            ParticipatingNodes = Nodes.Where(n => n != leader).ToList();
            ActiveAction = $"Líder Asignado - {leader.Name}";
            OnStateChanged?.Invoke();

            OnLogEvent?.Invoke($"Líder asignado: {leader.Name}");
            await SimDelay(1000);

            int messages = 1;
            block.Validator = "Comité PBFT";

            // 3.8 — Verificación criptográfica
            string expectedHash;
            if (leader.IsMalicious)
            {
                leader.VisualState = NodeVisualState.Attacking;
                OnLogEvent?.Invoke($"¡El líder {leader.Name} (malicioso) propone algo inconsistente!");
                block.Hash = "bloque_alterado";
                expectedHash = block.CalculateHash();
                OnStateChanged?.Invoke();
            }
            else
            {
                block.Hash = block.CalculateHash();
                expectedHash = block.Hash;
            }

            int requiredVotes = (int)Math.Ceiling(2.0 * Nodes.Count / 3.0);
            ActiveAction = "Fase PBFT: Votación (Prepare/Commit)";
            OnStateChanged?.Invoke();

            OnLogEvent?.Invoke($"Votos requeridos (2/3): {requiredVotes}");
            await SimDelay(1000);

            int yesVotes = 0;
            int noVotes = 0;

            foreach (var node in Nodes)
            {
                if (node == leader) continue;

                node.VisualState = NodeVisualState.Voting;
                CurrentProposer = leader;
                ParticipatingNodes.Clear();
                ParticipatingNodes.Add(node);
                ActiveAction = $"Votando: {node.Name}";
                OnStateChanged?.Invoke();
                messages++;

                if (node.IsMalicious)
                {
                    noVotes++;
                    node.VisualState = NodeVisualState.Attacking;
                    OnLogEvent?.Invoke($"- {node.Name} (M) vota en contra/miente.");
                }
                else
                {
                    bool blockValid = (block.Hash == expectedHash);
                    if (blockValid)
                    {
                        yesVotes++;
                        OnLogEvent?.Invoke($"- {node.Name} verifica hash y vota a favor.");
                    }
                    else
                    {
                        noVotes++;
                        OnLogEvent?.Invoke($"- {node.Name} detecta hash alterado y vota en contra.");
                    }
                }
                await SimDelay(500);
            }

            yesVotes += leader.IsMalicious ? 0 : 1;
            noVotes += leader.IsMalicious ? 1 : 0;

            CurrentMetrics.MessagesExchanged = messages;

            CurrentProposer = null;
            ParticipatingNodes.Clear();
            ActiveAction = $"Cálculo de Votos PBFT. ({yesVotes} vs {noVotes})";
            OnStateChanged?.Invoke();
            await SimDelay(1000);

            OnLogEvent?.Invoke($"Resultados Commit: {yesVotes} Favor, {noVotes} Contra.");
            if (yesVotes >= requiredVotes)
            {
                block.Hash = block.CalculateHash();
                OnLogEvent?.Invoke("Consenso ALCANZADO.");
                return true;
            }
            else
            {
                OnLogEvent?.Invoke("Consenso FALLIDO (No se alcanzó 2/3 de votos).");
                return false;
            }
        }
    }
}
