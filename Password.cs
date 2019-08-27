using System;
using System.Collections.Generic;

namespace PasswordGenerator
{
    public class Password
    {
        private const string caracteres_validos = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789{}()[]#:;^,.?!|&_`~@$%/\\=+-*'\"";

        private const int inicio_mayus = 26;
        /// <summary></summary>
        private const int inicio_especiales = 52;

        //Esta combinación de tipos permite que el arreglo de passwords sea de solo lectura
        private List<string> passwords = new List<string>();

        public string[] Passwords => passwords.ToArray();

        private int mPassSize;
        private int mNumberOfMayus;
        private int mNumberOfSpecChars;

        public Password(int ammount, int passSize, int mayus, int specials)
        {
            mPassSize = passSize;
            mNumberOfMayus = mayus;
            mNumberOfSpecChars = specials;
            //Podemos generar las passwords una vez tenemos los tamaños
            generatePassword(ammount);
        }

        private void generatePassword(int numberOfPasswords)
        {
            var rand = new Random();
            while (numberOfPasswords-- > 0)
            {
                int may = mNumberOfMayus;
                int spec = mNumberOfSpecChars;
                int size = mPassSize;
                string result = "";
                for (int i = size; i > 0; i--)
                {
                    int war = rand.Next(i);
                    int offset;

                    /* No queremos que todos los tipos de caracter tengan la misma posibilidad de aparecer, especificamente por casos
                     * en los que haya una cantidad muy pequeña de caracteres no normales, esto haria que la posibilidad de que estén
                     * al frente se multiplicara por cada uno que no está
                     * En cambio, damos a cada tipo restante una posibilidad fraccion de los totales restantes*/

                    if (war >= may + spec)
                    {
                        offset = rand.Next(inicio_mayus);
                    }
                    else if (war >= may)
                    {
                        spec--;
                        offset = rand.Next(inicio_especiales, caracteres_validos.Length);
                    }
                    else
                    {
                        may--;
                        offset = rand.Next(inicio_mayus, inicio_especiales);
                    }

                    result += caracteres_validos[offset];
                }
                passwords.Add(result);
            }
        }

        public void encryptPassword(int interval)
        {
            //Si el intervalo es cero no requerimos cambiar nada, por ende retornamos
            if (interval == 0)
                return;

            for (int i = 0; i < passwords.Count; i++)
            {
                string result = "";
                foreach (char c in passwords[i])
                {
                    int index = caracteres_validos.IndexOf(c) + (interval % caracteres_validos.Length);

                    /*Para intervalos positivos podemos usar simplemente el modulo del indice calculado para obtener el indice final
                     *Cuando el indice calculado es negativo, queremos que se inicie en el ultimo elemento, asi que tomamos el 
                     *modulo ya negativo y le sumamos la longitud del arreglo */
                    index = index >= 0 ? index % caracteres_validos.Length : caracteres_validos.Length + (index % caracteres_validos.Length);
                    result += caracteres_validos[index];
                }

                passwords[i] = result;
            }
        }

        public void decryptPassword(int interval)
        {
            /*Ya que es una operación de reversión, podemos simplemente encriptar en espejo para obtener la cadena original
             * Esta es una de las razones por las cuales estas funciones deben de aceptar argumento negativo*/
            encryptPassword(-interval);
        }
    }
}
