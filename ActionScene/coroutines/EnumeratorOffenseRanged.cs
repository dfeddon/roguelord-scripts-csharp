// In this example we show how to invoke a coroutine and wait until it
// is completed

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class EnumeratorOffenseRanged : MonoBehaviour
{
    CharacterView source;
    List<CharacterView> targets;
    string prefabId;
    AbilityVO ability;
    public void Go(CharacterView _source, List<CharacterView> _targets, AbilityVO vo)
    {
        Debug.Log("<color=yellow>== EnumeratorOffenseRanged.Go ==</color>");
        source = _source;
        targets = _targets;
        ability = vo;
        // prefabId = _prefabId;

        print("Starting " + Time.time);
        StartCoroutine("doStart");
    }

	IEnumerator doStart()
	{
		Debug.Log("<color=yellow>== EnumeratorOffenseRanged.doStart ==</color>");
		yield return StartCoroutine(WaitAndPrint(0.25f));
		print("Done " + Time.time);
        source.animator.SetInteger("SkillNumber", ability.slot - 1);
        source.animator.SetTrigger("UseSkill");
	}


	// suspend execution for waitTime seconds
	IEnumerator WaitAndPrint(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        print("WaitAndPrint " + Time.time);
    }
}