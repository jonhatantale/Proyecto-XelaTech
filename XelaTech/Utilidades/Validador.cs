using System;

namespace XelaTechSoporte
{
    // Lee datos del teclado sin romper el menu (aqui si se usa try-catch)
    public static class Validador
    {
        public static bool TextoValido(string texto)
        {
            return !string.IsNullOrWhiteSpace(texto);
        }

        public static int LeerEntero(string mensaje, int minimo, int maximo)
        {
            while (true)
            {
                Console.Write(mensaje);
                try
                {
                    int valor = int.Parse(Console.ReadLine());
                    if (valor < minimo || valor > maximo)
                    {
                        Console.WriteLine("Ingrese un número entre " + minimo + " y " + maximo + ".");
                    }
                    else
                    {
                        return valor;
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Entrada inválida. Ingrese un número entero.");
                }
            }
        }

        public static double LeerDecimal(string mensaje, double minimo)
        {
            while (true)
            {
                Console.Write(mensaje);
                try
                {
                    double valor = double.Parse(Console.ReadLine());
                    if (valor < minimo)
                    {
                        Console.WriteLine("El valor no puede ser menor a " + minimo + ".");
                    }
                    else
                    {
                        return valor;
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Entrada inválida. Ingrese un número.");
                }
            }
        }

        public static string LeerTexto(string mensaje)
        {
            while (true)
            {
                Console.Write(mensaje);
                string texto = Console.ReadLine();
                if (TextoValido(texto))
                {
                    return texto.Trim();
                }
                Console.WriteLine("Este campo no puede estar vacío.");
            }
        }
    }
}
