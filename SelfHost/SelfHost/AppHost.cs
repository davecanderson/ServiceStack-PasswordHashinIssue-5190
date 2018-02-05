using Funq;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using System.Security.Cryptography;

namespace SelfHost
{
    [Route("/hello/{Name}")]
    public class Hello
    {
        public string Name { get; set; }
    }

    public class HelloResponse
    {
        public string Result { get; set; }
    }

    public class HelloService : Service
    {
        public object Any(Hello request)
        {
            return new HelloResponse { Result = "Hello, " + request.Name };
        }
    }

    //VS.NET Template Info: https://servicestack.net/vs-templates/EmptySelfHost
    public class AppHost : AppSelfHostBase
    {
        private const string db = "sqlite.db3";

        /// <summary>
        /// Base constructor requires a Name and Assembly where web service implementation is located
        /// </summary>
        public AppHost()
            : base("Password Hashing Issue 5190", typeof(HelloService).Assembly) { }

        /// <summary>
        /// Application specific configuration
        /// This method should initialize any IoC resources utilized by your web service classes.
        /// </summary>
        public override void Configure(Container container)
        {
            SetConfig(new HostConfig
            {
                DebugMode = true,
                //UseSaltedHash = true
            });

            container.Register<IHashProvider>(c => new SaltedHash(new SHA512Managed(), 5));

            Plugins.Add(new AuthFeature(() => new AuthUserSession(),
                new IAuthProvider[] {
                        new CredentialsAuthProvider(AppSettings),
                })
            {
                //CreateDigestAuthHashes = true,
                IncludeRegistrationService = true
            });

            container.Register<IDbConnectionFactory>(c =>
                    new OrmLiteConnectionFactory($"~/../../{db}".MapServerPath(), SqliteDialect.Provider));

            container.Register<IAuthRepository>(c =>
                 new OrmLiteAuthRepository(c.Resolve<IDbConnectionFactory>()));
        }
    }
}
