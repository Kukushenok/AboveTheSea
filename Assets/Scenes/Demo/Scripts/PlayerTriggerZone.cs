using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Demo
{
    /// <summary>
    /// Класс позволяет осуществить какое-либо событие в том случае, когда игроr зашёл в триггер
    /// </summary>
    public class PlayerTriggerZone : MonoBehaviour
    {
        [SerializeField] private UnityEvent OnPlayerEnter;
        public void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                OnPlayerEnter.Invoke();
            }
        }
    }
}