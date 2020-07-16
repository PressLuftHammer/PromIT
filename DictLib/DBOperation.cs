using DictLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DictLib
{
    /// <summary>
    /// Класс для работы со словарем
    /// </summary>
    public class DBOperation:IDisposable
    {
        private PromITContext _db;

        /// <summary>
        /// Создание словаря
        /// </summary>
        /// <param name="db_name">имя базы</param>
        public DBOperation(string  db_name)
        {
            _db = new PromITContext(string.Format(Settings.CONNECT_STRING, db_name));          
        }  

        //очистка словаря
        public void Clear()
        {
           _db.WordsStat.RemoveRange(_db.WordsStat);
           _db.SaveChanges();
        }

        /// <summary>
        /// Создание словаря
        /// </summary>
        /// <param name="words">последовательность слов и количество повторений их в тексте</param>
        private void _Create(IEnumerable<DictElement> words)
        {
           _db.WordsStat.AddRange(words);
           _db.SaveChanges();
        }

        /// <summary>
        /// Создание словаря если словарь не пуст вызывает исключение
        /// </summary>
        /// <param name="filename">имя файла</param>
        public void Create(string filename)
        {
            if (_db.WordsStat.Any())
                throw new Exception("Словарь содержит данные!\nНеобходимо предварительно его очистить.");

            _Create(FileStat.GetFileStat(filename));
        }

        /// <summary>
        /// обновление данных словаря
        /// </summary>
        /// <param name="words">последовательность слов и количество повторений их в тексте</param>
        private void _Update(IEnumerable<DictElement> words)
        {
            foreach(var w in words)
            {
               var word = _db.WordsStat.FirstOrDefault(a => a.Word == w.Word);

               if (word==null)
               {
                  _db.WordsStat.Add(w);
               }
               else
               {
                    word.Count += w.Count;                       
               }
               
            }

            _db.SaveChanges();            
        }

        public void Update(string filename)
        {
            _Update(FileStat.GetFileStat(filename));
        }


        /// <summary>
        /// Выполняется комманда с задаными параметрами
        /// </summary>
        /// <param name="cmds">параметры</param>
        public void Command(string[] cmds)
        {
            if (cmds.Length > 0)
            {
                switch (cmds[0])
                {
                    case "create":
                        if (cmds.Length >= 2)
                        {
                            Create(cmds[1]);
                        }
                        else
                            throw new Exception("Не задано имя файла!");
                        break;

                    case "update":
                        if (cmds.Length >= 2)
                        {
                            Update(cmds[1]);
                        }
                        else
                            throw new Exception("Не задано имя файла!");

                        break;


                    case "clear":
                        Clear();
                        break;

                    default:
                        throw new Exception(string.Format("Неизвестная комманда '{0}'", cmds[0]));
                }

            }

        

        }

        /// <summary>
        /// Получение слов атодополнения
        /// </summary>
        /// <param name="text">начало слова для автодополнения</param>
        /// <returns>Слова отсортированые по встречаемости и алфавиту</returns>
        public string[] GetWords(string text)
        {          
            text = text.ToLower();

            return _db.WordsStat
                      .Where(k => k.Word.StartsWith(text))
                      .OrderByDescending(b => b.Count)
                      .ThenBy(c => c.Word)
                      .Take(5)
                      .Select(a => a.Word)
                      .ToArray();
        }

        #region IDisposable Support
        private bool disposedValue = false; // Для определения избыточных вызовов

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: освободить управляемое состояние (управляемые объекты).
                    _db.Dispose();
                }

                // TODO: освободить неуправляемые ресурсы (неуправляемые объекты) и переопределить ниже метод завершения.
                // TODO: задать большим полям значение NULL.

                disposedValue = true;
            }
        }

        // TODO: переопределить метод завершения, только если Dispose(bool disposing) выше включает код для освобождения неуправляемых ресурсов.
        // ~DBOperation() {
        //   // Не изменяйте этот код. Разместите код очистки выше, в методе Dispose(bool disposing).
        //   Dispose(false);
        // }

        // Этот код добавлен для правильной реализации шаблона высвобождаемого класса.
        public void Dispose()
        {
            // Не изменяйте этот код. Разместите код очистки выше, в методе Dispose(bool disposing).
            Dispose(true);
            // TODO: раскомментировать следующую строку, если метод завершения переопределен выше.
            // GC.SuppressFinalize(this);
        }
        #endregion

    }
}
