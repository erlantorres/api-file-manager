
using api.Domain.FileServices.Data.Entities;

namespace api.Domain.FileServices.Data.Repositories.Interfaces;

public interface IBatchRepository
{
    Task<int> CreateBatchAsync(BatchEntity batchEntity);
}
