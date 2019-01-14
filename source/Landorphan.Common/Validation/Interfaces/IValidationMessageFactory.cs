namespace Landorphan.Common.Validation
{
   using System;

   /// <summary>
   /// Represents a factory for instantiating <see cref="IValidationMessage"/>.
   /// </summary>
   public interface IValidationMessageFactory
   {
      /// <summary>
      /// Creates a read-only error <see cref="ValidationMessage"/> instance.
      /// </summary>
      /// <param name="text">
      /// The message text.
      /// </param>
      /// <returns>
      /// The new instance.
      /// </returns>
      IValidationMessage CreateErrorMessage(String text);

      /// <summary>
      /// Creates a read-only error <see cref="ValidationMessage"/> instance.
      /// </summary>
      /// <param name="format">
      /// A composite format String.
      /// </param>
      /// <param name="args">
      /// An Object array that contains zero or more objects to format.
      /// </param>
      /// <returns>
      /// The new instance.
      /// </returns>
      IValidationMessage CreateErrorMessage(String format, params Object[] args);

      /// <summary>
      /// Creates a read-only information <see cref="ValidationMessage"/> instance.
      /// </summary>
      /// <param name="text">
      /// The message text.
      /// </param>
      /// <returns>
      /// The new instance.
      /// </returns>
      IValidationMessage CreateInformationMessage(String text);

      /// <summary>
      /// Creates a read-only information <see cref="ValidationMessage"/> instance.
      /// </summary>
      /// <param name="format">
      /// A composite format String.
      /// </param>
      /// <param name="args">
      /// An Object array that contains zero or more objects to format.
      /// </param>
      /// <returns>
      /// The new instance.
      /// </returns>
      IValidationMessage CreateInformationMessage(String format, params Object[] args);

      /// <summary>
      /// Creates a read-only verbose <see cref="ValidationMessage"/> instance.
      /// </summary>
      /// <param name="text">
      /// The message text.
      /// </param>
      /// <returns>
      /// The new instance.
      /// </returns>
      IValidationMessage CreateVerboseMessage(String text);

      /// <summary>
      /// Creates a read-only verbose <see cref="ValidationMessage"/> instance.
      /// </summary>
      /// <param name="format">
      /// A composite format String.
      /// </param>
      /// <param name="args">
      /// An Object array that contains zero or more objects to format.
      /// </param>
      /// <returns>
      /// The new instance.
      /// </returns>
      IValidationMessage CreateVerboseMessage(String format, params Object[] args);

      /// <summary>
      /// Creates a read-only warning <see cref="ValidationMessage"/> instance.
      /// </summary>
      /// <param name="text">
      /// The message text.
      /// </param>
      /// <returns>
      /// The new instance.
      /// </returns>
      IValidationMessage CreateWarningMessage(String text);

      /// <summary>
      /// Creates a read-only warning <see cref="ValidationMessage"/> instance.
      /// </summary>
      /// <param name="format">
      /// A composite format String.
      /// </param>
      /// <param name="args">
      /// An Object array that contains zero or more objects to format.
      /// </param>
      /// <returns>
      /// The new instance.
      /// </returns>
      IValidationMessage CreateWarningMessage(String format, params Object[] args);
   }
}
