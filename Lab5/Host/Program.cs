using System;
using System.ServiceModel;

namespace Host
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var host = new ServiceHost(typeof(WCF.Service1))) 
            {
                host.Open();
                Console.WriteLine("The service is ready at {0}", host.BaseAddresses[0].AbsoluteUri);  // http://localhost:8733/Design_Time_Addresses/WCF/Service1/
                Console.WriteLine("The service is ready at {0}", host.BaseAddresses[1].AbsoluteUri);  // net.tcp://localhost:8734/Design_Time_Addresses/WCF/Service1/
                Console.WriteLine("Press <Enter> to stop the service");
                Console.ReadLine();
                host.Close();
            }
        }
    }
}
