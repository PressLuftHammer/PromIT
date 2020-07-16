using System;
using DictLib;
using Utils;

namespace NetServer
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length >= 2)
            {
                using (Server srv = new Server(args[0],int.Parse(args[1])))
                {
                    ConsoleUtils.InputWhile((s) =>
                    {
                        string[] cmds = s.Trim()
                                         .Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
                        try
                        {
                            using (DBOperation db = new DBOperation(args[0]))
                            {
                                db.Command(cmds);
                                Console.WriteLine(string.Format("Комманда '{0}' выполнена",cmds[0]));     
                            }                           
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    });
                }
            }
        }
    }
}
