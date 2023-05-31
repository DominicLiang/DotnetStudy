using Com.Ctrip.Framework.Apollo;
using Com.Ctrip.Framework.Apollo.Logging;
using LogLevel = Com.Ctrip.Framework.Apollo.Logging.LogLevel;

namespace Apollo;

public static class AddApollosService
{
    public static void AddApolloService(this IConfigurationBuilder builder)
    {
        
        var apolloBuilder = builder.AddApollo();
    }
}
