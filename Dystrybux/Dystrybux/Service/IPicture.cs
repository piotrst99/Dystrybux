using System;
using System.Collections.Generic;
using System.Text;

namespace Dystrybux.Service {
    public interface IPicture {
        void SavePictureToDisk(string filename, byte[] imageData);
    }
}
