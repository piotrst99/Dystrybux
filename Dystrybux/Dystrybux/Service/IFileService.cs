using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Dystrybux.Service {
    public interface IFileService {
        void SavePicture(string name, Stream data, string location = "temp");
    }
}
