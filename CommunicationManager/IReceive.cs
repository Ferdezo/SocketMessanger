using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationManager
{
    public interface IReceive
    {
        void ReceiveMessage(String message);
    }
}
