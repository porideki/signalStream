﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.porideki.parts;

namespace Assets.Scripts.porideki.gates {

    class MultiplicationGate : Gate {

        public InputSocket<double> multiplierSocket0;
        public InputSocket<double> multiplierSocket1;
        public OutputSocket<double> resultSocket;

        public MultiplicationGate() {
            this.multiplierSocket0 = new InputSocket<double>();
            this.multiplierSocket1 = new InputSocket<double>();
            this.resultSocket = new OutputSocket<double>();
        }

        protected override void Process() {
            base.Process();

            this.resultSocket.Set(this.multiplierSocket0.Get() * this.multiplierSocket1.Get());

        }

        internal override object[] GetInputSockets() {
            return new object[] { this.multiplierSocket0, this.multiplierSocket1 };
        }

        internal override object[] GetOutputSockets() {
            return new object[] { this.resultSocket };
        }

    }
}
