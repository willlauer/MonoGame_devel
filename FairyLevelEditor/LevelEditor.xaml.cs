using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FairyLevelEditor
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class LevelEditor : Window
    {
        private LevelEditorViewModel viewModel;
        public LevelEditor(LevelEditorViewModel vm)
        {
            DataContext = viewModel = vm;
            InitializeComponent();
        }

        private void LvCanvas_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.All;
            }
        }

        private void LvCanvas_Drop(object sender, DragEventArgs e)
        {
            var position = e.GetPosition(LvCanvas);

            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            foreach (string file in files)
            {
                if (!file.Contains("fairy_components")) continue;
            }
        }
    }

    public class LevelEditorViewModel : ViewModelBase
    {
        
    }
}
