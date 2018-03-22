This system handles the programmatic animation of a space ship in Unity3D, but can easily be adapted for other animations as well.

The system is based architecturally an adapted MVC design (the View being viewport of the Unity scene), and offers a simple plug, play and configure interface. Animations are atomized, and structure in the object hierarchy must be strictly adhered to, so as to avoid Euler Angle madness.

Included are Unity Editor extensions for WYSIWYG editing.

*NOTE: This requires the free DOTween engine, http://dotween.demigiant.com/