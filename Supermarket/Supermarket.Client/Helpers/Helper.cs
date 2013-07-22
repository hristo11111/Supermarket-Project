using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Supermarket.Client.Helpers
{
    public class Helper
    {
        public static bool DirectoryExist(string directoryPatch)
        {
            DirectoryInfo objDirectory = new DirectoryInfo(directoryPatch);
            if (objDirectory.Exists)
            {
                return true;
            }
            else
            {
                try
                {
                    Directory.CreateDirectory(directoryPatch);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }
    }
}
