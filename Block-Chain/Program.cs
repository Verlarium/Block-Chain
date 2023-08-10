using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Block_Chain
{
    class Program
    {
        private static Blockchain chain = new Blockchain();

        static void Main(string[] args)
        {
            Console.WriteLine("Original Block Chain:");
            DisplayBlockChain();
            Console.WriteLine("Modified Block Chain:");
            chain.blocks[3].data = "sunday";
            DisplayBlockChain();
            Console.WriteLine("Modified Block Chain with Update:");
            chain.Update(chain.blocks[3]);
            DisplayBlockChain();
            Console.ReadLine();
        }

        public static void DisplayBlockChain()
        {
            foreach (Block block in chain.blocks)
            {
                Console.WriteLine($"Block <{block.index}> [ data: {block.data}, timeStamp: {block.timeStamp} ]");
            }
            Console.WriteLine("Block chain is " + (chain.isValid() ? "valid!" : "not valid!") + Environment.NewLine);
        }
    }

    public class Blockchain
    {
        public List<Block> blocks = new List<Block>();

        public Blockchain()
        {
            // Create first block
            CreateGensisBlock();

            // Add new blocks to chain
            AddBlock(new Block(1, "08/07/2023", "Monday"));
            AddBlock(new Block(2, "08/08/2023", "Tuesday"));
            AddBlock(new Block(3, "08/09/2023", "Wednesday"));
            AddBlock(new Block(4, "08/10/2023", "Thursday"));
            AddBlock(new Block(5, "08/11/2023", "Friday"));
            AddBlock(new Block(6, "08/12/2023", "Saturday"));
        }

        public void CreateGensisBlock()
        {
            blocks.Add(new Block(0, "08/06/2023", "Sunday"));
        }

        public void AddBlock(Block block)
        {
            block.previousHash = blocks[blocks.Count - 1].hash;
            block.hash = CalculateHash(block);
            blocks.Add(block);
        }

        public void Update(Block block)
        {
            block.hash = CalculateHash(block);
            if (block.index != blocks.Count - 1)
            {
                for (int i = block.index + 1; i < blocks.Count; i++)
                {
                    blocks[i].previousHash = CalculateHash(blocks[i - 1]);
                    blocks[i].hash = CalculateHash(blocks[i]);
                }
            }
        }

        public bool isValid()
        {
            for (int i = 1; i < blocks.Count; i++)
            {
                var currentBlock = blocks[i];
                var previousBlock = blocks[i - 1];

                if (currentBlock.hash != CalculateHash(currentBlock))
                {
                    return false;
                }
                if (currentBlock.previousHash != previousBlock.hash)
                {
                    return false;
                }
            }
            return true;
        }

        public static string CalculateHash(Block block)
        {
            using (var sha256 = new SHA256Managed())
            {
                return BitConverter.ToString(sha256.ComputeHash(Encoding.UTF8.GetBytes(block.index.ToString() + block.previousHash + block.timeStamp + block.data))).Replace("-", "");
            }
        }
    }

    public class Block
    {
        public int index;
        public string timeStamp;
        public string data;
        public string previousHash;
        public string hash;

        public Block(int index, string timeStamp, string data, string previousHash = "")
        {
            this.index = index;
            this.timeStamp = timeStamp;
            this.data = data;
            this.previousHash = previousHash;
            this.hash = Blockchain.CalculateHash(this);
        }
    }
}
