using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.porideki.parts;

using UnityEngine;

namespace Assets.Scripts.porideki.gates {
    class FunctionGate<T, R> : Gate{

        public InputSocket<T> inputSocket;
        public OutputSocket<R> outputSocket;

        private Func<T, R> function;

        public FunctionGate() : this((value) => default(R)) { }

        public FunctionGate(Func<T, R> function) {

            this.inputSocket = new InputSocket<T>();
            this.outputSocket = new OutputSocket<R>();

            this.SetFunction(function);

        }

        protected override void Process() {
            base.Process();

            this.outputSocket.Set(this.Execute(this.inputSocket.Get()));
        }

        private R Execute(T arg) {
            return this.function(arg);
        }

        public void SetFunction(Func<T, R> function) {
            this.function = function;
        }

        internal override object[] GetInputSockets() {
            return new object[] { this.inputSocket };
        }

        internal override object[] GetOutputSockets() {
            return new object[] { this.outputSocket };
        }

    }
}
