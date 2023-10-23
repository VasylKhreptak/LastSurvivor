using UnityEngine;
using Zenject;

namespace TransformUtilities.Looker
{
    public class TransformLooker : ITickable
    {
        public readonly TransformLookerPreferences Preferences;

        public TransformLooker(TransformLookerPreferences preferences)
        {
            Preferences = preferences;
        }

        public void Tick() => LookStep();

        private void LookStep()
        {
            if (Preferences.Source == null || Preferences.Target == null)
                return;

            Vector3 direction = Preferences.Target.position - Preferences.Source.position;

            Quaternion targetRotation = Quaternion.LookRotation(direction, Preferences.Upwards);

            Preferences.Source.rotation =
                Quaternion.Lerp(Preferences.Source.rotation, targetRotation, Preferences.RotationSpeed * Time.deltaTime);
        }
    }
}