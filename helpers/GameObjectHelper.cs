using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class GameObjectHelper
{
	//Get the screen size of an object in pixels, given its distance and diameter.
	public static float DistanceAndDiameterToPixelSize(float distance, float diameter)
	{

		float pixelSize = (diameter * Mathf.Rad2Deg * Screen.height) / (distance * Camera.main.fieldOfView);
		return pixelSize;
	}

	//Get the distance of an object, given its screen size in pixels and diameter.
	public static float PixelSizeAndDiameterToDistance(float pixelSize, float diameter)
	{

		float distance = (diameter * Mathf.Rad2Deg * Screen.height) / (pixelSize * Camera.main.fieldOfView);
		return distance;
	}

	//Get the diameter of an object, given its screen size in pixels and distance.
	public static float PixelSizeAndDistanceToDiameter(float pixelSize, float distance)
	{

		float diameter = (pixelSize * distance * Camera.main.fieldOfView) / (Mathf.Rad2Deg * Screen.height);
		return diameter;
	}

    
    public static void setSize(Component component, float width, float height)
    {
        if (component != null)
        {
            setSize(component.gameObject, width, height);
        }
    }

    public static void setSize(GameObject gameObject, float width, float height)
    {
        if (gameObject != null)
        {
            var rectTransform = gameObject.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                rectTransform.sizeDelta = new Vector2(width, height);
            }
        }
    }


    public static void setHeight(Component component, float height)
    {
        if (component != null)
        {
            setHeight(component.gameObject, height);
        }
    }

    public static void setHeight(GameObject gameObject, float height)
    {
        if (gameObject != null)
        {
            var rectTransform = gameObject.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, height);
            }
        }
    }


    public static void setWidth(Component component, float width)
    {
        if (component != null)
        {
            setWidth(component.gameObject, width);
        }
    }

    public static void setWidth(GameObject gameObject, float width)
    {
        if (gameObject != null)
        {
            var rectTransform = gameObject.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                rectTransform.sizeDelta = new Vector2(width, rectTransform.sizeDelta.y);
            }
        }
    }
}