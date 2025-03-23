using AncientForgeQuest.Models;
using UnityEngine;

namespace AncientForgeQuest.UI.DesignSystem
{
    public abstract class BindableBehaviour<T> : MonoBehaviour
    {
        public T Model { get; private set; }

        public void Bind(T model)
        {
            Model = model;
            OnBind();
        }

        protected abstract void OnBind();
    }
}
