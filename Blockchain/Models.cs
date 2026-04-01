using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Blockchain.Models
{
    public class Transaction
    {
        public string Sender { get; set; }
        public string Receiver { get; set; }
        public decimal Amount { get; set; }

        public override string ToString()
        {
            return $"{Sender} -> {Receiver}: {Amount}";
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
}
