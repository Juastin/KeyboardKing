﻿using DatabaseController;
using Model;
using System;

namespace Controller
{
    public static class InfiniteModeController
    {
        public static bool IsStarted {get;set;}
        public static int? AllowedMistakes {get;set;}
        public static int Mistakes {get;set;}
        public static string SelectedGamemode {get;set;}
        public static int Score {get;set;}

        public static void Initialise(int allowed_mistakes, string selected_gamemode)
        {
            IsStarted = true;
            AllowedMistakes = allowed_mistakes;
            Mistakes = 0;
            SelectedGamemode = selected_gamemode;
            EpisodeController.Initialise(GetRandomEpisode(), false);
        }

        /// <summary>
        /// Set the current episode to a random episode, selected from the db based on the total count of episodes.
        /// </summary>
        public static void SetRandomEpisode()
        {
            int total_episode_amount = DBQueries.GetTotalEpisodeAmount();
            int random_episode_id = new Random().Next(1, total_episode_amount+1);

            Episode episode = EpisodeController.ParseEpisode(random_episode_id);
            EpisodeController.CurrentEpisode = episode;
        }

        public static Episode GetRandomEpisode()
        {
            int total_episode_amount = DBQueries.GetTotalEpisodeAmount();
            int random_episode_id = new Random().Next(1, total_episode_amount+1);
            return EpisodeController.ParseEpisode(random_episode_id);
        }

        public static void Checks()
        {
            // Exit the gamemode if the set amount of mistakes was made.
            if (Mistakes==AllowedMistakes)
                Exit();
            // Refill episode steps if no steps are left.
            else if (EpisodeController.CurrentEpisode.EpisodeSteps.Count==1)
                SetRandomEpisode();
        }

        /// <summary>
        /// Exit the IniniteMode view and clear all used properties and classes.
        /// </summary>
        public static void Exit()
        {
            // Update proper table row based on selected gamemode.
            User user = (User)Session.Get("student");
            switch (SelectedGamemode)
            {
                case "InfiniteMode":
                    DBQueries.UpdateScoreInfiniteMode(user.Id, Score);
                    break;
                case "ThreeLifesMode":
                    DBQueries.UpdateScore3LifesMode(user.Id, Score);
                    break;
                default:
                    break;
            }

            // Reset class properties.
            IsStarted = false;
            AllowedMistakes = null;
            Mistakes = 0;
            SelectedGamemode = null;
            Score = 0;

            // Remove used session var.
            Session.Remove("InfiniteMode");

            EpisodeController.StopAndSetEpisodeResult();
            Session.Add("FetchGamemodeScores", true);
        }
    }
}
