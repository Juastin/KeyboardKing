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
        public Item CurrentItem { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public ShopItemDataContext()
        {
            ShopController.CurrentItemChanged += OnItemChanged;
        }

        private void OnItemChanged()
        {
            CurrentItem = ShopController.CurrentItem;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
        }
    }
}
