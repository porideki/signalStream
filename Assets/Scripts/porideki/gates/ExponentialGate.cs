using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.porideki.parts;

namespace Assets.Scripts.porideki.gates {
    class ExponentialGate : Gate {

        public InputSocket<double> baseSocket;
        public InputSocket<double> exponentSocket;
        public InputSocket<double> defaultSocket;
        public OutputSocket<double> resultSocket;

        public ExponentialGate() {

            this.baseSocket = new InputSocket<double>();
            this.exponentSocket = new InputSocket<double>();
            this.defaultSocket = new InputSocket<double>();
            this.resultSocket = new OutputSocket<double>();

        }

        protected override void Process() {
            base.Process();

            if(this.baseSocket.Get() != 0) {    //底が0でない時
                this.resultSocket.Set(Math.Pow(this.baseSocket.Get(), this.exponentSocket.Get()));
            } else {    //底が0の時
                this.resultSocket.Set(this.defaultSocket.Get());
            }

        }

        internal override NormalizedInputSocket[] GetInputSockets() {
            return new NormalizedInputSocket[] { (NormalizedInputSocket)this.baseSocket, (NormalizedInputSocket)this.exponentSocket, (NormalizedInputSocket)this.defaultSocket };
        }

        internal override NormalizedOutputSocket[] GetOutputSockets() {
            return new NormalizedOutputSocket[] { (NormalizedOutputSocket)this.resultSocket };
        }

    }
}
