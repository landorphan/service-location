namespace Landorphan.Abstractions.Console
{
   using System;
   using System.Diagnostics.CodeAnalysis;
   using System.IO;
   using System.Text;
   using Landorphan.Abstractions.Console.Interfaces;
   using Landorphan.Abstractions.Internal;
   using Landorphan.Common;
   using Landorphan.Ioc.ServiceLocation;

   /// <summary>
   /// A console utilities.
   /// </summary>
   [SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]

   // ReSharper disable once RedundantExtendsListEntry
   public class ConsoleUtilities : DisposableObject, IConsole
   {
      private readonly SourceWeakEventHandlerSet<ConsoleCancelEventArgs> _cancelKeyPressListeners;

      [DoNotDispose]
      private readonly IConsoleInternalMapping _consoleImplementation;

      // This is for disposable use only (not disposing of the field that is service located).
      [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "Used to dispose.")]

      // ReSharper disable once NotAccessedField.Local
      private readonly IConsoleInternalMapping _ownedConsoleImplementation;

      /// <summary>
      /// Initializes a new instance of the <see cref="ConsoleUtilities"/> class.
      /// </summary>
      public ConsoleUtilities()
      {
         _cancelKeyPressListeners = new SourceWeakEventHandlerSet<ConsoleCancelEventArgs>();

         // reference the mapping in a field that is not disposed but is utilized.
         _consoleImplementation = IocServiceLocator.Resolve<IConsoleInternalMapping>();

         // wire the CancelKeyPress event.
         _consoleImplementation.CancelKeyPress += ConsoleImplementation_CancelKeyPress;
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="ConsoleUtilities"/> class.
      /// </summary>
      /// <param name="mapping">
      /// The <see cref="IConsoleInternalMapping"/> instance to use in the implementation.
      /// </param>
      /// <remarks>
      /// This instance takes responsibility for disposing of <paramref name="mapping"/>.
      /// </remarks>
      internal ConsoleUtilities(IConsoleInternalMapping mapping)
      {
         mapping.ArgumentNotNull(nameof(mapping));

         _cancelKeyPressListeners = new SourceWeakEventHandlerSet<ConsoleCancelEventArgs>();

         // reference the mapping in a field that is disposed;
         _ownedConsoleImplementation = mapping;

         // reference the mapping in a field that is not disposed but is utilized.
         _consoleImplementation = mapping;

         // wire the CancelKeyPress event.
         _consoleImplementation.CancelKeyPress += ConsoleImplementation_CancelKeyPress;
      }

      /// <inheritdoc/>
      protected override void ReleaseManagedResources()
      {
         // un-wire the CancelKeyPress event.
         _consoleImplementation.CancelKeyPress -= ConsoleImplementation_CancelKeyPress;
         base.ReleaseManagedResources();
      }

      /// <inheritdoc/>
      public event EventHandler<ConsoleCancelEventArgs> CancelKeyPress
      {
         add => _cancelKeyPressListeners.Add(value);
         remove => _cancelKeyPressListeners.Remove(value);
      }

      /// <inheritdoc/>
      public ConsoleColor BackgroundColor
      {
         get => _consoleImplementation.BackgroundColor;
         set => _consoleImplementation.BackgroundColor = value;
      }

      /// <inheritdoc/>
      public Int32 BufferHeight
      {
         get => _consoleImplementation.BufferHeight;
         set => _consoleImplementation.BufferHeight = value;
      }

      /// <inheritdoc/>
      public Int32 BufferWidth
      {
         get => _consoleImplementation.BufferWidth;
         set => _consoleImplementation.BufferWidth = value;
      }

      /// <inheritdoc/>
      public Boolean CapsLock => _consoleImplementation.CapsLock;

      /// <inheritdoc/>
      public Int32 CursorLeft
      {
         get => _consoleImplementation.CursorLeft;
         set => _consoleImplementation.CursorLeft = value;
      }

      /// <inheritdoc/>
      public Int32 CursorSize
      {
         get => _consoleImplementation.CursorSize;
         set => _consoleImplementation.CursorSize = value;
      }

      /// <inheritdoc/>
      public Int32 CursorTop
      {
         get => _consoleImplementation.CursorTop;
         set => _consoleImplementation.CursorTop = value;
      }

      /// <inheritdoc/>
      public Boolean CursorVisible
      {
         get => _consoleImplementation.CursorVisible;
         set => _consoleImplementation.CursorVisible = value;
      }

      /// <inheritdoc/>
      public TextWriter Error => _consoleImplementation.Error;

      /// <inheritdoc/>
      public ConsoleColor ForegroundColor
      {
         get => _consoleImplementation.ForegroundColor;
         set => _consoleImplementation.ForegroundColor = value;
      }

      /// <inheritdoc/>
      public TextReader Input => _consoleImplementation.Input;

      /// <inheritdoc/>
      public Encoding InputEncoding
      {
         get => _consoleImplementation.InputEncoding;
         set => _consoleImplementation.InputEncoding = value;
      }

      /// <inheritdoc/>
      public Boolean IsErrorRedirected => _consoleImplementation.IsErrorRedirected;

      /// <inheritdoc/>
      public Boolean IsInputRedirected => _consoleImplementation.IsInputRedirected;

      /// <inheritdoc/>
      public Boolean IsOutputRedirected => _consoleImplementation.IsOutputRedirected;

      /// <inheritdoc/>
      public Boolean KeyAvailable => _consoleImplementation.KeyAvailable;

      /// <inheritdoc/>
      public Int32 LargestWindowHeight => _consoleImplementation.LargestWindowHeight;

      /// <inheritdoc/>
      public Int32 LargestWindowWidth => _consoleImplementation.LargestWindowWidth;

      /// <inheritdoc/>
      public Boolean NumberLock => _consoleImplementation.NumberLock;

      /// <inheritdoc/>
      public TextWriter Output => _consoleImplementation.Output;

      /// <inheritdoc/>
      public Encoding OutputEncoding
      {
         get => _consoleImplementation.OutputEncoding;
         set => _consoleImplementation.OutputEncoding = value;
      }

      /// <inheritdoc/>
      public String Title
      {
         get => _consoleImplementation.Title;
         set => _consoleImplementation.Title = value;
      }

      /// <inheritdoc/>
      public Boolean TreatControlCAsInput
      {
         get => _consoleImplementation.TreatControlCAsInput;
         set => _consoleImplementation.TreatControlCAsInput = value;
      }

      /// <inheritdoc/>
      public Int32 WindowHeight
      {
         get => _consoleImplementation.WindowHeight;
         set => _consoleImplementation.WindowHeight = value;
      }

      /// <inheritdoc/>
      public Int32 WindowLeft
      {
         get => _consoleImplementation.WindowLeft;
         set => _consoleImplementation.WindowLeft = value;
      }

      /// <inheritdoc/>
      public Int32 WindowTop
      {
         get => _consoleImplementation.WindowTop;
         set => _consoleImplementation.WindowTop = value;
      }

      /// <inheritdoc/>
      public Int32 WindowWidth
      {
         get => _consoleImplementation.WindowWidth;
         set => _consoleImplementation.WindowWidth = value;
      }

      /// <inheritdoc/>
      public void Beep()
      {
         _consoleImplementation.Beep();
      }

      /// <inheritdoc/>
      public void Beep(Int32 frequency, Int32 duration)
      {
         _consoleImplementation.Beep(frequency, duration);
      }

      /// <inheritdoc/>
      public void Clear()
      {
         _consoleImplementation.Clear();
      }

      /// <inheritdoc/>
      public void MoveBufferArea(Int32 sourceLeft, Int32 sourceTop, Int32 sourceWidth, Int32 sourceHeight, Int32 targetLeft, Int32 targetTop)
      {
         _consoleImplementation.MoveBufferArea(sourceLeft, sourceTop, sourceWidth, sourceHeight, targetLeft, targetTop);
      }

      /// <inheritdoc/>
      public void MoveBufferArea(
         Int32 sourceLeft,
         Int32 sourceTop,
         Int32 sourceWidth,
         Int32 sourceHeight,
         Int32 targetLeft,
         Int32 targetTop,
         Char sourceChar,
         ConsoleColor sourceForeColor,
         ConsoleColor sourceBackColor)
      {
         _consoleImplementation.MoveBufferArea(
            sourceLeft,
            sourceTop,
            sourceWidth,
            sourceHeight,
            targetLeft,
            targetTop,
            sourceChar,
            sourceForeColor,
            sourceBackColor);
      }

      /// <inheritdoc/>
      public Stream OpenStandardError()
      {
         return _consoleImplementation.OpenStandardError();
      }

      /// <inheritdoc/>
      public Stream OpenStandardError(Int32 bufferSize)
      {
         return _consoleImplementation.OpenStandardError(bufferSize);
      }

      /// <inheritdoc/>
      public Stream OpenStandardInput()
      {
         return _consoleImplementation.OpenStandardInput();
      }

      /// <inheritdoc/>
      public Stream OpenStandardInput(Int32 bufferSize)
      {
         return _consoleImplementation.OpenStandardInput(bufferSize);
      }

      /// <inheritdoc/>
      public Stream OpenStandardOutput()
      {
         return _consoleImplementation.OpenStandardOutput();
      }

      /// <inheritdoc/>
      public Stream OpenStandardOutput(Int32 bufferSize)
      {
         return _consoleImplementation.OpenStandardOutput(bufferSize);
      }

      /// <inheritdoc/>
      public Int32 Read()
      {
         return _consoleImplementation.Read();
      }

      /// <inheritdoc/>
      public ConsoleKeyInfo ReadKey()
      {
         return _consoleImplementation.ReadKey();
      }

      /// <inheritdoc/>
      public ConsoleKeyInfo ReadKey(Boolean intercept)
      {
         return _consoleImplementation.ReadKey();
      }

      /// <inheritdoc/>
      public String ReadLine()
      {
         return _consoleImplementation.ReadLine();
      }

      /// <inheritdoc/>
      public void ResetColor()
      {
         _consoleImplementation.ResetColor();
      }

      /// <inheritdoc/>
      public void SetBufferSize(Int32 width, Int32 height)
      {
         _consoleImplementation.SetBufferSize(width, height);
      }

      /// <inheritdoc/>
      public void SetCursorPosition(Int32 left, Int32 top)
      {
         _consoleImplementation.SetCursorPosition(left, top);
      }

      /// <inheritdoc/>
      public void SetError(TextWriter newError)
      {
         _consoleImplementation.SetError(newError);
      }

      /// <inheritdoc/>
      public void SetInput(TextReader newInput)
      {
         _consoleImplementation.SetInput(newInput);
      }

      /// <inheritdoc/>
      public void SetOutput(TextWriter newOutput)
      {
         _consoleImplementation.SetOutput(newOutput);
      }

      /// <inheritdoc/>
      public void SetWindowPosition(Int32 left, Int32 top)
      {
         _consoleImplementation.SetWindowPosition(left, top);
      }

      /// <inheritdoc/>
      public void SetWindowSize(Int32 width, Int32 height)
      {
         _consoleImplementation.SetWindowSize(width, height);
      }

      /// <inheritdoc/>
      public void Write(Boolean value)
      {
         _consoleImplementation.Write(value);
      }

      /// <inheritdoc/>
      public void Write(Char value)
      {
         _consoleImplementation.Write(value);
      }

      /// <inheritdoc/>
      public void Write(Char[] buffer)
      {
         _consoleImplementation.Write(buffer);
      }

      /// <inheritdoc/>
      public void Write(Decimal value)
      {
         _consoleImplementation.Write(value);
      }

      /// <inheritdoc/>
      public void Write(Double value)
      {
         _consoleImplementation.Write(value);
      }

      /// <inheritdoc/>
      public void Write(Int32 value)
      {
         _consoleImplementation.Write(value);
      }

      /// <inheritdoc/>
      public void Write(Int64 value)
      {
         _consoleImplementation.Write(value);
      }

      /// <inheritdoc/>
      public void Write(Object value)
      {
         _consoleImplementation.Write(value);
      }

      /// <inheritdoc/>
      public void Write(Single value)
      {
         _consoleImplementation.Write(value);
      }

      /// <inheritdoc/>
      public void Write(String value)
      {
         _consoleImplementation.Write(value);
      }

      /// <inheritdoc/>
      [CLSCompliant(false)]
      public void Write(UInt32 value)
      {
         _consoleImplementation.Write(value);
      }

      /// <inheritdoc/>
      [CLSCompliant(false)]
      public void Write(UInt64 value)
      {
         _consoleImplementation.Write(value);
      }

      /// <inheritdoc/>
      public void Write(String format, Object arg0)
      {
         _consoleImplementation.Write(format, arg0);
      }

      /// <inheritdoc/>
      public void Write(String format, params Object[] arg)
      {
         _consoleImplementation.Write(format, arg);
      }

      /// <inheritdoc/>
      public void Write(Char[] buffer, Int32 index, Int32 count)
      {
         _consoleImplementation.Write(buffer, index, count);
      }

      /// <inheritdoc/>
      public void Write(String format, Object arg0, Object arg1)
      {
         _consoleImplementation.Write(format, arg0, arg1);
      }

      /// <inheritdoc/>
      public void Write(String format, Object arg0, Object arg1, Object arg2)
      {
         _consoleImplementation.Write(format, arg0, arg1, arg2);
      }

      /// <inheritdoc/>
      public void WriteLine()
      {
         _consoleImplementation.WriteLine();
      }

      /// <inheritdoc/>
      public void WriteLine(Boolean value)
      {
         _consoleImplementation.WriteLine(value);
      }

      /// <inheritdoc/>
      public void WriteLine(Char value)
      {
         _consoleImplementation.WriteLine(value);
      }

      /// <inheritdoc/>
      public void WriteLine(Char[] buffer)
      {
         _consoleImplementation.WriteLine(buffer);
      }

      /// <inheritdoc/>
      public void WriteLine(Decimal value)
      {
         _consoleImplementation.WriteLine(value);
      }

      /// <inheritdoc/>
      public void WriteLine(Double value)
      {
         _consoleImplementation.WriteLine(value);
      }

      /// <inheritdoc/>
      public void WriteLine(Int32 value)
      {
         _consoleImplementation.WriteLine(value);
      }

      /// <inheritdoc/>
      public void WriteLine(Int64 value)
      {
         _consoleImplementation.WriteLine(value);
      }

      /// <inheritdoc/>
      public void WriteLine(Object value)
      {
         _consoleImplementation.WriteLine(value);
      }

      /// <inheritdoc/>
      public void WriteLine(Single value)
      {
         _consoleImplementation.WriteLine(value);
      }

      /// <inheritdoc/>
      public void WriteLine(String value)
      {
         _consoleImplementation.WriteLine(value);
      }

      /// <inheritdoc/>
      [CLSCompliant(false)]
      public void WriteLine(UInt32 value)
      {
         _consoleImplementation.WriteLine(value);
      }

      /// <inheritdoc/>
      [CLSCompliant(false)]
      public void WriteLine(UInt64 value)
      {
         _consoleImplementation.WriteLine(value);
      }

      /// <inheritdoc/>
      public void WriteLine(String format, Object arg0)
      {
         _consoleImplementation.WriteLine(format, arg0);
      }

      /// <inheritdoc/>
      public void WriteLine(String format, params Object[] arg)
      {
         _consoleImplementation.WriteLine(format, arg);
      }

      /// <inheritdoc/>
      public void WriteLine(Char[] buffer, Int32 index, Int32 count)
      {
         _consoleImplementation.WriteLine(buffer, index, count);
      }

      /// <inheritdoc/>
      public void WriteLine(String format, Object arg0, Object arg1)
      {
         _consoleImplementation.WriteLine(format, arg0, arg1);
      }

      /// <inheritdoc/>
      public void WriteLine(String format, Object arg0, Object arg1, Object arg2)
      {
         _consoleImplementation.WriteLine(format, arg0, arg1, arg2);
      }

      private void OnCancelKeyPress(ConsoleCancelEventArgs e)
      {
         _cancelKeyPressListeners.Invoke(this, e);
      }

      private void ConsoleImplementation_CancelKeyPress(Object sender, ConsoleCancelEventArgs e)
      {
         OnCancelKeyPress(e);
      }
   }
}
