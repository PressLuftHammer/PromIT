using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using DictLib;

namespace NetServer
{
    class Server:IDisposable
    {
        private TcpListener listener;
        private List<Client> clients = new List<Client>();
      
        public Server(string DB_Path,int port)
        {
            try
            {
                listener = new TcpListener(port);
               
                listener.Start(5);
                Console.WriteLine("Сервер запущен и ожидает подключения...");

                Thread lt = new Thread(()=> {
                    try
                    {
                        while (true)
                        {

                            Client client = new Client(listener.AcceptTcpClient(),DB_Path);

                            clients.Add(client);

                            Thread thr = new Thread(new ThreadStart(client.Process));
                            thr.Start();
                        }
                        
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                    listener.Server.Close();

                    foreach (var client in clients)
                        client.Close();                    

                });

                lt.Start();
                

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }            

        }


        private class Client
        {
            private const string COMMAND_GET = "get";
            private TcpClient _client;
            private readonly string _db_path;
            public Client(TcpClient client, string db_path)
            {
                _client = client;
                _db_path = db_path;
            }

            public void Close()
            {
                _client.Close();
            }

            public void Process()
            {
                using (DBOperation db = new DBOperation(_db_path))
                {
#if DEBUG
                    Console.WriteLine("Подключился клиент ");
#endif
                    NetworkStream ns = _client.GetStream();
                    StreamReader reader = new StreamReader(ns);
                    StreamWriter writer = new StreamWriter(ns);
                    try
                    {
                        string msg = string.Empty;
                        while (!string.IsNullOrEmpty(msg = reader.ReadLine()))
                        {
#if DEBUG
                            Console.WriteLine("Получено: " + msg);
#endif
                            string[] ss = msg.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);

                            if(ss.Length>=2 && ss[0]==COMMAND_GET)
                            { 

                                foreach(var w in db.GetWords(ss[1]))
                                {
                                    writer.WriteLine(w);
                                }
                                writer.WriteLine();
                                writer.Flush();
                            }
#if DEBUG
                            else
                                Console.WriteLine("Неизвестная комманда :" + msg);
#endif                           
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    finally
                    {
                        writer.Close();
                        reader.Close();
                        ns.Close();
                        _client.Close();
                    }
                }

            }
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
                    listener.Stop();                    
                }

                // TODO: освободить неуправляемые ресурсы (неуправляемые объекты) и переопределить ниже метод завершения.
                // TODO: задать большим полям значение NULL.

                disposedValue = true;
            }
        }

        // TODO: переопределить метод завершения, только если Dispose(bool disposing) выше включает код для освобождения неуправляемых ресурсов.
        // ~Server() {
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
