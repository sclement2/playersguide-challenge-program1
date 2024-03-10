using System;

namespace dotnetcore
{
    class Program
    {
        static void Main(string[] args)
        {
            int var1 = 0;
            string sayThis = "Hello World!";
            SayHello(sayThis);
        }

        static void SayHello(string sayThis)
        {
            Console.WriteLine(sayThis);
        }
    }
}
