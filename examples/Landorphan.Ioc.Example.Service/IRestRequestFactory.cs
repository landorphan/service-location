namespace Landorphan.Ioc.Example.Service
{
    using RestSharp;

    public interface IRestRequestFactory
   {
       IRestRequest Create();
   }
}
