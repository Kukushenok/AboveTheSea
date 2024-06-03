using DeathScreen;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Player
{
    public class PlayerKillManager : MonoBehaviour
    {
        [SerializeField] private int deathSceneID;
        public void KillMe()
        {
            DeathAnimationBehaviour.ShowImmediateDeathScreen(deathSceneID);
        }
    }
}