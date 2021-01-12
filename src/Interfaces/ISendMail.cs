using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoSi.Interfaces
{
    public interface ISendMail
    {
        Task SendMailAsync(int orderId);
    }
}
