using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UniRx;

namespace Assets.Scripts.porideki.util {
    class Range {

        private ReactiveProperty<double> minProperty;
        private ReactiveProperty<double> maxProperty;

        public double min {
            get { return this.minProperty.Value; }
            set { this.minProperty.Value = value; }
        }
        public double max {
            get { return this.maxProperty.Value; }
            set { this.maxProperty.Value = value; }
        }

        public Range(double min, double max) {

            this.minProperty = new ReactiveProperty<double>(min);
            this.maxProperty = new ReactiveProperty<double>(max);

            this.minProperty.Subscribe(_ => this.Compare());
            this.maxProperty.Subscribe(_ => this.Compare());

        }

        //大小比較
        private void Compare() {

            if(this.maxProperty.Value < this.minProperty.Value) {
                double tmp = this.maxProperty.Value;
                this.maxProperty.Value = this.minProperty.Value;
                this.minProperty.Value = tmp;
            }

        }

        public override string ToString() {
            return "[" + this.minProperty.Value + " ," + this.maxProperty.Value + "]";
        }
        
        //UnityEngine.Vector2へのキャスト
        public static explicit operator Vector2(Range range) {
            return new Vector2((float)range.min, (float)range.max);
        }

    }
}
