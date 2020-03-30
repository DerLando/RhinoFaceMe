using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoFaceMe.UI.Models
{
    public class ParticleConduitListViewModel
    {
        private int _selectedParticleConduit;

        public ObservableCollection<ParticleConduitModel> Conduits { get; set; } =
            new ObservableCollection<ParticleConduitModel>();

        public int SelectedIndex
        {
            get => _selectedParticleConduit;
            set { _selectedParticleConduit = value; }
        }

        public void Add(ParticleConduitModel model)
        {
            Conduits.Add(model);
        }
    }
}
