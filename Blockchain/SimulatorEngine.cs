using System;
using System.Collections.Generic;
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

        // Visual State properties added here
        public Node CurrentProposer { get; private set; }
        public List<Node> ParticipatingNodes { get; private set; } = new();
        public string ActiveAction { get; private set; } = "";

        public event Action<string> OnLogEvent;
        public event Action OnStateChanged;

        public SimulatorEngine()
        {
            InitializeGenesisBlock();
        }

        private void InitializeGenesisBlock()
        {
            var genesisBlock = new Block(0, "0", new List<Transaction>());
            genesisBlock.Validator = "Genesis";
            genesisBlock.Hash = genesisBlock.CalculateHash();
            Chain.Add(genesisBlock);
        }

        public void SetupNodes(int count, int maliciousCount)
        {
            Nodes.Clear();
            Random rnd = new Random();
            for (int i = 0; i < count; i++)
            {
                Nodes.Add(new Node
                {
                    Id = i + 1,
                    Stake = rnd.Next(10, 100),
                    ComputationalPower = rnd.Next(1, 10),
                    IsMalicious = i < maliciousCount
                });
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

            bool success = false;

            if (Consensus == ConsensusType.ProofOfWork)
            {
                success = await SimulatePoW(newBlock);
            }
            else if (Consensus == ConsensusType.ProofOfStake)
            {
                success = await SimulatePoS(newBlock);
            }
            else if (Consensus == ConsensusType.PBFT)
            {
                success = await SimulatePBFT(newBlock);
            }

            if (success)
            {
                Chain.Add(newBlock);
                ActiveAction = "Bloque Aceptado";
                CurrentProposer = null;
                ParticipatingNodes.Clear();
                OnLogEvent?.Invoke($"Bloque #{newBlock.Index} validado y agregado a la cadena.\nHash: {newBlock.Hash.Substring(0, 10)}...");
            }
            else
            {
                ActiveAction = "Bloque Rechazado";
                CurrentProposer = null;
                ParticipatingNodes.Clear();
                OnLogEvent?.Invoke($"Bloque #{newBlock.Index} RECHAZADO.");
                // Return transactions to pool
                PendingTransactions.InsertRange(0, txToProcess);
            }
            OnStateChanged?.Invoke();
            await Task.Delay(1000);
            ActiveAction = "";
            OnStateChanged?.Invoke();
        }

        private async Task<bool> SimulatePoW(Block block)
        {
            ActiveAction = "Compitiendo (Minería PoW)...";
            ParticipatingNodes = Nodes.ToList();
            OnStateChanged?.Invoke();

            OnLogEvent?.Invoke("Los nodos están compitiendo por minar el bloque...");
            Random rnd = new();
            string prefix = new string('0', PoWDifficulty);
            
            // Weight probability by computational power
            var miners = Nodes.OrderByDescending(n => n.ComputationalPower + rnd.Next(-2, 3)).ToList();
            
            var winner = miners.First();
            CurrentProposer = winner;
            ActiveAction = $"Minando (PoW) - {winner.Name}";
            OnStateChanged?.Invoke();

            OnLogEvent?.Invoke($"{winner.Name} está intentando resolver el puzzle (dificultad {PoWDifficulty}).");
            
            await Task.Delay(1000 + (PoWDifficulty * 500) / winner.ComputationalPower); // Simulate work

            if (winner.IsMalicious && rnd.Next(100) < 50)
            {
                // Malicious node tries to propose invalid block
                ActiveAction = $"Bloque Inválido Propuesto - {winner.Name}";
                ParticipatingNodes = Nodes.Where(n => n != winner).ToList();
                OnStateChanged?.Invoke();

                OnLogEvent?.Invoke($"¡{winner.Name} (Malicioso) intentó inyectar un bloque inválido!");
                block.Validator = winner.Name;
                block.Hash = "invalidhash123";
                
                OnLogEvent?.Invoke("La red verifica el bloque...");
                await Task.Delay(1000);
                OnLogEvent?.Invoke("La red RECHAZA el bloque inválido.");
                return false;
            }
            
            // Actually mine
            while (!block.Hash.StartsWith(prefix))
            {
                block.Nonce++;
                block.Hash = block.CalculateHash();
            }
            
            ActiveAction = $"Bloque Encontrado - {winner.Name}";
            ParticipatingNodes.Clear();
            OnStateChanged?.Invoke();

            block.Validator = winner.Name;
            OnLogEvent?.Invoke($"{winner.Name} encontró el hash: {block.Hash.Substring(0, 15)}... tras {block.Nonce} intentos.");
            await Task.Delay(1000);
            return true;
        }

        private async Task<bool> SimulatePoS(Block block)
        {
            ActiveAction = "Seleccionando Validador (PoS)...";
            OnStateChanged?.Invoke();

            OnLogEvent?.Invoke("Seleccionando validador proporcional al Stake...");
            await Task.Delay(1000);
            
            double totalStake = Nodes.Sum(n => n.Stake);
            Random rnd = new Random();
            double randVal = rnd.NextDouble() * totalStake;
            
            double sum = 0;
            Node selected = Nodes.Last();
            foreach (var node in Nodes)
            {
                sum += node.Stake;
                if (randVal <= sum)
                {
                    selected = node;
                    break;
                }
            }

            CurrentProposer = selected;
            ActiveAction = $"Validador Elegido - {selected.Name}";
            OnStateChanged?.Invoke();

            OnLogEvent?.Invoke($"Validador seleccionado: {selected.Name} (Stake: {selected.Stake})");
            await Task.Delay(1000);

            if (selected.IsMalicious)
            {
                ActiveAction = $"Ataque PoS - {selected.Name}";
                ParticipatingNodes = Nodes.Where(n => n != selected).ToList();
                OnStateChanged?.Invoke();

                OnLogEvent?.Invoke($"¡{selected.Name} es malicioso e intentó robar fondos!");
                block.Hash = "invalid_pos_hash";
                OnLogEvent?.Invoke("Los atestiguadores (attestors) revisan el bloque...");
                await Task.Delay(1500);
                ActiveAction = $"Slashing a {selected.Name}!";
                OnStateChanged?.Invoke();

                OnLogEvent?.Invoke($"El bloque fue RECHAZADO por la red. ¡Parte del Stake de {selected.Name} ha sido destruido (Slashing)!");
                selected.Stake *= 0.5; // Slashing punishment
                await Task.Delay(1000);
                return false;
            }

            ActiveAction = $"Bloque Propuesto - {selected.Name}";
            ParticipatingNodes = Nodes.Where(n => n != selected).ToList();
            OnStateChanged?.Invoke();
            await Task.Delay(1000);

            block.Validator = selected.Name;
            block.Hash = block.CalculateHash();
            OnLogEvent?.Invoke($"{selected.Name} atestiguó el bloque correctamente.");
            return true;
        }

        private async Task<bool> SimulatePBFT(Block block)
        {
            ActiveAction = "Fase PBFT: Pre-Prepare";
            OnStateChanged?.Invoke();

            OnLogEvent?.Invoke("PBFT: Iniciando fases Pre-prepare, Prepare y Commit...");
            if(Nodes.Count < 4)
            {
                OnLogEvent?.Invoke("Se necesitan al menos 4 nodos para PBFT (f = 1).");
                return false;
            }

            var leader = Nodes[0];
            CurrentProposer = leader;
            ParticipatingNodes = Nodes.Where(n => n != leader).ToList();
            ActiveAction = $"Líder Asignado - {leader.Name}";
            OnStateChanged?.Invoke();

            OnLogEvent?.Invoke($"Líder asignado: {leader.Name}");
            await Task.Delay(1000);

            if (leader.IsMalicious)
            {
                OnLogEvent?.Invoke($"¡El líder {leader.Name} (malicioso) propone algo inconsistente!");
            }

            // Simulate voting
            int honestCount = Nodes.Count(n => !n.IsMalicious);
            int maliciousCount = Nodes.Count(n => n.IsMalicious);
            int requiredVotes = (int)Math.Ceiling(2.0 * Nodes.Count / 3.0); 

            ActiveAction = "Fase PBFT: Votación (Prepare/Commit)";
            OnStateChanged?.Invoke();

            OnLogEvent?.Invoke($"Votos requeridos (2/3): {requiredVotes}");
            await Task.Delay(1000);

            int yesVotes = 0;
            int noVotes = 0;

            foreach (var node in Nodes)
            {
                if (node == leader) continue; // lider no se vota a si mismo en esta simulacion simple
                
                CurrentProposer = leader;
                ParticipatingNodes.Clear();
                ParticipatingNodes.Add(node);
                ActiveAction = $"Votando: {node.Name}";
                OnStateChanged?.Invoke();

                if (node.IsMalicious)
                {
                    noVotes++;
                    OnLogEvent?.Invoke($"- {node.Name} (M) vota en contra/miente.");
                }
                else
                {
                    if (leader.IsMalicious)
                    {
                        noVotes++;
                        OnLogEvent?.Invoke($"- {node.Name} evalúa líder y vota en contra.");
                    }
                    else
                    {
                        yesVotes++;
                        OnLogEvent?.Invoke($"- {node.Name} vota a favor.");
                    }
                }
                await Task.Delay(500);
            }

            // Lider vota
            yesVotes += leader.IsMalicious ? 0 : 1;
            noVotes += leader.IsMalicious ? 1 : 0;

            CurrentProposer = null;
            ParticipatingNodes.Clear();
            ActiveAction = $"Cálculo de Votos PBFT. ({yesVotes} vs {noVotes})";
            OnStateChanged?.Invoke();
            await Task.Delay(1000);

            OnLogEvent?.Invoke($"Resultados Commit: {yesVotes} Favor, {noVotes} Contra.");
            if (yesVotes >= requiredVotes)
            {
                block.Validator = "Comité PBFT";
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
