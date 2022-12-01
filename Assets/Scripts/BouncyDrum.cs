using System;
using UnityEngine;

namespace MMM
{
    public class BouncyDrum : MonoBehaviour
    {
        private void OnTriggerExit2D(Collider2D other)
        {
            var player = other.GetComponentInParent<PlayerMovement>();
            if (player)
            {
                GetComponentInChildren<Collider2D>().isTrigger = false;
            }
        }
    }
}