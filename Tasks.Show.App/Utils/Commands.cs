using System.Windows;
using System.Windows.Input;
using PixelLab.Contracts;

namespace Tasks.Show.Utils
{
    public static class Commands
    {
        public static RoutedUICommand Cancel { get { return s_cancelCommand; } }

        public static void MapCommand(ICommand source, RoutedCommand target, UIElement element)
        {
            Contract.Requires(null != source, "source");
            Contract.Requires(null != target, "target");
            Contract.Requires(null != element, "element");

            var binding = new CommandBinding(target,
                (sender, args) => { source.Execute(args.Parameter); },
                (sender, args) => { args.CanExecute = source.CanExecute(args.Parameter); }
            );

            element.CommandBindings.Add(binding);

            source.CanExecuteChanged += (sender, args) =>
            {
                target.CanExecute(null, element);
            };
        }

        private static readonly RoutedUICommand s_cancelCommand = new RoutedUICommand("Cancel", "Cancel", typeof(Commands));

    }
}
