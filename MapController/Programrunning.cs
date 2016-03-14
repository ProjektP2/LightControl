using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using Triangulering;
using SimEnvironment;
using LightControl;

namespace Organising
{
    class Program
    {
        GEngine gEngine;
        public void Start()
        {
           
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            Window window = new Window();
            gEngine = new GEngine(window);
            Application.Run(window);
            gEngine.LoadLevel();
            gEngine.Init();


            //Creating two instances of routers.
            Circle Router1 = new Circle();
            Circle Router2 = new Circle();
            //The routers contain a coordinate for their position in a room, and the strength of the signal they're receiving.
            //These values are set here. lololololololol
            Router1.SetRouterPosition(3, 4);
            Router1.SetSignalStrength(43);
            Router2.SetRouterPosition(21, 34);
            Router2.SetSignalStrength(55);

            //Preparing an array of two set of Coordinates. The result of triangulation gives os two possible positions, one of which
            //is the correct one. We can check the results later.
            Coords[] PossiblePositions = new Coords[2];





            // Jeg aner ikke hvad der er hvad herefter -.-

            /*

            //Applying the static method from Triangulate. We give the two routers as input, and receive two set of coordinates.
            ///// PossiblePositions = Triangulate.TriangulateSignalSource(Router1, Router2);

            //Print results
            Console.WriteLine($"The possible positions of signal source are:");
            // Console.WriteLine($"P1 = ({Math.Round(PossiblePositions[0].x, 3)},{Math.Round(PossiblePositions[0].y, 3)})");
            //ole.WriteLine("CALCULATION FINISHED.\n\n");
            //To calculate the velocity, we assume that the first set of coordinates is the correct one.

            //We create an instance of a room occupant - this is the signal source. We set his starting position to be the 
            //first possible position.
            Occupant SignalSource = new Occupant();
            SignalSource.SetStartingPosition(PossiblePositions[0]);

            //We now update the routers received signal strength as to simulate that the signal source has moved.
            Router1.SetSignalStrength(823);
            Router2.SetSignalStrength(1400);

            //Once again, we calculate the two possible positions of the signal. 
            Coords[] NewPossiblePositions = new Coords[2];
            //// NewPossiblePositions = Triangulate.TriangulateSignalSource(Router1, Router2);

            //We assume that the first possible solution is the correct one. We update the occupants second read position to
            //these coordinates.
            SignalSource.UpdatePositions(NewPossiblePositions[0]);

            //We now calculate the velocity of the occupant. Seeing that neither the distance units or time between readings
            //has been defined yet, the result is in the format: (distance) pr. (time)
            SignalSource.CalculateVelocity();
            Console.WriteLine($"The new possible positions of signal source are:");
            Console.WriteLine($"P1 = ({Math.Round(NewPossiblePositions[0].x, 3)},{Math.Round(NewPossiblePositions[0].y, 3)})");
            Console.WriteLine($"P2 = ({Math.Round(NewPossiblePositions[1].x, 3)},{Math.Round(NewPossiblePositions[1].y, 3)})");
            Console.WriteLine($"Velocity of the occupant is: {Math.Round(SignalSource.Velocity, 3)} m/s");
            Console.WriteLine("CALCULATION FINISHED.\n\n");


            //Giver fejl hvis den er slået til!
            // Console.ReadKey();
            */
            Loop();
        }
        public static bool ProgramRunning = true;
        private void Loop()
        {
            do
            {
                gEngine.Visualizing();

            } while (ProgramRunning);
        }
    }
}
