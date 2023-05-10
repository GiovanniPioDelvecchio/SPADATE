using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class Uccellagione : MonoBehaviour
    {
        public PlayerManager thisPlayerManager;
        public PlayerManager toSetTo1HP;

        private void OnTriggerEnter(Collider other)
        {
            
            if (other.gameObject.name == "felucachestabeneintesta" && !thisPlayerManager.uccellagioneFlag) {
                toSetTo1HP = other.gameObject.GetComponentInParent<PlayerManager>();
                toSetTo1HP.setTo1HP();
                other.gameObject.transform.position = this.transform.position;
                other.gameObject.transform.parent = this.transform;
                thisPlayerManager.uccellagioneFlag = true;
                
            }
        }
    }
}
