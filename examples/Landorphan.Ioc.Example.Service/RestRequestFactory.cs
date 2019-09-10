namespace Landorphan.Ioc.Example.Service
{
   using RestSharp;

   class RestRequestFactory : IRestRequestFactory
   {
      public IRestRequest Create()
      {
         return new RestRequest();
      }
   }
}
