using DictLib;
using System;
using Utils;

namespace PromIT
{
    class Program
    {
        private const string DB_NAME = "PromIT";
        
        static void Main(string[] args)
        {
            try
            {
                using (DBOperation db=new DBOperation(DB_NAME))
                {

                    if (args.Length == 0)
                    {
                        ConsoleUtils.InputWhile((s) =>
                        {
                            foreach (string w in db.GetWords(s))
                            {
                                Console.WriteLine("-{0}", w);
                            }
                        });

                    }
                    else
                        db.Command(args);                        

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadKey();
            }
            
        }
       
    }
}
