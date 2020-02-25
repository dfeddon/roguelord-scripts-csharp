using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;

public class CinecamController: MonoBehaviour
{
	public const int FRAME_ALL_CREWS = 1;
	// public const int HEALTH_MODIFIER_NONE = 2;
	// public const int HEALTH_MODIFIER_NONE = 3;
	// public const int HEALTH_MODIFIER_NONE = 4;

	public static CinecamController instance = null; //Static instance of GameManager which allows it to be accessed by any other
											   
	// CinemachineBlendDefinition someCustomBlend;
	private CinemachineVirtualCamera vcam1;
	private CinemachineVirtualCamera vcam2;
	private CinemachineVirtualCamera vcam3;
	private CinemachineVirtualCamera vcam4;
	private CinemachineVirtualCamera vcam25;
	private CinemachineVirtualCamera currentCam;
	// CinemachineFreeLook freelook;
	// CinemachineBasicMultiChannelPerlin noise;
	private CinemachineBrain brain;

	public GameObject idleWindow;
	public CharacterView combatSourceViewActive;
	public List<CharacterView> targets;

	private GameObject centeringGO;// = new GameObject("centeringGO");
	private float FOVdefault = 30;
	private float screenYdefault = 0.70f;
	private AudioSource audioSource;
	private bool filtersEnabled = false;

	// public void init()
	// {
	void Awake()
	{
		Debug.Log("== CinecamController.Awake ==");

		//Check if instance already exists
		if (instance == null)

			//if not, set instance to this
			instance = this;

		//If instance already exists and it's not this:
		else if (instance != this)

			//Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
			Destroy(gameObject);

		//Sets this to not be destroyed when reloading scene
		// DontDestroyOnLoad(gameObject);

		//Get a component reference to the attached BoardManager script
		// boardScript = GetComponent<BoardManager>();

		//Call the InitGame function to initialize the first level 
		// InitGame();
	}

	void Start()
	{
		centeringGO = new GameObject("centeringGO");
		// GameObject.FindWithTag("MainCamera").GetComponent<CameraFilterPack_3D_Shield>().enabled = false;
		// EnableFilter("shield", false);
		// EnableFilter("matrix", false);

		audioSource = this.gameObject.AddComponent<AudioSource>();

	}

	public CinemachineVirtualCamera getCurrentCam()
	{
		return currentCam;
	}
	// public void DoShake(float ampGain = 1f, float freqGain = 5f)
	// {
		// noise = currentCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
		// noise.m_AmplitudeGain = ampGain;
		// noise.m_FrequencyGain = freqGain;
	// }
	public CinemachineVirtualCamera getCamByInt(int num, bool headroom = false, CharacterView subject = null)
	{
		Debug.Log("== CinecamController.getCamByInt ==");
		Debug.Log("* num " + num);

		bool isNewCam = false;

		if (filtersEnabled == false)
		{
			filtersEnabled = true;
			EnableFilter("shield", false);
			EnableFilter("matrix", false);
		}

		CinemachineVirtualCamera returnCam = null;

		switch(num)
		{
			/////////////////////////////////////
			// Cam 1
			/////////////////////////////////////
			case 1:
				if (!vcam1)
				{
					isNewCam = true;
					vcam1 = new GameObject("VirtualCamera1").AddComponent<CinemachineVirtualCamera>();
					vcam1.m_Follow = idleWindow.transform;
					vcam1.m_Priority = 10;
					vcam1.gameObject.transform.position = new Vector3(0, 1, 0);
					vcam1.m_Lens.FieldOfView = FOVdefault;
					// frame shot
					CinemachineFramingTransposer composer = vcam1.AddCinemachineComponent<CinemachineFramingTransposer>();
					composer.m_ScreenY = screenYdefault;
					composer.m_DeadZoneWidth = 0.30f;
					composer.m_DeadZoneHeight = 0.0f;//35f;
					// composer.m_DeadZoneDepth = 0.5f;
					// composer.m_ZDamping = 2f;
					// add impulse listener
					vcam1.gameObject.AddComponent<CinemachineImpulseListener>();
				// 	vcam1.gameObject.AddComponent<CmBlendFinishedNotifier>();
				}
				returnCam = vcam1;
				break;

			/////////////////////////////////////
			// Cam 2
			/////////////////////////////////////
			case 2:
				if (!vcam2)
				{
					isNewCam = true;
					vcam2 = new GameObject("VirtualCamera2").AddComponent<CinemachineVirtualCamera>();
					vcam2.m_Priority = 20;
					vcam2.m_Lens.FieldOfView = 20; // zoom
					vcam2.gameObject.transform.position = new Vector3(0, 1, 0);
					// frame shot
					var composer = vcam2.AddCinemachineComponent<CinemachineFramingTransposer>();
					composer.m_ScreenY = 0.90f;
					composer.m_AdjustmentMode = CinemachineFramingTransposer.AdjustmentMode.DollyThenZoom;
					// vcam2.gameObject.SetActive(true);
					// add impulse listener
					vcam2.gameObject.AddComponent<CinemachineImpulseListener>();
				}
				vcam2.m_Follow = combatSourceViewActive.prefab.transform;
				
				returnCam = vcam2;
				vcam1.gameObject.SetActive(false);
				if (vcam3)
					vcam3.gameObject.SetActive(false);
				else Debug.Log("* vcam3 not available!");
				break;

			/////////////////////////////////////
			// Cam 3
			/////////////////////////////////////
			case 3:
				if (!vcam3)
				{
					isNewCam = true;
					vcam3 = new GameObject("VirtualCamera3").AddComponent<CinemachineVirtualCamera>();
					vcam3.m_Follow = GameObject.FindWithTag("CombatWindow").transform;// prefab.transform;
					vcam3.m_Priority = 30;
					vcam3.gameObject.transform.position = new Vector3(0, 1, 0);
					vcam3.m_Lens.FieldOfView = FOVdefault;

					// frame shot
					var composer = vcam3.AddCinemachineComponent<CinemachineFramingTransposer>();
					composer.m_ScreenY = screenYdefault;
					composer.m_DeadZoneWidth = 0.30f;
					composer.m_DeadZoneHeight = 0.35f;
					// add impulse listener
					vcam3.gameObject.AddComponent<CinemachineImpulseListener>();
				}
				returnCam = vcam3;
				break;

			/////////////////////////////////////
			// Cam 4
			/////////////////////////////////////
			case 4:
				if (!vcam4)
				{
					isNewCam = true;
					vcam4 = new GameObject("VirtualCamera4").AddComponent<CinemachineVirtualCamera>();
					vcam4.m_Priority = 40;
					int zoom = 20;
					// force it!
					headroom = true;
					if (headroom == true)
						zoom += 15;
					vcam4.m_Lens.FieldOfView = zoom; // zoom
					// frame shot
					var composer = vcam4.AddCinemachineComponent<CinemachineFramingTransposer>();
					composer.m_ScreenY = screenYdefault;
					composer.m_DeadZoneWidth = 0.30f;
					composer.m_DeadZoneHeight = 0.55f;
					// add impulse listener
					vcam1.gameObject.AddComponent<CinemachineImpulseListener>();
				}
				// GameObject go = new GameObject("Centering");
				// if targets.Count > 1 get end targets
				if (targets.Count > 1)
				{
					this.centeringGO.transform.position = Vector3.Lerp(targets[0].prefab.transform.position, targets[targets.Count - 1].prefab.transform.position, 0.25f);
				}
				else
				{
					this.centeringGO.transform.position = Vector3.Lerp(targets[0].prefab.transform.position, targets[0].prefab.transform.position, 0.5f);
				}
				vcam4.m_Follow = this.centeringGO.transform;// prefab.transform;

				returnCam = vcam4;
				// vcam3.gameObject.SetActive(false);
				break;
			case 25: // zoom on char
				if (!vcam25)
				{
					isNewCam = true;
					vcam25 = new GameObject("VirtualCamera2").AddComponent<CinemachineVirtualCamera>();
					vcam25.m_Priority = 0;
					vcam25.m_Lens.FieldOfView = 10; // zoom
					vcam25.gameObject.transform.position = new Vector3(0, 1, 0);
					// frame shot
					var composer = vcam25.AddCinemachineComponent<CinemachineFramingTransposer>();
					composer.m_ScreenY = 1.5f;
					composer.m_AdjustmentMode = CinemachineFramingTransposer.AdjustmentMode.DollyThenZoom;
					// vcam2.gameObject.SetActive(true);
					// add impulse listener
					vcam25.gameObject.AddComponent<CinemachineImpulseListener>();
				}
				// vcam25.m_Follow = combatSourceViewActive.prefab.transform;
				vcam25.m_Follow = subject.prefab.transform;

				returnCam = vcam25;
				// vcam1.gameObject.SetActive(false);
				// if (vcam3)
				// 	vcam3.gameObject.SetActive(false);
				// else Debug.Log("* vcam3 not available!");

			break;
		}

		returnCam.gameObject.SetActive(true);
		// if (this.vcam1) Debug.Log("%%%%% cam1");
		// if (this.vcam2) Debug.Log("%%%%% cam2");
		// if (this.vcam3) Debug.Log("%%%%% cam3");
		// if (this.vcam4) Debug.Log("%%%%% cam4");
		if (currentCam != null)
			returnCam.m_Priority = currentCam.m_Priority + 10;

		currentCam = returnCam;
		// if (isNewCam == true)
		// 	currentCam.gameObject.AddComponent<CmBlendFinishedNotifier>();
		return returnCam;

	}

	public CinemachineVirtualCamera GetVirtualCam(GameObject follow, GameObject lookAt, Vector3 position)
	{
		CinemachineVirtualCamera cam = new GameObject("VirtualCamera").AddComponent<CinemachineVirtualCamera>();
		// vcam1.m_LookAt = idleWindow.transform;//GameObject.Find("Cube").transform;
		// CinemachineGroupComposer group = vcam1.AddComponent("CinemachineGroupComposer")();
		// Debug.Log("* group " + group);
		if (follow)
			vcam1.m_Follow = follow.transform;//idleWindow.transform;
		vcam1.m_Priority = 10;
		vcam1.gameObject.transform.position = position;//new Vector3(0, 1, 0);

		// frame shot
		var composer = vcam1.AddCinemachineComponent<CinemachineFramingTransposer>();
		composer.m_ScreenY = screenYdefault;
		composer.m_DeadZoneWidth = 0.30f;
		composer.m_DeadZoneHeight = 0.35f;

		return cam;
	}

	public CinemachineTargetGroup getGroupCam()
	{
		CinemachineTargetGroup targetGroup = new GameObject("TargetGroup").AddComponent<CinemachineTargetGroup>();

		// targetGroup.m_Targets = new CinemachineTargetGroup.Target[];

		return targetGroup;
	}

	public CinemachineTrackedDolly getDolly()
	{
		CinemachineTrackedDolly dolly = new GameObject("Dolly").AddComponent<CinemachineTrackedDolly>();

		return dolly;
	}

	public CinemachineVirtualCamera tgroup()
	{
		CinemachineVirtualCamera vcamGroup = new GameObject("VirtualCamera").AddComponent<CinemachineVirtualCamera>();

		GameObject go = new GameObject();
		go.transform.position = Vector3.Lerp(targets[0].prefab.transform.position, targets[1].prefab.transform.position, 0.5f);
		Debug.Log("* " + go.transform.position.x + go.transform.position.y + go.transform.position.z);

		vcamGroup.m_Follow = go.transform;// prefab.transform;
		vcamGroup.m_Priority = 30;
		vcamGroup.m_Lens.FieldOfView = 20; // zoom
		// vcam1.m_SetActive = false;
		// vcamGroup.gameObject.SetActive = true;
		// vcam1.gameObject.transform.position = new Vector3(0, 1, 0);
		// vcamGroup.gameObject.transform.position = new Vector3(prefab.transform.position.x + 6f, prefab.transform.position.y + 1, prefab.transform.position.z - 6);

		// frame shot
		// var composer = vcamGroup.AddCinemachineComponent<CinemachineFramingTransposer>();
		// composer.m_ScreenY = 0.80f;
		// composer.m_DeadZoneWidth = 0.30f;
		// composer.m_DeadZoneHeight = 0.55f;
		// assign cam to targets
		GameObject targetGroupObject = new GameObject("TargetGroup");
		CinemachineTargetGroup camTargetGroup = targetGroupObject.AddComponent<CinemachineTargetGroup>();
        // camTargetGroup.SetActive(true);// = 50;
        List<CinemachineTargetGroup.Target> targets1 = new List<CinemachineTargetGroup.Target>();
        // Debug.Log(this.targets);//[0].prefab);
		targets1.Add(new CinemachineTargetGroup.Target { target = targets[0].prefab.transform, radius = 1f, weight = 1f });
		targets1.Add(new CinemachineTargetGroup.Target { target = targets[1].prefab.transform, radius = 1f, weight = 1f });
		camTargetGroup.m_Targets = targets1.ToArray();
		// camTargetGroup.m_RotationMode.GroupAverage;

		vcamGroup.gameObject.transform.position = new Vector3(2, 10, 6);

		vcamGroup.m_LookAt = camTargetGroup.transform;
        // vcam.m_Follow = camTargetGroup.transform;


		CinemachineGroupComposer composer = vcamGroup.AddCinemachineComponent<CinemachineGroupComposer>();
        composer.m_GroupFramingSize = 0.5f;//1;
        composer.m_FramingMode = CinemachineGroupComposer.FramingMode.Horizontal;//AndVertical;
		// composer.m_ScreenY = 5f;
        // composer.m_ScreenX = 2f;//0.5f;
		composer.m_AdjustmentMode = CinemachineGroupComposer.AdjustmentMode.DollyThenZoom;

		return vcamGroup;
	}
	public void EnableFilter(string name, bool b = true, bool flicker = false)
	{
		// if (b == true)
		// 	GameManager.instance.activeCameraFilter = name;

		IEnabler enabler = null;
		SoundVO soundFx = null;
		
		switch (name)
		{
			case "shield": 
				enabler = new FilterShieldWrapper(GameObject.FindWithTag("MainCamera").GetComponent<CameraFilterPack_3D_Shield>(), false) as FilterShieldWrapper;
				if (b == true)
					enabler.Enabler();//enabled = b; 
				else enabler.Disabler();
				break;
			case "matrix": 
				enabler = new FilterMatrixWrapper(GameObject.FindWithTag("MainCamera").GetComponent<CameraFilterPack_3D_Matrix>(), true);
				if (b == true)
					enabler.Enabler();
				else enabler.Disabler();
				soundFx = new SoundVO(200, "Matrix Glitch", SoundVO.ASSET_ACTIVATE_MATRIX_GLITCH);
				break;
		}

		if (b == true)
			GameManager.instance.activeCameraFilter = enabler;

		if (flicker == true) StartCoroutine(flickerFx(enabler, b, soundFx));
	}

	private IEnumerator flickerFx(IEnabler fx, bool b, SoundVO soundFx = null)
	{
		if (soundFx != null)
			audioSource.PlayOneShot(soundFx.audioClip, 0.95f);
		// b == false, flicker off
		// b == true, flicker on
		fx.Enabler();
		yield return new WaitForSeconds(0.05f);
		fx.Disabler();
		yield return new WaitForSeconds(0.15f);
		fx.Enabler();
		yield return new WaitForSeconds(0.5f);
		fx.Disabler();
		yield return new WaitForSeconds(0.05f);
		fx.Enabler();
		yield return new WaitForSeconds(0.05f);
		fx.Disabler();
	}

	// private 

}