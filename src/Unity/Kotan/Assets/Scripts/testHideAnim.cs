using System.Collections;
using UnityEngine;

public class testHideAnim : MonoBehaviour
{
    private void OnEnable()
    {
        StartCoroutine(HideHitEffectCR());
    }

    private IEnumerator HideHitEffectCR()
    {
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }
}
