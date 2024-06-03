using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public float maxhealth;
    public float physicalDamageMulitplier;
    public UnityEvent OnDeath;
    float health;
    void Awake(){
        health = maxhealth;
    }
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "physobj" && enabled)
        {
            health -= collision.impulse.magnitude * physicalDamageMulitplier;
            if (health <= 0)
            {
                OnDeath?.Invoke();
                enabled = false;
                //deathfunc();
            }
        }
    }
    //void deathfunc(){
    //    // Todo
    //    // DO NOT DESTROY
    //      GetComponent<Enemies.Shapeless.ShapelessMovingScript>().enabled = false;
    ////    GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
    //    GetComponent<CharacterController>().enabled = false;
    //    gameObject.SetActive(false);
    //}
}
