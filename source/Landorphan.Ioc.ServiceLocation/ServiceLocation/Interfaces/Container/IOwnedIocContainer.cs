namespace Landorphan.Ioc.ServiceLocation.Interfaces
{
   using System;
   using System.Diagnostics.CodeAnalysis;
   using Landorphan.Common;

   /// <summary>
   /// Represents an owned dependency injection container.
   /// </summary>
   /// <remarks>
   /// The holder of this reference is responsible for disposing of the instance.
   /// </remarks>
   [SuppressMessage("SonarLint.CodeSmell", "S1939: Inheritance list should not be redundant", Justification = "Being explicit (MWP)")]
   public interface IOwnedIocContainer : IIocContainer, IDisposable, IQueryDisposable
   {
   }
}
