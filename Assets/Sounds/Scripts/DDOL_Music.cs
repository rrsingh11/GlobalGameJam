using UnityEngine;

namespace Sounds.Scripts
{
    public class DDOL_Music : MonoBehaviour
    {
        private static DDOL_Music _instance ;
 
        private void Awake()
        {
            //if we don't have an [_instance] set yet
            if(!_instance)
                _instance = this ;
            //otherwise, if we do, kill this thing
            else
                Destroy(this.gameObject) ;
 
 
            DontDestroyOnLoad(this.gameObject) ;
        }
    }
}
