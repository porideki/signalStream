using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;


namespace Assets.Scripts.porideki.parts {
    public class InputSocket<T> {

        internal ReactiveProperty<OutputSocket<T>> parentSocket;
        private ReactiveProperty<T> valueProperty;
        //購読のみ
        public IReadOnlyReactiveProperty<T> readOnlyValueProperty {
            get { return this.valueProperty; }
        }

        public InputSocket(T initialValue) {

            //インスタンス
            this.parentSocket = new ReactiveProperty<OutputSocket<T>>();
            this.valueProperty = new ReactiveProperty<T>();

            //値初期化
            this.Set(initialValue);

            //入力元がnullった時
            this.parentSocket.Where(outputSocket => outputSocket == null)
                .Subscribe(outputSocket => this.Set(default(T)));

        }

        //既定値で初期化
        public InputSocket() : this(default(T)) { }

        public T Get() {
            return this.valueProperty.Value;
        }
        
        public void Set(T value) {
            this.valueProperty.Value = value;
        }

        public void Subscribe(Action<T> action) {
            this.valueProperty.Subscribe(action);
        }

    }

}
