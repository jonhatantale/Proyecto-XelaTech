using System;

namespace XelaTechSoporte
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Menu menu = new Menu();
            menu.Iniciar();
        }
    }
}
