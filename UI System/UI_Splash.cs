/*

Description:        Special case for a Splash screen, simply fades out from a color and back into it (eases)

David Griffith 2017
 
 */

using UnityEngine;
using DG.Tweening;
using System.Collections;

public class UI_Splash : MonoBehaviour
{
    public float waitToStart = 2.0f;
    public float durationToFade = 5.0f;

    public SpriteRenderer sprite;

    public Tweener splashTweener;

    public Color targetColor = Color.black;

    private bool isTransparent = false;

    public bool manualStart = false;

    public GameObject targetMenu;



    IEnumerator Start()
    {
        if (!manualStart)
        {
            yield return new WaitForSeconds(waitToStart);
            {
                splashTweener = sprite.DOColor(targetColor, durationToFade).OnComplete(() => { isTransparent = true; });
            }
        }
    }

    public void MakeSolid()
    {
        CanvasGroup[] canvasGroups = targetMenu.GetComponentsInChildren<CanvasGroup>();

        for (int i = 0; i < canvasGroups.GetLength(0); i++)
        {
            canvasGroups[i].blocksRaycasts = false;
        }

        splashTweener = sprite.DOColor(targetColor, durationToFade).OnComplete(() => { isTransparent = false ;});
    }
}
