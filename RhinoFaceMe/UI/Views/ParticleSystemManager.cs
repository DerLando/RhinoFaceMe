using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Eto.Drawing;
using Eto.Forms;
using Rhino.DocObjects;
using Rhino.UI;
using RhinoFaceMe.UI.Models;

namespace RhinoFaceMe.UI.Views
{
    [Guid("03A726C8-4D5D-4D9E-AD91-BE259B2A5BAC")]
    public class ParticleSystemManager : Panel, IPanel
    {
        // fields
        private readonly uint _document_sn;
        private ParticleConduitListViewModel _model_list;

        // controls
        private GridView _gV_ParticleSystems = new GridView();

        // Auto-initialized properties
        public static Guid PanelId => typeof(ParticleSystemManager).GUID;

        public ParticleSystemManager(uint documentSerialNumber)
        {
            _document_sn = documentSerialNumber;

            // set up grid vies
            _gV_ParticleSystems.ShowHeader = true;
            _model_list = RhinoFaceMePlugIn.Instance.FaceMeTable;

            // set up bindings for grid view
            _gV_ParticleSystems.DataContext = _model_list;
            _gV_ParticleSystems.DataStore = _model_list.Conduits;
            _gV_ParticleSystems.SelectedItemBinding.BindDataContext((ParticleConduitListViewModel avm) =>
                avm.SelectedIndex);

            // set up events for grid view
            _gV_ParticleSystems.CellClick += On_CellClick;

            #region Grid Columns

            // name
            // name
            _gV_ParticleSystems.Columns.Add(new GridColumn
            {
                HeaderText = "Name",
                DataCell = new TextBoxCell("Name"),
                Editable = false,
            });

            // active
            _gV_ParticleSystems.Columns.Add(new GridColumn
            {
                HeaderText = "Active",
                DataCell = new CheckBoxCell("IsActive"),
                Editable = true,
            });

            // size
            _gV_ParticleSystems.Columns.Add(new GridColumn
            {
                HeaderText = "Size",
                DataCell = new TextBoxCell("Size"),
                Editable = true,
            });

            // color
            var bitmapSize = 12;
            _gV_ParticleSystems.Columns.Add(new GridColumn
            {
                DataCell = new ImageViewCell { Binding = Binding.Property<ParticleConduitModel, Image>(l => new Bitmap(bitmapSize, bitmapSize, PixelFormat.Format24bppRgb, from index in Enumerable.Repeat(0, bitmapSize * bitmapSize) select l.Color.ToEto())) },
                HeaderText = "Color",
                Editable = true
            });

            #endregion

            // write layout
            var layout = new DynamicLayout();
            layout.Padding = 10;
            layout.Spacing = new Size(5, 5);

            layout.Add(_gV_ParticleSystems);
            layout.Add(null);

            Content = layout;
        }

        // bindings
        public void SetListModel(ParticleConduitListViewModel listModel)
        {
            _model_list = listModel;

            // set up bindings for grid view
            _gV_ParticleSystems.DataContext = _model_list;
            _gV_ParticleSystems.DataStore = _model_list.Conduits;
            _gV_ParticleSystems.SelectedItemBinding.BindDataContext((ParticleConduitListViewModel avm) =>
                avm.SelectedIndex);

            // redraw
            _gV_ParticleSystems.Invalidate();
        }

        private void On_CellClick(object sender, GridCellMouseEventArgs e)
        {
            // test if color
            if (e.GridColumn.HeaderText == "Color")
            {
                var model = e.Item as ParticleConduitModel;
                var color = model.Color;
                Dialogs.ShowColorDialog(ref color);
                model.Color = color;
            }
        }

        #region IPanel methods

            public void PanelShown(uint documentSerialNumber, ShowPanelReason reason)
        {
            // Called when the panel tab is made visible, in Mac Rhino this will happen
            // for a document panel when a new document becomes active, the previous
            // documents panel will get hidden and the new current panel will get shown.
            Rhino.RhinoApp.WriteLine($"Panel shown for document {documentSerialNumber}, this serial number {_document_sn} should be the same");
        }

        public void PanelHidden(uint documentSerialNumber, ShowPanelReason reason)
        {
            // Called when the panel tab is hidden, in Mac Rhino this will happen
            // for a document panel when a new document becomes active, the previous
            // documents panel will get hidden and the new current panel will get shown.
            Rhino.RhinoApp.WriteLine($"Panel hidden for document {documentSerialNumber}, this serial number {_document_sn} should be the same");
        }

        public void PanelClosing(uint documentSerialNumber, bool onCloseDocument)
        {
            // Called when the document or panel container is closed/destroyed
            Rhino.RhinoApp.WriteLine($"Panel closing for document {documentSerialNumber}, this serial number {_document_sn} should be the same");
        }

        #endregion IPanel methods
    }
}
