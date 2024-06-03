using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxhealth;
    public float damage;
    float health;
    void Start(){
        health = maxhealth;
    }
    void Update(){
        if(health <= 0){
            deathfunc();
        }
    }
    private void OnTriggerEnter(Collider other){
        if(other.tag == "physobj"){
            health -= damage * other.GetComponent<Rigidbody>().velocity.magnitude;
        }
    }
    void deathfunc(){
        // Todo
        // DO NOT DESTROY
          GetComponent<Enemies.Shapeless.ShapelessMovingScript>().enabled = false;
    //    GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
        GetComponent<CharacterController>().enabled = false;
        gameObject.SetActive(false);
    }
}
