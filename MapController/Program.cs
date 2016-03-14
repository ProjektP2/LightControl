using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Triangulering;
using SimEnvironment;
using Organising;


namespace LightControl
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // This should be here
            Organising.Program RunProgram = new Organising.Program();
            Running Loop = new Running();

            RunProgram.Start();
            Application.Exit();
        }
    }
}
