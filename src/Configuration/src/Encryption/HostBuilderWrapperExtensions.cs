// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information.

using Microsoft.Extensions.Logging;
using Steeltoe.Common.Hosting;
using Steeltoe.Common.Logging;
using Steeltoe.Configuration.Encryption.Decryption;

namespace Steeltoe.Configuration.Encryption;

internal static class HostBuilderWrapperExtensions
{
    public static HostBuilderWrapper AddEncryptionResolver(this HostBuilderWrapper wrapper, ILoggerFactory loggerFactory)
    {
        ArgumentNullException.ThrowIfNull(wrapper);
        ArgumentNullException.ThrowIfNull(loggerFactory);

        wrapper.ConfigureAppConfiguration((context, configurationBuilder) =>
        {
            ITextDecryptor textDecryptor = ConfigServerEncryptionSettings.CreateTextDecryptor(context.Configuration);
            configurationBuilder.AddEncryptionResolver(textDecryptor, loggerFactory);
        });

        if (loggerFactory is IBootstrapLoggerFactory)
        {
            BootstrapLoggerHostedService.Register(wrapper);
        }

        return wrapper;
    }
}
