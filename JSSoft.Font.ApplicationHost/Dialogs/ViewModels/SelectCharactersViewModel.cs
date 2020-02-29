using Ntreev.ModernUI.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSSoft.Font.ApplicationHost.Dialogs.ViewModels
{
    public class SelectCharactersViewModel : ModalDialogBase
    {
        private uint[] characters;
        private string charactersText;

        public SelectCharactersViewModel(params uint[] characters)
        {
            this.characters = characters ?? throw new ArgumentNullException(nameof(characters));
            this.charactersText = CharacterCollection.ToString(characters);
            this.DisplayName = "Select Characters";
        }

        public void Select()
        {
            this.TryClose(true);
        }

        public uint[] Characters
        {
            get => this.characters;
            set
            {
                this.characters = value ?? throw new ArgumentNullException(nameof(value));
                this.charactersText = CharacterCollection.ToString(value);
                this.NotifyOfPropertyChange(nameof(Characters));
            }
        }

        public string CharactersText
        {
            get => this.charactersText;
            set
            {
                try
                {
                    CharacterCollection.Validate(value);
                    this.charactersText = value;
                    this.characters = new CharacterCollection(value).ToArray();
                    this.CanSelect = true;
                    this.NotifyOfPropertyChange(nameof(Characters));
                    this.NotifyOfPropertyChange(nameof(CharactersText));
                    this.NotifyOfPropertyChange(nameof(CanSelect));
                }
                catch
                {
                    this.CanSelect = false;
                    this.NotifyOfPropertyChange(nameof(CanSelect));
                    throw;
                }
            }
        }

        public bool CanSelect { get; private set; }
    }
}
