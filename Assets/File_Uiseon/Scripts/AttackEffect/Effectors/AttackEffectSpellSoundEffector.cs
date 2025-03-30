public class AttackEffectSpellSoundEffector : AttackEffectEffector {

	public override void OnShoot(AttackEffect attackEffect) {
		SoundManager.Instance.PlaySpellCastSound();
		IsAbleToDestroy = true;
	}

}