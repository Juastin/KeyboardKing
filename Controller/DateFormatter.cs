using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller
{
    /// <summary>
    /// https://dotnetthoughts.net/time-ago-function-for-c/
    /// </summary>
    public static class DateFormatter
    {
        public static string TimeAgo(this DateTime dateTime)
        {
            string result;
            var timeSpan = DateTime.Now.Subtract(dateTime);

            if (timeSpan <= TimeSpan.FromSeconds(60))
            {
                result = string.Format("{0} seconds ago", timeSpan.Seconds);
            }
            else if (timeSpan <= TimeSpan.FromMinutes(60))
            {
                result = timeSpan.Minutes > 1 ? 
                    String.Format("about {0} minutes ago", timeSpan.Minutes) :
                    "about a minute ago";
            }
            else if (timeSpan <= TimeSpan.FromHours(24))
            {
                result = timeSpan.Hours > 1 ? 
                    String.Format("about {0} hours ago", timeSpan.Hours) : 
                    "about an hour ago";
            }
            else if (timeSpan <= TimeSpan.FromDays(30))
            {
                result = timeSpan.Days > 1 ? 
                    String.Format("about {0} days ago", timeSpan.Days) : 
                    "yesterday";
            }
            else if (timeSpan <= TimeSpan.FromDays(365))
            {
                result = timeSpan.Days > 30 ? 
                    String.Format("about {0} months ago", timeSpan.Days / 30) : 
                    "about a month ago";
            }
            else
            {
                result = timeSpan.Days > 365 ? 
                    String.Format("about {0} years ago", timeSpan.Days / 365) : 
                    "about a year ago";
            }

            return result;
        }

        public static string TimeAgoNL(this DateTime dateTime)
        {
            string result;
            var timeSpan = DateTime.Now.Subtract(dateTime);

            if (timeSpan <= TimeSpan.FromSeconds(60))
            {
                result = string.Format("{0} seconden geleden", timeSpan.Seconds);
            }
            else if (timeSpan <= TimeSpan.FromMinutes(60))
            {
                result = timeSpan.Minutes > 1 ? 
                    String.Format("{0} minuten geleden", timeSpan.Minutes) :
                    "een minuut geleden";
            }
            else if (timeSpan <= TimeSpan.FromHours(24))
            {
                result = timeSpan.Hours > 1 ? 
                    String.Format("{0} uur geleden", timeSpan.Hours) : 
                    "een uur geleden";
            }
            else if (timeSpan <= TimeSpan.FromDays(30))
            {
                result = timeSpan.Days > 1 ? 
                    String.Format("{0} dagen geleden", timeSpan.Days) : 
                    "gister";
            }
            else if (timeSpan <= TimeSpan.FromDays(365))
            {
                result = timeSpan.Days > 30 ? 
                    String.Format("{0} maanden geleden", timeSpan.Days / 30) : 
                    "een maand geleden";
            }
            else
            {
                result = timeSpan.Days > 365 ? 
                    String.Format("{0} jaar geleden", timeSpan.Days / 365) : 
                    "een jaar geleden";
            }

            return result;
        }
    }
}
