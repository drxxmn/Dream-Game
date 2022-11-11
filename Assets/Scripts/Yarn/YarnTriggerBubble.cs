using System;
using System.Collections;
using UnityEngine;

// This file is a demonstration of how to build a simple Dialogue View that
// presents lines, by subclassing DialogueViewBase and overriding certain
// important methods.

// Before using this class, you should first get familiar with using the
// built-in Line View and Options List View, which come built-in to Yarn
// Spinner. 

// This file also includes a class called 'Tween', which handles some animation
// work. While it's not actually making any use of Yarn Spinner APIs, it might
// be of interest.

// Import the Yarn.Unity namespace so we get access to Yarn classes.
using Yarn.Unity;

// SimpleSpeechBubbleView is a Dialogue View that shows text in a box that
// animates its size up and down.
public class YarnTriggerBubble : MonoBehaviour
{
    // The amount of time that lines will take to appear.
    [SerializeField] private float appearanceTime = 0.5f;

    // The amount of time that lines will take to disappear.
    [SerializeField] private float disappearanceTime = 0.5f;

    [SerializeField] private float yOffset = 4f;

    // The game object that should animate in and out.
    RectTransform container;

    [SerializeField] private RenderTexture pixelFilter;

    // The current coroutine that's playing out a scaling animation. When this
    // is not null, we're in the middle of an animation.
    Coroutine currentAnimation;

    GameObject characterSpeaking;

    private bool visible = false;

    // Sets the scale of the container view.
    private float Scale
    {
        // Take the value, which is a single number, and make it scale the
        // 'container' object by that amount on all three axes.
        set => container.localScale = new Vector3(value, value, value);
    }

    // Called on the first frame that this object is active.
    public void Start()
    {
        container = GetComponent<RectTransform>();
        // On start, we'll hide the line view by setting the scale to zero
        Scale = 0;
    }

    private void Update()
    {
        if (visible)
        {
            UpdatePosition();
        }
    }

    private void UpdatePosition()
    {
        if (characterSpeaking != null)
        {
            Vector2 screenPoint;
            Vector3 characterPos = characterSpeaking.transform.position;
            screenPoint = Camera.main.WorldToScreenPoint(characterPos + new Vector3(0, yOffset, 0));

            // When the pixel filter is enabled, the camera view gets resized.
            // The screenpoint therefore has to be multiplied by a scale 
            // factor, otherwise its position is not correct.
            if (pixelFilter != null)
            {
                screenPoint *= (Screen.width / pixelFilter.width * 1.15f);
            }

            container.position = screenPoint;
        }
    }

    public void Show(GameObject character)
    {
        if (visible) return;

        characterSpeaking = character;
        visible = true;
        // Animate from zero to full scale, over the course of appearanceTime.
        currentAnimation = this.Tween(
            0f, 1f,
            appearanceTime,
            (from, to, t) => Scale = Mathf.Lerp(from, to, t),
            () =>
            {
                // We're done animating!
                currentAnimation = null;
            }
            );
    }

    public void Hide()
    {
        if (!visible) return;

        // Animate the box's scale from full to zero, and when we're done,
        // report that the dismissal is complete.
        currentAnimation = this.Tween(
            1f, 0f,
            disappearanceTime,
            (from, to, t) => Scale = Mathf.Lerp(from, to, t),
            () =>
            {
                currentAnimation = null;
                visible = false;
            });
    }
}
