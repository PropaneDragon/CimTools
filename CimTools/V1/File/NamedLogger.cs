using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CimTools.V1.File
{
    public class NamedLogger
    {
        private CimToolSettings modSettings;

        public NamedLogger(CimToolSettings modSettings)
        {
            this.modSettings = modSettings;
        }
    }
}
