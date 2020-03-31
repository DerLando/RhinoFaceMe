using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eto.Forms;
using RhinoFaceMe.UI.Models;

namespace RhinoFaceMe.UI.Custom
{
    class AlignmentCell: CustomCell
    {
        protected override Control OnCreateCell(CellEventArgs args)
        {
            var dropdown = new EnumDropDown<ParticleAlignment>();
            // TODO: Finish this
            // TODO: This should work, but it is in fact not working :(
            dropdown.SelectedValueBinding.BindDataContext(
                (ParticleConduitModel m) => m.Alignment,
                                (m, val) => m.Alignment = val);

            //dropdown.SelectedValueBinding.BindDataContext(
            //    (ParticleConduitModel m) => m.GetAlignment(),
            //    (m, val) => m.SetAlignment(val));

            return dropdown;
        }
    }
}
