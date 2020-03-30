using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eto.Forms;
using RhinoFaceMe.UI.Models;

namespace RhinoFaceMe.UI.Custom
{
    class EnumCell<T> : CustomCell where T: struct, IConvertible
    {
        protected override Control OnCreateCell(CellEventArgs args)
        {
            var dropdown = new EnumDropDown<T>();
            // TODO: Finish this
            dropdown.BindDataContext<T>(c => (Enum.Parse(typeof(T), c.Text)), Binding.Property((ParticleConduitModel m) => m.Alignment));
            dropdown.Bind()
            return dropdown;
        }
    }
}
