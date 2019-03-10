namespace Landorphan.TestUtilities.ReusableTestImplementations.Architecture
{
   using System;
   using System.Collections.Generic;
   using System.Collections.Immutable;
   using System.Diagnostics;
   using System.Diagnostics.CodeAnalysis;
   using System.Globalization;
   using System.Linq;
   using System.Reflection;
   using System.Runtime.Serialization;
   using System.Text;
   using Landorphan.Common;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   /// <summary>
   /// Test implementations for exception validity requirements.
   /// </summary>
   public abstract class ExceptionValidityRequirements : TestBase
   {
      private static readonly Random t_random = new Random();

      /// <summary>
      /// Evaluates each exception to ensure that it is NOT decorated with [Serializable].
      /// </summary>
      [SuppressMessage("Microsoft.Naming", "CA1707: Identifiers should not contain underscores")]
      protected void Exceptions_In_DotNet_Core_Should_Not_Be_Marked_As_Serializable_Implementation()
      {
         var failureMessages = new List<String>();
         var exceptionTypes = FindExceptionTypes().ToList();
         foreach (var et in exceptionTypes)
         {
            failureMessages.AddRange(ValidateNotAttributedSerializable(et));
         }

         if (failureMessages.Count > 0)
         {
            throw new AssertFailedException(String.Join("\r\n", failureMessages.ToArray()));
         }
      }

      /// <summary>
      /// Evaluates each exception to ensure that it is either abstract or sealed.
      /// </summary>
      /// <exception cref="AssertFailedException" />
      [SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "AbstractOr", Justification = "Years old bug.")]
      [SuppressMessage("Microsoft.Naming", "CA1707: Identifiers should not contain underscores")]
      protected void Exceptions_Should_Be_Abstract_Or_Sealed_Implementation()
      {
         var failureMessages = new List<String>();
         var exceptionTypes = FindExceptionTypes().ToList();
         foreach (var et in exceptionTypes)
         {
            failureMessages.AddRange(ValidateExceptionTypeByModifier(et));
         }

         if (failureMessages.Count > 0)
         {
            throw new AssertFailedException(String.Join("\r\n", failureMessages.ToArray()));
         }
      }

      /// <summary>
      /// Evaluates each exception to ensure that it is public.
      /// </summary>
      /// <exception cref="AssertFailedException" />
      [SuppressMessage("Microsoft.Naming", "CA1707: Identifiers should not contain underscores")]
      protected void Exceptions_Should_Be_Public_Implementation()
      {
         var failureMessages = new List<String>();
         var exceptionTypes = FindExceptionTypes().ToList();
         foreach (var et in exceptionTypes)
         {
            failureMessages.AddRange(ValidateExceptionTypeByScope(et));
         }

         if (failureMessages.Count > 0)
         {
            throw new AssertFailedException(String.Join("\r\n", failureMessages.ToArray()));
         }

         foreach (var exceptionType in exceptionTypes)
         {
            Trace.WriteLine(exceptionType.Name + " Passed!");
         }
      }

      /// <summary>
      /// Evaluates each exception to ensure it is descended from an acceptable base exception.
      /// </summary>
      /// <remarks>
      /// Acceptable base classes are specified by the concrete test class by overriding
      /// <see cref="GetAcceptableBaseExceptionTypes" />
      /// </remarks>
      /// <exception cref="AssertFailedException" />
      [SuppressMessage("Microsoft.Naming", "CA1707: Identifiers should not contain underscores")]
      protected void Exceptions_Should_Descend_From_An_Acceptable_Base_Implementation()
      {
         var failureMessages = new List<String>();
         var exceptionTypes = FindExceptionTypes().ToList();
         foreach (var et in exceptionTypes)
         {
            failureMessages.AddRange(ValidateExceptionTypeAncestor(et));
         }

         if (failureMessages.Count > 0)
         {
            throw new AssertFailedException(String.Join("\r\n", failureMessages.ToArray()));
         }
      }

      /// <summary>
      /// Evaluates each exception to ensure that has a default constructor.
      /// </summary>
      [SuppressMessage("Microsoft.Naming", "CA1707: Identifiers should not contain underscores")]
      protected void Exceptions_Should_Have_A_Default_Constructor_Implementation()
      {
         var failureMessages = new List<String>();

         var exceptionTypes = FindExceptionTypes().ToList();
         foreach (var et in exceptionTypes)
         {
            failureMessages.AddRange(ValidatePublicDefaultConstructor(et));
         }

         if (failureMessages.Count > 0)
         {
            throw new AssertFailedException(String.Join("\r\n", failureMessages.ToArray()));
         }
      }

      /// <summary>
      /// Evaluates each exception to ensure that has message and inner exception constructor.
      /// </summary>
      [SuppressMessage("Microsoft.Naming", "CA1707: Identifiers should not contain underscores")]
      protected void Exceptions_Should_Have_A_Message_And_Inner_Exception_Constructor_Implementation()
      {
         var failureMessages = new List<String>();
         var exceptionTypes = FindExceptionTypes().ToList();
         foreach (var et in exceptionTypes)
         {
            failureMessages.AddRange(ValidatePublicMessageAndInnerExceptionConstructor(et));
         }

         if (failureMessages.Count > 0)
         {
            throw new AssertFailedException(String.Join("\r\n", failureMessages.ToArray()));
         }
      }

      /// <summary>
      /// Evaluates each exception to ensure that has a message constructor.
      /// </summary>
      [SuppressMessage("Microsoft.Naming", "CA1707: Identifiers should not contain underscores")]
      protected void Exceptions_Should_Have_A_Message_Constructor_Implementation()
      {
         var failureMessages = new List<String>();
         var exceptionTypes = FindExceptionTypes().ToList();
         foreach (var et in exceptionTypes)
         {
            failureMessages.AddRange(ValidatePublicMessageConstructor(et));
         }

         if (failureMessages.Count > 0)
         {
            throw new AssertFailedException(String.Join("\r\n", failureMessages.ToArray()));
         }
      }

      /// <summary>
      /// Evaluates each exception to ensure that has an serialization constructor.
      /// </summary>
      // WHAT IS THE BP?  [Serializable] is deprecated but BCL classes have this .ctor as well as 
      // GetObjectData
      [SuppressMessage("Microsoft.Naming", "CA1707: Identifiers should not contain underscores")]
      protected void Exceptions_Should_Have_A_Serialization_Constructor_Implementation()
      {
         var failureMessages = new List<String>();
         var exceptionTypes = FindExceptionTypes().ToList();
         foreach (var et in exceptionTypes)
         {
            failureMessages.AddRange(ValidateSerializationConstructor(et));
         }

         if (failureMessages.Count > 0)
         {
            throw new AssertFailedException(String.Join("\r\n", failureMessages.ToArray()));
         }
      }

      /// <summary>
      /// Evaluates each exception to ensure that has a inner exception constructor.
      /// </summary>
      [SuppressMessage("Microsoft.Naming", "CA1707: Identifiers should not contain underscores")]
      protected void Exceptions_Should_Have_An_Inner_Exception_Constructor_Implementation()
      {
         var failureMessages = new List<String>();
         var exceptionTypes = FindExceptionTypes().ToList();
         foreach (var et in exceptionTypes)
         {
            failureMessages.AddRange(ValidatePublicInnerExceptionConstructor(et));
         }

         if (failureMessages.Count > 0)
         {
            throw new AssertFailedException(String.Join("\r\n", failureMessages.ToArray()));
         }
      }

      /// <summary>
      /// Evaluates each exception to ensure that valid constructors excluding the default constructor, the inner exception
      /// constructor,
      /// the message constructor and the serialization constructor.
      /// </summary>
      [SuppressMessage("Microsoft.Naming", "CA1707: Identifiers should not contain underscores")]
      protected void Exceptions_Should_Have_Valid_Other_Public_Constructors_When_Present_Implementation()
      {
         var failureMessages = new List<String>();
         var exceptionTypes = FindExceptionTypes().ToList();
         foreach (var et in exceptionTypes)
         {
            failureMessages.AddRange(ValidateOtherPublicConstructors(et));
         }

         if (failureMessages.Count > 0)
         {
            throw new AssertFailedException(String.Join("\r\n", failureMessages.ToArray()));
         }
      }

      /// <summary>
      /// Gets the type or types from which all exceptions must descend.
      /// </summary>
      /// <returns>
      /// The acceptable base exception types.
      /// </returns>
      protected abstract IImmutableSet<Type> GetAcceptableBaseExceptionTypes();

      /// <summary>
      /// Gets the assemblies to be evaluated.
      /// </summary>
      /// <returns>
      /// The assemblies to be evaluated.
      /// </returns>
      protected abstract IImmutableSet<Assembly> GetAssembliesUnderTest();

      /// <summary>
      /// Gets a default test value for the given parameter type.
      /// </summary>
      /// <param name="parameterType">
      /// Type of the parameter.
      /// </param>
      /// <param name="defaultValue">
      /// The default value to use in the test.
      /// </param>
      /// <returns>
      /// <c>true</c> if the type is known, and a defaultValue is provided; otherwise <c>false</c>.
      /// </returns>
      /// <remarks>
      /// The base class, ExceptionValidityRequirements, "knows" how to instantiate values for the following types:
      /// Boolean              Guid?                   SByte?
      /// Boolean?             IEnumerable{String}     String
      /// Byte                 IEnumerable{type}       Timespan
      /// Byte?                Int16                   Timespan?
      /// DateTime             Int16?                  Type
      /// DateTime?            Int32                   UInt16
      /// DateTimeOffset       Int32?                  UInt16?
      /// DateTimeOffset?      Int64                   UInt32
      /// Enums                Int64?                  UInt32?
      /// Exception            Object                  UInt64
      /// Guid                 SByte                   UInt64?
      /// If your assembly uses exception constructor parameters or fields of another type, override this method and provide a
      /// default value for each type used in your assembly that is
      /// not in the list.
      /// Code Example
      /// <code>
      /// protected override GetDefaultValueForParameterType(Type parameterType, out Object defaultValue)
      /// {
      ///   if(parameterType == typeof(IMyObject))
      ///   {
      ///   defaultValue = new MyObject(Guid.NewGuid());
      ///   return true;
      ///   }
      /// 
      ///   defaultValue = null;
      ///   return false; 
      /// }
      /// </code>
      /// </remarks>
      [SuppressMessage("Microsoft.Design", "CA1021: Avoid out parameters")]
      [SuppressMessage("Microsoft.Design", "CA1007: Use generics where appropriate", Justification = "Sure, recommend generics, then require I implement this same method to avoid generics.")]
      protected virtual Boolean GetDefaultValueForParameterType(Type parameterType, out Object defaultValue)
      {
         defaultValue = null;
         return false;
      }

      private static IEnumerable<ConstructorInfo> GetOtherPublicConstructors(Type exceptionType)
      {
         var rv =
            from c in exceptionType.GetConstructors(BindingFlags.Instance | BindingFlags.Public)
            where
               c != GetPublicDefaultConstructor(exceptionType) &&
               c != GetPublicMessageConstructor(exceptionType) &&
               c != GetPublicStringAndExceptionConstructor(exceptionType) &&
               c != GetSerializationConstructor(exceptionType)
            select c;
         return rv;
      }

      private static ConstructorInfo GetPublicDefaultConstructor(Type exceptionType)
      {
         var rv = exceptionType.GetConstructor(BindingFlags.Instance | BindingFlags.Public, null, Type.EmptyTypes, null);
         return rv;
      }

      private static ConstructorInfo GetPublicInnerExceptionConstructor(Type exceptionType)
      {
         var rv = exceptionType.GetConstructor(BindingFlags.Instance | BindingFlags.Public, null, new[] {typeof(Exception)}, null);
         return rv;
      }

      private static ConstructorInfo GetPublicMessageConstructor(Type exceptionType)
      {
         var rv = exceptionType.GetConstructor(BindingFlags.Instance | BindingFlags.Public, null, new[] {typeof(String)}, null);
         return rv;
      }

      private static ConstructorInfo GetPublicStringAndExceptionConstructor(Type exceptionType)
      {
         var messageInnerExceptionConstructor = exceptionType.GetConstructor(
            BindingFlags.Instance | BindingFlags.Public,
            null,
            new[] {typeof(String), typeof(Exception)},
            null);
         return messageInnerExceptionConstructor;
      }

      private static ConstructorInfo GetSerializationConstructor(Type exceptionType)
      {
         var serializationConstructor = exceptionType.GetConstructor(
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
            null,
            new[] {typeof(SerializationInfo), typeof(StreamingContext)},
            null);
         return serializationConstructor;
      }

      private static IEnumerable<String> ValidateExceptionTypeByModifier(Type exceptionType)
      {
         var rv = new List<String>();
         if (!exceptionType.IsAbstract && !exceptionType.IsSealed)
         {
            var msg = $"The exception type '{exceptionType.Name}' should be abstract or sealed but is neither.";
            rv.Add(msg);
         }

         return rv;
      }

      private static IEnumerable<String> ValidateExceptionTypeByScope(Type exceptionType)
      {
         var rv = new List<String>();
         if (!exceptionType.IsPublic)
         {
            var msg = $"The exception type '{exceptionType.Name}' should be public but is not.";
            rv.Add(msg);
         }

         return rv;
      }

      [SuppressMessage("SonarLint.CodeSmell", "S3242: Consider using a more general type", Justification = "Considered (MWP)")]
      private static IEnumerable<String> ValidateNotAttributedSerializable(Type exceptionType)
      {
         var rv = new List<String>();

         // exceptionType.IsSerializable should not be used.
         if (exceptionType.GetCustomAttribute<SerializableAttribute>(false) != null)
         {
            rv.Add($"The exception type '{exceptionType.Name}' should NOT be decorated with [Serializable] but is.");
         }

         return rv;
      }

      [SuppressMessage("SonarLint.CodeSmell", "S4056:Overloads with a 'CultureInfo' or an 'IFormatProvider' parameter should be used", Justification = "reflection (MWP)")]
      [SuppressMessage("Microsoft.Design", "CA1031: Do not catch general exception types", Justification = "Appropriate in this case.")]
      private static IEnumerable<String> ValidatePublicDefaultConstructor(Type exceptionType)
      {
         var rv = new List<String>();

         var defaultConstructor = GetPublicDefaultConstructor(exceptionType);

         // The exception type should have a default constructor.
         if (defaultConstructor == null)
         {
            rv.Add(
               $"The exception type '{exceptionType.Name}' should have a public default constructor, but does not have one.\n");
         }
         else
         {
            try
            {
               // The exception type's default constructor should not throw.
               defaultConstructor.Invoke(Array.Empty<Object>());
            }
            catch
            {
               rv.Add(
                  $"The exception type '{exceptionType.Name}' should not throw when its default constructor is invoked, but it does throw.\n");
            }
         }

         return rv;
      }

      [SuppressMessage("SonarLint.CodeSmell", "S4056:Overloads with a 'CultureInfo' or an 'IFormatProvider' parameter should be used", Justification = "reflection (MWP)")]
      [SuppressMessage("Microsoft.Design", "CA1031: Do not catch general exception types", Justification = "Appropriate in this case.")]
      private static IEnumerable<String> ValidatePublicInnerExceptionConstructor(Type exceptionType)
      {
         var rv = new List<String>();

         var publicInnerExceptionConstructor = GetPublicInnerExceptionConstructor(exceptionType);

         // the exception type should a message, innerException constructor.
         if (publicInnerExceptionConstructor == null)
         {
            rv.Add(
               $"The exception type '{exceptionType.Name}' should have a public constructor with an exception parameter named 'innerException', but does not have one.\n");
         }
         else
         {
            var parameters = publicInnerExceptionConstructor.GetParameters();
            if (parameters[0].ParameterType == typeof(Exception) &&
                parameters[0].Name.Equals("innerException", StringComparison.Ordinal))
            {
               // the message, innerException constructor should not throw.
               try
               {
                  // invoke with nulls
                  publicInnerExceptionConstructor.Invoke(new Object[] {null});
               }
               catch
               {
                  rv.Add(
                     $"The exception type '{exceptionType.Name}' should not throw when its innerException constructor is invoked with a null values, but it does throw.\n");
               }

               try
               {
                  // invoke with non-null
                  var innerException = new ArgumentNullException(Guid.NewGuid().ToString());
                  var exception = publicInnerExceptionConstructor.Invoke(new Object[] {innerException}) as Exception;

                  // ReSharper disable once PossibleNullReferenceException
                  if (exception.InnerException != innerException)
                  {
                     rv.Add(
                        $"The exception type '{exceptionType.Name}' should not mangle the inner exception in the innerException constructor, but it does.\n");
                  }
               }
               catch
               {
                  rv.Add(
                     $"The exception type '{exceptionType.Name}' should not throw when its message, innerException constructor is invoked, but it does throw.\n");
               }
            }
            else
            {
               rv.Add(
                  $"The exception type '{exceptionType.Name}' should have a public constructor with the following signature: (string message, Exception innerException), but does not.\n");
            }
         }

         return rv;
      }

      [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
      [SuppressMessage("SonarLint.CodeSmell", "S3776: Cognitive Complexity of methods should not be too high", Justification = "Test code (MWP)")]
      [SuppressMessage("SonarLint.CodeSmell", "S4056: Overloads with a CultureInfo or an IFormatProvider parameter should be used", Justification = "reflection (MWP)")]
      private static IEnumerable<String> ValidatePublicMessageAndInnerExceptionConstructor(Type exceptionType)
      {
         var rv = new List<String>();

         var publicMessageInnerExceptionConstructor = GetPublicStringAndExceptionConstructor(exceptionType);

         // the exception type should have a message, innerException constructor.
         if (publicMessageInnerExceptionConstructor == null)
         {
            rv.Add(
               $"The exception type '{exceptionType.Name}' should have a public constructor with a string parameter named 'message' " +
               "and a exception parameter named \'innerException\', but does not have such a constructor.\n");
         }
         else
         {
            var parameters = publicMessageInnerExceptionConstructor.GetParameters();
            if (parameters[0].ParameterType == typeof(String) &&
                parameters[0].Name.Equals("message", StringComparison.Ordinal) &&
                parameters[1].ParameterType == typeof(Exception) &&
                parameters[1].Name.Equals("innerException", StringComparison.Ordinal))
            {
               // the message, innerException constructor should not throw.
               try
               {
                  // invoke with nulls
                  publicMessageInnerExceptionConstructor.Invoke(new Object[] {null, null});
               }
               catch
               {
                  rv.Add(
                     $"The exception type '{exceptionType.Name}' should not throw when its message, innerException constructor is " +
                     "invoked with a null values, but it does throw.\n");
               }

               try
               {
                  // invoke with non-nulls
                  var message = Guid.NewGuid().ToString("D", CultureInfo.InvariantCulture);
                  var innerException = new ArgumentNullException(Guid.NewGuid().ToString("D", CultureInfo.InvariantCulture));
                  var exception = publicMessageInnerExceptionConstructor.Invoke(new Object[] {message, innerException}) as Exception;

                  // ReSharper disable once PossibleNullReferenceException
                  if (!exception.Message.Equals(message, StringComparison.Ordinal))
                  {
                     rv.Add(
                        $"The exception type '{exceptionType.Name}' should not mangle the message in the message, innerException " +
                        $"constructor.  The message should be '{message}' but is '{exception.Message}'.\n");
                  }

                  if (exception.InnerException != innerException)
                  {
                     rv.Add(
                        $"The exception type '{exceptionType.Name}' should not mangle the inner exception in the message, innerException constructor, but it does mangle the inner exception.\n");
                  }
               }
               catch
               {
                  rv.Add(
                     $"The exception type '{exceptionType.Name}' should not throw when its message, innerException constructor is invoked, but it does throw.\n");
               }
            }
            else
            {
               rv.Add(
                  $"The exception type '{exceptionType.Name}' should have a public constructor with the following signature: " +
                  "(string message, Exception innerException), but does not have such a constructor.\n");
            }
         }

         return rv;
      }

      [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
      [SuppressMessage("SonarLint.CodeSmell", "S4056:Overloads with a 'CultureInfo' or an 'IFormatProvider' parameter should be used", Justification = "reflection (MWP)")]
      private static IEnumerable<String> ValidatePublicMessageConstructor(Type exceptionType)
      {
         var rv = new List<String>();

         var messageConstructor = GetPublicMessageConstructor(exceptionType);

         // The exception type should have a message constructor.
         if (messageConstructor == null)
         {
            rv.Add(
               $"The exception type '{exceptionType.Name}' should have a public constructor with a single string parameter named " +
               "\'message\', but does not have one.\n");
         }
         else
         {
            var stringParameter = messageConstructor.GetParameters().First();

            // the exception type should have a constructor with a single string parameter named 'message'.
            if (stringParameter.Name.Equals("message", StringComparison.Ordinal))
            {
               // the message constructor should not throw.
               try
               {
                  // invoke with a null message.

                  messageConstructor.Invoke(new Object[] {null});
               }
               catch
               {
                  rv.Add(
                     $"The exception type '{exceptionType.Name}' should not throw when its message constructor is invoked with a null value, but it does throw.\n");
               }

               try
               {
                  // invoke with a non-null message.
                  var message = Guid.NewGuid().ToString("D", CultureInfo.InvariantCulture);

                  var exception = messageConstructor.Invoke(new Object[] {message}) as Exception;

                  // the message constructor should not mangle the message.
                  if (!exception.Message.Equals(message, StringComparison.Ordinal))
                  {
                     rv.Add(
                        $"The exception type '{exceptionType.Name}' should not mangle the message in the message constructor.  " +
                        $"The message should be '{message}' but is '{exception.Message}'.\n");
                  }
               }
               catch
               {
                  rv.Add(
                     $"The exception type '{exceptionType.Name}' should not throw when its message constructor is invoked, " +
                     "but it does throw.");
               }
            }
            else
            {
               rv.Add(
                  $"The exception type '{exceptionType.Name}' should have a public constructor with a single string parameter named " +
                  "\'message\', but does not have one.  It has been replaced with a single string constructor with a" +
                  $" parameter named '{stringParameter.Name}'.\n");
            }
         }

         return rv;
      }

      private static IEnumerable<String> ValidateSerializationConstructor(Type exceptionType)
      {
         var rv = new List<String>();

         var serializationConstructor = GetSerializationConstructor(exceptionType);

         if (serializationConstructor == null)
         {
            rv.Add(
               $"The exception type '{exceptionType.Name}' should have a non-public serialization constructor with the " +
               "following signature: (SerializationInfo info, StreamingContext context), but does not.\n");
         }
         else
         {
            var parameters = serializationConstructor.GetParameters();

            if (parameters[0].ParameterType == typeof(SerializationInfo) &&
                parameters[0].Name.Equals("info", StringComparison.Ordinal) &&
                parameters[1].ParameterType == typeof(StreamingContext) &&
                parameters[1].Name.Equals("context", StringComparison.Ordinal))
            {
               if (serializationConstructor.IsPublic)
               {
                  rv.Add(
                     $"The exception type '{exceptionType.Name}' should have a non-public serialization constructor but has a public " +
                     "serialization constructor.\n");
               }

               // serialization itself is validated elsewhere.
            }
            else
            {
               rv.Add(
                  $"The exception type '{exceptionType.Name}' should have a non-public serialization constructor with the following " +
                  "signature: (SerializationInfo info, StreamingContext context), but does not.\n");
            }
         }

         return rv;
      }

      private IEnumerable<Type> FindExceptionTypes()
      {
         var exceptionTypes = new List<Type>();

         var assemblies = GetAssembliesUnderTest();

         var allTypes = new List<Type>();
         foreach (var assembly in assemblies)
         {
            allTypes.AddRange(assembly.SafeGetTypes());
         }

         exceptionTypes.AddRange(allTypes.Where(t => typeof(Exception).IsAssignableFrom(t) && !t.IsAbstract));
         var rv = (from et in exceptionTypes orderby et.Name select et).ToList();
         return rv;
      }

      private IEnumerable<String> ValidateExceptionTypeAncestor(Type exceptionType)
      {
         var rv = new List<String>();
         var acceptableBaseTypes = GetAcceptableBaseExceptionTypes();

         var acceptable = false;
         foreach (var acceptableType in acceptableBaseTypes)
         {
            if (acceptableType.IsAssignableFrom(exceptionType))
            {
               acceptable = true;
               break;
            }
         }

         var sb = new StringBuilder();
         foreach (var t in acceptableBaseTypes)
         {
            sb.Append("\t'" + t.Name + "', or\n");
         }

         var acceptableNameList = sb.ToString().Substring(0, sb.Length - 5);

         if (!acceptable)
         {
            var msg =
               $"The exception type '{exceptionType.Name}' should descend from one of the following:\n{acceptableNameList}\nbut does not.\n";
            rv.Add(msg);
         }

         return rv;
      }

      [SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
      [SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "System.String.Format(System.String,System.Object)")]
      [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode")]
      [SuppressMessage("Microsoft.Performance", "CA1809:AvoidExcessiveLocals")]
      [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
      [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")]
      [SuppressMessage("SonarLint.CodeSmell", "S3242: Consider using a more general type", Justification = "Considered (MWP)")]
      [SuppressMessage(
         "SonarLint.CodeSmell",
         "S3776: Cognitive Complexity of methods should not be too high",
         Justification = "Known issue (MWP)")]
      [SuppressMessage("SonarLint.CodeSmell", "S4056: Overloads with a CultureInfo or an IFormatProvider parameter should be used")]
      private IEnumerable<String> ValidateOtherPublicConstructors(Type exceptionType)
      {
         var rv = new List<String>();

         var otherPublicConstructors = GetOtherPublicConstructors(exceptionType);
         foreach (var constructorInfo in otherPublicConstructors)
         {
            var parameters = constructorInfo.GetParameters().ToList();

            // exception parameters
            //    must be last
            //    must be named 'innerException'
            //    must be of type Exception
            var exceptionParameters =
               (from p in parameters where typeof(Exception).IsAssignableFrom(p.ParameterType) select p).ToList();
            if (exceptionParameters.Count > 1)
            {
               rv.Add($"Exception type '{exceptionType.Name}' violates the pattern:  no more than one exception parameter.\n");
            }

            if (exceptionParameters.Count == 1)
            {
               var theExceptionParameter = exceptionParameters.Last();
               if (!ReferenceEquals(theExceptionParameter, parameters.Last()))
               {
                  rv.Add(
                     $"Exception type '{exceptionType.Name}' violates the pattern:  an exception parameter in a constructor must be the last parameter.\n");
                  break;
               }

               if (!theExceptionParameter.Name.Equals("innerException", StringComparison.Ordinal))
               {
                  rv.Add(
                     $"Exception type '{exceptionType.Name}' violates the pattern:  an exception parameter in a constructor must be named 'innerException'.\n");
                  break;
               }

               if (theExceptionParameter.ParameterType != typeof(Exception))
               {
                  rv.Add(
                     $"Exception type '{exceptionType.Name}' violates the pattern:  an exception parameter in a constructor must be of type Exception.\n");
                  break;
               }
            }

            // message parameters
            //    must be last
            //    or directly precede innerException
            var messageParameter =
               (from p in parameters where p.Name.Equals("message", StringComparison.Ordinal) select p).SingleOrDefault();
            if (messageParameter != null)
            {
               if (messageParameter.ParameterType != typeof(String))
               {
                  rv.Add(
                     $"Exception type '{exceptionType.Name}' violates the pattern:  'message' parameters in a constructor must be of type string.\n");
                  break;
               }

               var lastParameter = parameters.Last();
               if (!ReferenceEquals(messageParameter, lastParameter))
               {
                  var idxImmediatelyPrecedingLast = parameters.IndexOf(parameters.Last()) - 1;
                  var parameterImmediatelyPrecedingLast = parameters[idxImmediatelyPrecedingLast];
                  if (lastParameter.ParameterType != typeof(Exception) ||
                      !String.Equals(lastParameter.Name, "innerException", StringComparison.Ordinal) ||
                      !ReferenceEquals(messageParameter, parameterImmediatelyPrecedingLast))
                  {
                     rv.Add(
                        $"Exception type '{exceptionType.Name}' violates the pattern:  'message' parameters in a constructor are last, or immediately precede the innerException parameter.\n");
                     break;
                  }
               }
            }

            // any parameter in a constructor must have a read-only property of the same type with a Title-Cased name.
            var argumentNameValuePairs = new Dictionary<String, Object>();
            foreach (var p in parameters)
            {
               if (p.ParameterType == typeof(String))
               {
                  argumentNameValuePairs.Add(p.Name, Guid.NewGuid().ToString("D", CultureInfo.InvariantCulture));
               }
               else if (typeof(IEnumerable<String>) == p.ParameterType)
               {
                  argumentNameValuePairs.Add(
                     p.Name,
                     new List<String>
                     {
                        Guid.NewGuid().ToString("D", CultureInfo.InvariantCulture),
                        Guid.NewGuid().ToString("D", CultureInfo.InvariantCulture),
                        Guid.NewGuid().ToString("D", CultureInfo.InvariantCulture)
                     });
               }
               else if (typeof(SByte?) == p.ParameterType)
               {
                  argumentNameValuePairs.Add(p.Name, (Byte?)t_random.Next(1, SByte.MaxValue));
               }
               else if (typeof(SByte) == p.ParameterType)
               {
                  argumentNameValuePairs.Add(p.Name, (Byte)t_random.Next(1, SByte.MaxValue));
               }
               else if (typeof(Byte?) == p.ParameterType)
               {
                  argumentNameValuePairs.Add(p.Name, (Byte?)t_random.Next(1, SByte.MaxValue));
               }
               else if (typeof(Byte) == p.ParameterType)
               {
                  argumentNameValuePairs.Add(p.Name, (Byte)t_random.Next(1, SByte.MaxValue));
               }
               else if (typeof(Boolean?) == p.ParameterType)
               {
                  argumentNameValuePairs.Add(p.Name, (Boolean?)true);
               }
               else if (typeof(Boolean) == p.ParameterType)
               {
                  argumentNameValuePairs.Add(p.Name, true);
               }
               else if (typeof(Int16?) == p.ParameterType)
               {
                  argumentNameValuePairs.Add(p.Name, (Byte?)t_random.Next(1, Int16.MaxValue));
               }
               else if (typeof(Int16) == p.ParameterType)
               {
                  argumentNameValuePairs.Add(p.Name, (Byte)t_random.Next(1, Int16.MaxValue));
               }
               else if (typeof(UInt16?) == p.ParameterType)
               {
                  argumentNameValuePairs.Add(p.Name, (UInt16?)t_random.Next(1, Int16.MaxValue));
               }
               else if (typeof(UInt16) == p.ParameterType)
               {
                  argumentNameValuePairs.Add(p.Name, (UInt16)t_random.Next(1, Int16.MaxValue));
               }
               else if (typeof(Int32?) == p.ParameterType)
               {
                  argumentNameValuePairs.Add(p.Name, (Int32?)t_random.Next(1, Int32.MaxValue));
               }
               else if (typeof(Int32) == p.ParameterType)
               {
                  argumentNameValuePairs.Add(p.Name, t_random.Next(1, Int32.MaxValue));
               }
               else if (typeof(UInt32?) == p.ParameterType)
               {
                  argumentNameValuePairs.Add(p.Name, (Int32?)t_random.Next(1, Int32.MaxValue));
               }
               else if (typeof(UInt32) == p.ParameterType)
               {
                  argumentNameValuePairs.Add(p.Name, t_random.Next(1, Int32.MaxValue));
               }
               else if (typeof(Int64?) == p.ParameterType)
               {
                  argumentNameValuePairs.Add(p.Name, (Int64?)t_random.Next(1, Int32.MaxValue));
               }
               else if (typeof(Int64) == p.ParameterType)
               {
                  argumentNameValuePairs.Add(p.Name, (Int64)t_random.Next(1, Int32.MaxValue));
               }
               else if (typeof(UInt64) == p.ParameterType)
               {
                  argumentNameValuePairs.Add(p.Name, (UInt64)t_random.Next(1, Int32.MaxValue));
               }
               else if (typeof(UInt64?) == p.ParameterType)
               {
                  argumentNameValuePairs.Add(p.Name, (UInt64?)t_random.Next(1, Int32.MaxValue));
               }
               else if (typeof(Guid) == p.ParameterType)
               {
                  argumentNameValuePairs.Add(p.Name, Guid.NewGuid());
               }
               else if (typeof(Guid?) == p.ParameterType)
               {
                  argumentNameValuePairs.Add(p.Name, (Guid?)Guid.NewGuid());
               }
               else if (typeof(DateTime) == p.ParameterType)
               {
                  argumentNameValuePairs.Add(p.Name, new DateTime(t_random.Next(1, Int32.MaxValue), DateTimeKind.Utc));
               }
               else if (typeof(DateTime?) == p.ParameterType)
               {
                  argumentNameValuePairs.Add(p.Name, (DateTime?)new DateTime(t_random.Next(1, Int32.MaxValue), DateTimeKind.Utc));
               }
               else if (typeof(DateTimeOffset) == p.ParameterType)
               {
                  argumentNameValuePairs.Add(p.Name, new DateTimeOffset(new DateTime(t_random.Next(1, Int32.MaxValue), DateTimeKind.Utc)));
               }
               else if (typeof(DateTimeOffset?) == p.ParameterType)
               {
                  argumentNameValuePairs.Add(
                     p.Name,
                     (DateTimeOffset?)new DateTimeOffset(new DateTime(t_random.Next(1, Int32.MaxValue), DateTimeKind.Utc)));
               }
               else if (typeof(TimeSpan) == p.ParameterType)
               {
                  argumentNameValuePairs.Add(p.Name, new TimeSpan(t_random.Next(1, Int32.MaxValue)));
               }
               else if (typeof(TimeSpan?) == p.ParameterType)
               {
                  argumentNameValuePairs.Add(p.Name, (TimeSpan?)new TimeSpan(t_random.Next(1, Int32.MaxValue)));
               }
               else if (typeof(Type) == p.ParameterType)
               {
                  argumentNameValuePairs.Add(p.Name, typeof(TypeCode));
               }
               else if (typeof(IEnumerable<Type>) == p.ParameterType)
               {
                  argumentNameValuePairs.Add(p.Name, new List<Type> {typeof(Object), typeof(Guid), typeof(ConsoleColor)});
               }
               else if (typeof(Exception) == p.ParameterType)
               {
                  argumentNameValuePairs.Add(
                     p.Name,
                     new InvalidOperationException(Guid.NewGuid().ToString("D", CultureInfo.InvariantCulture)));
               }
               else if (p.ParameterType.IsEnum)
               {
                  var values = Enum.GetValues(p.ParameterType);
                  var value = values.Cast<Object>().First();
                  argumentNameValuePairs.Add(p.Name, Convert.ChangeType(value, p.ParameterType, CultureInfo.InvariantCulture));
               }
               else if (typeof(Object) == p.ParameterType)
               {
                  argumentNameValuePairs.Add(p.Name, new Object());
               }
               else
               {
                  if (GetDefaultValueForParameterType(p.ParameterType, out var defaultValue))
                  {
                     argumentNameValuePairs.Add(p.Name, defaultValue);
                  }
                  else
                  {
                     // Something is wrong, cease and desist, letting the developer know.
                     rv.Add(
                        "BUG IN EXCEPTION VALIDITY TEST: Maintenance is required, the test code does not know how to validate the type (\'" +
                        $"{p.ParameterType.Name}') of the parameter '{p.Name}' in exception type: '{exceptionType.Name}'.\n");
                     break;
                  }
               }
            }

            // invoke the constructor and verify what went in, is what comes out.
            if (argumentNameValuePairs.Count > 0)
            {
               var exception = constructorInfo.Invoke(argumentNameValuePairs.Values.ToArray()) as Exception;
               var publicProperties = exceptionType.GetProperties(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);

               foreach (var kvp in argumentNameValuePairs)
               {
                  var matchingProperties = (from pp in publicProperties where pp.Name.Equals(kvp.Key, StringComparison.OrdinalIgnoreCase) select pp).ToList();
                  if (matchingProperties.Count > 1)
                  {
                     rv.Add(
                        $"Exception type '{exceptionType.Name}' violates the pattern:  all constructor arguments have a matching " +
                        "read-only property of the same name, but title-cased.  More than one matching property was found for \'" +
                        "{kvp.Key}\' including \'{matchingProperties.First().Name}\' and \'{3}\'.\n");
                  }
                  else if (matchingProperties.Count == 1)
                  {
                     var theProperty = matchingProperties.First();
                     if (theProperty.GetSetMethod(false) != null)
                     {
                        rv.Add(
                           $"Exception type '{exceptionType.Name}' violates the pattern:  all constructor arguments have a matching " +
                           $"read-only property of the same name, but title-cased.  A public mutator is exposed on '{kvp.Key}'.\n");
                     }

                     var propertyValue = theProperty.GetValue(exception, null);
                     if (
                        !(ReferenceEquals(kvp.Value, null) &&
                          String.Equals(theProperty.Name, "Message", StringComparison.Ordinal) &&
                          typeof(ArgumentException).IsAssignableFrom(exceptionType)))
                     {
                        // ArgumentException and derivatives append the argument name so use starts with.
                        if (propertyValue is String stringValue && !stringValue.StartsWith((String)kvp.Value, StringComparison.CurrentCulture))
                        {
                           rv.Add(
                              $"Exception type '{exceptionType.Name}' violates the pattern:  argument exceptions and derived types start " +
                              $"Message values with the original Message.  Expected Message to start with '{kvp.Value}' but is " +
                              $"'{stringValue}'.\n");
                        }
                        else
                        {
                           if (propertyValue is IEnumerable<Object> objects)
                           {
                              if (!objects.SequenceEqual(kvp.Value as IEnumerable<Object>))
                              {
                                 rv.Add($"Exception type '{exceptionType.Name}' mangled the value of property '{theProperty.Name}'.\n");
                              }
                           }
                           else
                           {
                              if (!Equals(propertyValue, kvp.Value))
                              {
                                 if (typeof(ArgumentException).IsAssignableFrom(exceptionType) && theProperty.Name.Equals("Message", StringComparison.Ordinal))
                                 {
                                    // retest:  Argument Exception Appends \r\nParameter Name: parameter name;
                                    if (!((String)propertyValue).StartsWith((String)kvp.Value, StringComparison.Ordinal))
                                    {
                                       rv.Add($"Exception type '{exceptionType.Name}' mangled the value of property '{theProperty.Name}'.\n");
                                    }
                                 }
                                 else
                                 {
                                    rv.Add($"Exception type '{exceptionType.Name}' mangled the value of property '{theProperty.Name}'.\n");
                                 }
                              }
                           }
                        }
                     }
                     else
                     {
                        rv.Add(
                           $"Exception type '{exceptionType.Name}' violates the pattern:  all constructor arguments have a matching " +
                           $"read-only property of the same name, but title-cased.  No matching property was found for '{kvp.Key}'.\n");
                     }

                     // Clone and serialization Test

                     //using (var memoryStream = new MemoryStream())
                     //{
                     //Object clone;
                     // TODO: Need to research a correct strategy in .Net Core.  Binary Serialization is being removed, and does not work.
                     //}

                     //var clonedPropertyValue = theProperty.GetValue(clone, null);

                     //if (ReferenceEquals(propertyValue, null))
                     //{
                     //   if (!ReferenceEquals(clonedPropertyValue, null))
                     //   {
                     //      rv.Add($"Exception type '{exceptionType.Name}' did not serialize the property '{theProperty.Name}' correctly.\n");
                     //   }
                     //}
                     //else
                     //{
                     //   if (ReferenceEquals(clonedPropertyValue, null))
                     //   {
                     //      rv.Add($"Exception type '{exceptionType.Name}' did not serialize the property '{theProperty.Name}' correctly.\n");
                     //   }

                     //   // hack: comparing to string values rather than actual values because reference equality lost upon serialization.
                     //   if (!String.Equals(propertyValue.ToString(), clonedPropertyValue.ToString(), StringComparison.Ordinal))
                     //   {
                     //      rv.Add($"Exception type '{exceptionType.Name}' did not serialize the property '{theProperty.Name}' correctly.\n");
                     //   }
                     //}
                  }
                  else
                  {
                     TestHelp.DoNothing();
                  }
               }
            }
         }

         return rv;
      }
   }
}
