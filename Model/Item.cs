using System;
using System.Collections.Generic;
using System.IO;

namespace Model
{
    public enum ItemType
    {
        Theme, Song, Icon, Chapter
    }

    public class Item : IEquatable<Item>
    {
        private readonly List<string> defaultIcons = new List<string>
        {
            {"/KeyBoardking;component/resources/images/itemIcons/defaultIcons/box.png"},
            {"/KeyBoardking;component/resources/images/itemIcons/defaultIcons/star.png"},
            {"/KeyBoardking;component/resources/images/itemIcons/defaultIcons/grass_block.png"},
            {"/KeyBoardking;component/resources/images/itemIcons/defaultIcons/present.png"},
            {"/KeyBoardking;component/resources/images/itemIcons/defaultIcons/shopping_bag.png"},
            {"/KeyBoardking;component/resources/images/itemIcons/defaultIcons/ruby.png"}
        };
        
        private const string itemIconsPath = "/resources/images/itemIcons/";
        private const string itemImagePath = "/resources/images/itemImages/";

        private static Random _random = new Random();

        public int Id { get; set; }
        public string Name { get; set; }

        private string _iconPath;
        public string IconPath
        {
            get => _iconPath;
            set
            {
                string itemname = ConvertToItemImageName(value);
                string path = $"{itemIconsPath}{itemname}";
                _iconPath = IsValidPath(path) ? path : defaultIcons[_random.Next(defaultIcons.Count)];
            }
        }

        private string _imagePath;
        public string ImagePath
        {
            get => _imagePath;
            set
            {
                string itemname = ConvertToItemImageName(value);
                string path;
                switch (Type)
                {
                    case ItemType.Theme:
                        path = $"{itemImagePath}{Type.ToString().ToLower()}/{itemname}";
                        break;
                    default:
                        path = IconPath;
                        break;
                }
                _imagePath = IsValidPath(path) ? path : IconPath;

            }
        }
        public int Price { get; set; }
        public ItemType Type { get; set; }
        public string Purchased { get; set; }

        public Item(int id, string name, int price, ItemType type, string purchased)
        {
            Id = id;
            Type = type;
            Name = IconPath = name;
            ImagePath = name;
            Price = price;   
            Purchased = purchased;
        }

        public static Item ParseItem(List<string> input)
        {
            ItemType Type = (ItemType)Enum.Parse(typeof(ItemType), input[3]);
            return new Item(int.Parse(input[0]), input[1], int.Parse(input[2]), Type, input[4]);
        }

        public static List<Item> ParseItems(List<List<string>> input)
        {
            List<Item> items = new List<Item>();
            input.ForEach(e => items.Add(ParseItem(e)));

            return items;
        }

        public bool Equals(Item other)
        {
            if (object.ReferenceEquals(this, other)) return true;

            if (object.ReferenceEquals(this, null) || object.ReferenceEquals(other, null)) return false;

            return this.Id == other.Id && this.Name == other.Name && this.Price == other.Price && this.Type == other.Type;
        }

        public static string ConvertToItemImageName(string itemName)
        {
            return $"{itemName.Replace(" ", "_").ToLower()}.png";
        }

        public static bool IsValidPath(string path)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            return File.Exists($"{currentDirectory}/{path}");
        }
    }
}
