using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using LightControl;
using Triangulering;

namespace LightControl
{
    public class LoadMapUsingPath
    {
        public LoadMapUsingPath()
        {
            
        }
        public Bitmap LoadFileIntoBitMap(string fileName)
        {
            Bitmap bitmap;
            try
            {
                bitmap = new Bitmap(fileName);
            }
            catch (ArgumentException exception)
            {
                throw exception;
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return bitmap;
        }
    }
}
