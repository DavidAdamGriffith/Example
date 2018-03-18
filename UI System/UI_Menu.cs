/*

Description:        Handles the animation of the main top menu, including a delay for sliding in and out - clicks disabled when animating

David Griffith 2016
 
 */

using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
using UnityEngine.EventSystems;

public class UI_Menu : MonoBehaviour
{
    private float waitToStart = 2.0f;

    public Ease moveEase = Ease.Linear;
    public float moveTime = 1.0f;

    public float initialPivotX = -0.5f;
    public float initialPivotY = 0.5f;

    public float targetPivotX = 0.5f;
    public float targetPivotY = 0.5f;

    public float offsetPivotX = 1.25f;
    public float offsetPivotY = 0.5f;

    public UI_MenuElement menuElement;
    public UI_Cursor cursor;

    public GameObject clickOnOpen;

    public bool isOpen = false;

    public bool isInitial = false;

    private bool lastOpenState;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    private Tweener moveTween;
    public Tweener MoveTween
    {
        get
        {
            return moveTween;
        }
    }

    private GameObject selectedObject;

    private UI_MenuManager menuManager;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        menuManager = transform.parent.gameObject.GetComponent<UI_MenuManager>();
    }

    void Start()
    {
        lastOpenState = isOpen = false;
        canvasGroup.blocksRaycasts = false;
        rectTransform.pivot = new Vector2(initialPivotX, initialPivotY);
        selectedObject = clickOnOpen;

        if (isInitial)
        {
            StartCoroutine("Initialize");
        }
    }

    void Update()
    {
        if (isOpen != lastOpenState)
        {
            lastOpenState = isOpen;

            if (isOpen)
            {
                AnimateMenu(new Vector2(targetPivotX, targetPivotY));

                if (menuElement)
                {
                    menuElement.Animate(true);
                }
            }
            else
            {
                if (menuElement)
                {
                    menuElement.Animate(false);
                }

                AnimateMenu(new Vector2(initialPivotX, initialPivotY));   
            }
        }    
    }


    public void AnimateMenu(Vector2 target)
    {
        canvasGroup.blocksRaycasts = false;

        ClickObjectOnStateChange();

        if (moveTween != null && moveTween.IsPlaying())
        {
            moveTween.Kill(false);
        }

        moveTween = DOTween.To(() => rectTransform.pivot, x => rectTransform.pivot = x, target, moveTime)
                    .SetEase(moveEase)
                    .OnComplete(() => { if (isOpen) canvasGroup.blocksRaycasts = true; });
    }

    public void AnimateMenuOffset(Vector2 target)
    {
        canvasGroup.blocksRaycasts = false;

        //ClickObjectOnStateChange();

        if (moveTween != null && moveTween.IsPlaying())
        {
            moveTween.Kill(false);
        }

        moveTween = DOTween.To(() => rectTransform.pivot, x => rectTransform.pivot = x, target, moveTime)
                    .SetEase(moveEase)
                    .OnComplete(() => { if (isOpen) { canvasGroup.blocksRaycasts = true; } })
                    .Pause();

        if (cursor.MoveTween != null && cursor.MoveTween.IsPlaying())
        {
            cursor.MoveTween.OnComplete(() => moveTween.Play());
        }
        else
        {
            moveTween.Play();
        }
    }


    public void ClickObjectOnStateChange()
    {
        if (isOpen)
        {
            if (selectedObject)
            {
                PointerEventData pointer = new PointerEventData(EventSystem.current);
                ExecuteEvents.Execute(selectedObject, pointer, ExecuteEvents.pointerClickHandler);
            }
        }
    }

    public void SetSelectedObject(GameObject target)
    {
        selectedObject = target;
    }

    public void Toggle()
    {
        isOpen = !isOpen;
    }

    IEnumerator Initialize()
    {
        yield return new WaitForSeconds(waitToStart);

        lastOpenState = isOpen = true;

        AnimateMenu(new Vector2(targetPivotX, targetPivotY));

        if (menuElement)
        {
            menuElement.Animate(true);
        }
    }
}
