using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Practices.Prism.Commands;
using PixelLab.Contracts;
using Tasks.Show.Models;

namespace Tasks.Show.ViewModels
{
    public class Folders
    {
        #region Fields

        private readonly DelegateCommand<Color> m_setCurrentColorCommand;
        private readonly DelegateCommand<BaseFolder> m_setCurrentCommand;
        private readonly TaskData m_taskData;

        #endregion Fields

        #region Constructors

        public Folders(TaskData taskData)
        {
            Contract.Requires(null != taskData, "taskList");
            m_taskData = taskData;

            m_setCurrentCommand = new DelegateCommand<BaseFolder>(
                val => m_taskData.CurrentFolder = val,
                val => m_taskData.CurrentFolder != val);

            m_setCurrentColorCommand = new DelegateCommand<Color>(
                var => m_taskData.CurrentFolder.Color = var,
                var => IsCurrentUserFolder
            );
        }

        #endregion Constructors

        #region Properties

        public TaskData TaskData
        {
            get
            {
                return m_taskData;
            }
        }

        public bool IsCurrentUserFolder
        {
            get { return !m_taskData.CurrentFolder.IsSpecial; }
        }

        public ICommand SetCurrentColorCommand { get { return m_setCurrentColorCommand; } }

        public ICommand SetCurrentCommand { get { return m_setCurrentCommand; } }

        #endregion Properties
    }
}
