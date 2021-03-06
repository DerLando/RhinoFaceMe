﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino;
using Rhino.Display;
using Rhino.Geometry;
using RhinoFaceMe.Core;

namespace RhinoFaceMe.UI.Models
{
    public enum ParticleAlignment
    {
        Top,
        Mid,
        Bottom
    }

    public class ParticleConduitModel
    {
        // changeable private fields
        private float _size;
        private Color _color;
        private Bitmap _bm;
        private bool _isActive;
        private ParticleAlignment _alignment;

        // more complicated fields
        private FaceMeDisplay _fmd;

        public string Name { get; private set; }

        #region Getters/setters

        public float Size
        {
            get => _size;
            set
            {
                var locations = (from p in _fmd.System select GetOriginalPoint(p.Location)).ToList();
                _size = value;
                RebuildAfterChange(locations);
            }
        }

        public Color Color
        {
            get => _color;
            set
            {
                _color = value;
                UpdateColor();
            }
        }

        public Bitmap Bitmap
        {
            get => _bm;
            set
            {
                _bm = value;
                UpdateImage();
            }
        }

        public bool IsActive
        {
            get => _isActive;
            set
            {
                _isActive = value;
                UpdateActive();
            }
        }

        public ParticleAlignment Alignment
        {
            get => _alignment;
            set
            {
                var locations = (from p in _fmd.System select GetOriginalPoint(p.Location)).ToList();
                _alignment = value;
                RebuildAfterChange(locations);
            }
        }

        private void UpdateAlignment()
        {
            throw new NotImplementedException();
        }

        private void UpdateActive()
        {
            _fmd.Enabled = _isActive;
            RedrawAfterChange();
        }

        private void UpdateImage()
        {
            _fmd.Dbm = new DisplayBitmap(_bm);
            RebuildAfterChange();
        }

        private void UpdateColor()
        {
            RebuildAfterChange();
        }

        private void UpdateSize()
        {
            RebuildAfterChange();
        }

        #endregion

        public ParticleConduitModel(string name, float size, Color color, Bitmap bitmap, bool isActive)
        {
            Name = name;

            _size = size;
            _color = color;
            _bm = bitmap;
            _isActive = isActive;

            _fmd = new FaceMeDisplay(_bm);
            _fmd.Enabled = _isActive;
            _alignment = ParticleAlignment.Bottom;
        }

        private void RedrawAfterChange()
        {
            RhinoDoc.ActiveDoc.Views.Redraw();
        }

        private void RebuildAfterChange()
        {
            var locations = (from p in _fmd.System select GetOriginalPoint(p.Location)).ToList();
            _fmd.ResetSystem();
            foreach (var location in locations)
            {
                AddParticle(location);
            }

            RedrawAfterChange();
        }

        private void RebuildAfterChange(IEnumerable<Point3d> locations)
        {
            _fmd.ResetSystem();
            foreach (var location in locations)
            {
                AddParticle(location);
            }

            RedrawAfterChange();
        }

        private Point3d GetAlignedPoint(Point3d pt)
        {
            switch (_alignment)
            {
                case ParticleAlignment.Top:
                    pt.Z += _size / 2.0;
                    break;
                case ParticleAlignment.Mid:
                    break;
                case ParticleAlignment.Bottom:
                    pt.Z -= _size / 2.0;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return pt;
        }

        private Point3d GetOriginalPoint(Point3d pt)
        {
            switch (_alignment)
            {
                case ParticleAlignment.Top:
                    pt.Z -= _size / 2.0;
                    break;
                case ParticleAlignment.Mid:
                    break;
                case ParticleAlignment.Bottom:
                    pt.Z += _size / 2.0;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return pt;
        }

        public void AddParticle(Point3d pt)
        {
            var p = new Particle
            {
                Location = GetAlignedPoint(pt),
                Size = _size,
                Color = _color
            };
            _fmd.System.Add(p);
        }

        public ParticleAlignment GetAlignment() => _alignment;

        public void SetAlignment(ParticleAlignment pa)
        {
            _alignment = pa;
        }
    }
}
