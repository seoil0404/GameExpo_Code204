using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class MinoEffectHelper : MonoBehaviour {

	//======================================================================| Singleton

	public static MinoEffectHelper Instance = null;

	public void Awake() {
		if (
			Instance.IsUnityNull() ||
			Instance.IsDestroyed() ||
			Instance.isActiveAndEnabled == false
		) {
			Instance = this;
		}
	}

	//======================================================================| Fields

	[Header("Mino Disable Effect")]
	[SerializeField]
	private float GridDeactivateDuration;

	[SerializeField]
	private VisualEffect ExplosionEffect;

	//======================================================================| Methods

	public void PlayMinoEffect(List<GameObject> gridObjects, int[] targetIndices) {
		StartCoroutine(GridEffectCoroutine(gridObjects, targetIndices));
	}

	private IEnumerator GridEffectCoroutine(List<GameObject> gridObjects, int[] targetIndices) {
		
		float durationPerGrid = GridDeactivateDuration / targetIndices.Length;

		foreach (var targetIndex in targetIndices) {

			Vector2 position = gridObjects[targetIndex].transform.position;

			gridObjects[targetIndex].GetComponent<GridSquare>().Deactivate();
			VisualEffect effect = Instantiate(ExplosionEffect);

			effect.transform.position = position;
			effect.Play();

			StartCoroutine(RemoveEffectWhenDone(effect));

			yield return new WaitForSeconds(durationPerGrid);
		}

	}

    public void PlayMinoEffectSingle(GameObject gridObject)
    {
        StartCoroutine(GridEffectSingleCoroutine(gridObject));
    }

    private IEnumerator GridEffectSingleCoroutine(GameObject gridObject)
    {
        Vector2 position = gridObject.transform.position;

        gridObject.GetComponent<GridSquare>().Deactivate();
        VisualEffect effect = Instantiate(ExplosionEffect);

        effect.transform.position = position;
        effect.Play();

        StartCoroutine(RemoveEffectWhenDone(effect));

        yield return new WaitForSeconds(GridDeactivateDuration);
    }


    private IEnumerator RemoveEffectWhenDone(VisualEffect visualEffect) {
		
		yield return new WaitUntil(() => visualEffect.aliveParticleCount == 0);
		Destroy(visualEffect);

	}

}