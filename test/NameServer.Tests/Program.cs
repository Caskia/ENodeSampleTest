using System;

namespace NameServer.Tests
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