namespace Landorphan.Common.Resources
{
   using System;

   internal static class StringResources
   {
      internal const String ArgumentContainsInvalidValueExceptionDefaultMessage = "The value must not contain invalid values.";
      internal const String ArgumentContainsNullExceptionDefaultMessage = "The value must not contain null values.";
      internal const String ArgumentEmptyExceptionDefaultMessage = "The value must not be empty.";
      internal const String ArgumentWhitespaceExceptionDefaultMessage = "The value must not be composed entirely of white-space.";
      internal const String EmptyPriorityQueue = "The priority queue is empty.";
      internal const String EventHandlerMustNotHaveNullMethodArgumentExceptionFmt =
         "The event handler argument '{0}' must not return a null method, but did.";
      internal const String EventHandlerMustNotHaveStaticMethodArgumentExceptionFmt =
         "The event handler argument '{0}' must not return a static method, but did.";
      internal const String ExtendedInvalidEnumArgumentExceptionMessageFmt =
         "The value of argument '{0}' ({1}) is invalid for Enum type '{2}'.";
      internal const String InvalidLockTimeoutArgumentExceptionMessageFmt =
         @"The value of argument '{0}' ({1} ms) is invalid.  Timeout values must be between -1 (which represents ""never"") and {2} total milliseconds.";
      internal const String LockRecursionExceptionReadAfterUpgradeNotAllowed =
         "A read lock may not be acquired with the upgradeable read lock held in this mode.";
      internal const String MessageTypeError = "Error";
      internal const String MessageTypeInformation = "Information";
      internal const String MessageTypeVerbose = "Verbose";
      internal const String MessageTypeWarning = "Warning";
      internal const String NullReplacementValue = "null";
      internal const String TheCurrentInstanceIsReadOnly = "The current instance is read-only.";
      internal const String TimeoutElapsedBeforeLockObtainedExceptionDefaultMessageFmt =
         "A lock was not obtained before the timeout elapsed ({0}).";
      internal const String ValueMustBeGreaterThanFmt = "The value must be greater than '{0}' but is '{1}'.";
      internal const String ValueMustBeGreaterThanOrEqualToFmt = "The value must be greater than or equal to '{0}' but is '{1}'.";
      internal const String ValueMustBeLessThanFmt = "The value must be less than '{0}' but is '{1}'.";
      internal const String ValueMustBeLessThanOrEqualToFmt = "The value must be less than or equal to '{0}' but is '{1}'.";
   }
}
