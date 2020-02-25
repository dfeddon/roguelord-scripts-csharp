using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
// using Unity.Entities;

// public class Crust : MonoBehaviour
// {
// 	public Vector3 position;
// }

namespace TGS
{
	public class SprawlController : MonoBehaviour
	{
		private const string PREFAB_BUILDING_STADIUM = "Building_Stadium";
		private const string PREFAB_ARROW_30 = "arrow30";
		// private const string PREFAB_STRUCTURE_HQ = "HQ";

		TerrainGridSystem tgs;
		GUIStyle labelStyle, labelStyleShadow, buttonStyle, sliderStyle, sliderThumbStyle;
		float terrainSteepness = 2;
		float scaler = 0.3f;
		GameObject ball, ballParent;
		int ballCount;
		public Button startCombatButton;
		public Button actionButton;
		Roguelord.CameraController cam;
		private GameObject spinner;
		private Vector3 currentSelectedPosition;
		private List<TerritoryVO> gridData;
		public GameObject[] buildings;
		public GameObject revealer;
		public Terrain terrain;
		public GameObject gameManager;
		private AssetBundle assets;

		void Awake()
		{
			Debug.Log("== SprawlController.Awake ==");
			// spinner = GetComponent<UrbanCar00>();
			if (GameManager.instance == null)
				Instantiate(gameManager);
			
			// add ref to singelton
			GameManager.instance.sprawlController = this;
		}

		void Start()
		{
			// load assets
			assets = GameManager.instance.LoadAssetsBundle("sprawl");

			tgs = TerrainGridSystem.instance;

			// ball = Resources.Load<GameObject>("Ball");
			// ballParent = new GameObject("BallParent");
			// ballParent.transform.position = tgs.terrainCenter + Vector3.up * 200.0f;

			tgs.OnCellClick += OnCellClickHandler;

			// ResetTerrain();

			BuildMap();

			// SpawnBall();

			cam = GameObject.Find("camera_001").GetComponent<Roguelord.CameraController>();
			Debug.Log("* cam " + cam);

			// UI listeners
			startCombatButton.onClick.AddListener(() =>
			{
				StartCombatHandler();
			});

			// actionButton.onClick.AddListener(() =>
			// {
			// 	ActionButtonHandler();
			// 	EventManager.TriggerEvent("cellClickEvent");
			// });
		}

		void BuildMap()
		{
			Debug.Log("== BuildMap ==");
			// Bounds bounds = this.tgs;//.renderer.bounds;
			// Terrain terrain = GetComponent(Terrain);
			Vector3 terrainSize = terrain.terrainData.size;
			Debug.Log("* terrainSize " + terrainSize);

			gridData = new List<TerritoryVO>();
			// GameObject[] buildings;
			int mapWidth = 42;//tgs.columnCount;
			int mapHeight = 42;//tgs.rowCount;
			float buildingFootprint = 1f;//3;

			float seed = 42;
			seed = Random.Range(0, 100);
			float div = 8.0f;
			int iter = 0;
			GameObject obj;

			// assets
			GameObject building1 = assets.LoadAsset<GameObject>("building1");
			GameObject building2 = assets.LoadAsset<GameObject>("building2");
			GameObject building3 = assets.LoadAsset<GameObject>("building3");
			GameObject building4 = assets.LoadAsset<GameObject>("building4");
			GameObject building5 = assets.LoadAsset<GameObject>("building5");
			GameObject grass = assets.LoadAsset<GameObject>("Grass");

			// for (int h = 0; h < mapHeight; h++)
			for (int h = 0; h < terrainSize.x; h++)
			{
				// Debug.Log("H " + h);
				// for (int w = 0; w < mapWidth; w++)
				for (int w = 0; w < terrainSize.z; w++)
				{
					// Debug.Log("w " + w);
					int result = (int)(Mathf.PerlinNoise(w / div + seed, h / div + seed) * 10);
					Vector3 pos = new Vector3(w * buildingFootprint, 1f, h * buildingFootprint);

					if (result < 2)
					{
						// obj = Instantiate(buildings[0], pos, Quaternion.Euler(0, 0, 0));
						obj = Instantiate(building1, pos, Quaternion.Euler(0, 0, 0));
						obj.transform.localScale = Vector3.one * scaler;
					}
					else if (result < 4)
					{
						obj = Instantiate(building2, pos, Quaternion.Euler(0, 0, 0));
						obj.transform.localScale = Vector3.one * scaler;
					}
					else if (result < 5)
					{
						obj = Instantiate(building3, pos, Quaternion.Euler(0, 0, 0));
						obj.transform.localScale = Vector3.one * scaler;
					}
					else if (result < 6)
					{
						obj = Instantiate(building4, pos, Quaternion.Euler(0, 0, 0));
						obj.transform.localScale = Vector3.one * scaler;
					}
					else if (result < 7)
					{
						obj = Instantiate(building5, pos, Quaternion.Euler(0, 0, 0));
						obj.transform.localScale = Vector3.one * scaler;
					}
					else if (result < 10)
					{
						// set grass *beneath* grid system
						pos.y = 0f;
						obj = Instantiate(grass, pos, Quaternion.identity);
						// obj.transform.localScale = Vector3.one * scaler; //new Vector3(scale, scale, scale);
					}
				}
			}



			// for (int h = 0; h < mapHeight; h++)
			// {
			// 	for (int w = 0; w < mapWidth; w++)
			// 	{
			// 		int result = (int) (Mathf.PerlinNoise(w / div + seed, h / div + seed) * 10);
			// 		// Vector3 pos = new Vector3(w * buildingFootprint, 0, h * buildingFootprint);

			// 		/*switch(result) 
			// 		{
			// 			case 0: 
			// 			case 1: Debug.Log(result); break;
			// 			case 2: 
			// 			case 3: Debug.Log(result); break;
			// 			case 4: 
			// 			case 5: Debug.Log(result); break;
			// 			case 6: 
			// 			case 7: Debug.Log(result); break;
			// 			case 8: 
			// 			case 9: Debug.Log(result); break;
			// 		}
			// 		Debug.Log("iter " + iter);*/
			// 		// Debug.Log(result);
			// 		iter++;
			// 		gridData.Add(new TerritoryVO(iter * 2, iter, 1, result));
			// 	}
			// }

			// for (var i = 0; i < 64; i++)
			// {
			// 	gridData.Add(new TerritoryVO(i * 2, i, 1, Random.Range(1, 3)));
			// }
			// // Debug.Log(gridData);

			// for (var j = 0; j < gridData.Count; j++)
			// {
			// 	GameObject prefab = GameObject.Find(TerritoryVO.GetStructureByType(gridData[j].type));
			// 	// AddPrefabToMap(prefab, tgs.CellGetPosition(j), Quaternion.identity);
			// }
		}

		void StartCombatHandler()
		{
			Debug.Log("* start combat...");
			StartCoroutine(LoadNewScene());
		}

		void ActionButtonHandler()
		{
			Debug.Log("* actionButtonHandler");
			GameObject prefab = Resources.Load("Prefabs/Drone") as GameObject;
			// AddDroneToMap(prefab, currentSelectedPosition, Quaternion.identity);
		}

		IEnumerator LoadNewScene()
		{
			AsyncOperation async = SceneManager.LoadSceneAsync("CombatScene", LoadSceneMode.Single);
			while (!async.isDone)
			{
				Debug.Log("loading...");
				yield return null;
			}
		}

		void OnCellClickHandler(int cellIndex, int buttonIndex)
		{
			Debug.Log("Clicked on cell #" + cellIndex + " with button " + buttonIndex);
			Debug.Log("rows " + tgs.rowCount + "cols " + tgs.columnCount + " / total cells " + tgs.numCells);
			Debug.Log("position " + tgs.CellGetPosition(cellIndex));

			currentSelectedPosition = tgs.CellGetPosition(cellIndex);
			GameManager.instance.currentSelectedIndex = cellIndex;

			// EventManager.TriggerEvent("cellClickEvent", new EventParam(cellIndex));
			EventParam eventParam = new EventParam();
			eventParam.data = "hi";
			EventManager.TriggerEvent("cellClickEvent", eventParam);

			// GameObject prefab = GameObject.Find(PREFAB_ARROW_30);
			// GameObject prefab = Resources.Load("Prefabs/arrow30") as GameObject;
			// AddArrowPrefabToMap(prefab, tgs.CellGetPosition(cellIndex), Quaternion.identity);
			// this.revealer.transform.position = tgs.CellGetPosition(cellIndex);
			//.position.z += 1;
			// GameObject GameTerrain = GameObject.Find("Terrain");
			// Vector3 dimen = GameObject.Find("Terrain").GetComponent<Terrain>().terrainData.bounds.size;

			/*Debug.Log("/" + dimen.x + " " + dimen.y + " " + dimen.z);
			Debug.Log("##" + dimen.x / tgs.rowCount);
			float gridWidth = dimen.x;
			int totalIndecies = (tgs.rowCount * tgs.columnCount) - 1;
			Vector2 worldDimen = new Vector2(x:gridWidth * tgs.rowCount, y: gridWidth * tgs.rowCount);
			float gridX = (cellIndex + 1) / tgs.rowCount;
			// float gridY = gridWidth / 
			Debug.Log("* gridX " + gridX);*/
			// Vector2 tileClicked = new Vector2(x: cellIndex)

			cam.doMove(tgs.CellGetPosition(cellIndex));//Camera.main.transform.position);
		}

		/*void AddPrefabToMap(GameObject prefab, Vector3 pos, Quaternion rot)
		{
			// float scale = 0.1f;
			GameObject inst = Instantiate(prefab, pos, rot);
			// inst.transform.localScale = new Vector3(scale, scale, scale);
			// GameObject myComponent = inst.GetComponent<ArrowObject>();
		}*/
		void AddArrowPrefabToMap(GameObject prefab, Vector3 pos, Quaternion rot)
		{
			// pos.z = 6;
			float scale = 6f;
			pos.y = scale / 2;//3;
			Debug.Log("* adding arrow prefab " + prefab + "/" + pos + "/" + rot);
			GameObject inst = Instantiate(prefab, pos, rot);
			inst.transform.localScale = Vector3.one * scaler;
			ArrowObject myComponent = inst.GetComponent<ArrowObject>();
			inst.transform.localScale = new Vector3(scale, scale, scale);
			myComponent.doSpin();
			// inst.SetActive(true);
		}

		public void AddDroneToMap()
		{
			GameObject prefab = assets.LoadAsset<GameObject>("Drone");
			// GameObject prefab = Resources.Load("Prefabs/Drone") as GameObject;
			// AddDroneToMap(prefab, currentSelectedPosition, Quaternion.identity);
			// pos.z = 6;
			Vector3 pos = currentSelectedPosition;
			Quaternion rot = Quaternion.identity;
			// float scale = 6f;
			pos.y = 15;//scale / 2;//3;
			Debug.Log("* adding drone prefab " + prefab + "/" + pos + "/" + rot);
			GameObject inst = Instantiate(prefab, pos, rot);
			inst.transform.localScale = Vector3.one * scaler;
			DroneObject myComponent = inst.GetComponent<DroneObject>();
			myComponent.setNexus(pos);
			FogOfWarEntity script = inst.GetComponent<FogOfWarEntity>();
			Debug.Log("* fog " + script);
			Debug.Log("* vision range " + script.visionRange);
			//
			// inst.transform.localScale = new Vector3(scale, scale, scale);
			// myComponent.doSpin();
			// inst.SetActive(true);
		}

		public void AddLookoutToMap()
		{
			GameObject prefab = assets.LoadAsset<GameObject>("Revealer");
			// GameObject prefab = Resources.Load("Prefabs/Revealer") as GameObject;
			GameObject inst = Instantiate(prefab, currentSelectedPosition, Quaternion.identity);
			inst.transform.localScale = Vector3.one * scaler;
			LookoutView myComponent = inst.GetComponent<LookoutView>();
			myComponent.pos = currentSelectedPosition;
			myComponent.doSpin();
			// myComponent.drawPin(currentSelectedPosition);
			// myComponent.setLine();
			FogOfWarEntity script = inst.GetComponent<FogOfWarEntity>();
			Debug.Log("* vision range " + script.visionRange);
			// script.visionRange = 20;
		}
		/*void OnGUI()
		{
			// Do autoresizing of GUI layer	
			GUIResizer.AutoResize();

			GUI.backgroundColor = new Color(0.8f, 0.8f, 1f, 0.5f);

			GUI.Label(new Rect(10, 10, 160, 30), "Steepness", labelStyle);
			terrainSteepness = GUI.HorizontalSlider(new Rect(80, 25, 100, 30), terrainSteepness, 0, 10, sliderStyle, sliderThumbStyle);

			if (GUI.Button(new Rect(10, 70, 160, 30), "Randomize Terrain", buttonStyle))
			{
				RandomizeTerrain(0.75f);
			}

		}*/

		// private void FixedUpdate()
		// {
		// 	Debug.Log("*hi");
		// 	spinner.velocity = transform.forward * 10.0f;
		// }

		void Update()
		{
			// Camera.main.transform.RotateAround(tgs.terrainCenter, Vector3.up, Time.deltaTime * 2.0f);

			// if (ballCount < 12 && Random.value > 0.98f)
			// 	SpawnBall();

			// if (Time.time > 3 && Time.time < 6)
			// {
			// 	tgs.cellBorderAlpha = (Time.time - 3) / 8.0f;
			// }
		}

		// void ResetTerrain()
		// {
		// 	RandomizeTerrain(0);
		// 	while (ballParent.transform.childCount > 0)
		// 	{
		// 		Destroy(ballParent.transform.GetChild(0));
		// 	}

		// 	tgs.cellBorderAlpha = 0;
		// }

		/*void RandomizeTerrain(float strength)
		{
			tgs.terrain.heightmapMaximumLOD = 0;    // always show maximum detail

			int w = tgs.terrain.terrainData.heightmapWidth;
			int h = tgs.terrain.terrainData.heightmapHeight;
			float[,] heights = tgs.terrain.terrainData.GetHeights(0, 0, w, h);

			float z = Time.time;
			for (int k = 0; k < w; k++)
			{
				for (int j = 0; j < h; j++)
				{
					heights[k, j] = Mathf.PerlinNoise((float)((z + k) * terrainSteepness % w) / w, (float)((z + j) * terrainSteepness % h) / h) * strength;
				}
			}

			// Add a few towers
			int maxRadius = 20;
			for (int k = 0; k < 3; k++)
			{
				int x = Random.Range(maxRadius, w - maxRadius);
				int y = Random.Range(maxRadius, h - maxRadius);
				for (int r = 0; r < maxRadius; r++)
				{
					for (int a = 0; a < 360; a++)
					{
						int c = (int)(x + Mathf.Cos(a * Mathf.Deg2Rad) * r);
						int l = (int)(y + Mathf.Sin(a * Mathf.Deg2Rad) * r);
						heights[c, l] = 1;
					}
				}

			}

			tgs.terrain.terrainData.SetHeights(0, 0, heights);
			tgs.GenerateMap();

			// Show center of cells
			//			GameObject centroidParent = GameObject.Find("Centroids");
			//			if (centroidParent!=null) GameObject.Destroy(centroidParent);
			//			centroidParent = new GameObject("Centroids");
			//			for (int k=0;k<tgs.cells.Count;k++) {
			//				Vector3 center = tgs.CellGetPosition(k);
			//				GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			//				obj.transform.position = center;
			//				obj.transform.localScale = Misc.Vector3one * 5f;
			//				obj.transform.SetParent(centroidParent.transform, true);
			//				obj.GetComponent<Renderer>().sharedMaterial.color = Color.black;
			//			}
		}*/

		/// <summary>
		/// Instantiates the ball - the ball is controlled by the script "BallController" which orient the ball toward current selected cell and highlight cell beneath the ball
		/// </summary>
		void SpawnBall()
		{
			ballCount++;
			GameObject newBall = Instantiate(ball);
			newBall.transform.SetParent(ballParent.transform, false);
			newBall.transform.localPosition = Misc.Vector3zero;
			newBall.GetComponent<Rigidbody>().AddForce(Random.onUnitSphere * 20);
		}
	}

}