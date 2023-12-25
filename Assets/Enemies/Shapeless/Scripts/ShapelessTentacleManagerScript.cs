using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Enemies.Shapeless
{
    public class ShapelessTentacleManagerScript : MonoBehaviour
    {
        public ShapelessTentacleScript[] allTentacles;
        public void ShowUpTentacles()
        {
            foreach(ShapelessTentacleScript tentacle in allTentacles)
            {
                tentacle.StartAnimatingWithDelay();
            }
        }
        public void HideTentacles()
        {
            foreach (ShapelessTentacleScript tentacle in allTentacles)
            {
                tentacle.StopAnimating();
            }
        }
    }
}