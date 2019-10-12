using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;


namespace Assets.Scripts.porideki.parts {
    public class InputSocket<T> {

        private List<OutputSocket<T>> outputSockets;
        private ReactiveProperty<T> valuePorperty;



        public InputSocket() {

            

        }

    }

}
