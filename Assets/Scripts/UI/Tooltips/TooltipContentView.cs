using System;
using AncientForgeQuest.Instances;
using AncientForgeQuest.UI.DesignSystem;
using AncientForgeQuest.Utility;
using PrimeTween;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AncientForgeQuest.UI.Tooltips
{
    public class TooltipContentView : BindableBehaviour<TooltipContent>
    {
        [System.Serializable]
        private struct TweenSettings
        {
            public float Duration;
            public Ease Ease;
        }
        
        [Header("Components")]
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private TMP_Text _headerText;
        [SerializeField] private TMP_Text _descriptionText;
        [SerializeField] private TweenSettings _fadeInSettings;
        [SerializeField] private TweenSettings _fadeOutSettings;
        private bool _isActive;
        private Tween _tween;

        private void Start()
        {
            Hide();
        }

        private void Update()
        {
            if (!_isActive)
                return;
            
            var position = Mouse.current.position.ReadValue();
            float pivotX = position.x / Screen.width;
            float pivotY = position.y / Screen.height;
            
            _rectTransform.pivot = new Vector2(pivotX, pivotY);
            transform.position = position;
        }

        protected override void OnBind()
        {
            if (Model == null)
                return;

            _headerText.SetText(Model.Header);
            _descriptionText.SetText(Model.Description);
        }

        public void Show()
        {
            Fade(_fadeInSettings, 1);
            _isActive = true;
        }

        public void Hide()
        {
            Fade(_fadeOutSettings, 0);
            _isActive = false;
        }

        private void Fade(TweenSettings settings, float endValue)
        {
            _tween.Complete();
            var startValue = _canvasGroup.alpha;
            _tween = Tween.Custom(startValue, endValue, settings.Duration, x =>
            {
                _canvasGroup.alpha = x;
            }, settings.Ease);
        }
    }
}
