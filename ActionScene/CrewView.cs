using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Cinemachine;

public class CrewView : MonoBehaviour {

	// private const string CHARACTER_STEAMHERO1_ANIMATOR_CONTROLLER = "SteamHero1Controller";
    // private const string CHARACTER_DAGGERMASTER_ANIMATOR_CONTROLLER = "DaggerMasterController";
	// private const string CHARACTER_ANIMATION_WALK = "Walk";
    // private const string CHARACTER_ANIMATION_IDLE = "Idle";
    // private const string CHARACTER_ANIMATION_HIT = "Hit";
    public bool isDefendingCrew;
    private GameObject idleWindow;
    // private GameObject trashcan;
	private GameObject crewGroup;
    public List<CharacterView> pool = new List<CharacterView>();
    // public List<CharacterView> scriptPool = new List<CharacterView>();
	// private GameObject playerPrefab;
	private int lastStateKey = 0;
    private List<CharacterVO> crewData;
    private float baseY;
    private float adjX;
	// public GameObject gameManager;
	public AssetBundle assetBundle;

	public int stateKey = 0; // 0 = stop, 1 = right, 2 = left

    void Awake()
    {
		Debug.Log("<color=yellow>== CrewView.Awake ==</color>");
		if (this.isDefendingCrew)
		{
			crewGroup = GameObject.FindWithTag("DefendGroup");
			// crewGroup.transform.position += temp;
		}
		else
		{
			crewGroup = GameObject.FindWithTag("AttackGroup");
			// crewGroup.transform.position -= temp;
		}
		// Debug.Log("* crewgroup " + crewGroup);
	}

    void Start () {
        Debug.Log("<color=yellow>== CrewView.Start ==</color>");
        // Debug.Log(GameManager.instance);
		// if (GameManager.instance == null)
		// 	Instantiate(gameManager);
		// assetBundle = GameManager.instance.LoadAssetsBundle("combat");

		idleWindow = GameObject.FindWithTag("IdleWindow");
        Vector3 temp = new Vector3(307.0f, 0, 0);
        // find parent
        // crewGroup.transform.localScale = new Vector3(0.5f, 0.5f, 0);
    }

	// Update is called once per frame
	// void Update () {

		// moving
        // if (this.stateKey > 0)
        // {
        //     switch(this.stateKey) 
        //     {
        //         case 1: // moving right
        //             crewGroup.transform.Translate(Time.deltaTime, 0, 0);
        //         break;
        //         case 2: // moving left
        //             crewGroup.transform.Translate(-Time.deltaTime, 0, 0);
        //         break;
        //     }
        // }
	// }

    public List<Vector3> GetPositions()
    {
        List<Vector3> list = new List<Vector3>();
        pool = pool.OrderBy(x=>x.model.position).ToList();

		foreach(CharacterView c in pool)
        {
            Debug.Log("<color=orange># " + c.model.position + "</color>");
            list.Add(c.prefab.transform.position);
        }

        return list;
    }

    public void addCharactersToCrew(List<CharacterVO> crew, GameObject[] models)
    {
        Debug.Log("<color=yellow>== CrewView.addCharactersToCrew ==</color>");
        this.crewData = crew;
        GameObject go;
        CharacterView characterView;
		// Entity e;
        float baseY = 0f;
		// define space between each character
		// int totalUnits = crew.Count;
		// float screenWidthHalf = (float)Screen.width / 2;
		// float val = (float)screenWidthHalf / (totalUnits + 1) / 100;
		float adjX = 1.0f;//25f;
        // adjX = (float)adjX * val; //adj / scale
        // Debug.Log("<color=yellow>adjX " + val + " / " + adjX + "</color>");
        Animator animator;
        int i = 0;
		foreach(CharacterVO vo in crew)
        {
            // string ut = JsonUtility.ToJson(vo, true);
            // Debug.Log(ut);

			// string characterPrefab = CharacterVO.getPrefabFromRole(vo.role);
            string animationController = CharacterVO.getAnimatorFromRole(vo.role);

            Debug.Log("== prefab " + models[vo.role]);
            // prefab
			// GameObject prefab = assetBundle.LoadAsset<GameObject>(characterPrefab);
            GameObject prefab = models[vo.role];

            // position
			Vector3 pos = new Vector3(0f, 0f, 0f);

            // rotation (based on role: attack/defend)
            float rotY = 90;
            if (this.isDefendingCrew) rotY = -90;
			Quaternion rot = Quaternion.Euler(0, rotY, 0);//Quaternion.identity;

            // CharacterView should return GameObject
			go = Instantiate(prefab, pos, rot);

            // scale to fit screen
            // go.transform.localScale = new Vector3(val, val, 1);
            go.transform.localScale = Vector3.one * vo.viewScale;

			// set animator and animatorcontroller
			// animator = go.GetComponent<Animator>();
			// animator.runtimeAnimatorController = assetBundle.LoadAsset<RuntimeAnimatorController>(animationController);
			// Debug.Log("animator " + animator + "/" + animator.runtimeAnimatorController + "/" + animationController);

			// assign reference name (according to position)
			if (this.isDefendingCrew == true) 
                go.name = "defender" + vo.position.ToString();
            else go.name = "attacker" + vo.position.ToString();

            // add to List pool
            // this.pool.Add(go);
            // Debug.Log("pool len " + this.pool.Count);

            // add to parent group
            go.transform.SetParent(this.crewGroup.transform, false);

            // get registration point
            float pointX = this.crewGroup.transform.position.x;

            // assign by position
            if (this.isDefendingCrew == false)
            {
                go.transform.position = new Vector3(pointX - (adjX * (vo.position)), baseY, 0);
                // Debug.Log("++++++++++++ " + go.transform.position.x);
            }
            else 
            {
                go.transform.position = new Vector3(pointX + (adjX * (vo.position)), baseY, 0);
            }
            // animator.SetTrigger("idle");

            // component code access
            //*
            characterView = new GameObject("CrewCharView").AddComponent<CharacterView>();
            // characterView = go.AddComponent<CharacterView>();
            characterView.prefab = go;
            // Debug.Log("cview: " + characterView + " / " + vo);
            // Debug.Log("cmodel " + characterView.model);
            // assign character model to character view
            characterView.model = vo;
            // name it
            if (this.isDefendingCrew == true)
                characterView.name = "defender" + vo.position.ToString();
            else characterView.name = "attacker" + vo.position.ToString();
            // add to List pool
            this.pool.Add(characterView);
			// GameObjectHelper.setSize(this, 300, 300);
            //*/
            i++;
        }
    }

    public CharacterView getCharacterById(int id)
    {
        CharacterView returnValue = null;
		foreach (CharacterView cv in pool)
        {
            if (cv.model.uid == id)
                return cv;
        }

        return returnValue;
    }

    // animation
    public void changeAnimation(int state) {
		Debug.Log("change animation " + state.ToString());
        // Debug.Log(this.attacker1);
        switch(state) {
			case 0: // idle
                foreach(CharacterView view in this.pool)
                {
                    // view.GetComponent<Animator>().Play(CharacterView.CHARACTER_ANIMATION_IDLE);
                    view.changeAnimation(CharacterView.CHARACTER_ANIMATION_IDLE);
                }
                break;

			case 1: // walk right
				if (lastStateKey == 2 && state == 1)
					Debug.Log("flip!");
                foreach (CharacterView view in this.pool)
                {
                    view.prefab.GetComponent<Animator>().Play(CharacterView.CHARACTER_ANIMATION_WALK);
                }
                break;

			case 2: // walk left
                if (lastStateKey == 1 && state == 2)
                    Debug.Log("flip!");
                foreach (CharacterView go in this.pool)
                {
                    go.prefab.GetComponent<Animator>().Play(CharacterView.CHARACTER_ANIMATION_WALK);
                }
                break;

			case 3: // hit
				// this.attacker1.GetComponent<Animator>().Play(CHARACTER_ANIMATION_HIT);
                // GameObject target = ListHelper.getGameObjectByName(this.pool, "attacker1");
                // target.GetComponent<Animator>().Play(CharacterView.CHARACTER_ANIMATION_HIT);
			break;
		}
		if (state != 0)
			lastStateKey = state;
	}

    // public CharacterView getCharacterViewById(int id)
    // {
    //     CharacterView returnValue;
    //     Debug.Log("== CrewView.getCharacterViewById ==");
    //     Debug.Log(id);
    //     // returnValue = ListHelper.getCharacterViewById(id);
    //     return returnValue;
    // }
}
