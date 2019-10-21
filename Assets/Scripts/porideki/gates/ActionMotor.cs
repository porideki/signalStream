using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Assets.Scripts.porideki.parts;

namespace Assets.Scripts.porideki.gates {
    class ActionMotor<T> : Gate {

        public InputSocket<T> inputSocket;

        private Action<T> action;

        public ActionMotor(Action<T> action) {

            this.inputSocket = new InputSocket<T>();

            this.SetAction(action);

        }

        protected override void Process() {
            base.Process();

            this.Execute(this.inputSocket.Get());
        }

        private void Execute(T arg) {
            this.action(arg);
        }

        public void SetAction(Action<T> action) {
            this.action = action;
        }

        internal override object[] GetInputSockets() {
            return new object[] { this.inputSocket };
        }

        internal override object[] GetOutputSockets() {
            return new object[0];
        }

    }
}
