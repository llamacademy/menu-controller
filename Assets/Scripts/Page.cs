using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource), typeof(CanvasGroup))]
[DisallowMultipleComponent]
public class Page : MonoBehaviour
{
    private AudioSource AudioSource;
    private RectTransform RectTransform;
    private CanvasGroup CanvasGroup;

    [SerializeField]
    private float AnimationSpeed = 1f;
    public bool ExitOnNewPagePush = false;
    [SerializeField]
    private AudioClip EntryClip;
    [SerializeField]
    private AudioClip ExitClip;
    [SerializeField]
    private EntryMode EntryMode = EntryMode.SLIDE;
    [SerializeField]
    private Direction EntryDirection = Direction.LEFT;
    [SerializeField]
    private EntryMode ExitMode = EntryMode.SLIDE;
    [SerializeField]
    private Direction ExitDirection = Direction.LEFT;
    [SerializeField]
    private UnityEvent PrePushAction;
    [SerializeField]
    private UnityEvent PostPushAction;
    [SerializeField]
    private UnityEvent PrePopAction;
    [SerializeField]
    private UnityEvent PostPopAction;

    private Coroutine AnimationCoroutine;
    private Coroutine AudioCoroutine;

    private void Awake()
    {
        RectTransform = GetComponent<RectTransform>();
        CanvasGroup = GetComponent<CanvasGroup>();
        AudioSource = GetComponent<AudioSource>();

        AudioSource.playOnAwake = false;
        AudioSource.loop = false;
        AudioSource.spatialBlend = 0;
        AudioSource.enabled = false;
    }

    public void Enter(bool PlayAudio)
    {
        PrePushAction?.Invoke();

        switch(EntryMode)
        {
            case EntryMode.SLIDE:
                SlideIn(PlayAudio);
                break;
            case EntryMode.ZOOM:
                ZoomIn(PlayAudio);
                break;
            case EntryMode.FADE:
                FadeIn(PlayAudio);
                break;
        }
    }

    public void Exit(bool PlayAudio)
    {
		PrePopAction?.Invoke();

        switch (ExitMode)
        {
            case EntryMode.SLIDE:
                SlideOut(PlayAudio);
                break;
            case EntryMode.ZOOM:
                ZoomOut(PlayAudio);
                break;
            case EntryMode.FADE:
                FadeOut(PlayAudio);
                break;
        }
    }

    private void SlideIn(bool PlayAudio)
    {
        if (AnimationCoroutine != null)
        {
            StopCoroutine(AnimationCoroutine);
        }
        AnimationCoroutine = StartCoroutine(AnimationHelper.SlideIn(RectTransform, EntryDirection, AnimationSpeed, PostPushAction));

        PlayEntryClip(PlayAudio);
    }

    private void SlideOut(bool PlayAudio)
    {
        if (AnimationCoroutine != null)
        {
            StopCoroutine(AnimationCoroutine);
        }
        AnimationCoroutine = StartCoroutine(AnimationHelper.SlideOut(RectTransform, ExitDirection, AnimationSpeed, PostPopAction));

        PlayExitClip(PlayAudio);
    }
    
    private void ZoomIn(bool PlayAudio)
    {
        if (AnimationCoroutine != null)
        {
            StopCoroutine(AnimationCoroutine);
        }
        AnimationCoroutine = StartCoroutine(AnimationHelper.ZoomIn(RectTransform, AnimationSpeed, PostPushAction));

        PlayEntryClip(PlayAudio);
    }

    private void ZoomOut(bool PlayAudio)
    {
        if (AnimationCoroutine != null)
        {
            StopCoroutine(AnimationCoroutine);
        }
        AnimationCoroutine = StartCoroutine(AnimationHelper.ZoomOut(RectTransform, AnimationSpeed, PostPopAction));

        PlayExitClip(PlayAudio);
    }

    private void FadeIn(bool PlayAudio)
    {
        if (AnimationCoroutine != null)
        {
            StopCoroutine(AnimationCoroutine);
        }
        AnimationCoroutine = StartCoroutine(AnimationHelper.FadeIn(CanvasGroup, AnimationSpeed, PostPushAction));

        PlayEntryClip(PlayAudio);
    }

    private void FadeOut(bool PlayAudio)
    {
        if (AnimationCoroutine != null)
        {
            StopCoroutine(AnimationCoroutine);
        }
        AnimationCoroutine = StartCoroutine(AnimationHelper.FadeOut(CanvasGroup, AnimationSpeed, PostPopAction));

        PlayExitClip(PlayAudio);
    }

    private void PlayEntryClip(bool PlayAudio)
    {
        if (PlayAudio && EntryClip != null && AudioSource != null)
        {
            if (AudioCoroutine != null)
            {
                StopCoroutine(AudioCoroutine);
            }

            AudioCoroutine = StartCoroutine(PlayClip(EntryClip));
        }
    }
    
    private void PlayExitClip(bool PlayAudio)
    {
        if (PlayAudio && ExitClip != null && AudioSource != null)
        {
            if (AudioCoroutine != null)
            {
                StopCoroutine(AudioCoroutine);
            }

            AudioCoroutine = StartCoroutine(PlayClip(ExitClip));
        }
    }

    private IEnumerator PlayClip(AudioClip Clip)
    {
        AudioSource.enabled = true;

        WaitForSeconds Wait = new WaitForSeconds(Clip.length);

        AudioSource.PlayOneShot(Clip);

        yield return Wait;

        AudioSource.enabled = false;
    }
}
