using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.porideki.parts;
using UnityEngine;

namespace Assets.Scripts.porideki.gates {
    class UnfoldGate : Gate{

        public InputSocket<bool> valueSocket;
        public InputSocket<double> trueValueSocket;
        public InputSocket<double> falseValueSocket;
        public OutputSocket<double> resultSocket;

        public UnfoldGate() : this(1, 0) { }

        public UnfoldGate(double trueValue, double falseValue) {

            this.valueSocket = new InputSocket<bool>();
            this.trueValueSocket = new InputSocket<double>(trueValue);
            this.falseValueSocket = new InputSocket<double>(falseValue);
            this.resultSocket = new OutputSocket<double>();

            //応急
            this.trueValueSocket.Set(trueValue);
            this.falseValueSocket.Set(falseValue);

        }

        protected override void Process() {
            base.Process();

            this.resultSocket.Set(this.valueSocket.Get() 
                ? this.trueValueSocket.Get()
                : this.falseValueSocket.Get());
        }

        internal override object[] GetInputSockets() {
            return new object[] { this.valueSocket, this.trueValueSocket, this.falseValueSocket };
        }

        internal override object[] GetOutputSockets() {
            return new object[] { this.resultSocket };
        }

    }
}
