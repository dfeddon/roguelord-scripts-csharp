using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HeatVO
{
	public enum HeatType { Starter, Random, Offense1, Offense2, Defense1, Defense2 };
	int phasesTotal = 0;
	int heatsTotal = 0;
	List<PhaseVO> currentHeat = new List<PhaseVO>();
	int currentPhaseNum = 0;
	PhaseVO currentPhase;
	private PhaseVO phase1;
	private PhaseVO phase2;
	private PhaseVO phase3;
	private PhaseVO phase4;
	private PhaseVO phase5;

	public HeatVO()
	{
		phase1 = new PhaseVO();
		phase2 = new PhaseVO();
		phase3 = new PhaseVO();
		phase4 = new PhaseVO();
		phase5 = new PhaseVO();
	}

	public List<PhaseVO> CreateHeat(HeatType type)
	{
		Debug.Log("<color=white>== HeatVO.CreateHeat ==</color>");

		// clear previous heat
		currentHeat.Clear();

		// create heat base on heat type (econ, attack, defense, power, etc)
		switch(type)
		{
			case HeatType.Starter:
			break;
			
			case HeatType.Random:
			break;
			
			case HeatType.Offense1:
			break;
			
			case HeatType.Offense2:
			break;
			
			case HeatType.Defense1:
			break;
			
			case HeatType.Defense2:
			break;
		}

		return currentHeat;
	}
}