using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp.Forms_UI
{
    /// <summary>
    /// Interface để đánh dấu UserControl hỗ trợ sắp xếp
    /// </summary>
    public interface ISortable
    {
        /// <summary>
        /// Sắp xếp file theo một tiêu chí nhất định
        /// </summary>
        void ApplySort(string option);
    }
}
