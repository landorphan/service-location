namespace Ioc.Collections.Performance.Tests
{
   using System;
   using System.Collections.Generic;

   public interface ITestTypesBuilder
   {
      IList<KeyValuePair<Type, Type>> BuildTypePairs(Int32 count);
   }
}
