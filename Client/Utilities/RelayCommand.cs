using System.Windows.Input;

namespace Client.Utilities
{
    /// <summary>
    /// A command implementation that relays its functionality to other objects by invoking delegates.
    /// This non-generic version is used for commands that don't require a command parameter.
    /// Implements the ICommand interface for WPF/MVVM command binding.
    /// </summary>
    public class RelayCommand : ICommand
    {
        private readonly Action _execute;          // The delegate to execute when command is invoked
        private readonly Func<bool>? _canExecute;   // Optional delegate to determine if command can execute

        /// <summary>
        /// Initializes a new instance of the RelayCommand class.
        /// </summary>
        /// <param name="execute">The execution logic (required).</param>
        /// <param name="canExecute">The can-execute logic (optional).</param>
        public RelayCommand(Action execute, Func<bool>? canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        /// <summary>
        /// Event that is raised when the conditions for command execution may have changed.
        /// </summary>
        public event EventHandler? CanExecuteChanged;

        /// <summary>
        /// Determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">Ignored for non-generic RelayCommand.</param>
        /// <returns>True if command can execute, otherwise false.</returns>
        public bool CanExecute(object? parameter) => _canExecute == null || _canExecute();

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="parameter">Ignored for non-generic RelayCommand.</param>
        public void Execute(object? parameter) => _execute();

        /// <summary>
        /// Raises the CanExecuteChanged event to notify command consumers (like buttons)
        /// that the command's executable status may have changed.
        /// </summary>
        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// A generic version of RelayCommand that accepts a command parameter of type T.
    /// Implements the ICommand interface for WPF/MVVM command binding.
    /// </summary>
    /// <typeparam name="T">The type of the command parameter.</typeparam>
    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T> _execute;          // The delegate to execute when command is invoked
        private readonly Func<T, bool>? _canExecute;  // Optional delegate to determine if command can execute

        /// <summary>
        /// Initializes a new instance of the generic RelayCommand class.
        /// </summary>
        /// <param name="execute">The execution logic (required).</param>
        /// <param name="canExecute">The can-execute logic (optional).</param>
        /// <exception cref="ArgumentNullException">Thrown if execute delegate is null.</exception>
        public RelayCommand(Action<T> execute, Func<T, bool>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        /// <summary>
        /// Determines whether the command can execute in its current state.
        /// Handles various parameter scenarios including null and type conversion.
        /// </summary>
        /// <param name="parameter">The command parameter.</param>
        /// <returns>True if command can execute, otherwise false.</returns>
        public bool CanExecute(object? parameter)
        {
            if (_canExecute == null)
                return true;

            if (parameter == null && typeof(T).IsValueType)
                return _canExecute(default!);

            if (parameter is T tParam)
                return _canExecute(tParam);

            return false;
        }

        /// <summary>
        /// Executes the command with the provided parameter.
        /// </summary>
        /// <param name="parameter">The command parameter.</param>
        /// <exception cref="ArgumentException">Thrown if parameter type is incompatible with T.</exception>
        public void Execute(object? parameter)
        {
            if (parameter is T tParam)
                _execute(tParam);
            else if (parameter == null && typeof(T).IsValueType)
                _execute(default!);
            else
                throw new ArgumentException($"Invalid command parameter type. Expected {typeof(T)}.");
        }

        /// <summary>
        /// Event that is raised when the conditions for command execution may have changed.
        /// </summary>
        public event EventHandler? CanExecuteChanged;

        /// <summary>
        /// Raises the CanExecuteChanged event to notify command consumers (like buttons)
        /// that the command's executable status may have changed.
        /// </summary>
        public void RaiseCanExecuteChanged() =>
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}