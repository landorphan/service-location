namespace Landorphan.Abstractions.Console.Interfaces
{
   using System;

   /// <summary>
   /// Interface for console abc.
   /// </summary>
   public interface IConsoleMisc
   {
      /// <summary>
      /// Occurs when the Control modifier key (Ctrl) and either the ConsoleKey.C console key (C) or the Break key are pressed simultaneously (Ctrl+C or
      /// Ctrl+Break).
      /// </summary>
      /// <remarks>
      /// <p>
      /// This event is used in conjunction with <see cref="EventHandler"/> and <see cref="ConsoleCancelEventArgs"/>.  The CancelKeyPress event
      /// enables a console application to intercept the Ctrl+C signal so the event handler can decide whether to continue executing or terminate.
      /// For more information about handling events,
      /// <see href="http://msdn.microsoft.com/en-us/library/edzehd2t(v=vs.110).aspx">
      /// Handling and Raising Events
      /// </see>
      /// </p>
      /// <p>
      /// When the user presses either Ctrl+C or Ctrl+Break, the CancelKeyPress event is fired and the application's
      /// <see cref="EventHandler{ConsoleCancelEventArgs}"/> event handler is executed.  The event handler is passed a
      /// <see cref="ConsoleCancelEventArgs"/> Object that has two useful properties:
      /// </p>
      /// • <see cref="ConsoleCancelEventArgs.SpecialKey"/>, which allows you to determine whether the handler was invoked as a result of the user
      /// pressing Ctrl+C (the property value is ConsoleSpecialKey.ControlC) or Ctrl+Break (the property value is ConsoleSpecialKey.ControlBreak).
      /// <p>
      /// • <see cref="ConsoleCancelEventArgs.Cancel"/>, which allows you to determine how to your application should respond to the user pressing
      /// Ctrl+C or Ctrl+Break.  By default, the Cancel property is false, which causes program execution to terminate when the event handler exits.
      /// Changing its property to true specifies that the application should continue to execute.
      /// </p>
      /// <p>
      /// The event handler for this event is executed on a thread pool thread.
      /// </p>
      /// </remarks>
      event EventHandler<ConsoleCancelEventArgs> CancelKeyPress;

      /// <summary>
      /// Gets a value indicating whether the CAPS LOCK keyboard toggle is turned on or turned off.
      /// </summary>
      /// <value>
      /// <c> true </c> if CAPS LOCK is turned on; false if CAPS LOCK is turned <c> off </c>.
      /// </value>
      Boolean CapsLock { get; }

      /// <summary>
      /// Gets a value indicating whether the NUM LOCK keyboard toggle is turned on or turned off.
      /// </summary>
      /// <value>
      /// <c> true </c> if NUM LOCK is turned on; <c> false </c> if NUM LOCK is turned off.
      /// </value>
      Boolean NumberLock { get; }

      /// <summary>
      /// Plays the sound of a beep through the console speaker.
      /// </summary>
      /// <remarks>
      /// By default, the beep plays at a frequency of 800 hertz for a duration of 200 milliseconds.
      /// Whether Beep produces a sound on versions of Windows before Windows 7 depends on the
      /// presence of a 8254 programmable interval timer chip.  Starting with Windows 7, it depends on the default sound device.
      /// <para>
      /// In .Net Framework, this would throw System.Security.HostProtectionException when executed on a server, such as MS SQL Server, that does not permit access to a user interface.
      /// TODO: find out what exception is thrown in .Net Standard 2.0
      /// </para>
      /// </remarks>
      void Beep();

      /// <summary>
      /// Plays the sound of a beep of a specified frequency and duration through the console speaker.
      /// </summary>
      /// <param name="frequency">
      /// The frequency of the beep, ranging from 37 to 32767 hertz.
      /// </param>
      /// <param name="duration">
      /// The duration of the beep measured in milliseconds.
      /// </param>
      /// <remarks>
      /// Beep wraps a call to the Windows Beep function.
      /// Whether Beep produces a sound on versions of Windows before Windows 7 depends on the
      /// presence of a 8254 programmable interval timer chip.  Starting with Windows 7, it depends on the default sound device.
      /// <para>
      /// In .Net Framework, this would throw System.Security.HostProtectionException when executed on a server, such as MS SQL Server, that does not permit access to a user interface.
      /// TODO: find out what exception is thrown in .Net Standard 2.0
      /// </para>
      /// </remarks>
      /// <exception cref="ArgumentOutOfRangeException">
      /// <i> frequency </i> is less than 37 or more than 32767 hertz.
      /// -or-
      /// <i> duration </i> is less than or equal to zero.
      /// </exception>
      void Beep(Int32 frequency, Int32 duration);
   }
}
