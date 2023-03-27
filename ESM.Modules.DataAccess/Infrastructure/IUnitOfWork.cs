﻿using ESM.Modules.DataAccess.Repositories;

namespace ESM.Modules.DataAccess.Infrastructure
{
    public interface IUnitOfWork : IDisposable
    {
        IAccountRepository Accounts { get; }
        ILaptopRepository Laptops { get; }
        IMonitorRepository Monitors { get; }
        IPccpuRepository Pccpus { get; }
        IPcRepository Pcs { get; }
        IPcharddiskRepository Pcharddisks { get; }
        ISmartphoneRepository Smartphones { get; }
        IVgaRepository Vgas { get; }
        IBillRepository Bills { get; }
        Task<int> SaveChangesAsync();
        int SaveChange();
    }
}
