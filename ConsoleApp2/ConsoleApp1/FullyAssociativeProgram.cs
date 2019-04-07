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
        static int totalCPI;
        static int numMisses;

        static void Main(string[] args)
        {
            totalCPI = 0;
            numMisses = 0;
            Console.WriteLine("How many rows?");
            Int32.TryParse(Console.ReadLine(), out numRows);
            Console.WriteLine("What is the block size (in bytes)?");
            Int32.TryParse(Console.ReadLine(), out blockSize);

            Cache c = new Cache();

            c.firstIteration();
            c.addAll();

            double averageCPI = (totalCPI * 1.0) / blockSize;
            int numLRUandTAGBits = Convert.ToInt32(Math.Ceiling(Math.Log(numRows * 1.0, 2.0)));
            int numOffsetBits = Convert.ToInt32(Math.Ceiling(Math.Log(blockSize * 1.0, 2.0)));
            int totalBits = numRows * ((blockSize * 8) + 1 + (numLRUandTAGBits * 2) + numOffsetBits);
            double missRate = numMisses * 1.0 / addresses.Length;

            Console.WriteLine("Average CPI: " + averageCPI + "\nTotal CPI: " + totalCPI + "\nTotal Bits: " + totalBits
                + "\nMiss Rate: " + missRate);

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
                    foreach (block b in totalBlocks.Values)
                        b.LRU--;

                    totalBlocks.Add(tag, new block(tag));
                    Console.WriteLine("Accessing: " + address + " (tag: " + tag + "): miss");
                    numMisses++;
                    totalCPI += 12 + blockSize;
                }
                else if (!totalBlocks[tag].data[offset].Equals(address))
                {
                    totalBlocks[tag].data[offset] = address;

                    foreach (block b in totalBlocks.Values)
                        b.LRU--;

                    totalBlocks[tag].LRU = numRows;


                    Console.WriteLine("Accessing: " + address + " (tag: " + tag + "): miss");
                    totalCPI += 12 + blockSize;
                    numMisses++;
                }
                else
                {
                    Console.WriteLine("Accessing: " + address + " (tag: " + tag + "): hit");
                    totalCPI++;
                }
            }

            public void AddDataWithoutPrinting(int address)
            {
                int tag = address / (blockSize * numRows);
                int offset = address % blockSize;

                if (!totalBlocks.ContainsKey(tag))
                {
                    totalBlocks.Add(tag, new block(tag));
                }
                else if (!totalBlocks[tag].data[offset].Equals(address))
                {
                    totalBlocks[tag].data[offset] = address;
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

