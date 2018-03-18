/*

Description:        Special case for a Credits screen, simply fades in/out and handles button to return

David Griffith 2017
 
 */

using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UI_Credits : MonoBehaviour
{
    public UI_SubMenu target;
    public CanvasGroup targetMenu;

    public Ease ease = Ease.Linear;
    public Ease clickEase = Ease.Linear;
    public float duration = 1.0f;
    public float clickDuration = 0.1f;

    public Color initialColor = Color.clear;
    public Color clickColor = Color.white;
    public Color targetColor = Color.white;

    private bool open = false;

    private RawImage image;

    // Use this for initialization
    void Start()
    {
        open = false;
        image = GetComponent<RawImage>();
        image.color = initialColor;
        image.raycastTarget = false;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!open && !target.isOpen && targetMenu.blocksRaycasts)
        {
            open = true;
            image.DOColor(targetColor, duration)
                .SetEase(ease)
                .OnComplete(() => { image.raycastTarget = true; });
        }
        else if (open && target.isOpen)
        {
            open = false;
            image.raycastTarget = false;
            image.DOColor(initialColor, duration).SetEase(ease);
        }
    }

    public void AnimateClick()
    {
        Sequence sequence = DOTween.Sequence();

        sequence.Append(image.DOColor(clickColor, clickDuration / 2.0f).SetEase(clickEase))
                .Append(image.DOColor(targetColor, clickDuration / 2.0f).SetEase(clickEase));
    }
}
