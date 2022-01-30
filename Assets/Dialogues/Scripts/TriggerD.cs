using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerD : MonoBehaviour
{
    [SerializeField] DialougeManager Manager;
    public Dialouge dialouge;
    public void OnTriggerEnter(Collider other)
    {
        
        GetComponent<BoxCollider>().enabled = false; 
        Manager.StartDialouge(dialouge);
        //Destroy(gameObject);
       
    }
}
