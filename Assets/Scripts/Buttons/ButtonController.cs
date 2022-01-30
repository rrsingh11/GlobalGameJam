using UnityEngine;

namespace Buttons
{
    public class ButtonController : MonoBehaviour
    {
        [SerializeField] private ButtonObject affectedObject;

        public void ResetPosition()
        {
            affectedObject.ResetPosition();
        }
        
        public void Trigger()
        {
            affectedObject.Perform();
        }
    }
}
