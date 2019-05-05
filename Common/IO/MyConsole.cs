using System;

namespace Common.IO
{
    public class MyConsole : IConsole
    {
        public ConsoleKeyInfo ReadKey() => Console.ReadKey();
        public string ReadLine() => Console.ReadLine();

        public void Write(string value = "") => Console.Write(value);

        public void WriteLine(string value = "") => Console.WriteLine(value);
    }
}
