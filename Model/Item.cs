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
        private const string defaultIcon = "/KeyBoardking;component/resources/images/itemIcons/icon.png";
        private const string defaultImage = "/KeyBoardking;component/resources/images/itemIcons/icon.png";
        private const string itemIconsPath = "/resources/images/itemIcons/";
        private const string itemImagePath = "/resources/images/itemImage/";

        public int Id { get; set; }
        public string Name { get; set; }

        private string _iconPath;
        public string IconPath
        {
            get => _iconPath;
            set
            {
                string itemname = $"{value.Replace(" ", "_").ToLower()}.png";
                string path = $"{itemIconsPath}{itemname}";
                _iconPath = PathController.IsValidPath(path) ? path : defaultIcon;
            }
        }

        private string _imagePath;
        public string ImagePath
        {
            get => _imagePath;
            set
            {
                string itemname = $"{value.Replace(" ", "_").ToLower()}.png";
                string path;
                switch (Type)
                {
                    case ItemType.Theme:
                        path = $"{itemImagePath}{Type.ToString().ToLower()}/{itemname}";
                        break;
                    default:
                        path = $"{itemIconsPath}{itemname}";
                        break;
                }
                _imagePath = PathController.IsValidPath(path) ? path : defaultImage;

            }
        }
        public int Price { get; set; }
        public ItemType Type { get; set; }
        public string Purchased { get; set; }

        public Item(int id, string name, int price, ItemType type, string purchased)
        {
            Id = id;
            Type = type;
            Name = IconPath = ImagePath = name;
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
    }

    public static class PathController
    {
        public static bool IsValidPath(string path)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            return File.Exists($"{currentDirectory}/{path}");
        }
    }
}
