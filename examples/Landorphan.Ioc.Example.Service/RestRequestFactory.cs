namespace Landorphan.Ioc.Example.Service
{
    using RestSharp;

    internal class RestRequestFactory : IRestRequestFactory
   {
       public IRestRequest Create()
      {
         return new RestRequest();
      }
   }
}
