using System.Collections.Generic;
using Model;
using NUnit.Framework;

namespace ModelTest
{
    class Model_User
    {
        private User User { get; set; }

        [SetUp]
        public void Setup()
        {
            User = new User()
            {
                Id = 1313,
                Email = "tester@test.com",
                Coins = 13,
                AudioOn = true,
                AudioOnAtLogin = true,
                Dyslectic = true,
                DyslecticAtLogin = true,
                SkillLevel = SkillLevel.gemiddeld,
                Theme = "space_theme",
                Username = "tester"
            };
        }

        [Test]
        public void PropertiesAreAccessible()
        {
            bool r_a = User.Id == 1313;
            bool r_b = User.Email.Equals("tester@test.com");
            bool r_c = User.Coins == 13;
            bool r_d = User.AudioOn;
            bool r_e = User.AudioOnAtLogin;
            bool r_f = User.Dyslectic;
            bool r_g = User.DyslecticAtLogin;
            bool r_h = User.SkillLevel == SkillLevel.gemiddeld;
            bool r_i = User.Theme.Equals("space_theme");
            bool r_j = User.Username.Equals("tester");

            Assert.IsTrue( ((r_a) && (r_b) && (r_c) && (r_d) && (r_e) && (r_f) && (r_g) && (r_h) && (r_i) && (r_j)) );
        }

        [Test]
        public void ParseUserIds()
        {
            List<List<string>> user_id_list = new List<List<string>>()
            {
                new List<string>(){"13", "Tom", "tom-online@test.com"},
                new List<string>(){"14", "Jerry", "jerry-online@test.com"}
            };
            List<User> user_list = User.ParseUserIds(user_id_list);

            Assert.IsTrue( (user_list[0].Id==13) && (user_list[1].Id==14) );
        }

        [Test]
        public void ParseUser()
        {
            List<List<string>> user_id_list = new List<List<string>>()
            {
                new List<string>()
                {
                    "13",                    // Id
                    "Tom",                   // Username
                    "tom-online@test.com",   // Email
                    "110",                   // Coins
                    "",                      // PW
                    "",                      // Salt
                    "",                      // SkillLevel
                    "0",                     // AudioOn
                    "1",                     // Dyslectic
                    "light_theme"            // Theme
                },
            };
            User result = User.ParseUser(user_id_list);

            Assert.IsTrue( (result.Id==13) && (result.Username.Equals("Tom")) && (result.Email.Equals("tom-online@test.com")) );
        }
    }
}
