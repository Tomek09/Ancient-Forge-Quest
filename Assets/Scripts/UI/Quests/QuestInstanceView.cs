using AncientForgeQuest.Instances;
using AncientForgeQuest.UI.DesignSystem;
using R3;
using TMPro;
using UnityEngine;

namespace AncientForgeQuest.UI.Quests
{
    public class QuestInstanceView : BindableBehaviour<QuestInstance>
    {
        [Header("Components")]
        [SerializeField] private TMP_Text _label;
        [SerializeField] private TMP_Text _description;
        [SerializeField] private ProgressBar _progressBar;
        private bool _isInitialized;

        protected override void OnBind()
        {
            if (Model == null)
                return;

            _label.SetText(Model.BaseModel.Label);
            var description = Model.GetDescription();
            _description.SetText(description);
            Model.Progress.Subscribe(OnProgressChange).AddTo(_disposables);
        }

        private void OnProgressChange(int _)
        {
            float percentage = Model.GetCompletionProgress();
            _progressBar.SetFill(percentage, !_isInitialized);

            if (!_isInitialized)
            {
                _isInitialized = true;
            }
        }
    }
}
