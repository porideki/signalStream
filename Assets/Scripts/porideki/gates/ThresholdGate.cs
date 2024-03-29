﻿using System;
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
        public OutputSocket<bool> resultSocket;

        private Range range;

        public ThresholdGate() : this(0, 1) { }

        public ThresholdGate(double minValue, double maxValue){

            //インスタンス
            this.valueSocket = new InputSocket<double>();
            this.minSocket = new InputSocket<double>(minValue);
            this.maxSocket = new InputSocket<double>(maxValue);
            this.resultSocket = new OutputSocket<bool>();

            this.range = new Range(this.minSocket.Get(), this.maxSocket.Get());

            //購読
            this.minSocket.Subscribe(min => this.range.min = min);
            this.maxSocket.Subscribe(max => this.range.max = max);

        }

        protected override void Process() {
            base.Process();

            this.resultSocket.Set(
                    this.range.min <= this.valueSocket.Get() 
                &&  this.valueSocket.Get() <= this.range.max
                    );
        }

        internal override object[] GetInputSockets() {
            return new object[] { this.valueSocket, this.minSocket, this.maxSocket };
        }

        internal override object[] GetOutputSockets() {
            return new object[] { this.resultSocket };
        }

    }
}
