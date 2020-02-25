///////////////////////////////////
/// Create and edit by QerO
/// 09.2018
/// lidan-357@mail.ru
///////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Missions
{
    public class MapDraw : MonoBehaviour
    {

        public GameObject marker;
        public int draw_ray_density;

        private Ray see = new Ray();
        private RaycastHit see_hit;
        private int wall_mask;
        public Color map_print_color;
        private float draw_ray_diffusion;

        private Transform parent_for_markers;

        void Start()
        {

            wall_mask = LayerMask.GetMask("Wall");

            parent_for_markers = new GameObject("Minimap Draw Parent").transform;

            draw_ray_diffusion = Mathf.Sqrt(draw_ray_density)*2;

        }

        private void Update()
        {

            foreach (Transform child in parent_for_markers.transform)
            {
                GameObject.Destroy(child.gameObject);
            }

            see.origin = gameObject.transform.position;
            for (int n = -draw_ray_density; n < draw_ray_density+1; n++)
            {

                Vector3 direct = new Vector3();

                direct = (gameObject.transform.forward + gameObject.transform.right * n / draw_ray_diffusion);

                see.direction = direct;

                if (Physics.Raycast(see, out see_hit, 15, wall_mask))
                {
                    GameObject created_marker = Instantiate(marker, (new Vector3(see_hit.point.x, -1000, see_hit.point.z)), new Quaternion(0, 0, 0, 0));
                    if (see_hit.collider.gameObject.GetComponentInParent<DoorColor_onMap>() != null)
                    {
                        created_marker.GetComponent<MeshRenderer>().material.color = see_hit.collider.gameObject.GetComponentInParent<DoorColor_onMap>().door_color_on_map;
                    }
                    else
                    {
                        created_marker.GetComponent<MeshRenderer>().material.color = map_print_color;
                    }
                    created_marker.transform.SetParent(parent_for_markers);
                }
            }
        }
    }
}
