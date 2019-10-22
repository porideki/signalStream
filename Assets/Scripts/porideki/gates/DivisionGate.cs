using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Assets.Scripts.porideki.parts;

namespace Assets.Scripts.porideki.gates {
    class DivisionGate : Gate {

        public InputSocket<double> dividendSocket;
        public InputSocket<double> divisorSocket;
        public InputSocket<double> defaultSocket;
        public OutputSocket<double> resultSocket;
        public OutputSocket<double> remainderSocket;

        public DivisionGate() {

            this.dividendSocket = new InputSocket<double>();
            this.divisorSocket = new InputSocket<double>();
            this.defaultSocket = new InputSocket<double>();
            this.resultSocket = new OutputSocket<double>();
            this.remainderSocket = new OutputSocket<double>();

        }

        protected override void Process() {
            base.Process();

            if(this.divisorSocket.Get() != 0.0) { //除数が0でない時
                this.resultSocket.Set(this.dividendSocket.Get() / this.divisorSocket.Get());
                //負数を含む剰余演算は全て正数に変えて処理する
                this.remainderSocket.Set(Math.Abs(this.dividendSocket.Get()) % Math.Abs(this.divisorSocket.Get()));
            } else {    //除数が0
                this.resultSocket.Set(this.defaultSocket.Get());
                this.remainderSocket.Set(0);
            }

        }

        internal override object[] GetInputSockets() {
            return new object[] { this.dividendSocket, this.divisorSocket, this.defaultSocket };
        }

        internal override object[] GetOutputSockets() {
            return new object[] { this.resultSocket, this.remainderSocket };
        }

    }
}
