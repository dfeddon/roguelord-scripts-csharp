using UnityEngine;
using System.Collections;

//This is a basic interface with a single required
//method.
public interface IEnabler
{
	void Enabler();
	void Disabler();
}

//This is a generic interface where T is a placeholder
//for a data type that will be provided by the 
//implementing class.
// public interface IDamageable<T>
// {
// 	void Damage(T damageTaken);
// }