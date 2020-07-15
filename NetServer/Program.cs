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
                        string result_oper;
                        try
                        {
                            switch (cmds[0])
                            {
                                case "create":
                                    if (cmds.Length >= 2)
                                    {
                                        using (DBOperation db = new DBOperation(args[0]))
                                            db.Create(cmds[1]);

                                        result_oper = "Словарь создан";
                                    }
                                    else
                                        result_oper = "Незадано имя фалйа";
                                    break;
                                case "update":
                                    if (cmds.Length >= 2)
                                    {
                                        using (DBOperation db = new DBOperation(args[0]))
                                            db.Update(cmds[1]);

                                        result_oper = "Словарь обновлен";
                                    }
                                    else
                                        result_oper = "Незадано имя фалйа";
                                    break;
                                case "clear":
                                    using (DBOperation db = new DBOperation(args[0]))
                                        db.Clear();
                                    result_oper = "Словарь очищен";
                                    break;

                                default:
                                    result_oper = "Неизвестная комманда";
                                    break;
                            }
                        }
                        catch (Exception ex)
                        {
                            result_oper = ex.Message;
                        }
                        Console.WriteLine(result_oper);
                    });
                }
            }
        }
    }
}
