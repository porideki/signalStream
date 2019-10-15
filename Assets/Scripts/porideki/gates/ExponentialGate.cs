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

            this.baseSocket = new InputSocket<double>(0);
            this.exponentSocket = new InputSocket<double>(0);
            this.defaultSocket = new InputSocket<double>(1);
            this.resultSocket = new OutputSocket<double>(0);

        }

        protected override void Process() {
            base.Process();

            if(this.baseSocket.Get() != 0) {    //底が0でない時
                this.resultSocket.Set(Math.Pow(this.baseSocket.Get(), this.exponentSocket.Get()));
            } else {    //底が0の時
                this.resultSocket.Set(this.defaultSocket.Get());
            }

        }

    }
}
