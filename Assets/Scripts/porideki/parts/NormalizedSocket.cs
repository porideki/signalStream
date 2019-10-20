using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.porideki.parts {
     public class NormalizedSocket {

        public object socket;
        public Type socketType;

        public NormalizedSocket(Object socket) {
            this.Initialize(socket);
        }

        private void Initialize(Object socket) {
            this.socket = socket;
            if (this.socket.GetType().IsGenericType) {
                this.socketType = this.socket.GetType().GetGenericArguments()[0];
            }
        }

    }

    //ラッパークラス
    public class NormalizedInputSocket : NormalizedSocket {

        public NormalizedInputSocket(Object socket) : base(socket) { }

    }

    public class NormalizedOutputSocket : NormalizedSocket {

        public NormalizedOutputSocket(Object socket) : base(socket) { }

    }

}
