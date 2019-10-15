using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniRx;

namespace Assets.Scripts.porideki.parts {

    public class OutputSocket<T> {

        //値バッファ
        private ReactiveProperty<T> valueBuffer;

        //送信先
        public List<InputSocket<T>> inputSockets;

        public OutputSocket(T value) {

            //インスタンス生成
            this.valueBuffer = new ReactiveProperty<T>();
            this.inputSockets = new List<InputSocket<T>>();

            //初期値
            this.Set(value);

            //リスナ
            this.valueBuffer.Subscribe(v => this.Flush(v));

        }

        //既定値で初期化
        public OutputSocket() : this(default(T)) { }

        private void Flush(T value) {
            foreach (InputSocket<T> inputSocket in this.inputSockets) {
                inputSocket.Set(value);
            }
        }

        public void Set(T value) {
            this.valueBuffer.Value = value;
        }

    }

}
