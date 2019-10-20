using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.porideki.parts;

namespace Assets.Scripts.porideki.gates {
    class DifferenceGate : Gate {

        public InputSocket<double> minuendSocket;
        public InputSocket<double> subtrahendSocket;
        public OutputSocket<double> resultSocket;

        private SumGate addGate;
        private MultiplicationGate mulGate;

        public DifferenceGate() {

            //インスタンス
            this.minuendSocket = new InputSocket<double>();
            this.subtrahendSocket = new InputSocket<double>();

            this.mulGate = new MultiplicationGate();
            this.addGate = new SumGate();

            //ソケット設定
            this.resultSocket = this.addGate.resultSocket;  //結果出力ソケット
            this.addGate.addendSocket0 = this.minuendSocket;    //被減数入力ソケット
            this.mulGate.resultSocket.inputSockets.Add(this.addGate.addendSocket1); //被減数反転結果
            this.mulGate.multiplierSocket0 = this.subtrahendSocket; //減数入力ソケット
            this.mulGate.multiplierSocket1.Set(-1); //反転

        }

        internal override NormalizedInputSocket[] GetInputSockets() {
            return new NormalizedInputSocket[] { (NormalizedInputSocket)this.minuendSocket, (NormalizedInputSocket)this.subtrahendSocket };
        }

        internal override NormalizedOutputSocket[] GetOutputSockets() {
            return new NormalizedOutputSocket[] { (NormalizedOutputSocket)this.resultSocket };
        }

    }
}
