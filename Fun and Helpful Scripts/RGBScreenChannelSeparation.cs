/*

Description:        Full-screen RGB channel seperator, separates by percentage (0 to 1 full screen) on all channels

                    *Needs to be attached to a Camera!

David Griffith 2017
 
 */

using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]

public class RGBScreenChannelSeparation : ImageEffectBase
{
    //Distance the R, G and B channels are offset by from their initial positions - 0 = none, 1 = screen width/height
    public Vector2 R = Vector2.zero;
    public Vector2 G = Vector2.zero;
    public Vector2 B = Vector2.zero;

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        this.material.SetVector("_Red", this.R);
        this.material.SetVector("_Green", this.G);
        this.material.SetVector("_Blue", this.B);
        Graphics.Blit(source, destination, this.material);
    }
}
