///////////////////////////////////
/// Create and edit by QerO
/// 09.2018
/// lidan-357@mail.ru
///////////////////////////////////

using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Missions
{
    public class Missions_MapGenerator : MonoBehaviour
    {

        public GameObject player_prefab;
        public bool spawn_player;

        public float map_generate_speed; //Pause between creating different locations and objects

        public GameObject[] corridors_pool;
        public int locations_need_spawn_pool; //How many places to create locations need to be supported

        [SerializeField]
        public Location[] locations_pool;

        [System.Serializable]
        public class Location
        {
            [SerializeField]
            public GameObject prefab;
            [SerializeField]
            public int min;
            [SerializeField]
            public int max;
        }

        [SerializeField]
        public Item[] items_pool;

        [System.Serializable]
        public class Item
        {
            [SerializeField]
            public GameObject prefab;
            [SerializeField]
            public int min;
            [SerializeField]
            public int max;
        }

        public static Vector2 map_center; //For shift of center of minimap
        private Scene scene;
        private NavMeshSurface selfnavsuf;
        private Transform locations_parent;
		public GameObject gameManager;
		private AssetBundle assets;
		private GameObject wallMats;// = assets.LoadAsset<GameObject>("WallMats");
		private MeshRenderer wallMatsRend;// = wallMats.GetComponent<MeshRenderer>();

		void Awake()
		{
			Debug.Log("== SprawlController.Awake ==");
			// spinner = GetComponent<UrbanCar00>();
		}

        void Start()
        {
            Debug.Log("== Missions_MapGenerator.Start ==");
			if (GameManager.instance == null)
				Instantiate(gameManager);

			// add ref to singelton
			GameManager.instance.missions_MapGenerator = this;

			// load assets and materials
			assets = GameManager.instance.LoadAssetsBundle("missions");
			wallMats = assets.LoadAsset<GameObject>("WallMats");
			wallMatsRend = wallMats.GetComponent<MeshRenderer>();

			// get scene
			scene = SceneManager.GetActiveScene();
            // get start room
            GameObject startRoom = GetGameObjectByName("Start Room");
            Debug.Log("Start Room = " + startRoom);
            AddMaterialsToLocation(startRoom);
			// spawn rooms, corridors and objects
			StartCoroutine(Map_Spawn());
            // get nav mesh surface
            selfnavsuf = gameObject.GetComponent<NavMeshSurface>();
            // locations holder
            locations_parent = new GameObject("Locations Parent").transform;
        }

        GameObject GetGameObjectByName(string name)
        {
			GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
			foreach (GameObject go in allObjects)
			{
				// Debug.Log("go " + go.name);
                if (go.name == name)
                    return go;
			}
            return null;
		}

        void AddMaterialToMesh(MeshRenderer mesh, int index)
        {
			// MeshRenderer myRend = currentItem.GetComponent<MeshRenderer>();
			Material[] materials = mesh.materials;
			mesh.material = wallMatsRend.sharedMaterials[index];
		}

        void AddMaterialsToLocation(GameObject loc)
        {
			// iterate room's children
			for (int i = 0; i < loc.transform.childCount; ++i)
			{
				Transform currentItem = loc.transform.GetChild(i);

				string objLayer = LayerMask.LayerToName(currentItem.gameObject.layer);
				string objName = currentItem.gameObject.name;

				// add wall, ceiling and floor materials
				if (objLayer == "Wall")
				{
					AddMaterialToMesh(currentItem.GetComponent<MeshRenderer>(), 1);
				}
				else if (objName == "Floor" || objName == "Floor1" || objName == "Floor2" || objName == "Floor3" || objName == "Plane" || objName == "Ceiling") // TODO <- convert Plane to Ceiling
				{
					AddMaterialToMesh(currentItem.GetComponent<MeshRenderer>(), 2);
				}
			}
		}

        IEnumerator Map_Spawn()
        {

            Debug.Log("MAP GENERATE START!");

            yield return new WaitForSeconds(map_generate_speed); //A short pause for other scripts

            ////////////////////////////////////////////////
            //How many locations and objects will be created?
            ////////////////////////////////////////////////
            int loc_spawn_count = 0;
            int obj_spawn_count = 0;

            for (int i = 0; i < items_pool.Length; i++)
            {
                items_pool[i].max = Random.Range(items_pool[i].min, items_pool[i].max + 1);
            }

            for (int l = 0; l < locations_pool.Length; l++)
            {
                locations_pool[l].max = Random.Range(locations_pool[l].min, locations_pool[l].max + 1);
            }

            for (int a = 0; a < locations_pool.Length; a++)
            {
                loc_spawn_count += locations_pool[a].max;
            }

            for (int b = 0; b < items_pool.Length; b++)
            {
                obj_spawn_count += items_pool[b].max;
            }

            Debug.Log("* " + loc_spawn_count + " / " + obj_spawn_count);
            // Debug.Log(Missions.Missions_PlayerUI);
            Missions.Missions_PlayerUI.Load_Bar_Max(loc_spawn_count + obj_spawn_count); //Set the value for the load bar


            ////////////////////////////////////////////////
            //Process of creation
            ////////////////////////////////////////////////
            for (int loc_pos = 0; loc_pos < locations_pool.Length; loc_pos++)
            {
                for (int loc_count = 0; loc_count < locations_pool[loc_pos].max; loc_count++)
                {
                    Debug.Log("Trying to create location " + locations_pool[loc_pos].prefab);
                    for (bool have_spawn_places = false; have_spawn_places == false;)
                    {
                        Debug.Log("have_spawn_places == false");
                        Debug.Log("tag: " + locations_pool[loc_pos].prefab.GetComponent<Missions.Missions_LocationSettings>().spawn_place_type);
                        GameObject[] loc_spawns = GameObject.FindGameObjectsWithTag(locations_pool[loc_pos].prefab.GetComponent<Missions.Missions_LocationSettings>().spawn_place_type);
                        Debug.Log("Found the places for create location : " + loc_spawns.Length);

                        if (loc_spawns.Length < locations_need_spawn_pool)
                        {
                            Debug.Log("Too few corridors, create corridors...");

                            Corridors_Create();
                            
                            yield return new WaitForSeconds(map_generate_speed);
                        }
                        else // adding room
                        {
                            int random_spawn_loc = Random.Range(0, loc_spawns.Length);
                            map_center += new Vector2(loc_spawns[random_spawn_loc].transform.position.x, loc_spawns[random_spawn_loc].transform.position.z);
                            GameObject new_location = Instantiate(locations_pool[loc_pos].prefab, loc_spawns[random_spawn_loc].transform.position, loc_spawns[random_spawn_loc].transform.rotation);
                            new_location.transform.SetParent(locations_parent);
                            have_spawn_places = true;

                            AddMaterialsToLocation(new_location);

							yield return new WaitForSeconds(map_generate_speed);
                        }
                    }
                    Missions.Missions_PlayerUI.Load_Bar_Progress();
                }
            }

            Debug.Log("MAP GENERATED COMPLETE!");

            yield return new WaitForSeconds(map_generate_speed);

            selfnavsuf.BuildNavMesh();

            yield return new WaitForSeconds(map_generate_speed);

            Debug.Log("GENERATE OBJECTS!");

            GameObject[] spawn_points = GameObject.FindGameObjectsWithTag("ObjectSpawn");
            for (int r = 0; r < spawn_points.Length; r++)
            {
                int random_index = Random.Range(0, spawn_points.Length);
                GameObject temp = spawn_points[random_index];
                spawn_points[random_index] = spawn_points[r];
                spawn_points[r] = temp;
            }

            Debug.Log("Found " + spawn_points.Length + " places for objects spawn");

            for (int object_pos = 0; object_pos < items_pool.Length; object_pos++)
            {
                Debug.Log("* obj " + items_pool[object_pos].prefab);
                string spawn_tag = items_pool[object_pos].prefab.GetComponent<Missions.Missions_ObjectsSpawnSettings>().object_spawn_type;

                Debug.Log("Spawn object: " + items_pool[object_pos].prefab + " Maximum: " + items_pool[object_pos].max);
                Debug.Log("Type of object : " + spawn_tag);

                for (int object_count = 0; object_count < items_pool[object_pos].max; object_count++)
                {
                    for (int spawn_pos = 0; spawn_pos < spawn_points.Length; spawn_pos++)
                    {
                        if (spawn_points[spawn_pos] != null)
                        {
                            Debug.Log("Found place for object spawn : " + spawn_points[spawn_pos]);
                            Debug.Log("Create instance # " + (object_count + 1));
                            Debug.Log("Tag " + spawn_tag);
                            Debug.Log("* " + spawn_points[spawn_pos]);
							Debug.Log("* " + spawn_points[spawn_pos].GetComponent<Missions.Missions_ObjectsSpawnSettings>());
							if (spawn_points[spawn_pos].GetComponent<Missions.Missions_ObjectsSpawnSettings>().object_spawn_type == spawn_tag)
                            {
                                GameObject created_object = Instantiate(items_pool[object_pos].prefab, spawn_points[spawn_pos].transform.position, spawn_points[spawn_pos].transform.rotation);
                                created_object.transform.SetParent(spawn_points[spawn_pos].transform.parent);
                                Destroy(spawn_points[spawn_pos]);
                                spawn_points[spawn_pos] = null;
                                yield return new WaitForSeconds(map_generate_speed);
                                spawn_pos = spawn_points.Length;
                            }
                        }
                    }
                    Missions.Missions_PlayerUI.Load_Bar_Progress();
                }
            }

            ////////////////////////////////////////////////
            //Map clean
            ////////////////////////////////////////////////

            GameObject[] objects_clean = GameObject.FindGameObjectsWithTag("MapGenerated_Clean");
            if (objects_clean.Length != 0)
            {
                for (int x = 0; x < objects_clean.Length; x++)
                {
                    Destroy(objects_clean[x]);
                }
            }

            for (int clean = 0; clean < spawn_points.Length; clean++)
            {
                if (spawn_points[clean] != null)
                {
                    Destroy(spawn_points[clean]);
                }
            }

            ////////////////////////////////////////////////
            ////////////////////////////////////////////////
            ////////////////////////////////////////////////


            Debug.Log("ALL OBJECTS COMPLETE!");

			Debug.Log("locations_parent " + locations_parent.transform.childCount);

			map_center.x /= (loc_spawn_count + obj_spawn_count);
            map_center.y /= (loc_spawn_count + obj_spawn_count);

            if (spawn_player == true)
            {
                Instantiate(player_prefab);
            }
            Missions.Missions_PlayerUI.Player_UI_GameStart();
        }

        void Corridors_Create()
        {
            int scene_reload = 0;
            GameObject corridor = corridors_pool[Random.Range(0, corridors_pool.Length)];
            GameObject[] mg_corridor_spawns = GameObject.FindGameObjectsWithTag(corridor.GetComponent<Missions.Missions_LocationSettings>().spawn_place_type);
            if (mg_corridor_spawns.Length != 0)
            {
                int random_spawn_cor = Random.Range(0, mg_corridor_spawns.Length);
                map_center += new Vector2(mg_corridor_spawns[random_spawn_cor].transform.position.x, mg_corridor_spawns[random_spawn_cor].transform.position.z);
                GameObject new_corridor = Instantiate(corridor, mg_corridor_spawns[random_spawn_cor].transform.position, mg_corridor_spawns[random_spawn_cor].transform.rotation);
                new_corridor.transform.SetParent(locations_parent);
                Debug.Log("The corridor is ready");
                AddMaterialsToLocation(new_corridor);
            }
            else
            {
                scene_reload++;
                if (scene_reload < 20)
                {
                    Debug.Log("No place found, try again");
                }
                else
                {
                    Debug.Log("ERROR! RELOAD SCENE!"); //It is very rare, but it happens that the map is looped
                    SceneManager.LoadScene(scene.name);
                }
            }
        }

        ////////////////////////////////////////////////
        //For convenient display of parameters in the editor
        ////////////////////////////////////////////////

#if UNITY_EDITOR
        [CustomPropertyDrawer(typeof(Location))]
        public class LocationPropertyDrawer : PropertyDrawer
        {

            private const float space = 5;

            public override void OnGUI(Rect rect,
                                       SerializedProperty property,
                                       GUIContent label)
            {
                int indent = EditorGUI.indentLevel;
                EditorGUI.indentLevel = 0;

                var firstLineRect = new Rect(
                    x: rect.x,
                    y: rect.y,
                    width: rect.width,
                    height: EditorGUIUtility.singleLineHeight
                );
                DrawMainProperties(firstLineRect, property);

                EditorGUI.indentLevel = indent;
            }

            private void DrawMainProperties(Rect rect,
                                            SerializedProperty Locations)
            {
                rect.width = (rect.width + 35 * space) / 3;
                DrawProperty(rect, Locations.FindPropertyRelative("prefab"));
                rect.x += rect.width + space;
                rect.width = rect.width / 3;
                DrawProperty(rect, Locations.FindPropertyRelative("min"));
                rect.x += rect.width + space;
                DrawProperty(rect, Locations.FindPropertyRelative("max"));
            }

            private void DrawProperty(Rect rect,
                                      SerializedProperty property)
            {
                EditorGUI.PropertyField(rect, property, GUIContent.none);
            }
        }

        [CustomPropertyDrawer(typeof(Item))]
        public class ItemPropertyDrawer : PropertyDrawer
        {

            private const float space = 5;

            public override void OnGUI(Rect rect,
                                       SerializedProperty property,
                                       GUIContent label)
            {
                int indent = EditorGUI.indentLevel;
                EditorGUI.indentLevel = 0;

                var firstLineRect = new Rect(
                    x: rect.x,
                    y: rect.y,
                    width: rect.width,
                    height: EditorGUIUtility.singleLineHeight
                );
                DrawMainProperties(firstLineRect, property);

                EditorGUI.indentLevel = indent;
            }

            private void DrawMainProperties(Rect rect,
                                            SerializedProperty Items)
            {
                rect.width = (rect.width + 35 * space) / 3;
                DrawProperty(rect, Items.FindPropertyRelative("prefab"));
                rect.x += rect.width + space;
                rect.width = rect.width / 3;
                DrawProperty(rect, Items.FindPropertyRelative("min"));
                rect.x += rect.width + space;
                DrawProperty(rect, Items.FindPropertyRelative("max"));
            }

            private void DrawProperty(Rect rect,
                                      SerializedProperty property)
            {
                EditorGUI.PropertyField(rect, property, GUIContent.none);
            }
        }
#endif

    }
}
