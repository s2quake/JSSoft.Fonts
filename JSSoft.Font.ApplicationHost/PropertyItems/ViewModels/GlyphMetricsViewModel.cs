﻿using Ntreev.Library;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSSoft.Font.ApplicationHost.PropertyItems.ViewModels
{
    [Export(typeof(IPropertyItem))]
    [Dependency(typeof(FontInfoViewModel))]
    class GlyphMetricsViewModel : PropertyItemBase
    {
        private ICharacter character;

        public GlyphMetricsViewModel()
        {
            this.DisplayName = "Character Info";
        }

        public override bool CanSupport(object obj)
        {
            return obj is ICharacter;
        }

        public override void SelectObject(object obj)
        {
            if (obj is ICharacter character && character.IsEnabled == true)
            {
                this.GlyphMetrics = character.GlyphMetrics;
                this.character = character;
            }
            else
            {
                this.GlyphMetrics = GlyphMetrics.Empty;
                this.character = null;
            }
            this.NotifyOfPropertyChange(nameof(SelectedObject));
            this.NotifyOfPropertyChange(nameof(GlyphMetrics));
        }

        public GlyphMetrics GlyphMetrics { get; private set; }

        public override bool IsVisible => true;

        public override object SelectedObject => this.character;
    }
}
