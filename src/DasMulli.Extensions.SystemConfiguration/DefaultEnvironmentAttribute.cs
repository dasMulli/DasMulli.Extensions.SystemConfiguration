using System;

namespace DasMulli.Extensions.SystemConfiguration
{
    [AttributeUsage(AttributeTargets.Assembly, Inherited = false, AllowMultiple = false)]
    public sealed class DefaultEnvironmentAttribute : Attribute
    {
        /// <summary>
        /// Gets the name of the default environment.
        /// </summary>
        /// <value>
        /// The name of the default environment.
        /// </value>
        public string DefaultEnvironmentName { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultEnvironmentAttribute"/> class.
        /// </summary>
        /// <param name="defaultEnvironmentName">Name of the default environment to be used when no environment variable sets an environment name.</param>
        public DefaultEnvironmentAttribute(string defaultEnvironmentName)
        {
            DefaultEnvironmentName = defaultEnvironmentName;
        }
    }
}