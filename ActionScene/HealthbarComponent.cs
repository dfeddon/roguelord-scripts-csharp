using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class HealthbarComponent : MonoBehaviour
{
	private Color32 COLOR_GREEN = new Color32(6, 180, 21, 255);
	private Color32 COLOR_WHITE = new Color32(255, 255, 255, 255);
	private Color32 COLOR_ORANGE = new Color32(248, 129, 29, 255);
	private Color32 COLOR_HEAL = new Color32(103, 250, 115, 255);
	public Slider sliderPrimary;
	public Slider sliderSecondary;
	public int healthMax;
	public int health;
	private GameObject fillArea;
	private GameObject turnView;
	private GameObject primaryComponent;
	private GameObject primarybar;
	private GameObject secondarybar;
	private GameObject attributesBar;
	private Image secondaryImage;
	private Image primaryImage;
	private Image attackImage;
	private TextMeshProUGUI attackText;
	private Image defenseImage;
	private TextMeshProUGUI defenseText;
	private AudioSource audioSource;
	private List<DOTweenFX> fxList = new List<DOTweenFX>();
	private List<Image> attackList = new List<Image>();
	private List<Image> defenseList = new List<Image>();
	private int fxListLast = -1;
	private int attackListLast = -1;
	private int defenseListLast = -1;
	public bool _isDefender = false;

	void Awake()
	{

		// get bar refs
		turnView = gameObject.transform.GetChild(0).gameObject;
		fillArea = gameObject.transform.GetChild(2).gameObject;
		primaryComponent = gameObject.transform.GetChild(4).gameObject;
		attributesBar = gameObject.transform.GetChild(5).gameObject;
		secondarybar = fillArea.transform.GetChild(0).gameObject;
		primarybar = primaryComponent.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject;
		primaryImage = primarybar.GetComponent<Image>();
		secondaryImage = secondarybar.GetComponent<Image>();

		attackImage = attributesBar.transform.GetChild(0).gameObject.GetComponent<Image>();
		attackText = attackImage.transform.GetChild(0).transform.GetComponent<TextMeshProUGUI>();
		defenseImage = attributesBar.transform.GetChild(1).gameObject.GetComponent<Image>();
		defenseText = defenseImage.transform.GetChild(0).transform.GetComponent<TextMeshProUGUI>();

		sliderPrimary.wholeNumbers = true;
		sliderSecondary.wholeNumbers = true;

		audioSource = this.gameObject.AddComponent<AudioSource>();
	}
	void Start()
	{
		// populate object pools
		Image img;
		for (int i = 1; i <= 2; i++)
		{
			fxList.Add(new DOTweenFX());
			if (i == 1)
			{
				img = Instantiate(attackImage);
				attackList.Add(img);//Instantiate(attackImage));
			}
			else 
			{
				img = Instantiate(defenseImage);
				defenseList.Add(img);//Instantiate(defenseImage));
			}
			img.gameObject.SetActive(false);
		}
	}
	private void Update()
	{
		// UpdateHealthBar(counter);        // just for testing purposes
		// counter--;                        // just for testing purposes
	}
	public void SetHealth(HealthVO vo)
	{
		// Debug.Log("<color=yellow>== HealthbarComponent.SetHealth ==</color>");
		// Debug.Log("* healthMax = " + vo.max);
		this.healthMax = vo.max;
		this.health = vo.current;

		sliderSecondary.minValue = 0f;
		sliderSecondary.maxValue = vo.max;
		sliderPrimary.minValue = 0f;
		sliderPrimary.maxValue = vo.max;
		// slider.value = vo.current;
	}
	public void UpdateHealthBar(int type, HealthVO vo)
	{
		// if (this.gameObject.activeSelf == false)
		this.gameObject.SetActive(true);
		switch(type)
		{
			case HealthVO.HEALTH_MODIFIER_DAMAGE:
				// set secondary bar to orange color
				secondaryImage.color = Color.Lerp(COLOR_ORANGE, COLOR_ORANGE, 0.5f);
			break;

			case HealthVO.HEALTH_MODIFIER_HEAL:
				// set secondary bar to orange color
				secondaryImage.color = Color.Lerp(COLOR_HEAL, COLOR_HEAL, 0.5f);
				break;

			case HealthVO.HEALTH_MODIFIER_OTHER:
			break;
		}

		// update health
		SetHealth(vo);
		// start animation tweens
		if (this.gameObject.activeSelf == true)
			StartCoroutine(DamageTweens(type, vo));
	}

	public void UpdateAttack(int value, int diff, bool silent = false)
	{
		attackText.text = value.ToString();

		if (silent == false)
		{
			// value up or down
			if (diff < 0) // down
			{
				Debug.Log("down");
			} else { // up
				audioSource.PlayOneShot(SoundLibraryVO.GetUIFX("attack-increase").audioClip, 0.95f);
			}

			// fx
			string v = Mathf.Abs(diff).ToString();
			UIFloater(attackImage, v, (diff > 0) ? true : false);//, "attack-icon 4");
		}

		if (value == 0)
		{
			attackImage.transform.gameObject.SetActive(false);
			attackText.transform.gameObject.SetActive(false);
		}
	}

	public void UpdateDefense(int value, int diff, bool silent = false)
	{
		defenseText.text = value.ToString();

		if (silent == false)
		{
			if (diff < 0)
			{
				Debug.Log("down");
			} else {
				audioSource.PlayOneShot(SoundLibraryVO.GetUIFX("defense-increase").audioClip, 0.95f);
			}
			string v = Mathf.Abs(diff).ToString();
			UIFloater(defenseImage, v, (diff > 0) ? true : false);//, "defense-icon 4");
			// isOOP(true);
		}

		if (value == 0)
		{
			defenseImage.transform.gameObject.SetActive(false);
			defenseText.transform.gameObject.SetActive(false);
		}
	}

	private void UIFloaterComplete()
	{
		Debug.Log("complete...");
	}
	private void UIFloater(Image source, string val, bool isUpgrade = true)
	{
		// GameObject go = new GameObject(); // TODO: <- object pool this
		Image go;
		if (source == attackImage) {
			attackListLast++;
			if (attackListLast > 4)
				attackListLast = 0;
			go = attackList[attackListLast];
		} else {
			defenseListLast++;
			if (defenseListLast > 4)
				defenseListLast = 0;
			go = defenseList[defenseListLast];
		}
		// Image go = Instantiate(, new Vector3(source.transform.position.x, source.transform.position.y, source.transform.position.z), Quaternion.identity);
		go.transform.position = source.transform.position;
		go.transform.SetParent(source.transform);
		go.transform.position = source.transform.position;
		TextMeshProUGUI txt = go.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
		// if (int.Parse(val) >= 10)
		// 	txt.fontSize = 8;
		txt.SetText(val);
		// todo object pool fx
		if (fxListLast == 9)
			fxListLast = 0;
		else fxListLast++;
		// access floater instances from pool
		fxList[fxListLast].UIFloater(go, txt, isUpgrade);
		// DOTweenFX fx = new DOTweenFX().UIFloater(go, txt, isUpgrade);
		// Instantiate()
		Destroy(go, 8.0f);
		Destroy(txt, 8.0f);
		// Destroy(fx, 10.0f);
		// fx = null;

		/*
		Image img2 = go.AddComponent<Image>();
		img2.sprite = GameManager.instance.assetBundleCombat.LoadAsset<Sprite>(asset);
		img2.rectTransform.sizeDelta = new Vector2(35, 35);//attackImage.sprite.rect.width, attackImage.sprite.rect.height);
		
		GameObject go2 = new GameObject();
		go2.transform.SetParent(source.transform);
		TextMeshProUGUI txt = go2.AddComponent<TextMeshProUGUI>();
		txt.text = "5";//SetText("+5");
		*/

		/*
		float moveTo;
		if (isUpgrade == true)
			moveTo = 75;
		else moveTo = -75;

		Sequence s = DOTween.Sequence();
		float time = 2.0f;
		Vector3 toPos = new Vector3(0, go.transform.position.y + moveTo, 0);
		Destroy(go, time * 3);
		s.Join(go.transform.DOLocalMoveY(moveTo, time).SetEase(Ease.InOutQuint));
		s.Join(go.transform.DOScale(1.25f, time));
		s.Insert(1f, go.transform.DOLocalRotate(new Vector3(0, 360, 0), 0.5f, RotateMode.FastBeyond360).SetEase(Ease.Linear));
		s.Append(go.DOFade(0.0f, time * 2).SetEase(Ease.InOutQuint));
		s.Join(txt.DOFade(0.0f, time * 2).SetEase(Ease.InOutQuint));
		// s.Append(go.transform.DORotate(new Vector3(0, 180, 0), time).SetEase(Ease.InOutQuint));
		// s.Join(transform.GetComponent<SpriteRenderer>().DOFade(0f, "color", time));
		// s.Join(go.transform.DORotate(toPos, time, RotateMode.Fast));
		*/
	}

	IEnumerator DamageTweens(int type, HealthVO vo)
	{
		Debug.Log("<color=red>== HealthbarComponent.DamageTweens ==</color>");
		Slider slider1 = sliderPrimary;
		Slider slider2 = sliderSecondary;

		// if healing, flip bar progression order
		if (type == HealthVO.HEALTH_MODIFIER_HEAL)
		{
			slider1 = sliderSecondary;
			slider2 = sliderPrimary;
		}
		// update primary
		slider1.DOValue(vo.current, 0.5f);

		// update secondary
		yield return new WaitForSeconds(1.5f);
		slider2.DOValue(vo.current, 0.5f);
		
		yield return new WaitForSeconds(0.35f);

		// update secondary bar color if critically low
		if ((float)vo.current / vo.max <= 0.15)
			primaryImage.color = Color.Lerp(Color.red, Color.red, 0.5f);
		else primaryImage.color = Color.Lerp(COLOR_GREEN, COLOR_GREEN, 0.5f);
	}
	public void isOOP(bool b = true)
	{
		// pulse attack/defense icons
		if (b == true)
		{
			attackImage.transform.DOScale(1.2f, 0.95f).SetLoops(-1, LoopType.Yoyo);
			defenseImage.transform.DOScale(1.2f, 0.95f).SetLoops(-1, LoopType.Yoyo);
		}
		else
		{
			DOTween.Kill(attackImage.transform);
			DOTween.Kill(defenseImage.transform);
		}
	}

	public bool isDefender
	{
		get { return _isDefender; }
		set 
		{ 
			_isDefender = value; 

			if (value == true)
			{
				// flip attack/defense icons
				RectTransform atrans, dtrans;
				Vector3 atrans2;
				atrans = attackImage.GetComponent<RectTransform>();
				atrans2 = atrans.localPosition;
				dtrans = defenseImage.GetComponent<RectTransform>();
				atrans.localPosition = dtrans.localPosition;
				dtrans.localPosition = atrans2;
			}
		}
	}
}