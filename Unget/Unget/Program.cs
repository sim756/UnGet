using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace Unget
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("unget - NuGet Cache Cleaner\n");

                string[] dirs
                    = Directory.GetDirectories(NuGetPaths.GetNugetCachePathByPlatform(),
                        "*.*", SearchOption.TopDirectoryOnly);

                int revomalDirCount = 0;
                long totalDirSize = 0;

                Parallel.For(0, dirs.Length, i =>
                {
                    List<string> cachedDirs
                        = Directory.GetDirectories(dirs[i], "*.*", SearchOption.TopDirectoryOnly).ToList();

                    if (cachedDirs.Count > 1)
                    {
                        Interlocked.Add(ref revomalDirCount, cachedDirs.Count - 1);
                        cachedDirs.Sort();

                        for (int j = 0; j < cachedDirs.Count - 1; j++)
                        {
                            try
                            {
                                long dirSize = Directory.GetFiles(cachedDirs[j], "*", SearchOption.AllDirectories).Sum(t => (new FileInfo(t).Length));
                                Interlocked.Add(ref totalDirSize, dirSize);

                                Directory.Delete(cachedDirs[j], true);
                                Console.WriteLine($"[DEL] {$"{((double)dirSize / 1024.0 / 1024.0):F2}",10} \t{cachedDirs[j]}");
                            }
                            catch (Exception exception)
                            {
                                Console.WriteLine($"\n[ERR] {cachedDirs[j]}\n{exception}\n");
                            }
                        }
                    }
                });

                Console.WriteLine($"\nTotal {revomalDirCount} folder(s) of {((double)totalDirSize / 1024.0 / 1024.0):F2} MB are deleted from cache.");

                Console.ReadKey(false);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }
    }
}