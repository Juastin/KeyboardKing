﻿using System;
using System.Collections.Generic;
using System.Linq;

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
        public bool AudioOn { get; set; }
        public bool AudioOnAtLogin { get; set; }
        public bool Dyslectic { get; set; }
        public bool DyslecticAtLogin { get; set; }
        public int Coins { get; set; }
        public string Theme { get; set; }

        public static List<User> ParseUserIds(List<List<string>> input)
        {
            return input.Select(u => new User() { Id = int.Parse(u[0]) }).ToList();
        }

        public static User ParseUser(List<List<string>> input)
        {
            if (input.Any())
            {
                return new User()
                {
                    Id = int.Parse(input[0][0]),
                    Username = input[0][1],
                    Email = input[0][2],
                    Coins = int.Parse(input[0][3]),
                    Password = input[0][4],
                    Salt = input[0][5],
                    SkillLevel = input[0][6] == string.Empty ? SkillLevel.none : (SkillLevel)Enum.Parse(typeof(SkillLevel), input[0][6]),
                    AudioOn = Convert.ToBoolean(int.Parse(input[0][7])),
                    Dyslectic = Convert.ToBoolean(int.Parse(input[0][8])),
                    Theme = input[0][9]
                };
            }
            return null;
        }
    }
}