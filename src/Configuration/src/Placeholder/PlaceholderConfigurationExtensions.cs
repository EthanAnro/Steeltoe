// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Steeltoe.Configuration.Placeholder;

public static class PlaceholderConfigurationExtensions
{
    /// <summary>
    /// Adds a placeholder resolver configuration source to the <see cref="ConfigurationBuilder" />. The placeholder resolver source will capture and wrap
    /// all the existing sources <see cref="IConfigurationSource" /> contained in the builder.  The newly created source will then replace the existing
    /// sources and provide placeholder resolution for the configuration. Typically, you will want to add this configuration source as the last one so that
    /// you wrap all applications' configuration sources with placeholder resolution.
    /// </summary>
    /// <param name="builder">
    /// The <see cref="IConfigurationBuilder" /> to add configuration to.
    /// </param>
    /// <returns>
    /// The incoming <paramref name="builder" /> so that additional calls can be chained.
    /// </returns>
    public static IConfigurationBuilder AddPlaceholderResolver(this IConfigurationBuilder builder)
    {
        return AddPlaceholderResolver(builder, NullLoggerFactory.Instance);
    }

    /// <summary>
    /// Adds a placeholder resolver configuration source to the <see cref="ConfigurationBuilder" />. The placeholder resolver source will capture and wrap
    /// all the existing sources <see cref="IConfigurationSource" /> contained in the builder.  The newly created source will then replace the existing
    /// sources and provide placeholder resolution for the configuration. Typically, you will want to add this configuration source as the last one so that
    /// you wrap all applications' configuration sources with placeholder resolution.
    /// </summary>
    /// <param name="builder">
    /// The <see cref="IConfigurationBuilder" /> to add configuration to.
    /// </param>
    /// <param name="loggerFactory">
    /// Used for internal logging. Pass <see cref="NullLoggerFactory.Instance" /> to disable logging.
    /// </param>
    /// <returns>
    /// The incoming <paramref name="builder" /> so that additional calls can be chained.
    /// </returns>
    public static IConfigurationBuilder AddPlaceholderResolver(this IConfigurationBuilder builder, ILoggerFactory loggerFactory)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(loggerFactory);

        if (!builder.Sources.OfType<PlaceholderResolverSource>().Any())
        {
            if (builder is IConfigurationRoot configuration)
            {
                var source = new PlaceholderResolverSource(configuration, loggerFactory);
                builder.Add(source);
            }
            else
            {
                var source = new PlaceholderResolverSource(builder.Sources, loggerFactory);
                builder.Sources.Clear();
                builder.Add(source);
            }
        }

        return builder;
    }

    /// <summary>
    /// Creates a new <see cref="ConfigurationRoot" /> from a <see cref="PlaceholderResolverProvider" />. The placeholder resolver will be created using the
    /// existing configuration providers contained in the incoming configuration. This results in providing placeholder resolution for those configuration
    /// sources.
    /// </summary>
    /// <param name="configuration">
    /// The configuration to wrap.
    /// </param>
    /// <returns>
    /// A new configuration.
    /// </returns>
    public static IConfiguration AddPlaceholderResolver(this IConfiguration configuration)
    {
        return AddPlaceholderResolver(configuration, NullLoggerFactory.Instance);
    }

    /// <summary>
    /// Creates a new <see cref="ConfigurationRoot" /> from a <see cref="PlaceholderResolverProvider" />. The placeholder resolver will be created using the
    /// existing configuration providers contained in the incoming configuration. This results in providing placeholder resolution for those configuration
    /// sources.
    /// </summary>
    /// <param name="configuration">
    /// The configuration to wrap.
    /// </param>
    /// <param name="loggerFactory">
    /// Used for internal logging. Pass <see cref="NullLoggerFactory.Instance" /> to disable logging.
    /// </param>
    /// <returns>
    /// A new configuration.
    /// </returns>
    public static IConfiguration AddPlaceholderResolver(this IConfiguration configuration, ILoggerFactory loggerFactory)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentNullException.ThrowIfNull(loggerFactory);

        if (configuration is not IConfigurationRoot root)
        {
            throw new InvalidOperationException($"Configuration must implement '{typeof(IConfigurationRoot)}'.");
        }

        if (root.Providers.Any(provider => provider is IPlaceholderResolverProvider))
        {
            return configuration;
        }

        return new ConfigurationRoot(new List<IConfigurationProvider>
        {
            new PlaceholderResolverProvider(new List<IConfigurationProvider>(root.Providers), loggerFactory)
        });
    }

    /// <summary>
    /// Adds a placeholder resolver configuration source to the <see cref="ConfigurationBuilder" />. The placeholder resolver source will capture and wrap
    /// all the existing sources <see cref="IConfigurationSource" /> contained in the builder.  The newly created source will then replace the existing
    /// sources and provide placeholder resolution for the configuration. Typically, you will want to add this configuration source as the last one so that
    /// you wrap all applications' configuration sources with placeholder resolution.
    /// </summary>
    /// <param name="configurationManager">
    /// The <see cref="ConfigurationManager" /> to configure.
    /// </param>
    /// <returns>
    /// The incoming <paramref name="configurationManager" /> so that additional calls can be chained.
    /// </returns>
    public static ConfigurationManager AddPlaceholderResolver(this ConfigurationManager configurationManager)
    {
        return AddPlaceholderResolver(configurationManager, NullLoggerFactory.Instance);
    }

    /// <summary>
    /// Adds a placeholder resolver configuration source to the <see cref="ConfigurationBuilder" />. The placeholder resolver source will capture and wrap
    /// all the existing sources <see cref="IConfigurationSource" /> contained in the builder.  The newly created source will then replace the existing
    /// sources and provide placeholder resolution for the configuration. Typically, you will want to add this configuration source as the last one so that
    /// you wrap all applications' configuration sources with placeholder resolution.
    /// </summary>
    /// <param name="configurationManager">
    /// The <see cref="ConfigurationManager" /> to configure.
    /// </param>
    /// <param name="loggerFactory">
    /// Used for internal logging. Pass <see cref="NullLoggerFactory.Instance" /> to disable logging.
    /// </param>
    /// <returns>
    /// The incoming <paramref name="configurationManager" /> so that additional calls can be chained.
    /// </returns>
    public static ConfigurationManager AddPlaceholderResolver(this ConfigurationManager configurationManager, ILoggerFactory loggerFactory)
    {
        ArgumentNullException.ThrowIfNull(configurationManager);
        ArgumentNullException.ThrowIfNull(loggerFactory);

        ((IConfigurationBuilder)configurationManager).AddPlaceholderResolver(loggerFactory);

        return configurationManager;
    }
}
