using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Library
{
    public class SentenceExtractor
    {
        private string inputText;
        private int maxSymbols = 226;

        public SentenceExtractor(string inputText)
        {
            this.inputText = inputText;
        }

        public string GetFirstTwoSentences()
        {
            if (string.IsNullOrEmpty(inputText))
            {
                return string.Empty;
            }

            // Регулярное выражение для поиска предложений.
            string sentencePattern = @"(?<=[.!?])\s+";
            string[] sentences = Regex.Split(inputText, sentencePattern);

            // Проверка, достаточно ли предложений в тексте.
            string result;
            if (sentences.Length >= 2)
            {
                result = sentences[0] + " " + sentences[1];
            }
            else
            {
                result = inputText; // Вернуть весь текст, если предложений меньше двух.
            }

            // Если результат больше 400 символов, обрезаем его.
            if (result.Length > maxSymbols)
            {
                result = TrimToNearestWord(result, maxSymbols) + "...";
            }

            return result;
        }

        private string TrimToNearestWord(string text, int maxLength)
        {
            if (text.Length <= maxLength)
            {
                return text;
            }

            // Находим последнее вхождение пробела, запятой или дефиса до предела длины.
            int lastSpaceIndex = text.LastIndexOfAny(new char[] { ' ', ',', '-' }, maxLength);
            if (lastSpaceIndex == -1)
            {
                return text.Substring(0, maxLength); // Если нет пробелов, запятых или дефисов, обрезаем на maxLength.
            }

            // Удаляем символы пунктуации в конце строки, если они есть.
            string trimmedText = text.Substring(0, lastSpaceIndex).TrimEnd(',', '.', '!', '?', ';', ':');

            return trimmedText;
        }
    }
}
