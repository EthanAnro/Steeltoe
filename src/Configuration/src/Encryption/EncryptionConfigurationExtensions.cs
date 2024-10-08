// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Steeltoe.Configuration.Encryption;

public static class EncryptionConfigurationExtensions
{
    /// <summary>
    /// Adds an encryption resolver configuration source to the <see cref="ConfigurationBuilder" />. The encryption resolver source will capture and wrap all
    /// the existing sources <see cref="IConfigurationSource" /> contained in the builder.  The newly created source will then replace the existing sources
    /// and provide encryption resolution for the configuration. Typically, you will want to add this configuration source as the last one so that you wrap
    /// all applications' configuration sources with encryption resolution.
    /// </summary>
    /// <param name="builder">
    /// The <see cref="IConfigurationBuilder" /> to add configuration to.
    /// </param>
    /// <param name="textDecryptor">
    /// The decryptor to use.
    /// </param>
    /// <returns>
    /// The incoming <paramref name="builder" /> so that additional calls can be chained.
    /// </returns>
    public static IConfigurationBuilder AddEncryptionResolver(this IConfigurationBuilder builder, ITextDecryptor textDecryptor)
    {
        return AddEncryptionResolver(builder, textDecryptor, NullLoggerFactory.Instance);
    }

    /// <summary>
    /// Adds an encryption resolver configuration source to the <see cref="ConfigurationBuilder" />. The encryption resolver source will capture and wrap all
    /// the existing sources <see cref="IConfigurationSource" /> contained in the builder.  The newly created source will then replace the existing sources
    /// and provide encryption resolution for the configuration. Typically, you will want to add this configuration source as the last one so that you wrap
    /// all applications' configuration sources with encryption resolution.
    /// </summary>
    /// <param name="builder">
    /// The <see cref="IConfigurationBuilder" /> to add configuration to.
    /// </param>
    /// <param name="textDecryptor">
    /// The decryptor to use.
    /// </param>
    /// <param name="loggerFactory">
    /// Used for internal logging. Pass <see cref="NullLoggerFactory.Instance" /> to disable logging.
    /// </param>
    /// <returns>
    /// The incoming <paramref name="builder" /> so that additional calls can be chained.
    /// </returns>
    public static IConfigurationBuilder AddEncryptionResolver(this IConfigurationBuilder builder, ITextDecryptor textDecryptor, ILoggerFactory loggerFactory)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(textDecryptor);
        ArgumentNullException.ThrowIfNull(loggerFactory);

        if (!builder.Sources.OfType<EncryptionResolverSource>().Any())
        {
            if (builder is IConfigurationRoot configuration)
            {
                var source = new EncryptionResolverSource(configuration, textDecryptor, loggerFactory);
                builder.Add(source);
            }
            else
            {
                var source = new EncryptionResolverSource(builder.Sources, textDecryptor, loggerFactory);
                builder.Sources.Clear();
                builder.Add(source);
            }
        }

        return builder;
    }

    /// <summary>
    /// Creates a new <see cref="ConfigurationRoot" /> from a <see cref="EncryptionResolverProvider" />. The encryption resolver will be created using the
    /// existing configuration providers contained in the incoming configuration. This results in providing encryption resolution for those configuration
    /// sources.
    /// </summary>
    /// <param name="configuration">
    /// The <see cref="IConfiguration" /> to wrap.
    /// </param>
    /// <param name="textDecryptor">
    /// The decryptor to use.
    /// </param>
    /// <returns>
    /// A new configuration.
    /// </returns>
    public static IConfiguration AddEncryptionResolver(this IConfiguration configuration, ITextDecryptor textDecryptor)
    {
        return AddEncryptionResolver(configuration, textDecryptor, NullLoggerFactory.Instance);
    }

    /// <summary>
    /// Creates a new <see cref="ConfigurationRoot" /> from a <see cref="EncryptionResolverProvider" />. The encryption resolver will be created using the
    /// existing configuration providers contained in the incoming configuration. This results in providing encryption resolution for those configuration
    /// sources.
    /// </summary>
    /// <param name="configuration">
    /// The <see cref="IConfiguration" /> to wrap.
    /// </param>
    /// <param name="textDecryptor">
    /// The decryptor to use.
    /// </param>
    /// <param name="loggerFactory">
    /// Used for internal logging. Pass <see cref="NullLoggerFactory.Instance" /> to disable logging.
    /// </param>
    /// <returns>
    /// A new configuration.
    /// </returns>
    public static IConfiguration AddEncryptionResolver(this IConfiguration configuration, ITextDecryptor textDecryptor, ILoggerFactory loggerFactory)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentNullException.ThrowIfNull(textDecryptor);
        ArgumentNullException.ThrowIfNull(loggerFactory);

        if (configuration is not IConfigurationRoot root)
        {
            throw new InvalidOperationException($"Configuration must implement '{typeof(IConfigurationRoot)}'.");
        }

        if (root.Providers.Any(provider => provider is EncryptionResolverProvider))
        {
            return configuration;
        }

        return new ConfigurationRoot(new List<IConfigurationProvider>
        {
            new EncryptionResolverProvider(new List<IConfigurationProvider>(root.Providers), textDecryptor, loggerFactory)
        });
    }

    /// <summary>
    /// Adds an encryption resolver configuration source to the <see cref="ConfigurationBuilder" />. The encryption resolver source will capture and wrap all
    /// the existing sources <see cref="IConfigurationSource" /> contained in the builder.  The newly created source will then replace the existing sources
    /// and provide encryption resolution for the configuration. Typically, you will want to add this configuration source as the last one so that you wrap
    /// all applications' configuration sources with encryption resolution.
    /// </summary>
    /// <param name="configurationManager">
    /// The <see cref="ConfigurationManager" /> to configure.
    /// </param>
    /// <param name="textDecryptor">
    /// The decryptor to use.
    /// </param>
    /// <returns>
    /// The incoming <paramref name="configurationManager" /> so that additional calls can be chained.
    /// </returns>
    public static ConfigurationManager AddEncryptionResolver(this ConfigurationManager configurationManager, ITextDecryptor textDecryptor)
    {
        return AddEncryptionResolver(configurationManager, textDecryptor, NullLoggerFactory.Instance);
    }

    /// <summary>
    /// Adds an encryption resolver configuration source to the <see cref="ConfigurationBuilder" />. The encryption resolver source will capture and wrap all
    /// the existing sources <see cref="IConfigurationSource" /> contained in the builder.  The newly created source will then replace the existing sources
    /// and provide encryption resolution for the configuration. Typically, you will want to add this configuration source as the last one so that you wrap
    /// all applications' configuration sources with encryption resolution.
    /// </summary>
    /// <param name="configurationManager">
    /// The <see cref="ConfigurationManager" /> to configure.
    /// </param>
    /// <param name="textDecryptor">
    /// The decryptor to use.
    /// </param>
    /// <param name="loggerFactory">
    /// Used for internal logging. Pass <see cref="NullLoggerFactory.Instance" /> to disable logging.
    /// </param>
    /// <returns>
    /// The incoming <paramref name="configurationManager" /> so that additional calls can be chained.
    /// </returns>
    public static ConfigurationManager AddEncryptionResolver(this ConfigurationManager configurationManager, ITextDecryptor textDecryptor,
        ILoggerFactory loggerFactory)
    {
        ArgumentNullException.ThrowIfNull(configurationManager);
        ArgumentNullException.ThrowIfNull(textDecryptor);
        ArgumentNullException.ThrowIfNull(loggerFactory);

        ((IConfigurationBuilder)configurationManager).AddEncryptionResolver(textDecryptor, loggerFactory);

        return configurationManager;
    }
}
