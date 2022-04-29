using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Cloud77.Middleware
{
    public class LocalFileMiddleware
    {
        public LocalFileMiddleware()
        {
            if (!Directory.Exists(DesktopMiddleware.LocalApplicationData))
            {
                Directory.CreateDirectory(DesktopMiddleware.LocalApplicationData);
            }
        }
    }
}
