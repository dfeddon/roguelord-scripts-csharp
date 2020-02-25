using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class DOTweenFX// : MonoBehaviour
{
	// }
	// void Awake() {
		
	// }
	// private void Start() {
		
	// }

	private void UIFloaterComplete(Image go, TextMeshProUGUI txt)
	{
		Debug.Log("<color=blue>$$$$$$$$$$$$$$$$$$$$$$ complete "+ go + " / " + txt);
		go.transform.SetParent(null);
		txt.transform.SetParent(null);
		// this.transform.SetParent(null)
		// Destroy(go);
		// Destroy(txt);
		// Destroy(this);
	}
	public DOTweenFX UIFloater(Image go, TextMeshProUGUI txt, bool isUpgrade)
	{
		Debug.Log("<color=blue>== UIFloater ==</color>");
		// GameObject go = new GameObject(); // TODO: <- object pool this
		// Destroy(go, 10);
		// Destroy(txt, 10);
		// Image go = Instantiate(source, new Vector3(source.transform.position.x, source.transform.position.y, source.transform.position.z), Quaternion.identity);
		// go.transform.SetParent(source.transform);
		// go.transform.position = source.transform.position;
		// TextMeshProUGUI txt = go.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
		// // if (int.Parse(val) >= 10)
		// // 	txt.fontSize = 8;
		// txt.SetText(val);

		/*
		Image img2 = go.AddComponent<Image>();
		img2.sprite = GameManager.instance.assetBundleCombat.LoadAsset<Sprite>(asset);
		img2.rectTransform.sizeDelta = new Vector2(35, 35);//attackImage.sprite.rect.width, attackImage.sprite.rect.height);

		GameObject go2 = new GameObject();
		go2.transform.SetParent(source.transform);
		TextMeshProUGUI txt = go2.AddComponent<TextMeshProUGUI>();
		txt.text = "5";//SetText("+5");
		*/
		float moveTo;
		if (isUpgrade == true)
			moveTo = 75;
		else moveTo = -75;

		Sequence s = DOTween.Sequence();
		float time = 2.0f;
		Vector3 toPos = new Vector3(0, go.transform.position.y + moveTo, 0);
		// Destroy(go, time * 3);
		s.Join(go.transform.DOLocalMoveY(moveTo, time).SetEase(Ease.InOutQuint));
		// s.Join(go.transform.DOScale(1.25f, time));
		s.Insert(1f, go.transform.DOLocalRotate(new Vector3(0, 360, 0), 0.5f, RotateMode.FastBeyond360).SetEase(Ease.Linear));
		s.Append(go.DOFade(0.0f, 1).SetEase(Ease.InOutQuint));
		s.Join(txt.DOFade(0.0f, 1).SetEase(Ease.InOutQuint)).OnComplete(() => UIFloaterComplete(go, txt));
		// s.AppendCallback(UIFloaterComplete);
		// s.Append(go.transform.DORotate(new Vector3(0, 180, 0), time).SetEase(Ease.InOutQuint));
		// s.Join(transform.GetComponent<SpriteRenderer>().DOFade(0f, "color", time));
		// s.Join(go.transform.DORotate(toPos, time, RotateMode.Fast));

		return this;
	}

}