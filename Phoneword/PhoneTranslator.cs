using System.Text;

namespace Core
{
    public static class PhonewordTranslator
    {
        /// <summary>
        ///     Traduz um alfanumérico para um numérico
        /// </summary>
        /// <param name="raw">string</param>
        /// <returns>string</returns>
        public static string ToNumber(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw))
            {
                return "";
            }
            else
            {
                raw = raw.ToUpperInvariant();
            }
            var newNumber = new StringBuilder();
            foreach (var c in raw)
            {
                if (" -0123456789".Contains(c))
                {
                    newNumber.Append(c);
                }
                else
                {
                    var result = TranslateToNumber(c);
                    if (result != null)
                        newNumber.Append(result);
                }
            }
            return newNumber.ToString();
        }

        /// <summary>
        ///     Verifica se a string contém um caractere específico
        /// </summary>
        /// <param name="keyString">string</param>
        /// <param name="c">caracter</param>
        /// <returns>bool</returns>
        static bool Contains(this string keyString, char c)
        {
            return keyString.IndexOf(c) >= 0;
        }

        /// <summary>
        ///     Traduz as letras dos botões do teclado para o numérico correspondente
        /// </summary>
        /// <param name="c">caracter</param>
        /// <returns>numérico correspondente ao caracter ou nulo</returns>
        static int? TranslateToNumber(char c)
        {
            if ("ABC".Contains(c))
                return 2;
            else if ("DEF".Contains(c))
                return 3;
            else if ("GHI".Contains(c))
                return 4;
            else if ("JKL".Contains(c))
                return 5;
            else if ("MNO".Contains(c))
                return 6;
            else if ("PQRS".Contains(c))
                return 7;
            else if ("TUV".Contains(c))
                return 8;
            else if ("WXYZ".Contains(c))
                return 9;
            return null;
        }
    }
}