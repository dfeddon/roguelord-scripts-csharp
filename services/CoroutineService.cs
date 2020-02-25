using System.Collections;
using UnityEngine;
public class CoroutineService
{
	public Coroutine coroutine { get; private set; }
	public object result;
	private IEnumerator target;
	
	public CoroutineService(MonoBehaviour owner, IEnumerator target)
	{
		this.target = target;
		this.coroutine = owner.StartCoroutine(Run());
	}

	private IEnumerator Run()
	{
		while (target.MoveNext())
		{
			result = target.Current;
			yield return result;
		}
	}
}