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

        ////private IVehicles_TestRepository _vehicles_testRepository;
        //public CreateVehicles_TestCommandHandler(IVehicles_TestRepository vehicles_testRepository)
        //{
        //    //_vehicles_testRepository = vehicles_testRepository ;
        //}
        public async Task<Unit> Handle(CreateVehicles_TestCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
            ;
        }
    }
}