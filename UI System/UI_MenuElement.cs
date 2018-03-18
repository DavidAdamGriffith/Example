/*

Description:        Use with a non-button, non functional menu element (such as a logo), allowing to manipulate as part of the UI
                    Currently used for easing the logo in and out, but could be used for other UI elements

David Griffith 2016
 
 */

using UnityEngine;
using DG.Tweening;
using System.Collections;

public class UI_MenuElement : MonoBehaviour
{
    public Ease moveEase = Ease.Linear;
    public float moveTime = 1.0f;

    public float initialPivotX = -0.5f;
    public float initialPivotY = 0.5f;

    public float targetPivotX = 0.5f;
    public float targetPivotY = 0.5f;
    
    private RectTransform rectTransform;

    public Tweener moveTween;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Start()
    {
        rectTransform.pivot = new Vector2(initialPivotX, initialPivotY);
    }

    public void Animate(bool forward)
    {
        Vector2 target;

        if (forward)
        {
            target = new Vector2(targetPivotX, targetPivotY);
        }
        else
        {
            target = new Vector2(initialPivotX, targetPivotY);
        }

        moveTween = DOTween.To(() => rectTransform.pivot, x => rectTransform.pivot = x, target, moveTime)
        .SetEase(moveEase);
	}
}
