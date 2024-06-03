using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hooksk : MonoBehaviour
{
    public float speed;
    [HideInInspector]
    public Vector3 dir;
     [HideInInspector]
    public Abobask ropesk;
     [HideInInspector]
    public bool second;
    private void OnTriggerEnter(Collider other){
        if(other.tag != "Respawn" && other.tag != "Player"){
           
            transform.parent = other.transform;
            if(other.tag == "physobj"){ 
            if(second)
                ropesk.attractionobj2 = other.gameObject;
        }
        else{
            if(second){
                ropesk.attractionobj2 = gameObject;
            }
        }
        GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<Rigidbody>().useGravity = false;
            GetComponent<Collider>().enabled = false;
        }
    }
    public void func(){
        GetComponent<Rigidbody>().AddForceAtPosition(dir.normalized * speed * 1000, transform.position);
    }
}
