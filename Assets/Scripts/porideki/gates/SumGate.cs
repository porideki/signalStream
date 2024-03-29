﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.porideki.parts;

namespace Assets.Scripts.porideki.gates {

    class SumGate : Gate {

        public InputSocket<double> addendSocket0;
        public InputSocket<double> addendSocket1;
        public OutputSocket<double> resultSocket;

        public SumGate() {
            this.addendSocket0 = new InputSocket<double>();
            this.addendSocket1 = new InputSocket<double>();
            this.resultSocket = new OutputSocket<double>();
        }

        protected override void Process() {
            base.Process();

            this.resultSocket.Set(this.addendSocket0.Get() + this.addendSocket1.Get());

        }

        internal override object[] GetInputSockets() {
            return new object[] { this.addendSocket0, this.addendSocket1 };
        }

        internal override object[] GetOutputSockets() {
            return new object[] { this.resultSocket };
        }

    }
}
