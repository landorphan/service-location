namespace Landorphan.Common.Resources
{
   using System;

   public static class StringResources
   {
      public const String ArgumentContainsInvalidValueExceptionDefaultMessage = "The value must not contain invalid values.";
      public const String ArgumentContainsNullExceptionDefaultMessage = "The value must not contain null values.";
      public const String ArgumentEmptyExceptionDefaultMessage = "The value must not be empty.";
      public const String ArgumentWhitespaceExceptionDefaultMessage = "The value must not be composed entirely of white-space.";
      public const String EmptyPriorityQueue = "The priority queue is empty.";
      public const String EventHandlerMustNotHaveNullMethodArgumentExceptionFmt =
         "The event handler argument '{0}' must not return a null method, but did.";
      public const String EventHandlerMustNotHaveStaticMethodArgumentExceptionFmt =
         "The event handler argument '{0}' must not return a static method, but did.";
      public const String ExtendedInvalidEnumArgumentExceptionMessageFmt =
         "The value of argument '{0}' ({1}) is invalid for Enum type '{2}'.";
      public const String InvalidLockTimeoutArgumentExceptionMessageFmt =
         @"The value of argument '{0}' ({1} ms) is invalid.  Timeout values must be between -1 (which represents ""never"") and {2} total milliseconds.";
      public const String LockRecursionExceptionReadAfterUpgradeNotAllowed =
         "A read lock may not be acquired with the upgradeable read lock held in this mode.";
      public const String MessageTypeError = "Error";
      public const String MessageTypeInformation = "Information";
      public const String MessageTypeVerbose = "Verbose";
      public const String MessageTypeWarning = "Warning";
      public const String NullReplacementValue = "null";
      public const String TheCurrentInstanceIsReadOnly = "The current instance is read-only.";
      public const String TimeoutElapsedBeforeLockObtainedExceptionDefaultMessageFmt =
         "A lock was not obtained before the timeout elapsed ({0}).";
      public const String ValueMustBeGreaterThanFmt = "The value must be greater than '{0}' but is '{1}'.";
      public const String ValueMustBeGreaterThanOrEqualToFmt = "The value must be greater than or equal to '{0}' but is '{1}'.";
      public const String ValueMustBeLessThanFmt = "The value must be less than '{0}' but is '{1}'.";
      public const String ValueMustBeLessThanOrEqualToFmt = "The value must be less than or equal to '{0}' but is '{1}'.";
   }
}