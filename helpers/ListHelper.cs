using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public static class ListHelper 
{
    public static List<CardStoreVO> ShuffleCardStoreList(List<CardStoreVO> list)
    {
        var sorted = list.OrderBy(a => Guid.NewGuid()).ToList();
        list.Clear();
        list.AddRange(sorted);
        return list;
    }
    public static GameObject getGameObjectByName(List<GameObject> list, string name)
    {
        Debug.Log("== ListHelper.getGameObjectByName ==");
        GameObject returnValue = null;

        foreach (GameObject i in list)
        {
            if (i.name == name)
            {
                returnValue = i;
                break;
            }
        }

        return returnValue;
    }
	public static CharacterView getGameObjectByName(List<CharacterView> list, string name)
	{
		Debug.Log("== ListHelper.getCharacterViewByName ==");
		CharacterView returnValue = null;

		foreach (CharacterView i in list)
		{
			if (i.name == name)
			{
				returnValue = i;
				break;
			}
		}

		return returnValue;
	}
	public static CharacterView getCharacterViewByName(List<CharacterView> list, string name)
    {
        Debug.Log("== ListHelper.getCharacterViewByName == " + name);
        CharacterView returnValue = null;

        foreach (CharacterView i in list)
        {
            if (i.name == name)
            {
                returnValue = i;
                break;
            }
        }

        return returnValue;
    }

	public static CharacterView getCharacterViewByPosition(List<CharacterView> list, int position)
	{
		Debug.Log("== ListHelper.getCharacterViewByPosition == " + position);
		CharacterView returnValue = null;

		foreach (CharacterView i in list)
		{
			if (i.model.position == position)
			{
				returnValue = i;
				break;
			}
		}

		return returnValue;
	}

	public static CharacterView getCharacterViewById(List<CharacterView> list, int id)
    {
        Debug.Log("== ListHelper.getCharacterViewById ==");
        CharacterView returnValue = null;

        foreach (CharacterView i in list)
        {
            // Debug.Log(i);
            if (i.model.uid == id)
            {
                returnValue = i;
                break;
            }
        }

        return returnValue;
    }
}
