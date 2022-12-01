using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
  
public class BounceHandle2D : MonoBehaviour
{  
	private Rigidbody2D rb;
	private Vector3 beforeCollisionVelocity;

	private void Awake()
	{ 
		rb = GetComponent<Rigidbody2D>();
	} 

	private void FixedUpdate() => beforeCollisionVelocity = rb.velocity; 

	private void OnCollisionEnter2D(Collision2D col)
	{   
		var reflected = Vector2.Reflect(beforeCollisionVelocity.normalized, 
										col.contacts[0].normal);  
		rb.velocity = reflected.normalized * rb.velocity.magnitude; 
    }
}