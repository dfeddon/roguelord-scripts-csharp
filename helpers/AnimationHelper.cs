using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class AnimationHelper
{
    public static Vector3 flipX(Vector3 scale)
    {
        scale.x *= -1;
        return scale;
    }

}