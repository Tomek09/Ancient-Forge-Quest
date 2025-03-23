using PrimeTween;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AncientForgeQuest.UI.DesignSystem
{
    public class ProgressBar : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private TMP_Text[] _texts;
        [SerializeField] private Image _fillbar;

        [Header("Settings")]
        [SerializeField] private float _duration;
        [SerializeField] private Ease _ease;
        private Tween _tween;
        
        public void SetFill(float value, bool fastFill)
        {
            if (fastFill)
            {
                _tween.Complete();
                UpdateFillBar(value);
                UpdateText(value);
                return;
            }

            _tween.Complete();
            var startValue = _fillbar.fillAmount;
            _tween = Tween.Custom(startValue, value, _duration, x =>
            {
                UpdateFillBar(x);
                UpdateText(x);
            }, _ease);
        }
        
        private void UpdateFillBar(float value)
        {
            _fillbar.fillAmount = value;
        }
        
        private void UpdateText(float value)
        {
            var text = $"{Mathf.RoundToInt(value * 100)}%";
            foreach (var t in _texts)
            {
                t.SetText(text);
            }
        }
    }
}
