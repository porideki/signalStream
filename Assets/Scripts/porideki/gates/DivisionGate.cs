using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.porideki.parts;

namespace Assets.Scripts.porideki.gates {
    class DivisionGate : Gate {

        public InputSocket<double> dividendSocket;
        public InputSocket<double> divisorSocket;
        public InputSocket<double> defaultSocket;
        public OutputSocket<double> resultSocket;

        public DivisionGate() {

            this.dividendSocket = new InputSocket<double>(0);
            this.divisorSocket = new InputSocket<double>(0);
            this.defaultSocket = new InputSocket<double>(0);
            this.resultSocket = new OutputSocket<double>(0);

        }

        protected override void Process() {
            base.Process();

            if(this.divisorSocket.Get() != 0) {
                this.resultSocket.Set(this.dividendSocket.Get() / this.divisorSocket.Get());
            } else {
                this.resultSocket.Set(this.defaultSocket.Get());
            }

        }

    }
}
