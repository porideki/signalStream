using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;


namespace Assets.Scripts.porideki.parts {
    public class InputSocket<T> {

        private ReactiveProperty<T> valueProperty;
        //購読のみ
        public IReadOnlyReactiveProperty<T> readOnlyValueProperty {
            get { return this.valueProperty; }
        }

        public InputSocket(T initialValue) {

            this.valueProperty = new ReactiveProperty<T>();
            this.Set(initialValue);

        }

        public T Get() {
            return this.valueProperty.Value;
        }


        public void Set(T value) {
            this.valueProperty.Value = value;
        }

        /// <summary>
        /// ChangeListener的扱い
        /// </summary>
        /// <param name="action"></param>
        public void Subscribe(Action<T> action) {
            this.valueProperty.Subscribe(action);
        }

    }

}
