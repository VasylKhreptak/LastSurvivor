using UnityEngine;

namespace Flexalon
{
    /// <summary> Implement this interface and assign it to the Flexalon.InputProvider
    /// to override how FlexalonInteractables receive input. </summary>
    public interface InputProvider
    {
        /// <summary> True on the frame that the input becomes active. </summary>
        bool Activated { get; }

        /// <summary> True if the input is active, e.g. button is being held down. </summary>
        bool Active { get; }

        /// <summary> Ray to cast to determine what should be moved / hit. </summary>
        Ray Ray { get; }
    }
}