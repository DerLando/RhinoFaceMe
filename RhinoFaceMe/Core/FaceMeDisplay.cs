using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino;
using Rhino.Display;
using Rhino.Geometry;

namespace RhinoFaceMe.Core
{
    class FaceMeDisplay : DisplayConduit
    {
        public ParticleSystem System = new ParticleSystem();
        public DisplayBitmap Dbm;

        public FaceMeDisplay(Bitmap bitmap)
        {
            Dbm = new DisplayBitmap(bitmap);
            System.DisplaySizesInWorldUnits = true;
        }

        protected override void PreDrawObjects(DrawEventArgs e)
        {
            e.Display.DrawParticles(System, Dbm);
        }

        public void AddParticle(Particle p)
        {
            System.Add(p);
        }
    }
}
