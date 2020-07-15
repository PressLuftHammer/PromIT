using System;
using Utils;

namespace NetClient
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length >= 2)
            {
                try
                {
                    DictClient dc = new DictClient(args[0], int.Parse(args[1]));
                    try
                    {    
                        ConsoleUtils.InputWhile((s) =>{
                            foreach (string w in dc.SendQuest(s))
                            {
                                Console.WriteLine("-{0}", w);
                            }
                        });

                    }
                    finally
                    {
                        dc.Close();
                    }

                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else
            {
                Console.WriteLine("Не задан сетевой адрес и порт сервера!");
            }
        }
    }
}
