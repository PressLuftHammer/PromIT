using System;

namespace Utils
{
    /// <summary>
    /// Обработка пользовательского ввода с консоли
    /// </summary>
    public class ConsoleUtils
    {
        public delegate void UserInput(string text);


        /// <summary>
        /// Цикл обработки ввода пользователя
        /// </summary>
        /// <param name="userInput">обработчик введенных данных</param>
        public static void InputWhile(UserInput userInput)
        {
            string s = string.Empty;
            //Ожидает ввода пользователя пока пользователь не введет пустую строку
            while (!string.IsNullOrEmpty(s = Console.ReadLine()))
            {
                userInput(s);
            } 
        }
    }
}
