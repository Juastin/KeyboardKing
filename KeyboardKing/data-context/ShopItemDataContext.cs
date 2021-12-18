using Controller;
using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyboardKing.data_context
{
    public class ShopItemDataContext : INotifyPropertyChanged
    {
        public Item CurrentItem { get => ShopController.CurrentItem; }

        public event PropertyChangedEventHandler PropertyChanged;

        public ShopItemDataContext()
        {
            ShopController.CurrentItemChanged += ItemChanged;
        }

        private void ItemChanged()
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(""));
            }
        }
    }
}
