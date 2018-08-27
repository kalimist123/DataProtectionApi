using System;
using System.IO;

namespace DataProtectionApi
{
    using System;
    using Microsoft.AspNetCore.DataProtection;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Abstractions;
    public class Program
    {
        public static void Main(string[] args)
        {
            // add data protection services
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddDataProtection().PersistKeysToFileSystem(new DirectoryInfo(@"c:\dataprotectionkeyfolder")).SetDefaultKeyLifetime(TimeSpan.FromDays(10000)); 
            var services = serviceCollection.BuildServiceProvider();

            // create an instance of MyClass using the service provider
            var instance = ActivatorUtilities.CreateInstance<MyClass>(services);
            instance.RunSample();
        }

        public class MyClass
        {
            IDataProtector _protector;

            // the 'provider' parameter is provided by DI
            public MyClass(IDataProtectionProvider provider)
            {
                _protector = provider.CreateProtector("Contoso.MyClass.v1");
            }

            public void RunSample()
            {
                Console.Write("Enter input: ");
                string input = Console.ReadLine();

                // protect the payload
                string protectedPayload = _protector.Protect(input);
                Console.WriteLine($"Protect returned: {protectedPayload}");

                for (var i = 0; i < 5000; i++)
                {
                    protectedPayload = _protector.Protect(input);
                    Console.WriteLine($"Protect returned: {protectedPayload} {i}");

                    // unprotect the payload
                    string unprotectedPayload = _protector.Unprotect(protectedPayload);
                    Console.WriteLine($"Unprotect returned: {unprotectedPayload}");

                }





                Console.ReadLine();
            }
        }
    }

}
