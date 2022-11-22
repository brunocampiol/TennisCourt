using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TennisCourt.Infra.CrossCutting.Commons.Extensions
{
    public static class UserProvidedExtension
    {
        private const string _configPrefix = "vcap:services";

        public static void AddOptionsCloudFoundry<TOptions>(this IServiceCollection services, IConfiguration configuration, string serviceName) where TOptions : class
        {
            services.AddOptions<TOptions>()
                    .Configure(options =>
                    {
                        ConfigureCloudFoundryUserProvidedSettings(options, configuration, serviceName);
                    });
        }

        private static void ConfigureCloudFoundryUserProvidedSettings(object bindObject, IConfiguration configuration, string serviceName)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            if (string.IsNullOrEmpty(serviceName)) throw new ArgumentException("Cannot be null or empty", nameof(serviceName));

            var services = configuration.GetSection(_configPrefix);
            var section = GetServiceSection(services, serviceName);

            if (section != null)
            {
                section = section.GetSection("credentials");
                section.Bind(bindObject);
            }
        }

        private static IConfigurationSection GetServiceSection(IConfigurationSection section, string serviceName)
        {
            var children = section.GetChildren();
            foreach (var child in children)
            {
                string name = child.GetValue<string>("name");
                if (serviceName == name)
                {
                    return child;
                }
            }

            foreach (var child in children)
            {
                var result = GetServiceSection(child, serviceName);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }
    }
}