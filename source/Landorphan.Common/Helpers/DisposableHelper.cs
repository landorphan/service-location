namespace Landorphan.Common
{
   using System;
   using System.Diagnostics.CodeAnalysis;
   using System.Reflection;

   /// <summary>
   /// Helper class for working with disposable instances.
   /// </summary>
   public static class DisposableHelper
   {
      /// <summary>
      /// Safely creates a disposable Object with a default constructor.
      /// </summary>
      /// <remarks>
      /// The motivation is to avoid <see href="http://msdn.microsoft.com/en-us/library/ms182289.aspx"> CA2000 </see> warnings.
      /// </remarks>
      /// <exception cref="Exception">
      /// Thrown when an exception error condition occurs.
      /// </exception>
      /// <typeparam name="T">
      /// The type of Object to create.
      /// </typeparam>
      /// <returns>
      /// The disposable Object that has been safely created.
      /// </returns>
      [SuppressMessage("SonarLint.CodeSmell", "S4056:Overloads with a CultureInfo or an IFormatProvider parameter should be used", Justification = "Only available in .Net Core")]
      public static T SafeCreate<T>() where T : class, IDisposable, new()
      {
         // ReSharper disable PossibleNullReferenceException
         T rv = null;
         try
         {
            // use reflection to construct a new instance
            rv = new T();
         }
         catch (TargetInvocationException tie)
         {
            // Make life easier for the caller and strip the outer TIE, resetting the stack trace.
            var remoteStackTrace = typeof(Exception).GetField("_remoteStackTraceString", BindingFlags.Instance | BindingFlags.NonPublic);

            remoteStackTrace.SetValue(tie.InnerException, tie.InnerException.StackTrace + Environment.NewLine);
            throw tie.InnerException;
         }
         catch (Exception)
         {
            rv?.Dispose();

            throw;
         }

         return rv;
         // ReSharper restore PossibleNullReferenceException
      }

      /// <summary>
      /// Safely creates a disposable Object with a custom constructor.
      /// </summary>
      /// <remarks>
      /// The motivation is to avoid <see href="http://msdn.microsoft.com/en-us/library/ms182289.aspx"> CA2000 </see> warnings.
      /// </remarks>
      /// <exception cref="Exception">
      /// Thrown when an exception error condition occurs.
      /// </exception>
      /// <typeparam name="T">
      /// The type of Object to create.
      /// </typeparam>
      /// <param name="factoryFunction">
      /// The factoryFunction method used to construct the Object.
      /// </param>
      /// <returns>
      /// The disposable Object that has been safely created.
      /// </returns>
      public static T SafeCreate<T>(Func<T> factoryFunction) where T : class, IDisposable
      {
         factoryFunction.ArgumentNotNull(nameof(factoryFunction));

         T rv = null;
         try
         {
            rv = factoryFunction();
         }
         catch (Exception)
         {
            rv?.Dispose();

            throw;
         }

         return rv;
      }
   }
}
