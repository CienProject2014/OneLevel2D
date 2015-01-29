using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// ReSharper disable InconsistentNaming
#pragma warning disable 169
#pragma warning disable 649

namespace OneLevelJson.Model
{
    class Overlap2DProject
    {
        public string projectName;
        public string projectMainExportPath;
        public string lastOpenScene;
        public string projectVersion;

        public override string ToString()
        {
            return projectName ;
        }
    }
}
