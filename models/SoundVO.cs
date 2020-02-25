using UnityEngine;

[System.Serializable]
public class SoundVO
{
	public const string ASSET_IMPACT_LASER_LIGHT_1 = "Laser Impact Light_1";
	public const string ASSET_IMPACT_BULLET_8 = "Bullet Impact 8";
	public const string ASSET_ACTIVATE_LASER_LIGHT_1 = "Laser gun fire_2";
	public const string ASSET_ACTIVATE_MELEE_SWING = "Whip sound 3";
	public const string ASSET_ACTIVATE_MATRIX_GLITCH = "ESM_Robotic_Game_Notification_7_Glitch_Software_Particle_Processed_Beep_Chrip_Electronic";
	public const string ASSET_ACTIVATE_BLOCK_SHIELD_PHYSICAL = "Card_Game_Play_Shield_Earth_02";
	public const string ASSET_UI_ATTACK_INCREASE = "Spiritual Power Up 1";
	public const string ASSET_UI_DEFENSE_INCREASE = "Glass Marbles Power Up 2";
	public const string ASSET_UI_ATTACK_DECREASE = "Spiritual Power Up 1";
	public const string ASSET_UI_DEFENSE_DECREASE = "Glass Marbles Power Up 2";

	public const string CHARACTER_SELECT = "Bubble Clink 2";
	public const string ABILITY_SELECT = "Bubble Pop 2";
	public const string ABILITY_SELECT_MOVE = "Atmospheric Menu Select 1";
	public const string ABILITY_SELECT_WEAPON_MOD_LOCKED = "Quest_Game_Tribal_Organic_Metal_Trigger_Tap_2_Lock_Chirp";
	public const string ABILITY_SELECT_GEAR_MOD_LOCKED = "Quest_Game_Tribal_Organic_Metal_Trigger_Tap_2_Lock_Chirp";
	public const string ABILITY_SELECT_WEAPON_MOD_EMPTY = "Puzzle_Game_Meter_Empty_01";
	public const string ABILITY_SELECT_GEAR_MOD_EMPTY = "Puzzle_Game_Meter_Empty_01";
	public const string ABILITY_SELECT_UNLOCK = "Ambient_Game_Soft_Craft_Item_Unlock_Metal_2";
	public const string ABILITY_SELECT_UNLOCK_FAIL = "Puzzle_Game_Negative_Reaction_01";
	public const string ABILITY_SELECT_DEFAULT_SLOT_1 = "Epic Button Click 1";

	public int uid;
	public string name;
	public string asset;
	public AudioClip audioClip;

	public SoundVO(int _uid = 0, string _name = "none", string _asset = "")
	{
		uid = _uid;
		name = _name;
		asset = _asset;
		audioClip = GameManager.instance.assetBundleCombat.LoadAsset<AudioClip>(asset);
	}
}
