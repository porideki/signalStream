using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniRx;

namespace Assets.Scripts.porideki.parts {
    class Gate {

        public Gate() {

            Observable.EveryUpdate().Subscribe(_ => this.Process());

        }
        protected virtual void Process() { }


    }
}
