using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRPA
{
    public interface IExeCommandEventArgs
    {
        void OnExeCommandOccur(object sender, EventArgs e);
    }
}
