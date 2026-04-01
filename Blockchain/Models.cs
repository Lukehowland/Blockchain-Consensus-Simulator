using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Blockchain.Models
{
    // 4.1 — Enum para estado visual de nodos
    public enum NodeVisualState
    {
        Idle,
        Proposing,
        Mining,
        Voting,
        Attacking,
        Slashed
    }

    public class Transaction
    {
        public string Sender { get; set; }
        public string Receiver { get; set; }
        public decimal Amount { get; set; }

        public override string ToString()
        {
            return $"{Sender} -> {Receiver}: {Amount:F2}";
        }
    }

    public class Block
    {
        public int Index { get; set; }
        public DateTime Timestamp { get; set; }
        public List<Transaction> Transactions { get; set; }
        public string PreviousHash { get; set; }
        public string Hash { get; set; }
        public int Nonce { get; set; }
        public string Validator { get; set; }
        public ConsensusType ConsensusUsed { get; set; } // 4.8 — Para colorear bloques

        public Block(int index, string previousHash, List<Transaction> transactions)
        {
            Index = index;
            Timestamp = DateTime.Now;
            PreviousHash = previousHash;
            Transactions = transactions;
            Hash = CalculateHash();
        }

        public string CalculateHash()
        {
            string txData = string.Join(";", Transactions.Select(t => t.ToString()));
            string input = $"{Index}-{Timestamp:O}-{PreviousHash}-{Nonce}-{Validator}-{txData}";
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                return Convert.ToHexString(bytes).ToLower();
            }
        }
    }

    public class Node
    {
        public int Id { get; set; }
        public string Name => $"Nodo {Id}";
        public double Stake { get; set; }
        public int ComputationalPower { get; set; }
        public bool IsMalicious { get; set; }
        public NodeVisualState VisualState { get; set; } = NodeVisualState.Idle; // 4.1

        public override string ToString()
        {
            return $"{Name} (Stake: {Stake}, Poder: {ComputationalPower}, {(IsMalicious ? "Malicioso" : "Honesto")})";
        }
    }

    public enum ConsensusType
    {
        ProofOfWork,
        ProofOfStake,
        PBFT
    }

    // 4.2 — Clase para métricas de consenso
    public class ConsensusMetrics
    {
        public ConsensusType Type { get; set; }
        public int BlockIndex { get; set; }
        public long ElapsedMs { get; set; }
        public int HashAttempts { get; set; }
        public int MessagesExchanged { get; set; }
        public int NodesParticipating { get; set; }
        public bool Success { get; set; }
        public string EnergyEstimate { get; set; }
    }
}
