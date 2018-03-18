/*

Description:        Used for any menus not the main menu - handles the animation and easing aspects of the sub menu - clicks disabled when animating

David Griffith 2016
 
 */

using UnityEngine;
using DG.Tweening;
using System.Collections;
using UnityEngine.EventSystems;

public class UI_SubMenu : MonoBehaviour
{
    public bool useTargetMenuOffset = true;

    public Ease moveEase = Ease.Linear;
    public float moveTime = 1.0f;

    public float initialPivotX = -0.5f;
    public float initialPivotY = 0.5f;

    public float targetPivotX = 0.5f;
    public float targetPivotY = 0.5f;

    public bool isOpen = false;

    private bool lastOpenState;

    public UI_Menu targetMenu;

    public GameObject topMenuButton;
    public GameObject clickOnClose;
 
    private CanvasGroup menuCanvasGroup;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    public Tweener moveTween;

    private UI_MenuManager menuManager;

    // Use this for initialization
    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
        menuCanvasGroup = targetMenu.GetComponent<CanvasGroup>();
        menuManager = transform.parent.gameObject.GetComponent<UI_MenuManager>();
    }

    void Start()
    {
        lastOpenState = isOpen = false;
        canvasGroup.blocksRaycasts = false;
        rectTransform.pivot = new Vector2(initialPivotX, initialPivotY);
    }

    // Update is called once per frame
    void Update()
    {


        if (!useTargetMenuOffset)
        {
            if (topMenuButton && menuManager && menuManager.selectedMenu)
            {
                if (topMenuButton.gameObject.GetInstanceID() == menuManager.selectedMenu.GetInstanceID())
                {
                    isOpen = true;
                }
                else
                {
                    isOpen = false;
                }
            }
        }

        if (targetMenu.isOpen)
        {
            if (isOpen != lastOpenState)
            {
                lastOpenState = isOpen;

                if (isOpen)
                {
                    Vector2 target = new Vector2(targetMenu.offsetPivotX, targetMenu.offsetPivotY);

                    if (useTargetMenuOffset)
                    {
                        targetMenu.AnimateMenuOffset(target);
                        targetMenu.MoveTween.OnComplete(() => AnimateSubMenu(new Vector2(targetPivotX, targetPivotY)));
                    }
                    else
                    {
                        AnimateSubMenu(new Vector2(targetPivotX, targetPivotY));
                    }
                }
                else
                {
                    AnimateSubMenu(new Vector2(initialPivotX, initialPivotY));

                    if (useTargetMenuOffset)
                    {
                        moveTween.OnComplete(() =>
                        {
                            targetMenu.AnimateMenuOffset(new Vector2(targetMenu.targetPivotX, targetMenu.targetPivotY));
                            targetMenu.MoveTween.OnComplete(() => ClickObjectOnClose());
                        });
                    }
                }
            }
        }
        else
        {
            if (isOpen)
            {
                isOpen = false;

                AnimateSubMenu(new Vector2(initialPivotX, initialPivotY));
            }
        }
    }

    public void AnimateSubMenu(Vector2 target)
    {
        canvasGroup.blocksRaycasts = false;

        if (useTargetMenuOffset)
        {
            menuCanvasGroup.blocksRaycasts = false;
        }

        if (moveTween != null && moveTween.IsPlaying())
        {
            moveTween.Kill(false);
        }

        moveTween = DOTween.To(() => rectTransform.pivot, x => rectTransform.pivot = x, target, moveTime)
                    .SetEase(moveEase)
                    .OnComplete(() =>
                    {
                        if (!isOpen)
                        {
                            canvasGroup.blocksRaycasts = false;
                            ClickObjectOnClose();
                        }
                        else canvasGroup.blocksRaycasts = true;
                    });
    }

    public void ClickObjectOnClose()
    {
        if (clickOnClose)
        {
            PointerEventData pointer = new PointerEventData(EventSystem.current);
            ExecuteEvents.Execute(clickOnClose, pointer, ExecuteEvents.pointerClickHandler);
        }
    }

    public void Toggle()
    {
            isOpen = !isOpen;
    }
}
