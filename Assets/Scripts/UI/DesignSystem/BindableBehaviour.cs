using System;
using R3;
using UnityEngine;

namespace AncientForgeQuest.UI.DesignSystem
{
    public abstract class BindableBehaviour<T> : MonoBehaviour, IDisposable
    {
        public T Model { get; private set; }

        protected readonly CompositeDisposable _disposables = new CompositeDisposable();
        
        public void Bind(T model)
        {
            Model = model;
            OnBind();
        }

        protected abstract void OnBind();
        
        public void Dispose()
        {
            _disposables?.Dispose();
        }
    }
}
