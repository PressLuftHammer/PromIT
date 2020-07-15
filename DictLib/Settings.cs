using System;
using System.Collections.Generic;
using System.Text;

namespace DictLib
{
    /// <summary>
    /// Используемые глобальные константы
    /// </summary>
    class Settings
    {
        //минимальная длинна слова добавляемого в словарь
        public const int MIN_WORD_LEN = 3;
        //максимальная длинная слова добавляемая в словарь
        public const int MAX_WORD_LEN = 15;
        //минимальное количество повторов слова в файле для включения в словарь
        public const int MIN_FREQ_WORD = 3;
        //строка подключения
        public const string CONNECT_STRING = "Server=(localdb)\\mssqllocaldb;Database={0};Trusted_Connection=True;";
    }
}
