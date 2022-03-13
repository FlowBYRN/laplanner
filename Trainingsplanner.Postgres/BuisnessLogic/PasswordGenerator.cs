using System;
using System.Text;

namespace Trainingsplanner.Postgres.BuisnessLogic
{
    public class PasswordGenerator
    {
        public static string GetRandomPassword(int size = 14)
        {
            if (size < 8)
            {
                return null;
            }
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            for (int i = 0; i < size; i++)
            {
                int zeichen = Convert.ToInt32(Math.Floor(3 * random.NextDouble()));
                if (i == size - 2) // Vorletztes Zeichen Sonderzeichen
                    zeichen = 1;

                if (i == 2) // Großbuchstabe an pos 2
                    zeichen = 0;

                if (i == 3) // Zahl an pos 2
                    zeichen = 3;
                switch (zeichen)
                {
                    case 0:
                        builder.Append(Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)))); //Großbuchstabe
                        break;
                    case 1:
                        char[] sonderzeichen = new[] { '!', '&', '$', '%', '/', '(', ')', '[', ']', '{', '}', '?', '\\', '=', '<', '>', '+', '-', ';', ':' };
                        builder.Append(sonderzeichen[random.Next(0, 20)]); //Sonderzeichen
                        i++;
                        goto case 2;
                    case 2:
                        builder.Append(Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 97)))); //Kleinbuchstabe
                        break;
                    case 3:
                        builder.Append(random.Next(0, 9).ToString());
                        break;
                    default:
                        builder.Append(Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 97)))); //Kleinbuchstabe
                        break;
                }
            }

            return builder.ToString();
        }
    }
}