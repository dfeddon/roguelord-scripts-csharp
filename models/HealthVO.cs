using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class HealthVO
{
	public const int HEALTH_MODIFIER_NONE = 0;
	public const int HEALTH_MODIFIER_DAMAGE = 1;
	public const int HEALTH_MODIFIER_HEAL = 2;
	public const int HEALTH_MODIFIER_OTHER = 3;

	public int uid;
	public int max;
	private int _current;
	public int prePhase;
	public HealthVO()
	{

	}
	public bool RoundComplete()
	{
		bool doUpdate = false;

		// update cooldowns

		return doUpdate;
	}

	public float GetPercentage()
	{
		return (current / max) * 100;
	}

	public int current
	{
		get { return _current; }
		set 
		{
			// thresholds...
			if (value > max)
				_current = max;
			else if (value < 0)
				_current = 0;
			
			_current = value;
		}
	}

}