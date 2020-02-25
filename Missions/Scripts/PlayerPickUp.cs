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
    public class PlayerPickUp : MonoBehaviour
    {

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Pickup"))
            {
                if (other.gameObject.GetComponent<KeysSettings>() != null)
                {
                    Missions.Missions_PlayerUI.Pickup_Key(other.gameObject.GetComponent<KeysSettings>().key_color);
                    Destroy(other.gameObject);
                }
            }
        }

    }
}
