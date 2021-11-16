using SharedItems.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedItems.Models
{
    public class HyperlinkDescriptionModel : ViewModelBase
    {
        private string _hyperlinkTitle;
        private string _hyperlinkDescription;
        private byte[] _hyperlinkImage;

        public int Id { get; set; }
        public int MessageModelId { get; set; }

        public string HyperlinkTitle
        {
            get => _hyperlinkTitle;
            set
            {
                _hyperlinkTitle = value;
                OnPropertyChanged();
            }
        }

        public string HyperlinkDescription
        {
            get => _hyperlinkDescription;
            set
            {
                _hyperlinkDescription = value;
                OnPropertyChanged();
            }
        }

        public byte[] HyperlinkImage
        {
            get => _hyperlinkImage;
            set
            {
                _hyperlinkImage = value;
                OnPropertyChanged();
            }
        }
    }
}
