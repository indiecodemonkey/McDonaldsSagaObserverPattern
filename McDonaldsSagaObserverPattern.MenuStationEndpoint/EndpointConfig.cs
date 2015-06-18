namespace McDonaldsSagaObserverPattern.MenuStationEndpoint
{
    using NServiceBus;

	public class EndpointConfig : IConfigureThisEndpoint, AsA_Server, IWantCustomInitialization
    {
	    public void Init()
	    {
            Configure.With().DefaultBuilder();
	    }
    }
}
