using UnityEngine;

public class BuffTest : MonoBehaviour {
	
	public KeyCode KeyNiddle = KeyCode.Q;
	public KeyCode KeyPower;
	public KeyCode KeyWeek;
	public KeyCode KeyHealDec;
	public KeyCode KeySil;

	public void Update() {
	
		if (Input.GetKeyDown(KeyNiddle)) {
			EffectManager.Instance.OnBuff(gameObject, EffectManager.ColorOfThron);
		}
		if (Input.GetKeyDown(KeyPower)) {
			EffectManager.Instance.OnBuff(gameObject, EffectManager.ColorOfPower);
		}
		if (Input.GetKeyDown(KeyWeek)) {
			EffectManager.Instance.OnDebuff(gameObject, EffectManager.ColorOfWeakness);
		}
		if (Input.GetKeyDown(KeyHealDec)) {
			EffectManager.Instance.OnDebuff(gameObject, EffectManager.ColorOfHealDecreasment);
		}
		if (Input.GetKeyDown(KeySil)) {
			EffectManager.Instance.OnDebuff(gameObject, EffectManager.ColorOfSilence);
		}

	}

}