using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ge_repository.services
{
    public interface xISmsSender
    {
        Task SendSmsAsync(string number, string message);
    }
}
