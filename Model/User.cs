using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public enum SkillLevel
    {
        none, beginner, gemiddeld, gevorderd
    }
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public SkillLevel SkillLevel { get; set; }

        public static List<User> ParseUserIds(List<List<string>> input)
        {
            return input.Select(u => new User() { Id = int.Parse(u[0]) }).ToList();
        }
    }
}
