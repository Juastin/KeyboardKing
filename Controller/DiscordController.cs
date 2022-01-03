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

		public static void SetPresence(string state, string details, bool timestamp = false)
        {
			client.SetPresence(new RichPresence()
			{
				State = state,
				Details = details,
				Assets = new Assets()
				{
					LargeImageKey = "kk_icon",
					LargeImageText = "KeyboardKing",
				},
				Timestamps = timestamp ? new Timestamps() { Start = DateTime.UtcNow } : null
			});
		}

		private static void OnNavigate(NavigateEventArgs e)
        {
			switch (e.NewPage)
            {
				case Pages.EpisodePage:
					SetPresence(EpisodeController.CurrentEpisode.Name, "Playing episode", true);
					break;
				case Pages.MatchLobbyPage:
					SetPresence(EpisodeController.CurrentEpisode.Name, "Waiting in a match lobby");
					break;
				case Pages.MatchPlayingPage:
					SetPresence(EpisodeController.CurrentEpisode.Name, "Playing an online match", true);
					break;
				default:
					SetPresence("Just browsing", "Learning to type");
					break;
            }
        }
	}
}
