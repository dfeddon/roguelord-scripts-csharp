using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Entity <T>
{
    public GameObject gameObject;
    public T script;

    public Entity(GameObject _gameObject, System.Type _scriptClass)
    {
        Debug.Log("== Entity ==");
        Debug.Log(_gameObject);
        Debug.Log(_scriptClass);
        this.gameObject = _gameObject;
        // this.script = _gameObject.GetComponent(_scriptClass);
        Debug.Log(this.script);
    }

}