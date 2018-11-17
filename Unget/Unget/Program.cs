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
                                Directory.Delete(cachedDirs[j], true);
                                Console.WriteLine($"[DEL] {cachedDirs[j]}");
                            }
                            catch (Exception exception)
                            {
                                Console.WriteLine($"\n[ERR] {cachedDirs[j]}\n{exception}\n");
                            }
                        }
                    }
                });

                Console.WriteLine($"\nTotal {revomalDirCount} folder(s) are deleted from cache.");

                Console.ReadKey(false);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }
    }
}