using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//In order to use a collection's Sort() method, this class needs to implement the IComparable interface.
public class IncidentVO
{
	public const int INCIDENT_TYPE_INFILTRATION = 1;
	public const int INCIDENT_TYPE_SPRAWL_BRAWL = 2;

	private int _uid;
	private int _index;
	private int _owner;
	private int _status;
	private int _type;
	private string _description;

	public IncidentVO(int type, int owner)
	{
		// Debug.Log("IncidentVO.constructor " + uid + "/" + index + "/" + owner + "/" + type);

		this.uid = uid;
		this._type = type;
		this.index = index;
		this.owner = owner;
	}

	//This method is *required* by the IComparable interface. 
	public static string GetStructureByType(int type)
	{
		string returnVal = "0";
		switch (type)
		{
			case INCIDENT_TYPE_INFILTRATION: returnVal = "infil"; break;
			case INCIDENT_TYPE_SPRAWL_BRAWL: returnVal = "sprawlbrawl"; break;
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
