﻿using Abb.Euopc.SharedDesks.Domain.Entities;

namespace Abb.Euopc.SharedDesks.Domain.Interfaces.Repositories;

public interface IDeskItemRepository : IRepository<DeskItem>
{
    Task<List<DeskItem>> GetAllActiveAsync();
}

