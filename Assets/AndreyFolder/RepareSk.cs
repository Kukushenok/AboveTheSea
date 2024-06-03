using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RepareSk : MonoBehaviour
{
    public GameObject pump;
    Canvassc canv;
    void Start(){
        canv = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Canvassc>();
    }
    private void OnTriggerEnter(Collider other){
        if(other.tag == "physobj"){
            if(other.GetComponent<Rigidbody>().velocity.magnitude > 1){
                canv.t2.text = canv.s2 + canv.s3;
                pump.GetComponent<Animation>().Play();
            }
           
        }
    }
}
