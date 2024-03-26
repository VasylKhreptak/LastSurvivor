using GooglePlayGames;
using UI.Buttons.Core;
using UnityEngine;

namespace UI.Main.Buttons.Settings
{
    public class LeaderboardButton : BaseButton
    {
        protected override void OnClicked()
        {
            if (PlayGamesPlatform.Instance.IsAuthenticated() == false)
                return;

            Social.ShowLeaderboardUI();
        }
    }
}