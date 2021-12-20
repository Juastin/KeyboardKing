using Controller;
using System.ComponentModel;

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
