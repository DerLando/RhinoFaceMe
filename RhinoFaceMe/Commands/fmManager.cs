using System;
using Rhino;
using Rhino.Commands;
using Rhino.UI;

namespace RhinoFaceMe.Commands
{
    public class fmManager : Command
    {
        static fmManager _instance;
        public fmManager()
        {
            // register clipping plane manager panel
            Panels.RegisterPanel(PlugIn, typeof(UI.Views.ParticleSystemManager), "FaceMe", null);
            _instance = this;
        }

        ///<summary>The only instance of the fmManager command.</summary>
        public static fmManager Instance
        {
            get { return _instance; }
        }

        public override string EnglishName
        {
            get { return "fmManager"; }
        }

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            var panelId = UI.Views.ParticleSystemManager.PanelId;
            var visible = Panels.IsPanelVisible(panelId);

            var prompt = (visible)
                ? "FaceMe panel is visible"
                : "FaceMe panel is hidden";

            RhinoApp.WriteLine(prompt);

            // toggle visible
            if (!visible)
            {
                Panels.OpenPanel(panelId);
            }
            else Panels.ClosePanel(panelId);
            return Result.Success;
        }
    
    }
}