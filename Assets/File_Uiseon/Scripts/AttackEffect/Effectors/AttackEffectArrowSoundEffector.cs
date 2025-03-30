public class AttackEffectArrowSoundEffector : AttackEffectEffector {

	public override void OnShoot(AttackEffect attackEffect) {
		SoundManager.Instance.PlayFireArrowSound();
		IsAbleToDestroy = true;
	}

}