﻿using Jering.Javascript.NodeJS;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Jering.Web.SyntaxHighlighters.HighlightJS
{
    /// <summary>
    /// A class that provides static access to an instance of the default <see cref="IHighlightJSService"/> implementation's public methods.
    /// </summary>
    public static class StaticHighlightJSService
    {
        private static IServiceCollection _services;
        private static ServiceProvider _serviceProvider;
        private static IHighlightJSService _HighlightJSService;

        private static IHighlightJSService GetOrCreateHighlightJSService()
        {
            if (_HighlightJSService != null && _services == null)
            {
                // HighlightJSService already exists and no configuration pending
                return _HighlightJSService;
            }

            // Dispose of service provider
            _serviceProvider?.Dispose();

            // Create new service provider
            (_services ?? (_services = new ServiceCollection())).AddHighlightJS();
            _serviceProvider = _services.BuildServiceProvider();
            _services = null;

            return _HighlightJSService = _serviceProvider.GetRequiredService<IHighlightJSService>();
        }

        /// <summary>
        /// Configures options.
        /// </summary>
        /// <typeparam name="T">The type of options to configure.</typeparam>
        /// <param name="configureOptions">The action that configures the options.</param>
        public static void Configure<T>(Action<T> configureOptions) where T : class
        {
            (_services ?? (_services = new ServiceCollection())).Configure(configureOptions);
        }

        /// <summary>
        /// Highlights code of a specified language.
        /// </summary>
        /// <param name="code">Code to highlight.</param>
        /// <param name="languageAlias">A HighlightJS language alias. Visit http://highlightjs.readthedocs.io/en/latest/css-classes-reference.html#language-names-and-aliases 
        /// for the list of valid language aliases.</param>
        /// <param name="classPrefix">If not null or whitespace, this string will be appended to HighlightJS classes. Defaults to "hljs-".</param>
        /// <param name="cancellationToken">The cancellation token for the asynchronous operation.</param>
        /// <returns>Highlighted code.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="languageAlias"/> is not a valid HighlightJS language alias.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="code"/> is null.</exception>
        /// <exception cref="InvocationException">Thrown if a NodeJS error occurs.</exception>
        /// <exception cref="ObjectDisposedException">Thrown if this instance has been disposed or if an attempt is made to use one of its dependencies that has been disposed.</exception>
        /// <exception cref="OperationCanceledException">Thrown if <paramref name="cancellationToken"/> is cancelled.</exception>
        public static Task<string> HighlightAsync(string code, string languageAlias, string classPrefix = "hljs-", CancellationToken cancellationToken = default)
        {
            return GetOrCreateHighlightJSService().HighlightAsync(code, languageAlias, classPrefix, cancellationToken);
        }

        /// <summary>
        /// Determines whether a language alias is valid.
        /// </summary>
        /// <param name="languageAlias">Language alias to validate. Visit http://highlightjs.readthedocs.io/en/latest/css-classes-reference.html#language-names-and-aliases 
        /// for the list of valid language aliases.</param>
        /// <param name="cancellationToken">The cancellation token for the asynchronous operation.</param>
        /// <returns>true if the specified language alias is a valid HighlightJS language alias. Otherwise, false.</returns>
        /// <exception cref="InvocationException">Thrown if a NodeJS error occurs.</exception>
        /// <exception cref="ObjectDisposedException">Thrown if this instance has been disposed or if an attempt is made to use one of its dependencies that has been disposed.</exception>
        /// <exception cref="OperationCanceledException">Thrown if <paramref name="cancellationToken"/> is cancelled.</exception>
        public static Task<bool> IsValidLanguageAliasAsync(string languageAlias, CancellationToken cancellationToken = default)
        {
            return GetOrCreateHighlightJSService().IsValidLanguageAliasAsync(languageAlias, cancellationToken);
        }
    }
}
