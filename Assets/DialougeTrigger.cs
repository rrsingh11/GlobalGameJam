using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DialougeTrigger : MonoBehaviour
{
    [SerializeField] DialougeManager Manager;
    public Dialouge dialouge;

    public  void TriggerDialouge()
    {
        Manager.StartDialouge(dialouge);
    }
}
