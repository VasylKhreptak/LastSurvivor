using UI.Buttons.Core;
using UnityEngine;

namespace UI.Main.Buttons.Settings
{
    public class OpenAchievementsButton : BaseButton
    {
        protected override void OnClicked() => Social.ShowAchievementsUI();
    }
}