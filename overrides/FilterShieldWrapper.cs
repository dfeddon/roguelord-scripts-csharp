using UnityEngine;

public class FilterShieldWrapper : CameraFilterPack_3D_Shield, IEnabler
{

	private CameraFilterPack_3D_Shield src = null;
	public bool doFlicker;

	public FilterShieldWrapper(CameraFilterPack_3D_Shield _src, bool _doFlicker = false)
	{
		src = _src;
		doFlicker = _doFlicker;
	}
	void Awake()
	{
		
	}

	void Start() 
	{
	}

	public void Enabler()
	{
		src.enabled = true;
	}
	public void Disabler()
	{
		src.enabled = false;
	}
}