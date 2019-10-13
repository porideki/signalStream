using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniRx;
using Assets.Scripts.porideki.parts;
using Assets.Scripts.porideki.util;

namespace Assets.Scripts.porideki.gates {
    class ThresholdGate : Gate {

        public InputSocket<double> valueSocket;
        public InputSocket<double> minSocket;
        public InputSocket<double> maxSocket;
        public OutputSocket<bool> resultProperty;

        private Range range;

        public ThresholdGate(){

            //インスタンス
            this.valueSocket = new InputSocket<double>(0);
            this.minSocket = new InputSocket<double>(0);
            this.maxSocket = new InputSocket<double>(10);
            this.resultProperty = new OutputSocket<bool>(true);

            this.range = new Range(this.minSocket.Get(), this.maxSocket.Get());

            //購読
            this.minSocket.Subscribe(min => this.range.min = min);
            this.maxSocket.Subscribe(max => this.range.max = max);

        }

        protected override void Process() {
            base.Process();

            this.resultProperty.Set(
                    this.range.min <= this.valueSocket.Get() 
                &&  this.valueSocket.Get() <= this.range.max
                    );
        }

    }
}
