using System;
using System.Collections.Generic;
using System.Text;

namespace Dystrybux.Service {
    public interface ITestInterface {
        void StorePhotoToGallery(byte[] bytes, string fileName);
    }
}
