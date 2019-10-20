using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniRx;

using Assets.Scripts.porideki.parts;

namespace Assets.Scripts.porideki.parts {
    class Gate {

        public Gate() {

            Observable.EveryUpdate().Subscribe(_ => this.Process());

        }
        protected virtual void Process() { }

        internal virtual NormalizedInputSocket[] GetInputSockets() {
            return new NormalizedInputSocket[0];
        }

        internal virtual NormalizedOutputSocket[] GetOutputSockets() {
            return new NormalizedOutputSocket[0];
        }


    }
}
