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
                foreach (object outputSocket in this.outputSockets) {
                    foreach (object inputsocket in this.inputScokets) {
                        if (Circuit.HasConnection(inputScokets, outputSocket))
                            list.Add(Connection.CreateConnectionObject(inputsocket, outputSocket));
                    }
                }
                return list.ToArray();
            }
        }

        public Circuit() {

            this.gates = new ReactiveCollection<Gate>();

        }

        #region ソケット
        //型引数一つのジェネリクス -> ソケット型である事前提のゴリ押し
        //コネクション確認
        public static bool HasConnection(object inputSocketObj, object outputSocketObj) {

            switch (inputSocketObj) {
                //double型ソケット
                case InputSocket<double> inputSocket:
                    return Circuit.HasConnection<double>(inputSocket, outputSocketObj);

                //bool型ソケット
                case InputSocket<bool> inputSocket:
                    return Circuit.HasConnection<bool>(inputSocket, outputSocketObj);

                default:
                    Debug.LogError("ソケットの型に対する処理が未定義、又はソケット型ではありません");
                    break;
            }

            return false;

        }

        private static bool HasConnection<T>(InputSocket<T> inputSocket, object outputSocketObj) {
            if(outputSocketObj is OutputSocket<T>) {
                var outputSocket = outputSocketObj as OutputSocket<T>;
                return outputSocket.inputSockets.Contains(inputSocket);
            }
            return false;
        }

        //コネクション作成
        public static bool MakeConnection(object inputSocketObj, object outputSocketObj) {

            //既にコネクションが確立されている
            if (Circuit.HasConnection(inputSocketObj, outputSocketObj)) {
                Debug.Log("既にコネクションが確立されています");
                return false;
            }

            //コネクション作成
            switch (inputSocketObj) {
                //double型ソケット
                case InputSocket<double> inputSocket:
                    return Circuit.MakeConnection<double>(inputSocket, outputSocketObj);

                //bool型ソケット
                case InputSocket<bool> inputSocket:
                    return Circuit.MakeConnection<bool>(inputSocket, outputSocketObj);

                default:
                    System.Type[] inSockType = inputSocketObj.GetType().GetGenericArguments();
                    System.Type[] outSockType = outputSocketObj.GetType().GetGenericArguments();
                    Debug.LogError("ソケットの型に対する処理が未定義、又はソケット型ではありません: " + inSockType + " -> " + outputSocketObj);
                    break;
            }

            return false;

        }

        //親子設定
        private static bool MakeConnection<T>(InputSocket<T> inputSocket, object outputSocketObj) {
            if(outputSocketObj is OutputSocket<T>) {
                var outputSocket = outputSocketObj as OutputSocket<T>;
                outputSocket.inputSockets.Add(inputSocket); //追加
                inputSocket.parentSocket.Value = outputSocket;
                return true;
            }
            return false;
        }

        //
        public static bool RemoveConnection(object inputSocketObj, object outputSocketObj) {

            //コネクションが確立されている
            if (Circuit.HasConnection(inputSocketObj, outputSocketObj)) {

                switch (inputSocketObj) {
                    //double型
                    case InputSocket<double> inputSocket:
                        return Circuit.RemoveConnection<double>(inputSocket, outputSocketObj);

                    //bool型
                    case InputSocket<bool> inputSocket:
                        return Circuit.RemoveConnection<bool>(inputSocket, outputSocketObj);

                    default:
                        Debug.LogError("ソケットの型に対する処理が未定義、又はソケット型ではありません");
                        break;
                }

            }

            //コネクションが確立されていない
            Debug.Log("コネクションが存在しません");
            return false;

        }

        private static bool RemoveConnection<T>(InputSocket<T> inputSocket, object outputSocketObj) {
            if (outputSocketObj is OutputSocket<T>) {
                var outputSocket = outputSocketObj as OutputSocket<T>;
                outputSocket.inputSockets.Remove(inputSocket);  //親から削除
                inputSocket.parentSocket.Value = null;  //子から削除 
                return true;
            }
            return false;
        }

        public static bool RemoveConnection(object socketObj) { //IOソケット両対応

            //InputSocket
            switch (socketObj) {
                //double型
                case InputSocket<double> inputSocket:
                    return Circuit.RemoveConnection<double>(inputSocket);

                //bool型
                case InputSocket<bool> inputSocket:
                    return Circuit.RemoveConnection<bool>(inputSocket);

                default:
                    break;
            }

            //OutputSocket
            switch (socketObj) {
                //double型
                case OutputSocket<double> outputSocket:
                    return Circuit.RemoveConnection<double>(outputSocket);

                //bool型
                case OutputSocket<bool> outputSocket:
                    return Circuit.RemoveConnection<bool>(outputSocket);

                default:
                    break;
            }

            return false;

        }

        private static bool RemoveConnection<T>(InputSocket<T> inputSocket) {
            if (inputSocket.parentSocket.HasValue) {    //親取得
                var outputSocket = inputSocket.parentSocket.Value;
                return Circuit.RemoveConnection(inputSocket, outputSocket); //削除
            } else {
                return false;
            }
        }

        private static bool RemoveConnection<T>(OutputSocket<T> outputSocket) {
            //foreach内でコレクションを編集しない
            var InputSocketsBuffer = new List<InputSocket<T>>();
            foreach (InputSocket<T> inputSocket in outputSocket.inputSockets) {
                InputSocketsBuffer.Add(inputSocket);    //送信先INソケット
            }
            foreach (InputSocket<T> inputSocket in InputSocketsBuffer) {
                Circuit.RemoveConnection(inputSocket, outputSocket);
            }
            return true;
        }

        #endregion

        #region ゲート

        //捜査
        internal bool ContainsGate(Gate gate) {
            return this.gates.Contains(gate);
        }

        //追加
        internal bool AddGate(Gate gate) {

            //既に追加されているか確認
            if (this.ContainsGate(gate)) {
                Debug.LogError("Gateは既に追加されています");
                return false;
            } else {
                this.gates.Add(gate);
                return true;
            }

        }

        internal bool RemoveGate(Gate gate) {

            if (this.ContainsGate(gate)) {
                //コネクション削除
                foreach (object inputSocket in gate.GetInputSockets()) {
                    Circuit.RemoveConnection(inputSocket);
                }
                foreach (object outputSocket in gate.GetOutputSockets()) {
                    Circuit.RemoveConnection(outputSocket);
                }
                //Gate削除
                this.gates.Remove(gate);
                return true;
            } else {
                Debug.Log("指定したGateが見つかりませんでした");
                return false;
            }

        }

        #endregion

        internal void Start() {
            foreach(Gate gate in this.gates) {
                gate.Start();
            }
        }

        internal void Stop() {
            foreach (Gate gate in this.gates) {
                gate.Stop();
            }
        }

    }

    public class Connection {

        public object inputSocket;
        public object outputSocket;

        private Connection(object inputSocket, object outputSocket) {
            this.inputSocket = inputSocket;
            this.outputSocket = outputSocket;
        }

        public static Connection CreateConnectionObject(object inputSocket, object outputSocket) {

            //コネクションなし
            if (!Circuit.HasConnection(inputSocket, outputSocket)) return null;

            return new Connection(inputSocket, outputSocket);

        }

    }

}
