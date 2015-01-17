/*
 * ArticleSetControl
 *
 * When screening the articles, the researcher will have to move articles from one
 * category to another. Each category will contain a set of articles and will have
 * to have a list and the possibility to:
 * - open the article details
 * - add comments to the article
 * - move the article to a different category
 *   - the reason can be mentioned in the comments OR at the move a dialog with
 *     predefined reasons can appear
 */
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MedicalResearchAssistant.View
{
    /// <summary>
    /// Interaction logic for ArticleSetControl.xaml
    /// </summary>
    public partial class ArticleSetControl : UserControl
    {
        public ArticleSetControl()
        {
            InitializeComponent();
        }
    }
}
