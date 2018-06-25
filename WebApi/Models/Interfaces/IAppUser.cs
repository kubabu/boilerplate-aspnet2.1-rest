using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public interface IAppUser
    {
        string Name { get; }
        string QrIdentifier { get; }
        string StartupUri { get; }
    }
}
