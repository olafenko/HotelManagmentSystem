using HotelManageSys.API.Exceptions;
using HotelManageSys.API.Features.Workers.DTO_s;
using HotelManageSys.API.Features.Workers.Messages.Queries;
using HotelManageSys.API.Features.Workers.Providers;
using Mapster;
using MediatR;

namespace HotelManageSys.API.Features.Workers.Handlers.Queries
{
    public class GetWorkerByIdHandler : IRequestHandler<GetWorkerByIdQuery, WorkerDTO?>
    {
        private readonly IWorkerProvider _workerProvider;

        public GetWorkerByIdHandler(IWorkerProvider workerProvider)
        {
            _workerProvider = workerProvider;
        }

        public async Task<WorkerDTO?> Handle(GetWorkerByIdQuery request, CancellationToken cancellationToken)
        {
            var worker = await _workerProvider.GetWorkerByIdAsync(request.Id, true, cancellationToken);

            if (worker == null) throw new NotFoundException("Worker", request.Id);
            
            return worker.Adapt<WorkerDTO>();
        }
    }
}

