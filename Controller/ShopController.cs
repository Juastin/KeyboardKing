using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseController;

namespace Controller
{
    public static class ShopController
    {
        public delegate void ItemChange();
        public static event ItemChange CurrentItemChanged;

        public static event EventHandler ShopDataChanged;

        public static int MaxPage { get; set; }
        public static int ItemsPerPage { get; set; } = 8;

        public static Item CurrentItem { get; set; }
        public static List<Item> AllItems { get; set; }

        private static int _currentPage = 1;
        public static int CurrentPage
        {
            get
            {
                return _currentPage;
            }
            set
            {
                _currentPage = value;
                if (_currentPage < 1)
                {
                    _currentPage = 1;
                }
                else if (_currentPage > MaxPage)
                {
                    _currentPage = MaxPage;
                }
            }
        }

        /// <summary>
        /// Initialize the properties in the controller.
        /// </summary>
        public static void Initialize()
        {
            CurrentPage = 1;
            AllItems = GetAllItems();
            MaxPage = CalculateMaxPage();
        }

        /// <summary>
        /// Get all data and return it as a List<Item>
        /// </summary>
        /// <returns></returns>
        public static List<Item> GetAllItems()
        {
            // Get all the items data (Dummy data at the moment)
            return DBQueries.GetAllItems((User)Session.Get("student")); ;
        }

        /// <summary>
        /// Load the Items at a certain page with a certain items per page it can show.
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public static List<Item> GetPageItems(int page)
        {
            var query = (from pageItem in AllItems
                         select pageItem)
                         .Skip(ItemsPerPage * (page - 1))
                         .Take(ItemsPerPage);

            return query.ToList();
        }

        /// <summary>
        /// Load the Items at its current page.
        /// </summary>
        /// <returns></returns>
        public static List<Item> GetPageItems()
        {
            return GetPageItems(CurrentPage);
        }

        /// <summary>
        /// Change current item and invoke the delegate CurrentItemChanged.
        /// </summary>
        /// <param name="item"></param>
        public static void SetCurrentItem(Item item)
        {
            CurrentItem = item;
            CurrentItemChanged?.Invoke();
        }

        /// <summary>
        /// Calulates the MaxPage the current list of all items has.
        /// </summary>
        /// <returns></returns>
        public static int CalculateMaxPage()
        {
            return (int)Math.Ceiling((decimal)AllItems.Count / ItemsPerPage);
        }

        /// <summary>
        /// Checks if the item exists before doing something with it.
        /// </summary>
        /// <returns>Returns a boolean if item still exists in database.</returns>
        public static bool CheckItemExists()
        {
            return DBQueries.CheckIfItemExists(ShopController.CurrentItem) > 0;
        }

        /// <summary>
        /// Calls the methods needed to complete the step of adding 
        /// </summary>
        public static bool BuyItem()
        {
            bool buyCompleted = false;
            User student = (User)Session.Get("student");
            int currentCoins =  DBQueries.GetCoinsOfUser(student);
            
            if (CheckSufficientCoins(currentCoins))
            {
                Item item = CurrentItem;
                DBQueries.AddItem(student, item);
                DBQueries.UpdateCoins(student, item);
                buyCompleted = true;
                student.Coins = currentCoins - CurrentItem.Price;
            }
            else
            {
                student.Coins = currentCoins;
            }

            Session.Add("student", student);
            return buyCompleted;
        }

        public static bool CheckSufficientCoins(int coins)
        {
            if (coins >= CurrentItem.Price)
            {
                return true;
            }
            return false;
        }

        public static void UpdateItemsList()
        {
            AllItems = GetAllItems();
            MaxPage = CalculateMaxPage();

            ShopDataChanged?.Invoke(null, EventArgs.Empty);
        }
    }
}
