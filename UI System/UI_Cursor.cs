/*

Description:        Menu option cursor, buttons work fine without this, but makes a cursor highlight the selected option
                    Mostly aesthetic, but disables interaction when easing between positions

David Griffith 2016
 
 */

using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class UI_Cursor : MonoBehaviour
{
    public GameObject targetMenu;
    public GameObject initialPositionTarget;
    public MeshRenderer meshRenderer;

    public float targetOffsetY = 0.0f;

    private CanvasGroup canvasGroup;
    private UI_Menu menu;

    

    public Ease moveEase = Ease.Linear;
    public float moveTime = 1.0f;

    private Color color;
    private RectTransform rectTransform;

    private Tweener moveTween;
    public Tweener MoveTween
    {
        get
        {
            return moveTween;
        }
    }


    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

        canvasGroup = targetMenu.GetComponent<CanvasGroup>();
        menu = targetMenu.GetComponent<UI_Menu>();
    }

    void Start()
    {
        color = meshRenderer.material.color;
    }


    public void MoveCursor(GameObject target)
    {
        canvasGroup.blocksRaycasts = false;

        Vector2 targetPosition = target.GetComponent<RectTransform>().anchoredPosition;
        Vector2 cursorPosition = rectTransform.anchoredPosition;

        moveTween = DOTween.To(() => rectTransform.anchoredPosition, x => rectTransform.anchoredPosition = x, new Vector2(cursorPosition.x, targetPosition.y - targetOffsetY), moveTime)
                    .SetEase(moveEase)
                    .OnComplete(() => { if(!menu.MoveTween.IsPlaying()) canvasGroup.blocksRaycasts = true; } );
    }
}
