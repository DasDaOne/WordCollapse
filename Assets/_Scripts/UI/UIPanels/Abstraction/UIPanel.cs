using System;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(CanvasGroup))]
public abstract class UIPanel : MonoBehaviour
{	
	protected const float AnimationTime = 0.5f;

	// InstantCallback
	public UnityEvent OnShowEvent { get; } = new ();
	public UnityEvent OnHideEvent { get; } = new ();
	// End InstantCallback
	
	// Components
	private CanvasGroup cachedCanvasGroup;
	protected CanvasGroup AttachedCanvasGroup 
	{
		get
		{
			cachedCanvasGroup ??= GetComponent<CanvasGroup>();
			return cachedCanvasGroup;
		}
	}
	
	private RectTransform cachedRectTransform;
	protected RectTransform AttachedRectTransform 
	{
		get
		{
			cachedRectTransform ??= transform as RectTransform;
			return cachedRectTransform;
		}
	}

	private RectTransform cachedCanvasRT;
	protected RectTransform AttachedCanvasRT
	{
		get
		{
			cachedCanvasRT ??= GetComponentInParent<Canvas>().transform as RectTransform;
			return cachedCanvasRT;
		}
	}
	// End Components
	
	public bool UIPanelState {get; protected set;}
	public bool IsInAnimation {get; private set;}
	
	public void Show(bool notifyPanel = true, bool playAnimation = true)
	{
		UIPanelState = true;

		OnShowEvent.Invoke();
		
		if(notifyPanel)
			OnShow();
			
		ShowPanel(playAnimation);
	}
	
	public void Hide(bool notifyPanel = true, bool playAnimation = true)
	{
		UIPanelState = false;
		
		OnHideEvent.Invoke();
		
		HidePanel(notifyPanel, playAnimation);
	}

	private void ShowPanel(bool playAnimation)
	{
		AttachedCanvasGroup.blocksRaycasts = false;

		if(playAnimation)
		{
			IsInAnimation = true;
		
			ShowPanelAnimated(() =>
			{
				AttachedCanvasGroup.blocksRaycasts = true;
				IsInAnimation = false;
			});
		}
		else
		{
			AttachedCanvasGroup.blocksRaycasts = true;
			ShowPanelInstant();
		}
	}

	protected abstract void ShowPanelAnimated(TweenCallback onComplete);
	protected abstract void ShowPanelInstant();

	private void HidePanel(bool notifyPanel, bool playAnimation)
	{
		AttachedCanvasGroup.blocksRaycasts = false;
		
		if(!playAnimation)
		{
			HidePanelInstant();
					
			if(notifyPanel)
				OnHide();
			
			return;
		}
		
		IsInAnimation = true;
		
		HidePanelAnimated(() =>
		{
			if(notifyPanel)
				OnHide();
			
			IsInAnimation = false;
		});
	}

	protected abstract void HidePanelAnimated(TweenCallback onComplete);
	protected abstract void HidePanelInstant();

	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.Escape) && UIPanelState && !IsInAnimation)
		{
			OnEscapeClick();
		}
	}
	
	protected virtual void OnEscapeClick(){}
	protected virtual void OnHide(){}
	protected virtual void OnShow(){}
	
	#if UNITY_EDITOR
	[ContextMenu("Show Panel")]
	public void EditorShowPanel()
	{
		CanvasGroup cg = GetComponent<CanvasGroup>();
		AttachedCanvasGroup.alpha = 1;
		AttachedCanvasGroup.blocksRaycasts = true;
		EditorUtility.SetDirty(cg);
	}
	
	[ContextMenu("Hide Panel")]
	public void EditorHidePanel()
	{
		CanvasGroup cg = GetComponent<CanvasGroup>();
		AttachedCanvasGroup.alpha = 0;
		AttachedCanvasGroup.blocksRaycasts = false;
		EditorUtility.SetDirty(cg);
	}
	#endif
}
