using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHearyng : MonoBehaviour
{
    [SerializeField] private Patrool _patrool;
    [SerializeField] private GameObject target;

    private void OnTriggerStay(Collider other) 
    {
        if(other.tag=="Player"&_patrool._isChasing==false)
        {
            if( target.GetComponent<PlayerMovement>()._makeSound==true)
            {
                Debug.Log("I CAN HEAR YOU");
                _patrool.SwitchAgentStateByID(1);
            }
            else
            {
                Debug.Log("cant hear you");
            }
            
        }
    }
}
