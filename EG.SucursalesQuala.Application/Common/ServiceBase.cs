using EG.SucursalesQuala.Application.Interfaces;

namespace EG.SucursalesQuala.Application.Common
{
    public abstract class ServiceBase
    {
        protected readonly IEncriptacionService _encriptacionService;

        protected ServiceBase(IEncriptacionService encriptacionService)
        {
            _encriptacionService = encriptacionService;
        }
    }
}
