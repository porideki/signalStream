using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.porideki.parts;

namespace Assets.Scripts.porideki.gates {
    class FunctionSensor<R> : Gate {

        public OutputSocket<R> outputSocket;

        private Func<R> function;

        public FunctionSensor() : this(() => default(R)) { }

        public FunctionSensor(Func<R> function) {

            this.outputSocket = new OutputSocket<R>();

            this.SetFunction(function);

        }

        protected override void Process() {
            base.Process();

            this.outputSocket.Set(this.Execute());
        }

        private R Execute() {
            return this.function();
        }

        public void SetFunction(Func<R> function) {
            this.function = function;
        }

        internal override object[] GetInputSockets() {
            return new object[0];
        }

        internal override object[] GetOutputSockets() {
            return new object[0];
        }

    }
}
