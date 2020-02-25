using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HighlightPlus;

public class CharController: MonoBehaviour
{
	public GameObject []effect;
	// public Transform []effectTransform;
	public List<WeaponVO> weapon;
	public List<WeaponVO> gear;
	public Transform head;

	public Animator animator;
	public CharacterView characterView;
	public HighlightEffect highlightEffect;
	public HighlightTrigger highlightTrigger;
	public Rigidbody rigidBody;
	public MeshRenderer meshRenderer;
	// private AudioSource audioSource;
	public GameObject gameManager;
	private AbilityVO currentAbility;
	public bool isIdleCharacter = true;
	private bool isHit = false;
	public ParticleSystem part;
	public List<ParticleCollisionEvent> collisionEvents;
	private AssetBundle assetBundle;
	bool mouseOver = false;
	// private Color startColor;
	// private CapsuleCollider capCollider;
	// private Renderer renderer;

	void Awake()
	{
		// animationEventHandlerHandler = new Action<EventParam>(cellClickHandlerFunction);
	}
	void Start()
	{
		Debug.Log("== CharController.Start ==");
		animator = GetComponent<Animator>();
		// part = GetComponent<ParticleSystem>();
		// audioSource = this.gameObject.AddComponent<AudioSource>();
		collisionEvents = new List<ParticleCollisionEvent>();
		// Debug.Log("Anim " + animator + " AudioSrc " + audioSource);
		// meshRenderer = gameObject.AddComponent<MeshRenderer>();

		// add highlight fx
		if (isIdleCharacter == true)
		{
			// meshRenderer.enabled = false;
			highlightEffect = this.gameObject.AddComponent<HighlightEffect>();
			// add trigger
			highlightTrigger = this.gameObject.AddComponent<HighlightTrigger>();
			// SetHightLightOOP();
		} 
		else
		{
			// add rigidbody
			rigidBody = gameObject.AddComponent<Rigidbody>();
			rigidBody.isKinematic = true;
			// add meshrenderer
			meshRenderer = gameObject.AddComponent<MeshRenderer>();
		}

		if (GameManager.instance == null)
			Instantiate(gameManager);
	}
	public void SetHightLightOOP()//int val = 0)
	{
		Debug.Log("== SetHighLightOOP == " + characterView.model.handle);
		Debug.Log(characterView.model.oopState);

		HighlightEffect h = highlightEffect;
		h.overlay = 0f;
		h.overlayMinIntensity = 0.0f;
		h.overlayBlending = 0.0f;
		// h.outlineAlwaysOnTop = true;
		// h.cullBackFaces = false;
		
		if (characterView.model.oopState == CharacterVO.OOPState.None || mouseOver == true)
		{
			if (highlightTrigger == null)
				highlightTrigger = this.gameObject.AddComponent<HighlightTrigger>();

			h.glowDithering = false;
			h.glow = 0.1f;
			h.outlineColor = Color.white;
			h.outlineWidth = 0.1f;
			h.glowWidth = 0.05f;
			h.innerGlowWidth = 0.05f;
			h.seeThrough = HighlightPlus.SeeThroughMode.Never;

			h.Refresh();
			
			return;
		}
		// disable mouse trigger
		Destroy(highlightTrigger);
		Color color = Color.yellow;
		switch(characterView.model.oopState)
		{
			case CharacterVO.OOPState.Yellow: ColorUtility.TryParseHtmlString("#FF748C", out color); break;//color = Color.yellow; break;
			case CharacterVO.OOPState.Orange: ColorUtility.TryParseHtmlString("#ffa500", out color); break;
			case CharacterVO.OOPState.Red: ColorUtility.TryParseHtmlString("#800000", out color); break; //Color.red; break;
		}
		if (h != null)
		{
			h.overlayColor = color;
			// for (int i = 0; i < h.glowPasses.Length; i++)
			// {
			// 	var g = h.glowPasses[i];
			// 	g.color = color;
			// }
			h.glowDithering = true;
			h.glow = 0.75f;
			// h.outlineColor = Color.white;
			// h.outlineWidth = 0.5f;
			h.glowWidth = 0.5f;
			h.innerGlowWidth = 0.5f;
			// h.seeThrough = HighlightPlus.SeeThroughMode.Never;

			h.seeThrough = HighlightPlus.SeeThroughMode.Never;
			// h.seeThroughIntensity = 0.8f;
			// h.seeThroughTintAlpha = 0.5f;
			// h.seeThroughTintColor = color; 
			h.outlineWidth = 0.1f;
			h.glowHQColor = color;
			h.outlineColor = color;
			h.innerGlowColor = color;
			h.enabled = true;
			h.SetHighlighted(true);
		}

		h.Refresh();
	}
	void OnMouseEnter()
	{
	    // Debug.Log("OnMouseEnter " + characterView.model.position + " / " + characterView.model.oopState + " / " + characterView.model.crewType);
		// EventParam eventParam = new EventParam();
		// eventParam.data = characterView.model;
		// eventParam.name = "characterMouseEnter";
		// EventManager.TriggerEvent("combatEvent", eventParam);
		this.mouseOver = true;
		if (isIdleCharacter == true && GameManager.instance.roundIsLocked != true)
			SetHightLightOOP();
	}

	void OnMouseExit()
	{
		Debug.Log("OnMouseEnter " + characterView.model.position + " / " + characterView.model.crewType);
		// EventParam eventParam = new EventParam();
		// eventParam.data = characterView.model;
		// eventParam.name = "characterMouseExit";
		// EventManager.TriggerEvent("combatEvent", eventParam);
		this.mouseOver = false;
		if (isIdleCharacter == true && GameManager.instance.roundIsLocked != true)
			SetHightLightOOP();
	}

	void OnMouseDown()
	{
		Debug.Log("OnMouseDown" + characterView.model.position + " / " + characterView.model.crewType);
		SoundVO soundFx = new SoundVO(200, "Matrix Glitch", SoundVO.CHARACTER_SELECT);
		characterView.audioSource.PlayOneShot(soundFx.audioClip, 0.2f);
		// validate that card is stowed
		EventParam eventParam = new EventParam();
		eventParam.data = characterView.model;
		eventParam.name = "characterSelected";
		EventManager.TriggerEvent("combatEvent", eventParam);
	}

	void ActivateEffect(int effect)
	{
		Debug.Log("<color=orange>== CharController.ActivateEffect == " + effect + "</color>");

		if (effect == 100)
		{
			animator.SetTrigger("toIdle");
			return;
		}
		if (effect == 200) // preattack event
		{
			characterView.ActivatePreAttack();
			return;
		}
		// Debug.Log("i " + i);
		Debug.Log("effect " + effect);
		// TODO: get currentAbility from Singleton
		currentAbility = GameManager.instance.currentAbility;

		// effect 1 is ability sound fx
		if (effect == 1)
		{
			characterView.ActivateEffectAudio(currentAbility.soundOnActivate);
			return;
		}
		Debug.Log("* " + characterView.model.handle + " has executed effect #" + currentAbility.name);
		animator.SetTrigger("toIdle");
		characterView.animationEventHandler(currentAbility);

		// manage melee hits automagically
		if (characterView.isSource && currentAbility.actionType == AbilityVO.ABILITY_ACTIONTYPE_OFFENSE_MELEE)
		{
			Debug.Log("* melee attack affect!");

			// set single target isHit()
			CharacterView target = characterView.targets[0];
			target.isHit();
		}
	}

	// public void AnimationEventHandler(int effect)
	// {
	// 	Debug.Log("<color=green>==== GOT EVENT ==== " + effect + "</color>");
	// 	// EventParam eventParam = new EventParam();
	// 	// eventParam.data = "hi";
	// 	// EventManager.TriggerEvent("animationEvent", eventParam);
	// 	currentAbility = GameManager.instance.currentAbility;
	// 	characterView.animationEventHandler(currentAbility);
	// }

	void OnTriggerEnter(Collider other)
	{
		Debug.Log("<color=green>== CharacterController.OnTriggerEnter ==</color>" + other);
		if (characterView && isHit == false) {
			isHit = true;
			characterView.OnTriggerEnterHandler(other);
		}
	}
	void OnTriggerStay(Collider other)
	{
		Debug.Log("<color=green>== CharacterController.OnTriggerStay ==</color>");
		if (characterView)
			characterView.OnTriggerStayHandler(other);
	}
	void OnTriggerExit(Collider other)
	{
		Debug.Log("<color=green>== CharacterController.OnTriggerExit ==</color>");
		if (characterView)
			characterView.OnTriggerExitHandler(other);
	}
	
	public void OnCollisionEnter(Collision collision = null)
	{
		Debug.Log("<color=green>== CharController.OnCollisionEnter ==</color>");
		// ignore floor collisions
		if (characterView && isHit == false)
		{

			isHit = true;
			characterView.OnCollisionEnterHandler(collision);
			
			// // bloodFX
			// if (collision != null && collision.gameObject.name != "Floor") 
			// {
			// 	Vector3 point = collision.contacts[0].point;
			// 	Debug.Log("<color=red>contact " + point.x + "/" + point.y + "/" + point.z + " </color>");
			// 	GameObject fx = GameManager.instance.assetBundleCombat.LoadAsset<GameObject>("Blood11_2 Variant");
			// 	GameObject cfx = Instantiate(fx) as GameObject;
			// 	cfx.transform.position = new Vector3(point.x, point.y, point.z);
			// 	// GameObject cfx = Instantiate(fx, transform.position, new Quaternion()) as GameObject;
			// 	Debug.Log("<color=red>prefab " + cfx + " </color>");
			// }
			// else
			// {
			// 	// cfx.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
			// }
		}
		// Debug.Log("******** collisions");
		// Debug.Log("<color=red>" + collision.gameObject.name + "</color>");
		// Time.timeScale = 0;
		// foreach (ContactPoint contact in collision.contacts)
		// {
		// 	Debug.DrawRay(contact.point, contact.normal, Color.white);
		// }
		// if (collision.relativeVelocity.magnitude > 2)
		// 	audioSource.Play();
	}
	void OnCollisionStay(Collision collision)
	{
		Debug.Log("<color=green> collstay " + collision.gameObject.name + "</color>");
		// ignore floor collisions
		if (collision.gameObject.name != "Floor")
			characterView.OnCollisionStayHandler(collision);
	}
	void OnCollisionExit(Collision collision)
	{
		Debug.Log("<color=green>== CharController.OnCollisionExit ==</color>");
		// ignore floor collisions
		if (characterView && collision.gameObject.name != "Floor")
			characterView.OnCollisionExitHandler(collision);
	}

	void OnParticleCollision(GameObject other)
	{
		if (isHit == false && isIdleCharacter == false && characterView.model.handle != "FloorController")
		{
			// Debug.Log("<color=orange>" + characterView.model.handle + " Collided with particle</color>");
			isHit = true;
			characterView.OnParticleCollisionHandler(other);

			// int numCollisionEvents = part.GetCollisionEvents(TargetedParticle, collisionEvents);

			// Rigidbody rb = TargetedParticle.GetComponent<Rigidbody>();
			// int i = 0;

			// while (i < numCollisionEvents)
			// {
			// 	if (rb)
			// 	{
			// 		Vector3 pos = collisionEvents[i].intersection;
			// 		Vector3 force = collisionEvents[i].velocity * 10;
			// 		rb.AddForce(force);
			// 	}
			// 	i++;
			// }		
		}
	}

	void OnParticleTrigger()
	{
		Debug.Log("<color=orange>Triggered with particle</color>");
	}

	private void CollisionEnter(object sender, RFX4_PhysicsMotion.RFX4_CollisionInfo e)
	{
		Debug.Log("<color=green>== CharController.CollisionEnter ==</color>");
		Debug.Log(e.HitPoint); //a collision coordinates in world space
		Debug.Log(e.HitGameObject.name); //a collided gameobject
		Debug.Log(e.HitCollider.name); //a collided collider :)
	}

	// public void GameObject GetEffect(int num)
	// {

	// }

	void Update()
	{

	}
}