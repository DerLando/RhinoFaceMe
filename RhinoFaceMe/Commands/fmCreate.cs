using System;
using System.Drawing;
using System.IO;
using Eto.Forms;
using Rhino;
using Rhino.Commands;
using Rhino.Geometry;
using RhinoFaceMe.Core;
using RhinoFaceMe.UI.Models;
using Command = Rhino.Commands.Command;

namespace RhinoFaceMe.Commands
{
    public class fmCreate : Command
    {
        static fmCreate _instance;
        public fmCreate()
        {
            _instance = this;
        }

        ///<summary>The only instance of the fmCreate command.</summary>
        public static fmCreate Instance
        {
            get { return _instance; }
        }

        public override string EnglishName
        {
            get { return "fmCreate"; }
        }

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            // Allow the user to select a bitmap file
            var fd = new Rhino.UI.OpenFileDialog { Filter = "Image Files (*.bmp;*.png;*.jpg)|*.bmp;*.png;*.jpg" };
            if (!fd.ShowOpenDialog())
                return Rhino.Commands.Result.Cancel;

            // Verify the file that was selected
            System.Drawing.Image image;
            try
            {
                image = System.Drawing.Image.FromFile(fd.FileName);
            }
            catch (Exception)
            {
                return Rhino.Commands.Result.Failure;
            }

            var model = new ParticleConduitModel(Path.GetFileName(fd.FileName), 180, Color.FromArgb(100, 255, 0, 255),
                new Bitmap(image), true);
            RhinoFaceMePlugIn.Instance.FaceMeTable.Add(model);

            while (true)
            {
                var result = Rhino.Input.RhinoGet.GetPoint("Pick point", false, out var pt);
                if (result != Result.Success) break;

                model.AddParticle(pt);

                doc.Views.Redraw();
            }

            return Result.Success;
        }
    }
}