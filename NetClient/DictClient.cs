using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;

namespace NetClient
{
    class DictClient
    {
        private TcpClient client;
        private StreamReader reader;
        private StreamWriter writer;
        private NetworkStream ns;

        public DictClient(string host,int port)
        {
            client = new TcpClient(host, port);
            ns = client.GetStream();
            reader = new StreamReader(ns);
            writer = new StreamWriter(ns);
        }


        /// <summary>
        /// Отправка запроса и получение результата
        /// </summary>
        /// <param name="text">часть слова требующая дополнения</param>
        /// <returns>список слов для автодополнения</returns>
        public List<string> SendQuest(string text)
        {
            //отсылка запроса
            writer.WriteLine(string.Format("get {0}", text));
            writer.Flush();
            List<string> list = new List<string>();
            string s;
            while(!string.IsNullOrEmpty(s=reader.ReadLine()))
            {
                list.Add(s);
            } 

            return list;
        }


        public void Close()
        {
            writer.Close();
            reader.Close();
            ns.Close();
            client.Close();
        }
    }
}
