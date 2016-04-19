using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightControl
{
    public class Fps
    {
        int frameRendered = 0;
        public int fps = 0;
        long startTime = Environment.TickCount;

        public void FPS()
        {
            frameRendered++;
            if (Environment.TickCount >= startTime + 1000)
            {
                fps = frameRendered;
                frameRendered = 0;
                startTime = Environment.TickCount;
            }
        }

    }
}
