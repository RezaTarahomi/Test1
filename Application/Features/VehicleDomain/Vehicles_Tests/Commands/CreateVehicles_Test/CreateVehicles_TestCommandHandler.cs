using Application.VehicleDomain;
using MediatR;

namespace Application.Features.VehicleDomain.Vehicles_Tests.Commands.CreateVehicles_Test
{
    public class CreateVehicles_TestCommandHandler : IRequestHandler<CreateVehicles_TestCommand>
    {
        private IVehicles_TestQuery _vehicles_TestQuery;
        public CreateVehicles_TestCommandHandler(IVehicles_TestQuery vehicles_TestQuery)
        {
            _vehicles_TestQuery = vehicles_TestQuery;
        }

        public async Task<Unit> Handle(CreateVehicles_TestCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
            var vehicles_Test = await _vehicles_TestQuery.FindById(request.Id);
            var vehicles_Tests = await _vehicles_TestQuery.GetAll();
        }
    }
}