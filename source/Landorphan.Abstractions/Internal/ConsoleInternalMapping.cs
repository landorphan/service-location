namespace Landorphan.Abstractions.Internal
{
   using System;
   using System.Diagnostics.CodeAnalysis;
   using System.IO;
   using System.Text;
   using Landorphan.Common;

   /// <summary>
   /// Provides methods for interacting with a console or virtual console.
   /// </summary>
   /// <remarks>
   /// Provides a near one-to-one mapping to <see cref="Console"/> but in an Object instance (as opposed to static) to support testability.
   /// </remarks>
   internal sealed class ConsoleInternalMapping : DisposableObject, IConsoleInternalMapping
   {
      private readonly SourceWeakEventHandlerSet<ConsoleCancelEventArgs> _cancelKeyPressListeners;

      /// <summary>
      /// Initializes a new instance of the <see cref="ConsoleInternalMapping"/> class.
      /// </summary>
      internal ConsoleInternalMapping()
      {
         _cancelKeyPressListeners = new SourceWeakEventHandlerSet<ConsoleCancelEventArgs>();

         // wire the CancelKeyPress event.
         Console.CancelKeyPress += Console_CancelKeyPress;
      }

      /// <inheritdoc/>
      protected override void ReleaseManagedResources()
      {
         // un-wire the CancelKeyPress event.
         Console.CancelKeyPress -= Console_CancelKeyPress;
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
         get => Console.BackgroundColor;
         set
         {
            value.ArgumentMustBeValidEnumValue("value");
            Console.BackgroundColor = value;
         }
      }

      /// <inheritdoc/>
      public Int32 BufferHeight
      {
         get => Console.BufferHeight;
         set => Console.BufferHeight = value;
      }

      /// <inheritdoc/>
      public Int32 BufferWidth
      {
         get => Console.BufferWidth;
         set => Console.BufferWidth = value;
      }

      /// <inheritdoc/>
      public Boolean CapsLock => Console.CapsLock;

      /// <inheritdoc/>
      public Int32 CursorLeft
      {
         get => Console.CursorLeft;
         set => Console.CursorLeft = value;
      }

      /// <inheritdoc/>
      public Int32 CursorSize
      {
         get => Console.CursorSize;
         set => Console.CursorSize = value;
      }

      /// <inheritdoc/>
      public Int32 CursorTop
      {
         get => Console.CursorTop;
         set => Console.CursorTop = value;
      }

      /// <inheritdoc/>
      public Boolean CursorVisible
      {
         get => Console.CursorVisible;
         set => Console.CursorVisible = value;
      }

      /// <inheritdoc/>
      public TextWriter Error => Console.Error;

      /// <inheritdoc/>
      public ConsoleColor ForegroundColor
      {
         get => Console.ForegroundColor;
         set
         {
            value.ArgumentMustBeValidEnumValue("value");
            Console.ForegroundColor = value;
         }
      }

      /// <inheritdoc/>
      public TextReader Input => Console.In;

      /// <inheritdoc/>
      public Encoding InputEncoding
      {
         get => Console.InputEncoding;
         set => Console.InputEncoding = value;
      }

      /// <inheritdoc/>
      public Boolean IsErrorRedirected => Console.IsErrorRedirected;

      /// <inheritdoc/>
      public Boolean IsInputRedirected => Console.IsInputRedirected;

      /// <inheritdoc/>
      public Boolean IsOutputRedirected => Console.IsOutputRedirected;

      /// <inheritdoc/>
      public Boolean KeyAvailable => Console.KeyAvailable;

      /// <inheritdoc/>
      public Int32 LargestWindowHeight => Console.LargestWindowHeight;

      /// <inheritdoc/>
      public Int32 LargestWindowWidth => Console.LargestWindowWidth;

      /// <inheritdoc/>
      public Boolean NumberLock => Console.NumberLock;

      /// <inheritdoc/>
      public TextWriter Output => Console.Out;

      /// <inheritdoc/>
      public Encoding OutputEncoding
      {
         get => Console.OutputEncoding;
         set => Console.OutputEncoding = value;
      }

      /// <inheritdoc/>
      public String Title
      {
         get => Console.Title;
         set => Console.Title = value;
      }

      /// <inheritdoc/>
      public Boolean TreatControlCAsInput
      {
         get => Console.TreatControlCAsInput;
         set => Console.TreatControlCAsInput = value;
      }

      /// <inheritdoc/>
      public Int32 WindowHeight
      {
         get => Console.WindowHeight;
         set => Console.WindowHeight = value;
      }

      /// <inheritdoc/>
      public Int32 WindowLeft
      {
         get => Console.WindowLeft;
         set => Console.WindowLeft = value;
      }

      /// <inheritdoc/>
      public Int32 WindowTop
      {
         get => Console.WindowTop;
         set => Console.WindowTop = value;
      }

      /// <inheritdoc/>
      public Int32 WindowWidth
      {
         get => Console.WindowWidth;
         set => Console.WindowWidth = value;
      }

      /// <inheritdoc/>
      public void Beep()
      {
         Console.Beep();
      }

      /// <inheritdoc/>
      public void Beep(Int32 frequency, Int32 duration)
      {
         Console.Beep(frequency, duration);
      }

      /// <inheritdoc/>
      public void Clear()
      {
         Console.Clear();
      }

      /// <inheritdoc/>
      public void MoveBufferArea(Int32 sourceLeft, Int32 sourceTop, Int32 sourceWidth, Int32 sourceHeight, Int32 targetLeft, Int32 targetTop)
      {
         Console.MoveBufferArea(sourceLeft, sourceTop, sourceWidth, sourceHeight, targetLeft, targetTop);
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
         Console.MoveBufferArea(sourceLeft, sourceTop, sourceWidth, sourceHeight, targetLeft, targetTop, sourceChar, sourceForeColor, sourceBackColor);
      }

      /// <inheritdoc/>
      public Stream OpenStandardError()
      {
         return Console.OpenStandardError();
      }

      /// <inheritdoc/>
      public Stream OpenStandardError(Int32 bufferSize)
      {
         return Console.OpenStandardError(bufferSize);
      }

      /// <inheritdoc/>
      public Stream OpenStandardInput()
      {
         return Console.OpenStandardInput();
      }

      /// <inheritdoc/>
      public Stream OpenStandardInput(Int32 bufferSize)
      {
         return Console.OpenStandardInput(bufferSize);
      }

      /// <inheritdoc/>
      public Stream OpenStandardOutput()
      {
         return Console.OpenStandardOutput();
      }

      /// <inheritdoc/>
      public Stream OpenStandardOutput(Int32 bufferSize)
      {
         return Console.OpenStandardOutput(bufferSize);
      }

      /// <inheritdoc/>
      public Int32 Read()
      {
         return Console.Read();
      }

      /// <inheritdoc/>
      public ConsoleKeyInfo ReadKey()
      {
         return Console.ReadKey();
      }

      /// <inheritdoc/>
      public ConsoleKeyInfo ReadKey(Boolean intercept)
      {
         return Console.ReadKey(intercept);
      }

      /// <inheritdoc/>
      public String ReadLine()
      {
         return Console.ReadLine();
      }

      /// <inheritdoc/>
      public void ResetColor()
      {
         Console.ResetColor();
      }

      /// <inheritdoc/>
      public void SetBufferSize(Int32 width, Int32 height)
      {
         Console.SetBufferSize(width, height);
      }

      /// <inheritdoc/>
      public void SetCursorPosition(Int32 left, Int32 top)
      {
         Console.SetCursorPosition(left, top);
      }

      /// <inheritdoc/>
      public void SetError(TextWriter newError)
      {
         Console.SetError(newError);
      }

      /// <inheritdoc/>
      public void SetInput(TextReader newIn)
      {
         Console.SetIn(newIn);
      }

      /// <inheritdoc/>
      public void SetOutput(TextWriter newOut)
      {
         Console.SetOut(newOut);
      }

      /// <inheritdoc/>
      public void SetWindowPosition(Int32 left, Int32 top)
      {
         Console.SetWindowPosition(left, top);
      }

      /// <inheritdoc/>
      public void SetWindowSize(Int32 width, Int32 height)
      {
         Console.SetWindowSize(width, height);
      }

      /// <inheritdoc/>
      [SuppressMessage("SonarLint.CodeSmell", "S106: Standard outputs should not be used directly to log anything")]
      [SuppressMessage("SonarLint.CodeSmell", "S2228: Console logging should not be used")]
      public void Write(Boolean value)
      {
         Console.Write(value);
      }

      /// <inheritdoc/>
      [SuppressMessage("SonarLint.CodeSmell", "S106: Standard outputs should not be used directly to log anything")]
      [SuppressMessage("SonarLint.CodeSmell", "S2228: Console logging should not be used")]
      public void Write(Char value)
      {
         Console.Write(value);
      }

      /// <inheritdoc/>
      [SuppressMessage("SonarLint.CodeSmell", "S106: Standard outputs should not be used directly to log anything")]
      [SuppressMessage("SonarLint.CodeSmell", "S2228: Console logging should not be used")]
      public void Write(Char[] buffer)
      {
         Console.Write(buffer);
      }

      /// <inheritdoc/>
      [SuppressMessage("SonarLint.CodeSmell", "S106: Standard outputs should not be used directly to log anything")]
      [SuppressMessage("SonarLint.CodeSmell", "S2228: Console logging should not be used")]
      public void Write(Decimal value)
      {
         Console.Write(value);
      }

      /// <inheritdoc/>
      [SuppressMessage("SonarLint.CodeSmell", "S106: Standard outputs should not be used directly to log anything")]
      [SuppressMessage("SonarLint.CodeSmell", "S2228: Console logging should not be used")]
      public void Write(Double value)
      {
         Console.Write(value);
      }

      /// <inheritdoc/>
      [SuppressMessage("SonarLint.CodeSmell", "S106: Standard outputs should not be used directly to log anything")]
      [SuppressMessage("SonarLint.CodeSmell", "S2228: Console logging should not be used")]
      public void Write(Int32 value)
      {
         Console.Write(value);
      }

      /// <inheritdoc/>
      [SuppressMessage("SonarLint.CodeSmell", "S106: Standard outputs should not be used directly to log anything")]
      [SuppressMessage("SonarLint.CodeSmell", "S2228: Console logging should not be used")]
      public void Write(Int64 value)
      {
         Console.Write(value);
      }

      /// <inheritdoc/>
      [SuppressMessage("SonarLint.CodeSmell", "S106: Standard outputs should not be used directly to log anything")]
      [SuppressMessage("SonarLint.CodeSmell", "S2228: Console logging should not be used")]
      public void Write(Object value)
      {
         Console.Write(value);
      }

      /// <inheritdoc/>
      [SuppressMessage("SonarLint.CodeSmell", "S106: Standard outputs should not be used directly to log anything")]
      [SuppressMessage("SonarLint.CodeSmell", "S2228: Console logging should not be used")]
      public void Write(Single value)
      {
         Console.Write(value);
      }

      /// <inheritdoc/>
      [SuppressMessage("SonarLint.CodeSmell", "S106: Standard outputs should not be used directly to log anything")]
      [SuppressMessage("SonarLint.CodeSmell", "S2228: Console logging should not be used")]
      public void Write(String value)
      {
         Console.Write(value);
      }

      /// <inheritdoc/>
      [SuppressMessage("SonarLint.CodeSmell", "S106: Standard outputs should not be used directly to log anything")]
      [SuppressMessage("SonarLint.CodeSmell", "S2228: Console logging should not be used")]
      public void Write(UInt32 value)
      {
         Console.Write(value);
      }

      /// <inheritdoc/>
      [SuppressMessage("SonarLint.CodeSmell", "S106: Standard outputs should not be used directly to log anything")]
      [SuppressMessage("SonarLint.CodeSmell", "S2228: Console logging should not be used")]
      public void Write(UInt64 value)
      {
         Console.Write(value);
      }

      /// <inheritdoc/>
      [SuppressMessage("SonarLint.CodeSmell", "S106: Standard outputs should not be used directly to log anything")]
      [SuppressMessage("SonarLint.CodeSmell", "S2228: Console logging should not be used")]
      public void Write(String format, Object arg0)
      {
         Console.Write(format, arg0);
      }

      /// <inheritdoc/>
      [SuppressMessage("SonarLint.CodeSmell", "S106: Standard outputs should not be used directly to log anything")]
      [SuppressMessage("SonarLint.CodeSmell", "S2228: Console logging should not be used")]
      public void Write(String format, params Object[] arg)
      {
         Console.Write(format, arg);
      }

      /// <inheritdoc/>
      [SuppressMessage("SonarLint.CodeSmell", "S106: Standard outputs should not be used directly to log anything")]
      [SuppressMessage("SonarLint.CodeSmell", "S2228: Console logging should not be used")]
      public void Write(Char[] buffer, Int32 index, Int32 count)
      {
         Console.Write(buffer, index, count);
      }

      /// <inheritdoc/>
      [SuppressMessage("SonarLint.CodeSmell", "S106: Standard outputs should not be used directly to log anything")]
      [SuppressMessage("SonarLint.CodeSmell", "S2228: Console logging should not be used")]
      public void Write(String format, Object arg0, Object arg1)
      {
         Console.Write(format, arg0, arg1);
      }

      /// <inheritdoc/>
      [SuppressMessage("SonarLint.CodeSmell", "S106: Standard outputs should not be used directly to log anything")]
      [SuppressMessage("SonarLint.CodeSmell", "S2228: Console logging should not be used")]
      public void Write(String format, Object arg0, Object arg1, Object arg2)
      {
         Console.Write(format, arg0, arg1, arg2);
      }

      /// <inheritdoc/>
      [SuppressMessage("SonarLint.CodeSmell", "S106: Standard outputs should not be used directly to log anything")]
      [SuppressMessage("SonarLint.CodeSmell", "S2228: Console logging should not be used")]
      public void WriteLine()
      {
         Console.WriteLine();
      }

      /// <inheritdoc/>
      [SuppressMessage("SonarLint.CodeSmell", "S106: Standard outputs should not be used directly to log anything")]
      [SuppressMessage("SonarLint.CodeSmell", "S2228: Console logging should not be used")]
      public void WriteLine(Boolean value)
      {
         Console.WriteLine(value);
      }

      /// <inheritdoc/>
      [SuppressMessage("SonarLint.CodeSmell", "S106: Standard outputs should not be used directly to log anything")]
      [SuppressMessage("SonarLint.CodeSmell", "S2228: Console logging should not be used")]
      public void WriteLine(Char value)
      {
         Console.WriteLine(value);
      }

      /// <inheritdoc/>
      [SuppressMessage("SonarLint.CodeSmell", "S106: Standard outputs should not be used directly to log anything")]
      [SuppressMessage("SonarLint.CodeSmell", "S2228: Console logging should not be used")]
      public void WriteLine(Char[] buffer)
      {
         Console.WriteLine(buffer);
      }

      /// <inheritdoc/>
      [SuppressMessage("SonarLint.CodeSmell", "S106: Standard outputs should not be used directly to log anything")]
      [SuppressMessage("SonarLint.CodeSmell", "S2228: Console logging should not be used")]
      public void WriteLine(Char[] buffer, Int32 index, Int32 count)
      {
         Console.WriteLine(buffer, index, count);
      }

      /// <inheritdoc/>
      [SuppressMessage("SonarLint.CodeSmell", "S106: Standard outputs should not be used directly to log anything")]
      [SuppressMessage("SonarLint.CodeSmell", "S2228: Console logging should not be used")]
      public void WriteLine(Decimal value)
      {
         Console.WriteLine(value);
      }

      /// <inheritdoc/>
      [SuppressMessage("SonarLint.CodeSmell", "S106: Standard outputs should not be used directly to log anything")]
      [SuppressMessage("SonarLint.CodeSmell", "S2228: Console logging should not be used")]
      public void WriteLine(Double value)
      {
         Console.WriteLine(value);
      }

      /// <inheritdoc/>
      [SuppressMessage("SonarLint.CodeSmell", "S106: Standard outputs should not be used directly to log anything")]
      [SuppressMessage("SonarLint.CodeSmell", "S2228: Console logging should not be used")]
      public void WriteLine(Single value)
      {
         Console.WriteLine(value);
      }

      /// <inheritdoc/>
      [SuppressMessage("SonarLint.CodeSmell", "S106: Standard outputs should not be used directly to log anything")]
      [SuppressMessage("SonarLint.CodeSmell", "S2228: Console logging should not be used")]
      public void WriteLine(Int32 value)
      {
         Console.WriteLine(value);
      }

      /// <inheritdoc/>
      [SuppressMessage("SonarLint.CodeSmell", "S106: Standard outputs should not be used directly to log anything")]
      [SuppressMessage("SonarLint.CodeSmell", "S2228: Console logging should not be used")]
      public void WriteLine(UInt32 value)
      {
         Console.WriteLine(value);
      }

      /// <inheritdoc/>
      [SuppressMessage("SonarLint.CodeSmell", "S106: Standard outputs should not be used directly to log anything")]
      [SuppressMessage("SonarLint.CodeSmell", "S2228: Console logging should not be used")]
      public void WriteLine(Int64 value)
      {
         Console.WriteLine(value);
      }

      /// <inheritdoc/>
      [SuppressMessage("SonarLint.CodeSmell", "S106: Standard outputs should not be used directly to log anything")]
      [SuppressMessage("SonarLint.CodeSmell", "S2228: Console logging should not be used")]
      public void WriteLine(UInt64 value)
      {
         Console.WriteLine(value);
      }

      /// <inheritdoc/>
      [SuppressMessage("SonarLint.CodeSmell", "S106: Standard outputs should not be used directly to log anything")]
      [SuppressMessage("SonarLint.CodeSmell", "S2228: Console logging should not be used")]
      public void WriteLine(Object value)
      {
         Console.WriteLine(value);
      }

      /// <inheritdoc/>
      [SuppressMessage("SonarLint.CodeSmell", "S106: Standard outputs should not be used directly to log anything")]
      [SuppressMessage("SonarLint.CodeSmell", "S2228: Console logging should not be used")]
      public void WriteLine(String value)
      {
         Console.WriteLine(value);
      }

      /// <inheritdoc/>
      [SuppressMessage("SonarLint.CodeSmell", "S106: Standard outputs should not be used directly to log anything")]
      [SuppressMessage("SonarLint.CodeSmell", "S2228: Console logging should not be used")]
      public void WriteLine(String format, Object arg0)
      {
         Console.WriteLine(format, arg0);
      }

      /// <inheritdoc/>
      [SuppressMessage("SonarLint.CodeSmell", "S106: Standard outputs should not be used directly to log anything")]
      [SuppressMessage("SonarLint.CodeSmell", "S2228: Console logging should not be used")]
      public void WriteLine(String format, Object arg0, Object arg1)
      {
         Console.WriteLine(format, arg0, arg1);
      }

      /// <inheritdoc/>
      [SuppressMessage("SonarLint.CodeSmell", "S106: Standard outputs should not be used directly to log anything")]
      [SuppressMessage("SonarLint.CodeSmell", "S2228: Console logging should not be used")]
      public void WriteLine(String format, Object arg0, Object arg1, Object arg2)
      {
         Console.WriteLine(format, arg0, arg1, arg2);
      }

      /// <inheritdoc/>
      [SuppressMessage("SonarLint.CodeSmell", "S106: Standard outputs should not be used directly to log anything")]
      [SuppressMessage("SonarLint.CodeSmell", "S2228: Console logging should not be used")]
      public void WriteLine(String format, params Object[] arg)
      {
         Console.WriteLine(format, arg);
      }

      private void OnCancelKeyPress(ConsoleCancelEventArgs e)
      {
         _cancelKeyPressListeners.Invoke(this, e);
      }

      private void Console_CancelKeyPress(Object sender, ConsoleCancelEventArgs e)
      {
         OnCancelKeyPress(e);
      }
   }
}
