Custom UI code for creating a tabular stylized modern menu systems primarily for mobile, but perfectly fine for other applications as well. Design is intended to be used for menus sliding in and out, but can be adapted easily. Requires DOTween for easing functionality:

http://dotween.demigiant.com/

UI_MenuManager: Helper class, top of object hierarchy, to select a menu through code
UI_Menu: Top-level menu, handles more animation (initial conditions, to/from splash or credits, etc.)
UI_SubMenu: Any and all sub menus, handles basic animation of menus
UI_ButtonManager: Primarily sets colors and animates buttons, importantly overrides Unity's default selection behavior

UI_Cursor: Handles movement of a cursor object to highlight selection
UI_MenuElement: Handles movement of any non-interactable elements (such as logos)
UI_CodeClick: Emulates a click of a menu element via code

UI_Splash: Special case to animate a splash screen (full-screen menu page instead of tabular)
UI_Credits: Special case to animate a credits screen (full-screen menu page instead of tabular)