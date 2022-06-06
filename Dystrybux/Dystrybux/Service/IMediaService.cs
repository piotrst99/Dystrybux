using System;
using System.Collections.Generic;
using System.Text;

namespace Dystrybux.Service {
    public interface IMediaService {
        void SaveImageFromByte(byte[] imageByte, string fileName);
    }
}
