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

        internal ReactiveProperty<bool> IsRunning;

        public Circuit() {

            this.gates = new ReactiveCollection<Gate>();
            this.IsRunning = new ReactiveProperty<bool>(false); //初期状態は「停止」

            //動作状態リスナー
            this.IsRunning.Subscribe(isRunning => {
                if (isRunning) {
                    //起動
                    foreach (Gate gate in this.gates) {
                        gate.Start();
                    }
                } else {
                    //停止
                    foreach (Gate gate in this.gates) {
                        gate.Stop();
                    }
                }
            });

        }

        #region ソケット
        //型引数一つのジェネリクス -> ソケット型である事前提のゴリ押し
        //コネクション確認
        public static bool HasConnection(object inputSocketObj, object outputSocketObj) {

            switch (inputSocketObj) {
                //double型ソケット
                case InputSocket<double> inputSocket:
                    if (outputSocketObj is OutputSocket<double>) {
                        var outputSocket = outputSocketObj as OutputSocket<double>;
                        return outputSocket.inputSockets.Contains(inputSocket);
                    }
                    break;

                //bool型ソケット
                case InputSocket<bool> inputSocket:
                    if (outputSocketObj is OutputSocket<bool>) {
                        var outputSocket = outputSocketObj as OutputSocket<bool>;
                        return outputSocket.inputSockets.Contains(inputSocket);
                    }
                    break;

                default:
                    Debug.LogError("ソケットの型に対する処理が未定義、又はソケット型ではありません");
                    break;
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
                    if (outputSocketObj is OutputSocket<double>) {
                        var outputSocket = outputSocketObj as OutputSocket<double>;
                        outputSocket.inputSockets.Add(inputSocket); //追加
                        inputSocket.parentSocket.Value = outputSocket;
                        return true;
                    }
                    break;

                //bool型ソケット
                case InputSocket<bool> inputSocket:
                    if (outputSocketObj is OutputSocket<bool>) {
                        var outputSocket = outputSocketObj as OutputSocket<bool>;
                        outputSocket.inputSockets.Add(inputSocket); //追加
                        inputSocket.parentSocket.Value = outputSocket;
                        return true;
                    }
                    break;

                default:
                    System.Type[] inSockType = inputSocketObj.GetType().GetGenericArguments();
                    System.Type[] ouSockType = outputSocketObj.GetType().GetGenericArguments();
                    Debug.LogError("ソケットの型に対する処理が未定義、又はソケット型ではありません: " + inSockType + " -> " + outputSocketObj);
                    break;
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
                        if (outputSocketObj is OutputSocket<double>) {
                            var outputSocket = outputSocketObj as OutputSocket<double>;
                            outputSocket.inputSockets.Remove(inputSocket);  //親から削除
                            inputSocket.parentSocket.Value = null;  //子から削除
                            return true;
                        }
                        break;

                    //bool型
                    case InputSocket<bool> inputSocket:
                        if (outputSocketObj is OutputSocket<bool>) {
                            var outputSocket = outputSocketObj as OutputSocket<bool>;
                            outputSocket.inputSockets.Remove(inputSocket);  //親から削除
                            inputSocket.parentSocket.Value = null;  //子から削除 
                            return true;
                        }
                        break;

                    default:
                        Debug.LogError("ソケットの型に対する処理が未定義、又はソケット型ではありません");
                        break;
                }

            }

            //コネクションが確立されていない
            Debug.Log("コネクションが存在しません");
            return false;

        }

        public static bool RemoveConnection(object socketObj) { //IOソケット両対応

            //InputSocket
            switch (socketObj) {
                //double型
                case InputSocket<double> inputSocket:
                    if (inputSocket.parentSocket.HasValue) {    //親取得
                        var outputSocket = inputSocket.parentSocket.Value;
                        return Circuit.RemoveConnection(inputSocket, outputSocket); //削除
                    } else return false;

                //bool型
                case InputSocket<bool> inputSocket:
                    if (inputSocket.parentSocket.HasValue) {    //親取得
                        var outputSocket = inputSocket.parentSocket.Value;
                        return Circuit.RemoveConnection(inputSocket, outputSocket); //削除
                    } else return false;

                default:
                    break;
            }

            //OutputSocket
            switch (socketObj) {
                //double型
                case OutputSocket<double> outputSocket: {
                        //foreach内でコレクションを編集しない
                        var InputSocketsBuffer = new List<InputSocket<double>>();
                        foreach (InputSocket<double> inputSocket in outputSocket.inputSockets) {
                            InputSocketsBuffer.Add(inputSocket);    //送信先INソケット
                        }
                        foreach (InputSocket<double> inputSocket in InputSocketsBuffer) {
                            Circuit.RemoveConnection(inputSocket, outputSocket);
                        }
                        return true;
                    }

                //bool型
                case OutputSocket<bool> outputSocket: {
                        var InputSocketsBuffer = new List<InputSocket<bool>>();
                        foreach (InputSocket<bool> inputSocket in outputSocket.inputSockets) {
                            InputSocketsBuffer.Add(inputSocket);    //送信先INソケット
                        }
                        foreach (InputSocket<bool> inputSocket in InputSocketsBuffer) {
                            Circuit.RemoveConnection(inputSocket, outputSocket);
                        }
                        return true;
                    }

                default:
                    break;
            }

            return false;

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
                //動作状態設定
                if (this.IsRunning.Value) gate.Start();
                else gate.Stop();
                Debug.Log("Gateを追加しました\nGateNum: " + this.gates.Count);
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
                //オブジェクト破棄
                gate.Dispose();
                //Log
                Debug.Log("Gateを削除しました\nGateNum: " + this.gates.Count);
                return true;
            } else {
                Debug.Log("指定したGateが見つかりませんでした");
                return false;
            }

        }

        #endregion

        internal void Start() {
            this.IsRunning.Value = true;
        }

        internal void Stop() {
            this.IsRunning.Value = false;
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
