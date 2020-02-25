using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
		GameManager.instance.floorController = this;
    }
	void OnParticleCollision(GameObject TargetedParticle)
	{
		Debug.Log("<color=orange> FloorController Collided with particle</color>");
		GameManager.instance.FloorCollisionHandler();
	}

	void OnParticleTrigger()
	{
		Debug.Log("<color=orange>FloorController Triggered with particle</color>");
		GameManager.instance.FloorCollisionHandler();
	}

	private void CollisionEnter(object sender, RFX4_PhysicsMotion.RFX4_CollisionInfo e)
	{
		Debug.Log("<color=green>== FloorController.CollisionEnter ==</color>");
		Debug.Log(e.HitPoint); //a collision coordinates in world space
		Debug.Log(e.HitGameObject.name); //a collided gameobject
		Debug.Log(e.HitCollider.name); //a collided collider :)
		
		GameManager.instance.FloorCollisionHandler();
	}

	// Update is called once per frame
	void Update()
    {
        
    }
}
