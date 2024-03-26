using UI.Buttons.Core;
using UnityEngine;

namespace UI.Main.Buttons.Settings
{
    public class OpenLeaderboardButton : BaseButton
    {
        protected override void OnClicked() => Social.ShowLeaderboardUI();
    }
}