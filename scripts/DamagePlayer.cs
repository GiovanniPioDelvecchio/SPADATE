using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG {
    public class DamagePlayer : MonoBehaviour
    {
        public int damage;


        private void OnTriggerEnter(Collider other)
        {
            PlayerManager playerStats = other.GetComponent<PlayerManager>();
            if (playerStats != null) {
                playerStats.TakeDamage(damage);
            }
        }
    }
}

