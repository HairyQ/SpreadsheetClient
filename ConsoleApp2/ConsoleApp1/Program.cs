using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    class Program
    {
        static int[] addresses = new int[] { 16, 20, 24, 28, 32, 36, 60, 64, 56, 60, 64, 68, 56, 60, 64, 72, 76, 92, 96, 100, 104, 108, 112, 120, 124, 128, 144, 148 };
        static int numRows;
        static int blockSize;

        static void Main(string[] args)
        {
            Console.WriteLine("How many rows?");
            Int32.TryParse(Console.ReadLine(), out numRows);
            Console.WriteLine("What is the block size (in bytes)?");
            Int32.TryParse(Console.ReadLine(), out blockSize);

            Cache c = new Cache();

            c.firstIteration();
            c.addAll();

            Console.Read();
        }

        public class Cache
        {
            Dictionary<int, block> totalBlocks;

            public Cache()
            {
                totalBlocks = new Dictionary<int, block>();
            }

            public void firstIteration()
            {
                foreach (int address in addresses)
                    AddDataWithoutPrinting(address);
            }

            public void addAll()
            {
                foreach (int address in addresses)
                    AddData(address);
            }

            public void AddData(int address)
            {
                int tag = address / (blockSize * numRows);
                int offset = address % blockSize;

                if (!totalBlocks.ContainsKey(tag))
                {
                    totalBlocks.Add(tag, new block(tag));
                    Console.WriteLine("Accessing: " + address + " (tag: " + tag + "): miss - cached to row ");
                }
                else if (!totalBlocks[tag].data[offset].Equals(address))
                {
                    totalBlocks[tag].data[offset] = address;
                    totalBlocks[tag].LRU = numRows;
                    foreach(int x in addresses)
                        
                    Console.WriteLine("Accessing: " + address + " (tag: " + tag + "): miss - cached to row ");
                }
                else
                {
                    Console.WriteLine("Accessing: " + address + " (tag: " + tag + "): hit");
                }
            }

            public void AddDataWithoutPrinting(int address)
            {
                int row = (address / blockSize) % numRows;
                int tag = address / (blockSize * numRows);
                int offset = address % blockSize;

                if (!totalBlocks.ContainsKey(row))
                {
                    totalBlocks.Add(row, new block(tag));
                }
                else if (!totalBlocks[row].data[offset].Equals(address))
                {
                    totalBlocks[row].data[offset] = address;
                }
            }
        }

        public class block
        {
            public int[] data;
            public int tag;
            public int LRU;

            public block(int TAG)
            {
                data = new int[blockSize];
                tag = TAG;
                LRU = numRows;
            }
        }
    }
}

