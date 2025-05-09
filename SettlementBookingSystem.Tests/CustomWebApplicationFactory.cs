using Microsoft.AspNetCore.Mvc.Testing;

namespace SettlementBookingSystem.Tests
{
    public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram>
        where TProgram : class { }
}
