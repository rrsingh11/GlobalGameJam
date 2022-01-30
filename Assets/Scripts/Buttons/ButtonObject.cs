using UnityEngine;

namespace Buttons
{
    public abstract class ButtonObject : MonoBehaviour
    {
        public abstract void Perform();
        public abstract void ResetPosition();
    }
}
