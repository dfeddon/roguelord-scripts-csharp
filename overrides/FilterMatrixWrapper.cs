using UnityEngine;

public class FilterMatrixWrapper : CameraFilterPack_3D_Matrix, IEnabler
{

	private CameraFilterPack_3D_Matrix src = null;
	public bool doFlicker;

	public FilterMatrixWrapper(CameraFilterPack_3D_Matrix _src, bool _doFlicker = false)
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