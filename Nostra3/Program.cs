using System;
using Nostra3.Module;
using Nostra3.Data;
using System.Linq;
namespace Nostra3
{
    class Program
    {
        static void Main(string[] args)
        {
            SystemMngr Manager = new SystemMngr();
            Manager.Run();
        }
    }
}
