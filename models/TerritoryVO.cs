using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//In order to use a collection's Sort() method, this class needs to implement the IComparable interface.
public class TerritoryVO
{
    public const string STRUCTURE_TYPE_HQ = "HQ";
    public const string STRUCTURE_TYPE_BANK = "Building Bank";
 
    private int _uid;
    private int _index;
    private int _owner;
    private int _status;
    private int _type;

    public TerritoryVO(int uid, int index, int owner, int type)
    {
		// Debug.Log("TerritoryVO.constructor " + uid + "/" + index + "/" + owner + "/" + type);

        this.uid = uid;
        this.index = index;
        this.owner = owner;
        this._type = type;
    }

    //This method is *required* by the IComparable interface. 
    public static string GetStructureByType(int type)
    {
        string returnVal = "0";
        switch(type)
        {
            case 0: returnVal = STRUCTURE_TYPE_BANK; break;
            case 1: returnVal = STRUCTURE_TYPE_HQ; break;
            case 2: returnVal = STRUCTURE_TYPE_BANK; break;
            case 3: returnVal = STRUCTURE_TYPE_HQ; break;
            case 4: returnVal = STRUCTURE_TYPE_BANK; break;
            case 5: returnVal = STRUCTURE_TYPE_HQ; break;
            case 6: returnVal = STRUCTURE_TYPE_BANK; break;
            case 7: returnVal = STRUCTURE_TYPE_HQ; break;
            case 8: returnVal = STRUCTURE_TYPE_BANK; break;
            case 9: returnVal = STRUCTURE_TYPE_HQ; break;
        }
        return returnVal;
    }

    // getters & setters
    public int uid
    {
        get { return _uid; }
        set { _uid = value; }
    }

    public int index
    {
        get { return _index; }
        set { _index = value; }
    }

    public int status
    {
        get { return _status; }
        set { _status = value; }
    }
    public int owner
    {
        get { return _owner; }
        set { _owner = value; }
    }
	public int type
	{
		get { return _type; }
		set { _type = value; }
	}

}
