using System;

namespace Broker.Tests
{
    public class Program
    {
        private static void Main(string[] args)
        {
            Bootstrap bootstrap = new Bootstrap();
            bootstrap.Start();
            Console.ReadLine();
        }
    }
}