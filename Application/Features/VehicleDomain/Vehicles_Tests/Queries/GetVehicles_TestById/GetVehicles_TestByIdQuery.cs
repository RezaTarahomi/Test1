//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Application.Common;
using MediatR;
using System.Linq;


namespace Application.Features.VehicleDomain.Vehicles_Tests.Queries.GetVehicles_TestById
{
    
    
    public class GetVehicles_TestByIdQuery : IRequest<Vehicles_TestModel>
    {
        
        public int Id { get; set; }//;
    }
}