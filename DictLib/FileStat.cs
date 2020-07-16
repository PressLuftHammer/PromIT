using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;
using DictLib.Models;

namespace DictLib
{
    /// <summary>
    ///Работа с входным текстовым файлом
    /// </summary>
    class FileStat
    {
        //разделители слов
        private static char[] Delimiters = new char[] { ' ', '.', ',', ':', '!', '?', ';', '\n', '\t', '\r','-','"','«','»','\'','-' };
  
        /// <summary>
        /// Извлечение слов из теста и их повторов с учетом заданых ограничений
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private static IEnumerable<DictElement> FreqCalculate(string text)
        {           
            //разбиение на слова с учетом ограничений по длинне  
            //получение количества повторов слов с отбрасыванием тех у которых число вхождений менее  MIN_FREQ_WORD
              return   text.ToLower()
                           .Split(Delimiters, StringSplitOptions.RemoveEmptyEntries)
                           .Where(a => a.Length >= Settings.MIN_WORD_LEN && a.Length <= Settings.MAX_WORD_LEN)
                           .GroupBy(x => x)
                           .Select(g => new DictElement
                            {
                                Word = g.Key,
                                Count = g.Count()
                            })
                           .Where(d => d.Count >= Settings.MIN_FREQ_WORD);
        }

        /// <summary>
        /// Извлечение статистических данных из указаного файла 
        /// </summary>
        /// <param name="filename">Имя файла</param>
        /// <returns>Последовательность статистики по словам</returns>
        public static IEnumerable<DictElement>  GetFileStat(string filename)
        {
            if(File.Exists(filename))
                return FreqCalculate(File.ReadAllText(filename,Encoding.UTF8));

            throw new Exception(string.Format("Файл {0} не найден!", filename));
        }

    }
}
