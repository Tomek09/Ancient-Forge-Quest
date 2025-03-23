using System;
using AncientForgeQuest.Managers;
using UnityEngine;
namespace AncientForgeQuest.UI.Quests
{
    public class QuestViewController : MonoBehaviour
    {
        [SerializeField] private QuestInstanceView _viewPrefab;
        [SerializeField] private Transform _viewParent;
        
        private void Start()
        {
            var quests = QuestsManager.Instance.ActiveQuests;
            foreach (var quest in quests)
            {
                var view = Instantiate(_viewPrefab, _viewParent);
                view.Bind(quest);
            }
        }
    }
}
