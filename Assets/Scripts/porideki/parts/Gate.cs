using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniRx;

using Assets.Scripts.porideki.parts;

namespace Assets.Scripts.porideki.parts {
    abstract class Gate {

        public Gate() {

            Observable.EveryUpdate().Subscribe(_ => this.Process());

        }
        protected virtual void Process() { }

        internal abstract object[] GetInputSockets();

        internal abstract object[] GetOutputSockets();


    }
}
