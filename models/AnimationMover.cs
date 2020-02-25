using System.Collections;
// using System.Collections.Generic;
using UnityEngine;
using System;

//In order to use a collection's Sort() method, this class needs to implement the IComparable interface.
public class AnimationMover//<T> where T: new()
{
    public GameObject obj;
    // public HealthbarComponent hbar;
    // public EffectsbarComponent ebar;
    public Vector3 endPosition;
    public float time;

    public AnimationMover(Vector3 _endPosition, GameObject _obj)//, HealthbarComponent _hbar = null, EffectsbarComponent _ebar = null)
    {
        Debug.Log(_endPosition.x);
		endPosition = _endPosition;
        obj = _obj;
		// if (_obj != null)
        //     obj = _obj;
        // else if (_hbar != null)
        //     hbar = _hbar;
        // else if (_ebar != null)
        //     ebar = _ebar;
    }
}
