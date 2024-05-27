using Microsoft.IdentityModel.Protocols.Configuration;

namespace Identity.Application.Exceptions;

[Serializable]
public class ConfigurationMissingException : InvalidConfigurationException
{
    public ConfigurationMissingException(string message) : base(message){}
}