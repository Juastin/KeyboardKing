using DiscordRPC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Model.event_args;

namespace Controller
{
    public static class DiscordController
    {
		public static DiscordRpcClient client;

		public static void Initialize()
		{
			NavigationController.AfterNavigate += OnNavigate;
			client = new DiscordRpcClient("925728317238288425");
			client.Initialize();

			SetPresence("Just browsing", "Learning to type");
		}

		public static void SetPresence(string state, string details)
        {
			client.SetPresence(new RichPresence()
			{
				State = state,
				Details = details,
				Assets = new Assets()
				{
					LargeImageKey = "kk_icon",
					LargeImageText = "KeyboardKing",
				}
			});
		}

		private static void OnNavigate(NavigateEventArgs e)
        {
			switch (e.NewPage)
            {
				case Pages.EpisodePage:
					SetPresence($"Playing episode: {EpisodeController.CurrentEpisode.Name}", "Learning to type");
					break;
				case Pages.MatchPlayingPage:
					SetPresence($"Playing an online match, episode: {EpisodeController.CurrentEpisode.Name}", "Learning to type");
					break;
				default:
					SetPresence("Just browsing", "Learning to type");
					break;
            }
        }
	}
}
