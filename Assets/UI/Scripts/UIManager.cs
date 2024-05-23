using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private GameObject gameplayElements;
        public bool EnableElements { get => gameplayElements.activeSelf; set => gameplayElements.SetActive(value); }
        // Start is called before the first frame update
        void Awake()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        

        //// Update is called once per frame
        //void Update()
        //{

        //}
    }
}