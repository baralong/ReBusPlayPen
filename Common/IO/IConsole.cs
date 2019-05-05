using System;

namespace Common.IO
{
    public interface IConsole
    {
        string ReadLine();
        ConsoleKeyInfo ReadKey();
        void WriteLine(string value = "");
        void Write(string value = "");
    }
}
