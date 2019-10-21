using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

using Assets.Scripts.porideki.parts;

namespace Assets.Scripts.porideki.circuit {

    public class Circuit {

        internal ReactiveCollection<Gate> gates;
        internal object[] inputScokets {
            get {
                var list = new List<object>();
                foreach (Gate gate in this.gates) {
                    list.Add(gate.GetInputSockets());
                }
                return list.ToArray();
            }
        }
        internal object[] outputSockets {
            get {
                var list = new List<object>();
                foreach (Gate gate in this.gates) {
                    list.Add(gate.GetOutputSockets());
                }
                return list.ToArray();
            }
        }

        internal Connection[] connections {
            get {
                var list = new List<Connection>();
                //ゴリ押し(全比較)
                foreach(object outputSocket in this.outputSockets) {
                    foreach (object inputsocket in this.inputScokets) {
                        if (CircuitProcessor.HasConnection(inputScokets, outputSocket)) 
                            list.Add(Connection.CreateConnectionObject(inputsocket, outputSocket));
                    }
                }
                return list.ToArray();
            }
        }

        public Circuit() {

            //インスタンス
            this.gates = new ReactiveCollection<Gate>();

        }

    }

    //コネクションを表現する
    public class Connection {

        public object inputSocket;
        public object outputSocket;

        private Connection(object inputSocket, object outputSocket) {
            this.inputSocket = inputSocket;
            this.outputSocket = outputSocket;
        }

        public static Connection CreateConnectionObject(object inputSocket, object outputSocket) {

            //コネクションなし
            if (!CircuitProcessor.HasConnection(inputSocket, outputSocket)) return null;

            return new Connection(inputSocket, outputSocket);

        }

    }

}


