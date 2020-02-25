using UnityEngine;

public class ArrowMouseFollower : MonoBehaviour
{
	public ArrowRenderer arrowRenderer;
	public float distanceFromScreen = 5f;
	public Vector3 start = Vector3.zero;
	public bool isActive = false;

	void Update()
	{
		if (isActive)
		{
			Vector3 mousePosition = Input.mousePosition;
			mousePosition.z = distanceFromScreen;

			Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
			arrowRenderer.SetPositions(start, worldMousePosition);
		}
	}
}
