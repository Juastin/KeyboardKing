using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller
{
    public static class ShopController
    {
        public static int CurrentPage { get; set; }
        public static int itemsPerPage { get; private set; }

        public static Item CurrentItem { get; set; }
        public static List<Item> AllItems { get; private set; }

        public static void Initialize()
        {
            CurrentPage = 1;
            itemsPerPage = 8;
            GetAllItems();
        }

        public static void GetAllItems()
        {
            List<List<string>> Items = new List<List<string>>();
            Items = new List<List<string>> {
                new List<string> {"1", "KeyboardKing Light", "/KeyBoardking;component/resources/images/kk_background_4K.png", "10", "Theme", "True" },
                new List<string> {"2", "KeyboardKing Dark", "/KeyBoardking;component/resources/images/kk_background_dark.png", "20", "Theme", "True" },
                new List<string> {"3", "Space", "/KeyBoardking;component/resources/images/space_theme_background.png", "50", "Theme", "True" },
                new List<string> {"4", "Chinese", "/KeyBoardking;component/resources/images/red_chinese_background.png", "50", "Theme", "True" },
                new List<string> {"5", "Paint", "/KeyBoardking;component/resources/images/paint_theme_background.png", "100", "Theme", "False" },
                new List<string> {"6", "Obsidian", "/KeyBoardking;component/resources/images/obsidian_theme_background.png", "250", "Theme", "True" },
                new List<string> {"7", "Hello Beertje", "/KeyBoardking;component/resources/images/hellobeertje_background_4k.png", "500", "Theme", "False" },
                new List<string> {"8", "Christmas", "/KeyBoardking;component/resources/images/kk_background_christmas.png", "1000", "Theme", "False" },
                new List<string> {"9", "A Shelf on a shelf", "/KeyBoardking;component/resources/images/shopping_shelf.png", "5000", "Theme", "False" },
                new List<string> {"10", "Crown", "/KeyBoardking;component/resources/images/icon.png", "5000", "Theme", "False" },
                new List<string> {"11", "A Coin", "/KeyBoardking;component/resources/images/icons/coin.png", "10000", "Theme", "False" },
            };
            List<Item> allItems = Item.ParseItems(Items);
            AllItems = allItems;
        }

        public static List<Item> GetPageItems(int page)
        {
            var query = (from pageItem in AllItems
                         select pageItem)
                         .Skip(ShopController.itemsPerPage * (page - 1))
                         .Take(ShopController.itemsPerPage);

            return query.ToList();
        }

        public static List<Item> GetPageItems()
        {
            var query = (from pageItem in AllItems
                         select pageItem)
                         .Skip(ShopController.itemsPerPage * (CurrentPage - 1))
                         .Take(ShopController.itemsPerPage);

            return query.ToList();
        }

    }
}
