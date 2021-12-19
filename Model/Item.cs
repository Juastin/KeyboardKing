using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public enum ItemType
    {
        Theme
    }

    public class Item : IEquatable<Item>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public int Price { get; set; }
        public ItemType Type { get; set; }
        public string Purchased { get; set; }

        public Item(int id, string name, string path, int price, ItemType type, string purchased)
        {
            Id = id;
            Name = name;
            Path = path;
            Price = price;
            Type = type;
            Purchased = purchased;
        }

        public static Item ParseItem(List<string> input)
        {
            ItemType Type = (ItemType)Enum.Parse(typeof(ItemType), input[4]);

            return new Item(int.Parse(input[0]), input[1], input[2], int.Parse(input[3]), Type, input[5]);
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
}
