﻿///////////////////////////////////
/// Create and edit by QerO
/// 09.2018
/// lidan-357@mail.ru
///////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Missions
{
    public class MapSystemOffset : MonoBehaviour
    {

        void Start()
        {
            gameObject.transform.SetPositionAndRotation(new Vector3(Missions_MapGenerator.map_center.x, 0, Missions_MapGenerator.map_center.y), new Quaternion(0f, 0f, 0f, 0f));
        }

    }
}
