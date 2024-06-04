using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LedSk : MonoBehaviour
{
    public Transform pos;
    InputSkript input;
    Transform player;
    public GameObject obj;
    public bool repare;
    Canvassc canv;
    void Start(){
        canv = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Canvassc>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        input = GameObject.FindGameObjectWithTag("Input").GetComponent<InputSkript>();
    }
    void Update(){
        if(input.e){
            if(!repare){
                if(Vector3.Distance(transform.position, player.position) <= 3){
                    StartCoroutine(ChangePos(player, pos));
            }
            }
            else{
                canv.t1.text = canv.s1 + canv.s3;
            }
        }
        if(Vector3.Distance(transform.position, player.position) <= 3){
            obj.SetActive(true);
        }
        else{
            obj.SetActive(false);
        }
    }
    IEnumerator ChangePos(Transform targetObj, Transform posititon)
    {
        targetObj.GetComponent<CharacterController>().enabled = false;
        yield return null;
        targetObj.transform.position = posititon.position;
        yield return null;
        targetObj.GetComponent<CharacterController>().enabled = true;
    }
}
