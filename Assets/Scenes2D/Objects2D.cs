using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objects2D : MonoBehaviour
{
    public Transform targetPlayer;

    [ContextMenu("Shoot")]
    public void Hit()
    {
        targetPlayer.GetComponent<Rigidbody2D>().AddForce(new Vector2(-50, 100) * 10);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
