using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    class DirectMappedProgram
    {
        static int[] addresses = new int[] { 16, 20, 24, 28, 32, 36, 60, 64, 56, 60, 64, 68, 56, 60, 64, 72, 76, 92, 96, 100, 104, 108, 112, 120, 124, 128, 144, 148 };
        static int numRows, blockSize, totalCPI, numMisses;

        static void Main(string[] args)
        {
            Console.WriteLine("How many rows?");
            Int32.TryParse(Console.ReadLine(), out numRows);
            Console.WriteLine("What is the block size (in bytes)?");
            Int32.TryParse(Console.ReadLine(), out blockSize);
            totalCPI = 0;

            Cache c = new Cache();

            c.firstIteration();
            c.addAll();

            double averageCPI = (totalCPI * 1.0) / addresses.Length;
            int numLRUandTAGBits = Convert.ToInt32(Math.Ceiling(Math.Log(numRows * 1.0, 2.0)));
            int numOffsetBits = Convert.ToInt32(Math.Ceiling(Math.Log(blockSize * 1.0, 2.0)));
            int numRowBits = Convert.ToInt32(Math.Ceiling(Math.Log(numRows * 1.0, 2.0)));
            int totalBits = numRows * ((blockSize * 8) + 1 + numLRUandTAGBits + numOffsetBits + numRowBits);
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
                int row = (address / blockSize) % numRows;
                int tag = address / (blockSize * numRows);
                int offset = address % blockSize;

                if (!totalBlocks.ContainsKey(row))
                {
                    totalBlocks.Add(row, new block(tag, row));
                    totalCPI += 12 + blockSize;
                    numMisses++;
                    Console.WriteLine("Accessing: " + address + " (tag: " + tag + "): miss - cached to row " + row);
                }
                else if (!totalBlocks[row].data[offset].Equals(address))
                {
                    totalBlocks[row].data[offset] = address;
                    totalCPI += 12 + blockSize;
                    numMisses++;
                    Console.WriteLine("Accessing: " + address + " (tag: " + tag + "): miss - cached to row " + row);
                }
                else
                {
                    totalCPI++;
                    Console.WriteLine("Accessing: " + address + " (tag: " + tag + "): hit from row " + row);
                }
            }

            public void AddDataWithoutPrinting(int address)
            {
                int row = (address / blockSize) % numRows;
                int tag = address / (blockSize * numRows);
                int offset = address % blockSize;

                if (!totalBlocks.ContainsKey(row))
                {
                    totalBlocks.Add(row, new block(tag, row));
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
            public int row;

            public block(int TAG, int ROW)
            {
                data = new int[blockSize];
                tag = TAG;
                row = ROW;
            }
        }
    }
}
