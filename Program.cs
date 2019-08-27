using System;

namespace PasswordGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            Password pass;

            int nPass, nSize, nMayus, nSpec;
            bool parseResult;

            Console.WriteLine("Cuantos passwords desea generar?");
            while (!int.TryParse(Console.ReadLine(), out nPass) || nPass < 1)
            {
                Console.WriteLine("Se debe introducir un numero mayor a 0");
            }

            Console.WriteLine("De que tamaño será cada password?");
            while (!int.TryParse(Console.ReadLine(), out nSize) || nSize < 1)
            {
                Console.WriteLine("Se debe introducir un numero mayor a 0");
            }

            Console.WriteLine("Cuántas mayúsculas debo poner en los passwords?");
            while (!(parseResult = int.TryParse(Console.ReadLine(), out nMayus)) || nMayus < 0 || nMayus > nSize)
            {
                if (!parseResult || nMayus < 0)
                {
                    Console.WriteLine("Se debe introducir un numero positivo o 0");
                }
                else
                {
                    Console.WriteLine("Se ha exedido la cantidad maxima de caracteres disponibles en cada password");
                    Console.WriteLine($"Le quedan {nSize} caracteres para mayúsculas y especiales, introduzca un numero menor o igual a {nSize}");
                }
            }

            Console.WriteLine("Cuántos caracteres especiales debo poner en los passwords?");
            while (!(parseResult = int.TryParse(Console.ReadLine(), out nSpec)) || nSpec < 0 || nSpec + nMayus > nSize)
            {
                if (!parseResult || nSpec < 0)
                {
                    Console.WriteLine("Se debe introducir un numero positivo o 0");
                }
                else
                {
                    Console.WriteLine("Se ha exedido la cantidad maxima de caracteres disponibles en cada password");
                    Console.WriteLine($"Le quedan {nSize - nMayus} caracteres para especiales, introduzca un numero menor o igual a {nSize - nMayus}");
                }
            }

            Console.Clear();
            Console.WriteLine("Estas son las password generadas:");
            foreach (string s in (pass = new Password(nPass, nSize, nMayus, nSpec)).Passwords)
                Console.WriteLine(s);

            Console.WriteLine();

            int intervalo;
            pass.encryptPassword(intervalo = new Random().Next(-92, 93));

            Console.WriteLine($"Encriptando las password con intervalo {intervalo}...");
            foreach (string s in pass.Passwords)
                Console.WriteLine(s);

            Console.WriteLine();

            Console.WriteLine($"Desencriptando las password con intervalo {intervalo}...");
            pass.decryptPassword(intervalo);
            foreach (string s in pass.Passwords)
                Console.WriteLine(s);

            Console.WriteLine();
            Console.WriteLine("Listo! Saliendo al presionar cualquier tecla");
            Console.ReadKey();
        }
    }
}