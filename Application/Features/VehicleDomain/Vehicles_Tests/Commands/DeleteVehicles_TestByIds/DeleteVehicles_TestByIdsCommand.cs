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


namespace Application.Features.VehicleDomain.Vehicles_Tests.Commands.DeleteVehicles_TestByIds
{
    
    
    public class DeleteVehicles_TestByIdsCommand : IRequest
    {
        
        public System.Collections.Generic.List<int> Ids { get; set; }//;
    }
}