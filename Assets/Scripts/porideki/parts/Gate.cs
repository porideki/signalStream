using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniRx;

using Assets.Scripts.porideki.parts;

namespace Assets.Scripts.porideki.parts {
    abstract class Gate {

        private bool isRun;
        public bool IsRun {
            get { return this.isRun; }
        }

        public Gate() {

            this.isRun = false;

            Observable.EveryUpdate()
                .Where(f => this.isRun)
                .Subscribe(_ => this.Process());

        }

        protected virtual void Process() { }

        private void setRun(bool isRun) {
            this.isRun = isRun;
        }

        public void Start() {
            this.setRun(true);
        }
        public void Stop() {
            this.setRun(false);
        }

        internal abstract object[] GetInputSockets();

        internal abstract object[] GetOutputSockets();


    }
}
