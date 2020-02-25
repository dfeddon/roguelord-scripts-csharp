using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// [System.Serializable]
public class SidecarVO
{
	CharacterVO owner;
	CharacterVO assignee;
	int health;
	public bool isActivated = false;
	public GameObject prefab;

	public SidecarVO(CharacterVO _owner, CharacterVO _assignee, GameObject _prefab)
	{
		if (_owner != null)
			owner = _owner;
		if (_assignee != null)
			assignee = _assignee;
		if (_prefab != null)
			prefab = _prefab;

	}
}