using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collisionHandler : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Articulation Body: " + collision.articulationBody);
        Debug.Log("Articulation Body: " + collision.collider);
        Debug.Log("Articulation Body: " + collision.gameObject);

    }
}
