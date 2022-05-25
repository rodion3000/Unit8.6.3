using System;
using System.IO;

class FileCleaner
{
    public static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Не указан путь до каталога.");
            return;
        }
        Console.WriteLine($"Исходный размер папки: {GetSize(args[0])} байт");
        var removed = ClearDirectory(args[0]);
        Console.WriteLine($"Удалено файлов: {removed.count}");
        Console.WriteLine($"Освобождено: {removed.size} байт");
        Console.WriteLine($"Текущий размер папки: {GetSize(args[0])} байт");
        Console.WriteLine("Завершено.");
    }
    public static long GetSize(string path = (@"\Users\ggg"))
    {
        if (!Directory.Exists(path))
            return 0;
        long size = 0;
        try
        {
            var root = new DirectoryInfo(path);
            var Directory = root.GetDirectories();
            var files = root.GetFiles();
            foreach (var Files in files)
            {
                if (Files.Exists)
                {
                    size += Files.Length;
                }
            }
            foreach (var Director in Directory)
            {
                size += GetSize(Director.FullName);
            }
            return size;
        }

        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return 0;
        }


    }
    static (int count, long size) ClearDirectory(string path = (@"\Users\ggg"), bool removeRoot = false)
    {
        if(!Directory.Exists(path))
            return(0,0);
        (int count, long size) value = (0, 0);
        try
        {
            
            var root = new DirectoryInfo(path);
            var directory = root.GetDirectories();
            var files = root.GetFiles();
            foreach (var Files in files)
            {
                if (DateTime.Now - Files.LastWriteTime >= TimeSpan.FromMinutes(30))
                {
                    try
                    {
                        value.count++;
                        value.size+=Files.Length;
                        Files.Delete();

                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            foreach (var Directors in directory)
            {
                (int count,long size) = ClearDirectory(Directors.FullName,true);
                value.count += count;
                value.size += size;
            }
            if(removeRoot && root.GetDirectories().Length == 0 && root.GetFiles().Length == 0)
                root.Delete();

        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        return value;
       
    }


}

