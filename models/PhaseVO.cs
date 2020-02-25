using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PhaseVO
{
	public enum PhaseType { Random, Dealer, Tech, Econ, Power, Offense, Defense, Condi, Boon, Weather, Fatality, Wager };
	public enum PhaseTechType { Echem, Nano, Aug, Hack };
	public enum PhaseSpecializationType { Medic, Nanomancer, Chromer, BlackHatter, Saboteur, Chemist, Engineer, Anarchist };

	PhaseType type;
	int bonus;

	public PhaseVO()
	{

	}

	public PhaseVO getPhaseEcon()
	{
		switch(type)
		{
			case PhaseType.Dealer:
			break;
		}
		return this;
	}
}